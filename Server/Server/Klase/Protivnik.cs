using Server.Interfejs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Klase
{
    internal class Protivnik : IKreirajProtivnike
    {
        public string Ime { get; set; }
        public int Poeni { get; set; }
        public Protivnik(string ime, int poeni)
        {
            Ime = ime;
            Poeni = poeni;
        }

        public List<Protivnik> KreirajProtivnike()
        {
            List<Protivnik> protivnici = new List<Protivnik>();
            for (int i = 0; i < 12; i++)
            {
                protivnici.Add(new Protivnik("Goblin", 1));
            }

            for (int i = 0; i < 11; i++)
            {
                protivnici.Add(new Protivnik("Ork", 2));
            }

            for (int i = 0; i < 8; i++)
            {
                protivnici.Add(new Protivnik("Trol", 3));
            }
            for (int i = 0; i < 4; i++)
            {
                protivnici.Add(new Protivnik("Veliki kamen", 0));
            }
            for (int i = 0; i < 2; i++)
            {
                protivnici.Add(new Protivnik("Kuga", 0));
            }
            protivnici.Add(new Protivnik("Prelazi u prethodnu traku", 0));
            protivnici.Add(new Protivnik("Postavi jos 3 protivnika", 0));
            for (int i = 0; i < 2; i++)
            {
                protivnici.Add(new Protivnik("Plavi, Zeleni ili Crveni se pomeraju 1 polje unazad", 0));
            }

            return protivnici;
        }

        public void IzbrisiProtivnika(Protivnik protivnik, List<Protivnik> protivnici)
        {
            protivnici.Remove(protivnik);
        }

        public Protivnik PretraziProtivnika(string imeProtivnika, List<Protivnik> protivnici)
        {
            foreach (var protivnik in protivnici)
            {
                if (protivnik.Ime == imeProtivnika)
                {
                    IzbrisiProtivnika(protivnik, protivnici);
                    return protivnik;
                }
            }
            return null;
        }
    }
}
