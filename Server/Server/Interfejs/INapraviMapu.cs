using Server.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interfejs
{
    internal interface INapraviMapu
    {
        List<Traka> kreirajMapu(int brojIgraca);
    }
}
