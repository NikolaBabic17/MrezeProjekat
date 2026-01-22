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
        int BrojIgraca;
        Boja BojaTrake;
        List<Protivnik> Protivnici;
        List<Protivnik> StrelacZona;
        List<Protivnik> VitezZona;
        List<Protivnik> MacevalacZona;
        int BrojZidinaZamka;

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
