using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fingerprint
{
    public class TFIDF
    {
        internal static List<string> Tokenize(string doc)
        {
            doc = Regex.Replace(doc, "<[^<>]+>", ""); //strip all HTML
            doc = Regex.Replace(doc, "[0-9]+", "number"); //strip numbers
            doc = Regex.Replace(doc, @"(http|https)://[^\s]*", "httpaddr"); //strip urls
            doc = Regex.Replace(doc, @"[^\s]+@[^\s]+", "emailaddr"); //strip email
            doc = Regex.Replace(doc, "[$]+", "dollar"); //strip currency
            doc = Regex.Replace(doc, @"@[^\s]+", "username"); //strip username
            return doc.Split(" @$/#.-:&*+=[]?!(){},''\">_<;%\\".ToCharArray()).ToList(); //tokenize and split based on punctuation
        }

        internal static List<List<double>> CalculateTFIDF(List<List<string>> vocabsInDoc)
        {
            Dictionary<string, int> count = new Dictionary<string, int>();
            Dictionary<string, double> result = new Dictionary<string, double>();
            foreach (var vocabs in vocabsInDoc)
            {
                //calculate multiappearing vocabs
                foreach (var vocab in vocabs)
                {
                    if (count.ContainsKey(vocab))
                        count[vocab]++;
                    else
                        count[vocab] = 1;

                    //calculate IDF and put to result
                    double countInAllDoc = vocabsInDoc.Where(p => p.Contains(vocab)).Count();
                    result[vocab] = Math.Log((double)vocabsInDoc.Count / ((double)1 + countInAllDoc));
                }

               
            }
            //calculate transform
            return Normalize(Transform(vocabsInDoc, result));
        }

        internal static double EuclideanDist(List<double> vectorA, List<double> vectorB)
        {
            if (vectorA.Count != vectorB.Count)
                throw new Exception("Vectors must be in the same dimension!");

            double sum = vectorB.Zip(vectorA, (p, q) => Math.Pow((q - p), 2)).Sum();
            return Math.Sqrt(sum);
        }

        internal static List<List<double>> Transform(List<List<string>> vocabInDocs, Dictionary<string, double> idfVectors)
        {
            List<List<double>> result = new List<List<double>>();
            foreach (var vocab in vocabInDocs)
            {
                List<double> tr = new List<double>();
                foreach (var idf in idfVectors)
                {
                    double tf = vocab.Where(p => p.Equals(idf.Key)).Count();
                    double tfidf = tf * idf.Value;
                    tr.Add(tfidf);
                }
                result.Add(tr);
            }
            return result;
        }

        private static List<List<double>> Normalize(List<List<double>> vectors)
        {
            List<List<double>> result = new List<List<double>>();
            foreach (var vector in vectors)
            {
                result.Add(Normalize(vector));
            }
            return result;
        }

        private static List<double> Normalize(List<double> vector)
        {
            List<double> result = new List<double>();
            double sumSquared = vector.Sum(p => Math.Pow(p,2));
            double sqrtss = Math.Sqrt(sumSquared);
            foreach (var val in vector)
            {
                result.Add(val / sqrtss);
            }
            return result;
        }
    }
}
