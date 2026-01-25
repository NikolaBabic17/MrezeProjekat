using Server.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interfejs
{
    internal interface ISpil
    {
        List<Karta> KreirajSpil(int brojIgraca);
        List<Karta> promesajSpil(List<Karta> spil);

    }
}
