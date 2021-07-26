using System;
using System.IO;
using System.Text;
using XOR_Unpacker.StaticAnalysis;

namespace XOR_Unpacker {
    public static class Unpacker {

        public static string UnpackXORScript(string input) {
            Debug.Log("[2/7] Parsing script..");
            Parser.ScriptData data = new Parser.ScriptData(input);
            Debug.Log("[4/7] Decrypting table data..", ConsoleColor.White);

            int[] fixed_1 = Simulation.L_FixByteTable(data.dataArray);
            Debug.Log("[5/7] Decoding table data..");
            return Simulation.L_DecodeTable(fixed_1, data.decryptionKey);
            

        }
    }
}
