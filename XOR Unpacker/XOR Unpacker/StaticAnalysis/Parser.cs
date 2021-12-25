using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace XOR_Unpacker.StaticAnalysis {
    public static class Parser {
        public static string DecodeLoader(string input) {
			Regex loaderData = new Regex(@"\(""(.{300,})""\)\(\)", RegexOptions.Singleline);
			if (loaderData.IsMatch(input)) {
				Debug.Log("[2/7] ---> Detected loader!", ConsoleColor.Yellow);
				input = loaderData.Match(input).Groups[1].Value;
				byte[] bytes = Array.ConvertAll(input.Substring(1).Split('\\'), b => (byte)(int.Parse(b)));
				return Encoding.ASCII.GetString(bytes);
				
			} else {
				return input;
            }

		}

		public static string GetMainCall(string script) {			
			int start = script.LastIndexOf("({") + 2;
			return script.Substring(start);
        }
		const string realScriptSeparator = @"10'\);? ?end;? ?end\);? ?end\);? ?end\);? ?end;? ?\w+?\(\{(?:-?\d,?)+?\},""\w+?""\);? ?";
		public static string GetRealScript(string script) { // analyzes the script and finds the main script between fake ones. (another funny xor mod apparently)
			Regex r = new Regex(realScriptSeparator, RegexOptions.Singleline);
			var m = r.Matches(script);
			int lastMaxSize = 0;
			string lastLargestScript = "";

			foreach (Match x in m) {
				string match = x.Value.ToString();
				if (match.Length > lastMaxSize) {
					lastMaxSize = match.Length;
					lastLargestScript = match;
                }
            }

			return lastLargestScript;
		}

		public class ScriptData {
			public int[] dataArray { get; set; }
			public string decryptionKey { get; set; }

			public ScriptData(string script) {

				script = DecodeLoader(script);
				

				Debug.Log("[3/7] Fetching handlers..");

				bool isOriginalXOR = false;
				bool isUnknownXOR = false;
				Regex r = new Regex(@"key *?= *?""(.+?)""", RegexOptions.Singleline);
				Regex r2 = new Regex(realScriptSeparator, RegexOptions.Singleline);

				if (r.IsMatch(script)) {
					isOriginalXOR = true;
					decryptionKey = r.Match(script).Groups[1].Value;
					Debug.Log("[3/7] ---> Detected original XOR encryption!", ConsoleColor.Yellow);
				} else if (r2.Matches(script).Count > 4) {
					Debug.Log("[3/7] ---> Detected unknown XOR encryption! (the one that's bloated with fake scripts)", ConsoleColor.Yellow);
					script = GetRealScript(script);
					isUnknownXOR = true;
				} else { 
					Debug.Log("[3/7] ---> Detected filesecuring mod XOR encryption!", ConsoleColor.Yellow);
				}

				if (isUnknownXOR) { }
				string main = GetMainCall(script).Replace(" ", "");
				string table = main.Substring(0, main.IndexOf("}"));
				string[] data = table.Split(',');
				dataArray = Array.ConvertAll(data, s => Int32.Parse(s));

				if (isOriginalXOR) return;
				main = main.Substring(main.IndexOf("\"") + 1);
				decryptionKey = main.Substring(0, main.IndexOf("\""));



            }
        }

    }
}
