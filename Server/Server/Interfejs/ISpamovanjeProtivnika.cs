using Server.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interfejs
{
    internal interface ISpamovanjeProtivnika
    {
        void SpamovanjeProtivnika(List<Traka> trake, List<Protivnik> protivnici, int brojPoteza);
    }
}
