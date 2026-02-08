using Server.Interfejs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Klase
{
    public class LogikaSpila : Interfejs.ILogikaSpila
    {
        public List<Karta> DopuniKarte(List<Karta> spil, int brojPotrebnihK)
        {
            List<Karta> dopunjeneKarte = new List<Karta>();
            for (int i = 0; i < brojPotrebnihK; i++)
            {
                if (spil.Count == 0)
                    break;
                dopunjeneKarte.Add(spil[0]);
                spil.RemoveAt(0);
            }
            return dopunjeneKarte;
        }

        List<Karta> ILogikaSpila.DodeliKarteIzSpila(List<Karta> spil, int brojIgraca)
        {
            List<Karta> dodeljeneKarte = new List<Karta>();
            int brojKartiZaDodeliti = brojIgraca == 1 ? 6 : 5;
            for (int i = 0; i < brojKartiZaDodeliti; i++)
            {
                if (spil.Count == 0)
                    break;
                dodeljeneKarte.Add(spil[0]);
                spil.RemoveAt(0);
            }
            return dodeljeneKarte;
        }

        void ILogikaSpila.VratiKarteNazad(List<Karta> KarteZaVracanje, List<Karta> spil)
        {
            spil.AddRange(KarteZaVracanje);
        }
    }
}
