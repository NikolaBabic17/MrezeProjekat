using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum BojaKarte
{
    Crvena,
    Plava,
    Zelena,
    Ljubicasta
}

namespace Server.Klase
{
    public class Karta
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

        // TODO: Mozda treba biti funkcija van Karta klase
        //       da ne bi imali kartaHelper?
        public List<Karta> KreirajSpil(int brojIgraca)
        {
            List<Karta> Spil = new List<Karta>();
            
            Random rand = new Random();

            for (int i = 0; i < 3; i++)
            {
                Spil.Add(new Karta("Strelac", "Udara jednog protivnika u strelac zoni",IzaberiBoju(brojIgraca,rand)));
            }

            for (int i = 0; i < 3; i++)
            {
                Spil.Add(new Karta("Vitaz", "Udara jednog protivnika u vitez zoni", IzaberiBoju(brojIgraca, rand)));
            }

            for (int i = 0; i < 3; i++)
            {
                Spil.Add(new Karta("Mazevalac", "Udara jednog protivnika u macevalac zoni", IzaberiBoju(brojIgraca, rand)));
            }
            
            Spil.Add(new Karta("Heroj", "Udara jednog protivnika u bilokojoj zoni", IzaberiBoju(brojIgraca, rand)));
            Spil.Add(new Karta("Varvarin", "Eliminise protivnika u bilokojoj zoni", BojaKarte.Ljubicasta));
            Spil.Add(new Karta("Strelac", "Udara jednog protivnika u strelac zoni", BojaKarte.Ljubicasta));
            Spil.Add(new Karta("Vitez", "Udara jednog protivnika u vitez zoni", BojaKarte.Ljubicasta));
            Spil.Add(new Karta("Mazevalac", "Udara jednog protivnika u macevalac zoni", BojaKarte.Ljubicasta));
            Spil.Add(new Karta("Vracanje nazad", "Vraca jednog protivnika nazad u sumu", BojaKarte.Ljubicasta));
            Spil.Add(new Karta("Katran", "Jedan protivnik se ne pomera na kraju poteza", BojaKarte.Ljubicasta));

            for (int i = 0; i < 3; i++)
            {
                Spil.Add(new Karta("Pojacanje zida", "Pojacava zidine u zavisnosti od broja igraca", BojaKarte.Ljubicasta));
            }

            Spil = promesajSpil(Spil);

            return Spil;
        }

        BojaKarte IzaberiBoju(int brojIgraca, Random rand)
        {
            if (brojIgraca == 1)
                return BojaKarte.Crvena;

            if (brojIgraca == 2)
                return rand.Next(0, 2) == 0 ? BojaKarte.Crvena : BojaKarte.Zelena;

            int izbor = rand.Next(0, 3);
            return (BojaKarte)izbor;
        }

        public List<Karta> promesajSpil(List<Karta> spil)
        {
            Random rand = new Random();
            int n = spil.Count;
            for (int i = 0; i < n; i++)
            {
                int r = i + rand.Next(n - i);
                Karta temp = spil[r];
                spil[r] = spil[i];
                spil[i] = temp;
            }
            return spil;
        }
    }
}
