using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace snake
{
    class Program
    {
        //1, ha fal van a megfelelő mezőn
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

    struct Pozíció
    {
        public int x;
        public int y;

        public Pozíció(int a, int b)
        {
            x = a;
            y = b;
        }
    }

    enum Irány
    {
        Fel, Balra, Le, Jobbra
    }

    static class IrányExtension
    {
        public static Irány SzembeIrány(this Irány merre)
        {
            int n = ((int)merre + 2) % 4;
            return (Irány)n;
        }

        public static int GetX(this Irány merre)
        {
            if (merre == Irány.Fel || merre == Irány.Le)
                return 0;
            if (merre == Irány.Jobbra)
                return 1;
            if (merre == Irány.Balra)
                return -1;
            return 666;
        }

        public static int GetY(this Irány merre)
        {
            if (merre == Irány.Jobbra || merre == Irány.Balra)
                return 0;
            if (merre == Irány.Fel)
                return -1;
            if (merre == Irány.Le)
                return 1;
            return 666;
        }
    }

    class Kígyó
    {
        // a kígyó "testének" x illetve y koordinátái
        private int[] x;
        private int[] y;
        // pillanatnyi haladási irány
        private Irány merreMegy;

        public Irány MerreMegy
        {
            // csak balra vagy jobbra fordulhat, visszafelé nem
            set
            {
                if (value != merreMegy.SzembeIrány())
                    merreMegy = value;
            }
        }

        public Kígyó()
        {
            x = new int[8] { 5, 5, 5, 5, 5, 5, 5, 5 };
            y = new int[8] { 9, 8, 7, 6, 5, 4, 3, 2 };
            merreMegy = Irány.Jobbra;
        }


        /* akkor hívódik, ha a kígyó helye megváltozik
         * hogyan: true, ha új mezőt veszünk hozzá, false, ha benne lévőt törlünk */
        public delegate void KígyóVáltozásKezelő(Pozíció hol, bool hogyan);
        public delegate void KígyóHalálKezelő();

        public event KígyóVáltozásKezelő KígyóVáltozás;
        public event KígyóHalálKezelő Megdöglött;

        public void KígyóRajzoló()
        {
            int hossz = x.Length;
            for (int i = 0; i < hossz; i++)
            {
                Pozíció hol = new Pozíció(x[i], y[i]);
                KígyóVáltozás(hol, true);
            }
        }

        public void Megyeget(int wait)
        {
            Thread.Sleep(wait);
            Léptet();
        }

        public void Léptet()
        {
            int hossz = x.Length;

            int újX = x[0] + merreMegy.GetX();
            int újY = y[0] + merreMegy.GetY();
            int régiX = x[hossz - 1];
            int régiY = y[hossz - 1];

            Pozíció újPoz = new Pozíció(újX, újY);
            Pozíció régiPoz = new Pozíció(régiX, régiY);
            for (int i = hossz - 1; i >= 1; i--)
            {
                x[i] = x[i - 1];
                y[i] = y[i - 1];
            }
            x[0] = újX;
            y[0] = újY;

            if (ÜtközöttE() && Megdöglött != null)
            {
                Megdöglött();
            }

            if (KígyóVáltozás != null)
            {
                KígyóVáltozás(újPoz, true);
                KígyóVáltozás(régiPoz, false);
            }
        }

        public void Hosszabbít()
        { }

        private bool ÜtközöttE()
        {
            return false;
        }
    }
}
