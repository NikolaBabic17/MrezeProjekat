using Server.Interfejs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum BojaKarte
{
    Crvena,
    Plava,
    Zelena,
    Ljubicasta
}

namespace Server.Klase
{
    internal class Karta
    {
        public string Naziv;
        public String Efekat;
        public BojaKarte Boja;

        public Karta(string naziv, string efekat, BojaKarte boja)
        {
            Naziv = naziv;
            Efekat = efekat;
            Boja = boja;
        }
    }
}
