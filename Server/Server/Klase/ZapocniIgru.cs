using Server.Interfejs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Server.Klase
{
    internal class ZapocniIgru : IZapocniIgru
    {
        IKreirajProtivnike kreirajProtivnike;
        public ZapocniIgru(IKreirajProtivnike kreirajProtivnike)
        {
            this.kreirajProtivnike = kreirajProtivnike;
        }

        //Logika za zapocinjanje igre
        void IZapocniIgru.ZapocniIgru(int brojIgraca, List<Traka> trake, List<Protivnik> protivnici)
        {
            Random rand = new Random();
            List<int> brojevi=new List<int>();
            switch (trake.Count)
            {
                case 2:
                    //Logika za 1 igraca
                    Protivnik p1a = kreirajProtivnike.PretraziProtivnika("Goblin", protivnici);
                    Protivnik p2a = kreirajProtivnike.PretraziProtivnika("Trol", protivnici);
                    
                    brojevi = KreirajListuBrojeva(trake.Count);

                    int traka1a = rand.Next(brojevi.Count);
                    trake[brojevi[traka1a]].StrelacZona.Add(p1a);
                    brojevi.Remove(traka1a);

                    int traka2a = rand.Next(brojevi.Count);
                    trake[brojevi[traka2a]].StrelacZona.Add(p2a);
                    brojevi.Remove(traka2a);

                    break;
                case 4:
                    //logika za 2 igraca
                    Protivnik p1b = kreirajProtivnike.PretraziProtivnika("Goblin", protivnici);
                    Protivnik p2b = kreirajProtivnike.PretraziProtivnika("Ork", protivnici);
                    Protivnik p3b = kreirajProtivnike.PretraziProtivnika("Trol", protivnici);
                    Protivnik p4b = kreirajProtivnike.PretraziProtivnika("Trol", protivnici);

                    brojevi = KreirajListuBrojeva(trake.Count);

                    int traka1b = rand.Next(brojevi.Count);
                    trake[brojevi[traka1b]].StrelacZona.Add(p1b);
                    brojevi.Remove(traka1b);

                    int traka2b = rand.Next(brojevi.Count);
                    trake[traka2b].StrelacZona.Add(p2b);
                    brojevi.Remove(traka2b);

                    int traka3b = rand.Next(brojevi.Count);
                    trake[brojevi[traka3b]].StrelacZona.Add(p3b);
                    brojevi.Remove(traka3b);

                    int traka4b = rand.Next(brojevi.Count);
                    trake[brojevi[traka4b]].StrelacZona.Add(p4b);
                    brojevi.Remove(traka4b);


                    break;
                case 6:
                    //logika za 3 igraca
                    Protivnik p1c = kreirajProtivnike.PretraziProtivnika("Goblin", protivnici);
                    Protivnik p2c = kreirajProtivnike.PretraziProtivnika("Ork", protivnici);
                    Protivnik p3c = kreirajProtivnike.PretraziProtivnika("Trol", protivnici);
                    Protivnik p4c = kreirajProtivnike.PretraziProtivnika("Trol", protivnici);
                    
                    brojevi = KreirajListuBrojeva(trake.Count);

                    int traka1c = rand.Next(brojevi.Count);
                    trake[brojevi[traka1c]].StrelacZona.Add(p1c);
                    brojevi.Remove(traka1c);

                    int traka2c = rand.Next(brojevi.Count);
                    trake[brojevi[traka2c]].StrelacZona.Add(p2c);
                    brojevi.Remove(traka2c);

                    int traka3c = rand.Next(brojevi.Count);
                    trake[brojevi[traka3c]].StrelacZona.Add(p3c);
                    brojevi.Remove(traka3c);

                    int traka4c = rand.Next(brojevi.Count);
                    trake[brojevi[traka4c]].StrelacZona.Add(p4c);
                    brojevi.Remove(traka4c);

                    break;
                default:
                    throw new Exception();
            }
        }

        public List<int> KreirajListuBrojeva(int brojTraka)
        {
            List<int>trake = new List<int>();
            for (int i = 0; i<brojTraka; i++)
            {
                trake.Add(i);
            }
            return trake;
        }
    }
}
