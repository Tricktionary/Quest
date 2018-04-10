using System;
using System.IO;
using UnityEngine;

public class Logger {

	//This constructor will call the init function
	//Should only be called once in your code
	public Logger() {
		//this.init();
	}

	//Can be called as many time as you want in your code (as it will still construct the logger but won't call the init function
	public Logger(bool b) {} //This constructor won't call the init function

	public void logCustom(string n, string type) {
		printToFile(generateTimestamp() + " [" + type.ToUpper() + "]: " + n + "\n");
	}

	public void info(string n) {
		printToFile(generateTimestamp() + " [INFO]: " + n + "\n");
	}

	public void debug(string n) {
		printToFile(generateTimestamp() + " [DEBUG]: " + n + "\n");
	}

	public void warn(string n) {
		printToFile(generateTimestamp() + " [WARN]: " + n + "\n");
	}

	public void error(string n) {
		printToFile(generateTimestamp() + " [ERROR]: " + n + "\n");
	}

	public void trace(string n) {
		printToFile(generateTimestamp() + " [TRACE]: " + n + "\n");
	}

	public void test(string n) {
		printToFile(generateTimestamp() + " [TEST]: " + n + "\n");
	}

	private void init() {
		printToFile("-------------------- INITIALIZE LOGGER ---------------------\n");
		printToFile(generateTimestamp() + ": Logger initialized\n");
	}

	private void printToFile(string n) {
		// Print logs to console too.
		Debug.Log(n);
		/*COMMENT THIS OUT BEFORE SHIPPING*/
		System.IO.File.AppendAllText(Directory.GetCurrentDirectory() + "/Logs/GameLog.log", n);
	}

	private string generateTimestamp() {
		return DateTime.Now.ToString("O");
	}
}
