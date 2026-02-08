using Server.Klase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interfejs
{
    public interface ISpamovanjeProtivnika
    {
        void SpamujProtivnike(List<Traka> trake, List<Protivnik> protivnici, int brojPoteza);
        void SpecialniProtivnici(List<Traka> trake);
    }
}
