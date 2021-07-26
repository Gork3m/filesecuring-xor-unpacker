using System;
using System.Collections.Generic;
using System.Text;

namespace XOR_Unpacker {
    public static class Debug {

        public static void Log(string msg, ConsoleColor color = ConsoleColor.Gray) {
            ConsoleColor old = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = old;

        }


    }
}
