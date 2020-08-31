using Core.Interfaces;
using System;
using System.IO;

namespace Core {
	public class CommonLog : ILogger {
		private const string LogFilePath = "REDConsole.log";
		private StreamWriter LogFile;

		private static CommonLog instance;

		public event EventHandler<string> MessageLogged;

		private CommonLog() {
			InitializeLogFile();
			LogToFile("New Logging Session Started");
		}

		public static CommonLog Instance {
			get {
				if (instance == null) instance = new CommonLog();
				return instance;
			}
		}

		private void InitializeLogFile() {
			try {
				LogFile = new StreamWriter(LogFilePath, true);
			}
			catch (Exception e) {
				MessageLogged?.Invoke(Instance, $"There was a problem opening the log file: {e}");
			}
		}

		public void Log(string text, params object[] args) {
			var msg = String.Format(text, args);
			MessageLogged?.Invoke(null, msg);
			LogToFile(msg);
		}

		public void LogToFile(string msg) {
			LogFile?.WriteLine("{0:o}: {1}", DateTime.Now, msg);
			LogFile?.Flush();
		}
	}
}
