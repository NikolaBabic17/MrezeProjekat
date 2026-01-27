using Server.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interfejs
{
    internal interface IZapocniIgru
    {
        void ZapocniIgru(int brojIgraca, List<Traka> trake, List<Protivnik> protivnici);
    }
}
