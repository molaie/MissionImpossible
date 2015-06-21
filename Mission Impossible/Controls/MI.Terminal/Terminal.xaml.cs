﻿using System;
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

		private SerialPortManager serialManager = new SerialPortManager();

		public Terminal() {
			InitializeComponent();
		}

		private void rtbTerminal_PreviewKeyDown(object sender, KeyEventArgs e) {
			if (e.Key != Key.Enter) {
				return;
			}
		}

		private void rtbTerminal_KeyUp(object sender, KeyEventArgs e) {
			if (!isConnected) {
				e.Handled = true;
				return;
			}

			//TextPointer t = new TextPointer(


			//rtbTerminal.Selection.Start = new TextRange(rtbTerminal.Document.ContentStart, rtbTerminal.Document.ContentEnd).Text.Length;
			//switch (e.Key) {
			//	case Key.Up:
			//		if (history > 0) {
			//			rtbTerminal.Select(inputStartPos, rtbTerminal.Text.Length - inputStartPos);
			//			rtbTerminal.Selection = "";
			//			rtbTerminal.AppendText(cmdHistory[--history]);
			//		}
			//		e.Handled = true;
			//		break;
			//	case Key.Down:
			//		if (history < cmdHistory.Count - 1) {
			//			rtbTerminal.Select(inputStartPos, rtbTerminal.Text.Length - inputStartPos);
			//			rtbTerminal.Selection = "";
			//			rtbTerminal.AppendText(cmdHistory[++history]);
			//		}
			//		e.Handled = true;
			//		break;
			//	case Key.Left:
			//	case Key.Back:
			//		if (rtbTerminal.Selection.Start <= inputStartPos)
			//			e.Handled = true;
			//		break;

			//	//case Keys.Right:
			//	//    break;
			//}
		}



		private void InsertLog(string log) {
			if (string.IsNullOrWhiteSpace(log)) {
				log = "NOP" + Environment.NewLine;
			}
			log = log.TrimEnd('\r');
			log = log.Replace("\0", "");
			log = log.Replace((char)0x1b + "[K", "");



			if (rtbTerminal.Dispatcher.CheckAccess()) {

			}


			//rtbTerminal.InvokeIfRequired(c => {
			//	rtbTerminal.AppendText(log);

			//	rtbTerminal.Selection.Start = new TextRange(rtbTerminal.Document.ContentStart, rtbTerminal.Document.ContentEnd).Text.Length;
			//	inputStartPos = rtbTerminal.Selection.Start;
			//	rtbTerminal.Focus();
			//}, null);


		}


		public void Connect() {
			if (string.IsNullOrWhiteSpace(SelectedComPort)) {
				InsertLog("There is no comport to connect");
			}

			var connectionResult = serialManager.InitSerial(SelectedComPort, handler: sePort_DataReceived);
			InsertLog(connectionResult);
			if ("Connected" == connectionResult) {
				//we need at least one enter to see Nuttx
				serialManager.WriteLine("");
				isConnected = true;
				serialManager.WriteLine("?");
			}
		}


		private void sePort_DataReceived(object sender, SerialDataReceivedEventArgs e) {
			string result = serialManager.ReadExisting();
			InsertLog(result);
		}


		private async Task WaitForBoot() {
			await Task.Delay(12000);
		}


	}
}
