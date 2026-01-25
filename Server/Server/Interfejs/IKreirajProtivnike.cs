using Server.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interfejs
{
    internal interface IKreirajProtivnike
    {
        List<Protivnik> KreirajProtivnike();
        void IzbrisiProtivnika(Protivnik protivnik, List<Protivnik> protivnici);
        Protivnik PretraziProtivnika(string imeProtivnika, List<Protivnik> protivnici);
    }
}
