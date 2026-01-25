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
        INapraviMapu napraviMapu;
        IKreirajProtivnike kreirajProtivnike;
        public ZapocniIgru(INapraviMapu kreirajMapu, IKreirajProtivnike kreirajProtivnike)
        {
            this.napraviMapu = kreirajMapu;
            this.kreirajProtivnike = kreirajProtivnike;
        }

        //Logika za zapocinjanje igre
        void IZapocniIgru.ZapocniIgru(int brojIgraca)
        {
            List<Traka> trake = napraviMapu.kreirajMapu(brojIgraca);
            List<Protivnik> protivnici = kreirajProtivnike.KreirajProtivnike();
            Random rand = new Random();
            switch (trake.Count)
            {
                case 1:
                    //Logika za 1 igraca
                    int traka1a = rand.Next(trake.Count);
                    int traka2a = rand.Next(trake.Count);

                    Protivnik p1a = kreirajProtivnike.PretraziProtivnika("Goblin", protivnici);
                    Protivnik p2a = kreirajProtivnike.PretraziProtivnika("Trol", protivnici);

                    trake[traka1a].StrelacZona.Add(p1a);
                    trake[traka2a].StrelacZona.Add(p2a);
                    break;
                case 2:
                    //logika za 2 igraca
                    int traka1b = rand.Next(trake.Count);
                    int traka2b = rand.Next(trake.Count);
                    int traka3b = rand.Next(trake.Count);
                    int traka4b = rand.Next(trake.Count);

                    Protivnik p1b = kreirajProtivnike.PretraziProtivnika("Goblin", protivnici);
                    Protivnik p2b = kreirajProtivnike.PretraziProtivnika("Ork", protivnici);
                    Protivnik p3b = kreirajProtivnike.PretraziProtivnika("Trol", protivnici);
                    Protivnik p4b = kreirajProtivnike.PretraziProtivnika("Trol", protivnici);

                    trake[traka1b].StrelacZona.Add(p1b);
                    trake[traka2b].StrelacZona.Add(p2b);
                    trake[traka3b].StrelacZona.Add(p3b);
                    trake[traka4b].StrelacZona.Add(p4b);

                    break;
                case 3:
                    //logika za 3 igraca
                    int traka1c = rand.Next(trake.Count);
                    int traka2c = rand.Next(trake.Count);
                    int traka3c = rand.Next(trake.Count);
                    int traka4c = rand.Next(trake.Count);

                    Protivnik p1c = kreirajProtivnike.PretraziProtivnika("Goblin", protivnici);
                    Protivnik p2c = kreirajProtivnike.PretraziProtivnika("Ork", protivnici);
                    Protivnik p3c = kreirajProtivnike.PretraziProtivnika("Trol", protivnici);
                    Protivnik p4c = kreirajProtivnike.PretraziProtivnika("Trol", protivnici);

                    trake[traka1c].StrelacZona.Add(p1c);
                    trake[traka2c].StrelacZona.Add(p2c);
                    trake[traka3c].StrelacZona.Add(p3c);
                    trake[traka4c].StrelacZona.Add(p4c);
                    break;
                default:
                    throw new Exception();
            }
        }
    }
}
