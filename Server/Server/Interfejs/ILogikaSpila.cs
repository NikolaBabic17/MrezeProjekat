using Server.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interfejs
{
    internal interface ILogikaSpila
    {
        List<Karta> DodeliKarteIzSpila(List<Karta> spil, int brojIgraca);
        void VratiKarteNazad(List<Karta> KarteZaVracanje, List<Karta> spil);
    }
}
