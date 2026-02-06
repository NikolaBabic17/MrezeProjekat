using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Klase
{
    public class PrebaciProtivnike : Interfejs.IPrebaciProtivnike
    {
        void Interfejs.IPrebaciProtivnike.PrebaciProtivnike(List<Traka> trake)
        {
            List<Protivnik> protivnici = new List<Protivnik>();
            foreach (Traka traka in trake)
            {
                if (traka.BrojZidinaZamka <= 0)
                {
                    protivnici.AddRange(traka.MacevalacZona);
                    traka.MacevalacZona.Clear();
                    
                    if (traka.SumaZona.Count == 0 && traka.StrelacZona.Count == 0 && 
                        traka.VitezZona.Count == 0 && traka.MacevalacZona.Count == 0)
                    {
                        trake.Remove(traka);
                    }
                }
            }
            if (protivnici.Count > 0)
            {
                Random random = new Random();
                foreach (Protivnik protivnik in protivnici)
                {
                    int indexTrake = random.Next(trake.Count);
                    trake[indexTrake].SumaZona.Add(protivnik);
                }
            }
        }
    }
}
