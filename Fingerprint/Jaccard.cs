using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprint
{
    public class Jaccard
    {
        public static double Calc<T>(HashSet<T> hs1, HashSet<T> hs2) where T: struct
        {
            return ((double)hs1.Intersect(hs2).Count() / (double)hs1.Union(hs2).Count());
        }

        public static double Calc<T>(List<T> ls1, List<T> ls2) where T : struct
        {
            HashSet<T> hs1 = new HashSet<T>(ls1);
            HashSet<T> hs2 = new HashSet<T>(ls2);
            return Calc<T>(hs1, hs2);
        }
    }
}
