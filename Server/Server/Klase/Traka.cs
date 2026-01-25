using Server.Interfejs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Boja
{
    Crvena,
    Plava,
    Zelena
}

namespace Server.Klase
{
    internal class Traka
    {
        public int BrojIgraca;
        public Boja BojaTrake;
        public List<Protivnik> Protivnici;
        public List<Protivnik> StrelacZona;
        public List<Protivnik> VitezZona;
        public List<Protivnik> MacevalacZona;
        public int BrojZidinaZamka;

        public Traka(int brojIgraca, Boja bojaTrake)
        {
            BrojIgraca = brojIgraca;
            BojaTrake = bojaTrake;
            Protivnici = new List<Protivnik>();
            StrelacZona = new List<Protivnik>();
            VitezZona = new List<Protivnik>();
            MacevalacZona = new List<Protivnik>();
            BrojZidinaZamka = 2;
        }
    }
}
