using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MI {
	public class SerialPortManager {


		public static IList<string> LoadPorts() {
			return SerialPort.GetPortNames().ToList();

		}


		private SerialPort sePort;
		public bool IsInitialized { get; set; }


		public string InitSerial(string COMPortName, bool DiscardIn = true, bool DiscardOut = true, SerialDataReceivedEventHandler handler = null) {
			try {
				if (sePort != null && sePort.IsOpen) {
					sePort.Close();
					sePort.Dispose();
				}
				sePort = new SerialPort();
				sePort.PortName = COMPortName;


				sePort.DataBits = 8;
				sePort.Parity = Parity.None;
				sePort.StopBits = StopBits.One;
				sePort.DtrEnable = true;
				sePort.RtsEnable = true;
				//sePort.Encoding = Encoding.GetEncoding("iso-8859-1");
				sePort.BaudRate = 57600;
				sePort.DataReceived += handler;

				sePort.Open();
				if (DiscardIn)
					sePort.DiscardInBuffer();
				if (DiscardOut)
					sePort.DiscardOutBuffer();
				return "Connected";
			} catch (Exception ex) {
				return ex.Message;
			}
		}


		public void Write(string text) {
			sePort.Write(text);
		}

		public void WriteLine(string text) {
			sePort.WriteLine(text);
		}


		public string ReadExisting() {
			return sePort.ReadExisting();
		}

		public void close() {
			if (sePort.IsOpen) {
				sePort.Close();
			}
		}


	}
}
