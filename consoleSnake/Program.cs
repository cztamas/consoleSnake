using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace consoleSnake
{
    class Program
    {
        /* 0, ha üres mező
         * 1, ha fal van ott
         * 2, ha "étel"
         * 3(, 4, ...) a kígyó (vagy kígyók) */
        private static int[,] pálya;
        private static int sorok = 25;
        private static int oszlopok = 30;
        private static Kígyó snake;
        private static bool ittAVége = false;
        private static int wait = 150;

        static void Main(string[] args)
        {            
            PályaKészítő();
            PályaFrissítő();
            snake = new Kígyó();
            snake.KígyóVáltozás += MegyAKígyó;
            snake.Megdöglött += Ütközött;
            snake.KígyóRajzoló();

            ThreadStart kígyóStart = new ThreadStart(MenjenAKígyó);
            Thread kígyóSzál = new Thread(kígyóStart);
            kígyóSzál.Start();
            MenjenAJáték();
        }

        private static void MenjenAKígyó()
        {
            while (!ittAVége)
            {
                snake.Megyeget(wait);
            }
        }

        private static void MenjenAJáték()
        {
            while (!ittAVége)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(false);
                ConsoleKey gomb = keyInfo.Key;
                switch (gomb)
                {
                    case ConsoleKey.UpArrow:
                        snake.MerreMegy = Irány.Fel;
                        break;
                    case ConsoleKey.DownArrow:
                        snake.MerreMegy = Irány.Le;
                        break;
                    case ConsoleKey.LeftArrow:
                        snake.MerreMegy = Irány.Balra;
                        break;
                    case ConsoleKey.RightArrow:
                        snake.MerreMegy = Irány.Jobbra;
                        break;
                    case ConsoleKey.Escape:
                        ittAVége = true;
                        break;
                }


            }
        }

        private static Kígyó.KígyóVáltozásKezelő MegyAKígyó = new Kígyó.KígyóVáltozásKezelő(KígyóFrissítő);
        private static Kígyó.KígyóHalálKezelő Ütközött = new Kígyó.KígyóHalálKezelő(Vége);

        private static void Vége()
        {
            ittAVége = true;
            Console.CursorTop = 5;
            Console.CursorLeft = 5;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("GAME OVER");
        }

        //Inicializálja a pálya tömböt
        private static void PályaKészítő()
        {
            pálya = new int[sorok, oszlopok];
            for (int i = 0; i < sorok; i++)
            {
                pálya[i, 0] = 1;
                pálya[i, oszlopok - 1] = 1;
            }
            for (int i = 0; i < oszlopok; i++)
            {
                pálya[0, i] = 1;
                pálya[sorok - 1, i] = 1;
            }
        }

        private static void PályaFrissítő()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            for (int i = 0; i < sorok; i++)
            {
                for (int j = 0; j < oszlopok; j++)
                {
                    if (pálya[i, j] == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.CursorTop = i;
                        Console.CursorLeft = j;
                        Console.Write(" ");
                    }
                    if (pálya[i, j] == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.CursorTop = i;
                        Console.CursorLeft = j;
                        Console.Write("|");
                    }
                }
            }
        }

        private static void KígyóFrissítő(Pozíció hol, bool hogyan)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.CursorTop = hol.y;
            Console.CursorLeft = hol.x;
            if (hogyan)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("×");
                if (FalnakMentE(hol))
                    Vége();
                return;
            }
            if (!hogyan)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.Write(" ");
            }
            Console.BackgroundColor = ConsoleColor.White;
        }

        private static bool PályánvanE(Pozíció hol)
        {
            if (hol.y < 0 || hol.y >= sorok)
                return false;
            if (hol.x < 0 || hol.x >= oszlopok)
                return false;
            return true;
        }

        private static bool FalnakMentE(Pozíció hol)
        {
            if (!PályánvanE(hol))
                return true;
            if (pálya[hol.y, hol.x] == 1)
                return true;
            return false;
        }
    }    
}
