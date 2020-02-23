using System;

namespace OnlineQualificationRound
{
    public static class Utils
    {
        public static string GetFormated(int number)
        {
            return String.Format("{0:n0}", number);
        }
        
        public static string GetFormatedWithDecimals(int number)
        {
            return String.Format("{0:n}", number);
        }

        public static void WriteLine(string str)
        {
            ClearCurrentConsoleLine();
            Console.WriteLine(str);
        }
        
        public static void Write(string str)
        {
            ClearCurrentConsoleLine();
            Console.Write(str);
        }
        
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth)); 
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public static void GoBackOneLine()
        {
            GoBackNLines(1);
        }

        public static void GoBackNLines(int i)
        {
            Console.SetCursorPosition(0, Console.CursorTop - i);
        }


    }
}