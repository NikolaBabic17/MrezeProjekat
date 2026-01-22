using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Server.Klase
{
    internal class Karta
    {
        string Naziv;
        String Efekat;

        public Karta(string naziv, string efekat)
        {
            Naziv = naziv;
            Efekat = efekat;
        }

        List<Karta> KreirajSpil()
        {
            List<Karta> Spil = new List<Karta>();
            Spil.Add(new Karta("Strelac", "Udara jednog protivnika u strelac zoni"));
            Spil.Add(new Karta("Vitez", "Udara jednog protivnika u vitez zoni"));
            Spil.Add(new Karta("Macevalac", "Udara jednog protivnika u macevalac zoni"));
            Spil.Add(new Karta("Heroj", "Udara jednog protivnika u bilokojoj zoni"));
            Spil.Add(new Karta("Varvarin", "Eliminise jednog protivnika u bilokojoj zoni"));
            Spil.Add(new Karta("Vracanje nazad!", "Vraca jednog protivnika nazad u sumu"));
            Spil.Add(new Karta("Katran", "Jedan protivnik se ne pomera na praju poteza"));
            Spil.Add(new Karta("Pojacanje zida", "Pojacava zidine"));

            return Spil;
        }
    }
}
