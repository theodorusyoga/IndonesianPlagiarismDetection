using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace Fingerprint
{
    class Program
    {
        //GLOBALS
        private static string path = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace(@"\Fingerprint.exe", "");
        private static MinHash minHash;
        private static Wordnet wn = new Wordnet();
        private static Stemmer stemmer = new Stemmer(wn.GetSemanticWord().Select(p => p.Item2).ToList());
        private static PDFParser parser = new PDFParser();
        private static List<string> lw1;
        private static List<string> lw2;
        private static List<int> sr1;
        private static List<int> sr2;
        private static List<uint> hash1;
        private static List<uint> hash2;

        private static List<string> vl1;
        private static List<string> vl2;

        static void Main(string[] args)
        {
            //coba
            List<double> list1 = new List<double>() { 3, 6, 0, 11, 0, 6 };
            List<double> list2 = new List<double>() { 3, 6, 0, 0, 0, 6 };
            Pearson pr = new Pearson();
            var res = pr.Calc(list1, list2);
            vl1 = new List<string>();
            vl2 = new List<string>();

            //get all inputs
            var files = Directory.GetFiles(path + @"\Datasets\");

            foreach (var file in files)
            {
                var filename = Path.GetFileName(file);
                parser.ExtractText(file, path + @"\DatasetsOut\" + filename.Replace(".pdf", ".txt").Replace(".PDF", ".txt"));
            }
            Console.Clear();

            var outfiles = Directory.GetFiles(path + @"\DatasetsOut\");
            int index = 0;
            lw1 = TFIDF.Tokenize(ReadFile(outfiles[0]).Replace("\r", string.Empty).Replace("\n", string.Empty)); //list words in 1st document

            Dictionary<int, string> res1 = new Dictionary<int, string>();
            sr1 = wn.GetSemanticRelation(lw1, 1, out vl1, out res1); //get semantic relation for 1st document
            int docno = 1;

            //START GENERATE FROM DOCUMENT
            foreach (var outfile in outfiles)
            {
                lw2 = TFIDF.Tokenize(ReadFile(outfile).Replace("\r", string.Empty).Replace("\n", string.Empty)); //list words in 2nd document

                Console.SetCursorPosition(0, 9); //begin from row 3, row 1 is for status
                Console.WriteLine("---------------------------------------");

                Dictionary<int, string> res2 = new Dictionary<int, string>();
                sr2 = wn.GetSemanticRelation(lw2, docno, outfiles.Count(), out vl2, out res2, stemmer); //get semantic relation for 2nd document
                docno++;
                //PrintSimilarWords(res1, res2);
                ExportExcel(index);
                index++;
            }

            //END GENERATE FROM DOCUMENTS

            //using (PlagarismDbEntities dbContent = new PlagarismDbEntities())
            //{
            //    var docs = dbContent.Documents;
            //    var firstdoc = docs.First();
            //    lw1 = TFIDF.Tokenize(ReadFile(path + @"\DatasetsOut\" + firstdoc.docname + ".txt"));
            //    sr1 = dbContent.Calculations.Where(p => p.docid == firstdoc.id).Select(p => p.value).ToList();
            //    foreach (var doc in docs)
            //    {
            //        var calcs = dbContent.Calculations.Where(p => p.docid == doc.id);
            //        if (calcs.Count() != 0)
            //        {
            //            lw2 = TFIDF.Tokenize(ReadFile(path + @"\DatasetsOut\" + doc.docname + ".txt"));
            //            sr2 = calcs.Select(p => p.value).ToList();
            //            ExportExcel(index);
            //        }
            //        index++;
            //    }
            //    Console.WriteLine("------------------FINISHED------------------");
            //}

            Console.ReadLine();
        }

        private static void PrintSimilarWords(Dictionary<int, string> res1, Dictionary<int, string> res2)
        {
            Console.WriteLine("-------------CHANGED WORDS--------------\n");
            List<string> printedIds = new List<string>();
            foreach (var item in res1)
            {
                if (res2.Where(p => p.Key == item.Key && p.Value != item.Value).Count() > 0 &&
                    !printedIds.Contains(item.Value))
                {
                    Console.Write(res1.Where(p => p.Key == item.Key).First());
                    Console.Write("\t");
                    var ress2 = res2.Where(p => p.Key == item.Key);
                    foreach (var res in ress2)
                    {
                        Console.Write(res);
                        Console.Write("\t");
                    }
                    Console.WriteLine();
                    printedIds.Add(item.Value);
                }
            }

            foreach (var item in res2)
            {
                if (res1.Where(p => p.Key == item.Key && p.Value != item.Value).Count() > 0 &&
                    !printedIds.Contains(item.Value))
                {
                    Console.Write(res2.Where(p => p.Key == item.Key).First());
                    Console.Write("\t");
                    var ress2 = res1.Where(p => p.Key == item.Key);
                    foreach (var res in ress2)
                    {
                        Console.Write(res);
                        Console.Write("\t");
                    }
                    Console.WriteLine();
                    printedIds.Add(item.Value);
                }
            }
        }

        private static void ExportExcel(int currentindex)
        {
            Pearson pr = new Pearson();
            int hashno = 100;
            using (ExcelPackage ep = new ExcelPackage())
            {
                ep.Workbook.Properties.Title = "FINGERPRINT ANALYSIS";
                ep.Workbook.Properties.Author = "THEODORUS YOGA M";

                var ws = ep.Workbook.Worksheets.Add("Fingerprint Result");
                ws.Cells[1, 1].Value = "Hash No.";
                ws.Cells[1, 2].Value = "Jaccard";
                ws.Cells[1, 3].Value = "Pearson";
                ws.Cells[1, 4].Value = "Euclidean";
                int rowstart = 2;
                List<double> jaccard = new List<double>();
                List<double> jaccardMin = new List<double>();
                List<double> pearson = new List<double>();
                List<double> euclidean = new List<double>();
                while (hashno <= 5000)
                {


                    //Console.WriteLine("Processing " + hashno + " hash functions");
                    MinHash mh = new MinHash(Math.Min(lw1.Count, lw2.Count), hashno);

                    //List<int> example1 = new List<int>() { 21, 50, 67, 101, 108 };
                    //List<int> example2 = new List<int>() { 23, 52, 90, 102, 110 };

                    hash1 = mh.GetMinHash(sr1); //get minimal hashing for 1st semantic relation
                    hash2 = mh.GetMinHash(sr2); //get minimal hashing for 2nd semantic relation

                    //hash1 = mh.GetMinHash(example1);
                    //hash2 = mh.GetMinHash(example2);

                    //convert to double for pearson
                    List<double> hashdb1 = new List<double>();
                    List<double> hashdb2 = new List<double>();
                    int count1 = hash1.Count;
                    int current = 1;
                    foreach (uint hash in hash1)
                    {
                        hashdb1.Add(Convert.ToDouble(hash));
                        //Console.SetCursorPosition(0, Wordnet.consoleln);
                        //Console.WriteLine("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t");
                        //Console.SetCursorPosition(0, Wordnet.consoleln);
                        //Console.WriteLine("STATUS: Processing hash functions in document 1 of 2: " + current + "/" + count1);
                        current++;
                    }
                    Wordnet.consoleln++;
                    int count2 = hash2.Count;
                    current = 1;
                    foreach (uint hash in hash2)
                    {
                        hashdb2.Add(Convert.ToDouble(hash));
                        //Console.SetCursorPosition(0, Wordnet.consoleln);
                        //Console.WriteLine("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t");
                        //Console.SetCursorPosition(0, Wordnet.consoleln);
                        //Console.WriteLine("STATUS: Processing hash functions in document 2 of 2: " + current + "/" + count2);
                        current++;
                    }
                    Wordnet.consoleln++;

                    //calculate TF-IDF
                    List<List<string>> vocabsInDoc = new List<List<string>>();
                    vocabsInDoc.Add(vl1);
                    vocabsInDoc.Add(vl2);
                    var tfidf = TFIDF.CalculateTFIDF(vocabsInDoc);

                    Wordnet.consoleln++;
                    //Console.WriteLine("JACCARD SIMILARITY DEGREE: " + mh.Similarity(hash1, hash2));
                    //Console.WriteLine("PEARSON SIMILARITY DEGREE: " + pr.Calc(hashdb1, hashdb2));
                    //Console.WriteLine("TF-IDF EUCLIDEAN DISTANCE:" + TFIDF.EuclideanDist(tfidf[0], tfidf[1]));
                    Console.WriteLine(hashno + "\t,\t" + ((double)mh.Similarity(hash1, hash2)) + "\t,\t" +
                        pr.Calc(hashdb1, hashdb2) + "\t,\t" + TFIDF.EuclideanDist(tfidf[0], tfidf[1]));
                    ws.Cells[rowstart, 1].Value = hashno.ToString();
                    var jacc = ((double)mh.Similarity(hash1, hash2));
                    var jaccmin = (double)(jacc - 1);
                    var pear = pr.Calc(hashdb1, hashdb2);
                    var euc = TFIDF.EuclideanDist(tfidf[0], tfidf[1]);
                    ws.Cells[rowstart, 2].Value = jacc;
                    jaccard.Add(jacc);
                    pearson.Add(pear);
                    euclidean.Add(euc);
                    jaccardMin.Add(Math.Abs(jaccmin));
                    ws.Cells[rowstart, 3].Value = pear;
                    ws.Cells[rowstart, 4].Value = TFIDF.EuclideanDist(tfidf[0], tfidf[1]);
                    //foreach (var vector in tfidf)
                    //{
                    //    foreach (var val in vector)
                    //    {
                    //        Console.Write(val + ";");
                    //    }
                    //    Console.WriteLine();
                    //}
                    hashno += 50;
                    rowstart++;
                }
                FileStream file;
                if (!Directory.Exists("Out"))
                    Directory.CreateDirectory("Out");
                if (!File.Exists("Out/naziefresult_" + currentindex.ToString() + ".xlsx"))
                {
                    FileStream fs = File.Create("Out/naziefresult_" + currentindex.ToString() + ".xlsx");
                    fs.Dispose();
                }
                file = File.Open("Out/naziefresult_" + currentindex.ToString() + ".xlsx", FileMode.Open);

                string append = string.Empty;

                append += "JaccardMin<-c(";
                foreach (var item in jaccardMin)
                {
                    append += item.ToString().Replace(',', '.');
                    append += ", ";
                }
                append += ")";
                append += "\n";
                append += "Jaccard<-c(";
                foreach (var item in jaccard)
                {
                    append += item.ToString().Replace(',', '.');
                    append += ", ";
                }
                append += ")";
                append += "\n";
                append += "Pearson<-c(";
                foreach (var item in pearson)
                {
                    append += item.ToString().Replace(',', '.');
                    append += ", ";
                }
                append += ")";
                append += "\n";
                append += "Euclidean<-c(";
                foreach (var item in euclidean)
                {
                    append += item.ToString().Replace(',', '.');
                    append += ", ";
                }
                append += ")";

                //Console.WriteLine("c(");
                //foreach (var item in res)
                //{
                //    Console.Write(item.ToString().Replace(',', '.'));
                //    Console.Write(", ");
                //}
                //Console.Write(")");

                ep.SaveAs(file);
                WriteFile(path + @"\Out\naziefr_" + currentindex.ToString() + ".txt", append);
                file.Close();
            }
        }

        private static void WriteFile(string filename, string content)
        {
            using (StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8))
            {
                sw.Write(content);
            }
        }

        private static string ReadFile(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                return sr.ReadToEnd().ToLower();
            }
        }

        private static List<string> SeparateWords(string file)
        {
            using (StreamReader sr = new StreamReader(file))
            {
                string doc = sr.ReadToEnd().ToLower(); //decapitalized characters
                if (doc.Length > 10) //more than one word
                {
                    List<string> result = new List<string>();
                    //separate words and phrases
                    foreach (var sw in Wordnet.punctuation)
                    {
                        doc = doc.Replace(sw.ToString(), "");
                    }
                    string[] words = doc.Split(' ');
                    if (words.Length > 0)
                    {
                        foreach (var word in words)
                        {
                            result.Add(word);
                        }
                        result.Distinct(); //remove duplicated words or phrases
                        return result.Where(p => p.Length > 2).ToList(); //eliminate single character
                    }
                    else return new List<string>();
                }
                else
                    return new List<string>();
            }
        }

        private static double CalculateSimilarity(List<int> wordId1, List<int> wordId2)
        {
            if (lw1 != null && lw2 != null)
            {
                int universeSize = (lw1.Count + lw2.Count) / 2;
                int numHashFunctions = 100;
                using (minHash = new MinHash(universeSize, numHashFunctions))
                {
                    List<uint> mh1 = minHash.GetMinHash(wordId1);
                    List<uint> mh2 = minHash.GetMinHash(wordId2);
                    return minHash.Similarity(mh1, mh2);
                }
            }
            else throw new NullReferenceException("Fill the word lists first before continuing to proceed similarity calculation.");
        }

        private static void PrintAllWords()
        {
            //print words
            //Console.WriteLine("WORDS/PHRASES IN DOCUMENT 1:");
            //foreach (var word in lw1)
            //{
            //    Console.Write(word);
            //    Console.Write("; ");
            //}
            //Console.WriteLine();
            //Console.WriteLine("COUNT IN DOCUMENT 1: " + lw1.Count + " WORDS");
            //Console.WriteLine("\n");
            //Console.WriteLine("WORDS/PHRASES IN DOCUMENT 2:");
            //foreach (var word in lw2)
            //{
            //    Console.Write(word);
            //    Console.Write("; ");
            //}
            //Console.WriteLine();
            //Console.WriteLine("COUNT IN DOCUMENT 2: " + lw2.Count + " WORDS");
        }
    }
}
