using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace XOR_Unpacker.StaticAnalysis {
    public static class Parser {
        public static string DecodeLoader(string input) {
			Regex loaderData = new Regex(@"\(""(.{300,})""\)", RegexOptions.Singleline);
			if (loaderData.IsMatch(input)) {
				Debug.Log("[2/7] ---> Detected loader!", ConsoleColor.Yellow);
				for (int i = 255; i >= 0; i--) {
					string a = ((char)i).ToString();
					if (a == "\\") a = "%ESCAPED_BSLASH%";
					input = input.Replace(@"\" + i, "%REPLACE%" + a + "%REPLACE%");
				}
				return input.Replace("%REPLACE%", "").Replace("%ESCAPED_BSLASH%", "\\");
			} else {
				return input;
            }

			/*
			return Regex.Unescape(input);			
			*/
		}

		public static string GetMainCall(string script) {
			script = DecodeLoader(script);

			int start = script.LastIndexOf("({") + 2;
			return script.Substring(start);
        }



		public class ScriptData {
			public int[] dataArray { get; set; }
			public string decryptionKey { get; set; }

			public ScriptData(string script) {

				string main = GetMainCall(script).Replace(" ","");

				Debug.Log("[3/7] Fetching handlers..");
				// + 20 test paragrafını çözdün mü???
				// - paragrafı siktir et abi daha önemli birşey var


				bool isOriginalXOR = false;
				Regex r = new Regex(@"key *?= *?""(.+?)""", RegexOptions.Singleline);
				if (r.IsMatch(script)) {
                    isOriginalXOR = true;
					decryptionKey = r.Match(script).Groups[1].Value;
					Debug.Log("[3/7] ---> Detected original XOR encryption!", ConsoleColor.Yellow);
                } else {

					Debug.Log("[3/7] ---> Detected filesecuring mod XOR encryption!", ConsoleColor.Yellow);
				}


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
