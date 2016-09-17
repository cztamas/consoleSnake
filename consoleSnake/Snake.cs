using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace consoleSnake
{
    public class Kígyó
    {
        // a kígyó "teste"
        private Pozíció[] holVan;
        private Pozíció fej;
        [Obsolete]
        private int[] x;
        [Obsolete]
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

        public Kígyó(int[][] holVan)
        { }


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

