using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprint
{
    public class Pearson
    {

        public double Calc(List<double> ls1, List<double> ls2)
        {
            if (ls1.Count == 0 || ls2.Count == 0) //check for empty list
                throw new Exception("One of the list is empty!");
         

            //length is adjusted in the same length
            if (ls1.Count != ls2.Count)
            {
                int min = Math.Min(ls1.Count, ls2.Count);
                ls1 = ls1.Take(min).ToList();
                ls2 = ls2.Take(min).ToList();
            }

            var av1 = ls1.Cast<double>().Average();
            var av2 = ls2.Cast<double>().Average();

            var allav = ls1.Cast<double>().Zip(ls2.Cast<double>(), (a, b) =>
                (a - av1) * (b - av2));
            var allsum = allav.Sum();
            var sum1 = ls1.Cast<double>().Sum(p => Math.Pow((p - av1), 2.0));
            var sum2 = ls2.Cast<double>().Sum(p => Math.Pow((p - av2), 2.0));

            return allsum / Math.Sqrt(sum1 * sum2);
        }
    }
}
