using System;
using System.Collections.Generic;
using System.Text;

namespace XOR_Unpacker.StaticAnalysis {
    public static class Simulation {
        public static int L_XORDecrypt(int data, int keyByte) {
            int[][] XOR_l = { new int[] { 0, 1 }, new int[] { 1, 0 } };
            int pow = 1;
            int c = 0;

            while (data > 0 || keyByte > 0) {
                c += (XOR_l[data % 2][keyByte % 2] * pow);
                data = (int)(data / 2);
                keyByte = (int)(keyByte / 2);
                pow *=  2;
            }
            return c;
        }

        public static int[] L_FixByteTable(int[] data) {
            List<int> result = new List<int>();

            int idx = 0;
            int i = data[idx];

            while(i >= 0) {
                result.Add(data[i]);
                idx++;
                i = data[idx];
            }

            return result.ToArray();
        }

        public static string L_DecodeTable(int[] data, string key) {
            StringBuilder chars = new StringBuilder();
            char decChar;

            for (int i = 0; i < data.Length; i++) {
                decChar = key[i % key.Length];
                chars.Append((char)L_XORDecrypt(data[i], (int)decChar));                
            }
            return chars.ToString();
        }
    }
}
