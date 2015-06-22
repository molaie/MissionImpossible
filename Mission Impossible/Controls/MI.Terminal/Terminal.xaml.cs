using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MI.Terminal {
	/// <summary>
	/// Interaction logic for Terminal.xaml
	/// </summary>
	public partial class Terminal : UserControl {

		List<string> cmdHistory = new List<string>();

		private List<string> TerminalCommands = new List<string>() {
			"cls"
			, "......"

		};


		int history = 0;
		//save last position of caret, so user can go up & down in history
		int inputStartPos = 0;
		//Which comport?
		public string SelectedComPort { get; set; }
		private bool isConnected;
		//command that is going to send to terminal
		string cmd = string.Empty;

		private SerialPortManager serialManager = new SerialPortManager();

		public Terminal() {
			InitializeComponent();
		}

		private void InsertLog(string log) {
			if (string.IsNullOrWhiteSpace(log)) {
				log = "NOP" + Environment.NewLine;
			}
			log = log.TrimEnd('\r');
			log = log.Replace("\0", "");
			log = log.Replace((char)0x1b + "[K", "");



			if (txtTerminal.Dispatcher.CheckAccess()) {

			}


			//txtTerminal.InvokeIfRequired(c => {
			//	txtTerminal.AppendText(log);

			//	txtTerminal.Selection.Start = new TextRange(txtTerminal.Document.ContentStart, txtTerminal.Document.ContentEnd).Text.Length;
			//	inputStartPos = txtTerminal.Selection.Start;
			//	txtTerminal.Focus();
			//}, null);


		}


		public void Connect() {
			if (string.IsNullOrWhiteSpace(SelectedComPort)) {
				InsertLog("There is no comport to connect");
			}
			//txtTerminal.Select

			var connectionResult = serialManager.InitSerial(SelectedComPort, handler: sePort_DataReceived);
			InsertLog(connectionResult);
			if ("Connected" == connectionResult) {
				//we need at least one enter to see Nuttx
				serialManager.WriteLine("");
				isConnected = true;
				serialManager.WriteLine("?");
			}
		}

		public void ClearLog() {
			txtTerminal.Text = "";
			serialManager.WriteLine("");
		}


		private void sePort_DataReceived(object sender, SerialDataReceivedEventArgs e) {
			string result = serialManager.ReadExisting();
			InsertLog(result);
		}


		private async Task WaitForBoot() {
			await Task.Delay(12000);
		}




		private async void txtTerminal_PreviewKeyDown(object sender, KeyEventArgs e) {
			switch (e.Key) {
				case Key.Up:
					if (history > 0) {
						txtTerminal.Select(inputStartPos, txtTerminal.Text.Length - inputStartPos);
						txtTerminal.SelectedText = "";
						txtTerminal.AppendText(cmdHistory[--history]);
					}
					e.Handled = true;
					break;
				case Key.Down:
					if (history < cmdHistory.Count - 1) {
						txtTerminal.Select(inputStartPos, txtTerminal.Text.Length - inputStartPos);
						txtTerminal.SelectedText = "";
						txtTerminal.AppendText(cmdHistory[++history]);
					}
					e.Handled = true;
					break;
				case Key.Left:
					if (txtTerminal.SelectionStart <= inputStartPos)
						e.Handled = true;
					break;
				case Key.Back:
					if (txtTerminal.SelectionStart <= inputStartPos)
						e.Handled = true;
					break;


				//Sending command to terminal and get results back
				case Key.Enter:
					try {
						cmd = txtTerminal.Text.Substring(inputStartPos, txtTerminal.Text.Length - inputStartPos).ToLower();
						if (cmd.Length > 0 && (cmdHistory.Count == 0 || cmdHistory.Last() != cmd)) {
							cmdHistory.Add(cmd);
							history = cmdHistory.Count;
						}
						if (cmd == "cls") {
							ClearLog();
						} else if (cmd == "reboot") {
							serialManager.WriteLine("reboot");
							serialManager.close();
							await WaitForBoot();
							Connect();
						} else {
							serialManager.WriteLine(cmd);
						}
					} catch {
					}
					//2 counts for \r\n respectively, we are in an enter
					inputStartPos = txtTerminal.Text.Length + 2;
					cmd = string.Empty;
					break;
			}
		}

		private void txtTerminal_PreviewTextInput(object sender, TextCompositionEventArgs e) {
			//cmd += e.Text;
		}


	}
}
