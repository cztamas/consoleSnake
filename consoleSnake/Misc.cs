using System;

namespace consoleSnake
{

    public struct Pozíció
    {
        public int x;
        public int y;

        public Pozíció(int a, int b)
        {
            x = a;
            y = b;
        }
    }

    public enum Irány
    {
        Fel, Balra, Le, Jobbra
    }

    public static class IrányExtension
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

        public static Pozíció Elmozdít(this Pozíció pozi, Irány merre)
        {
            Pozíció újPoz = new Pozíció();
            újPoz.x = pozi.x + merre.GetX();
            újPoz.y = pozi.y + merre.GetY();
            return újPoz;
        }
    }


}