using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprint
{
    public class Wordnet
    {
        public static int consoleln;
        //indonesian stop words
        //reference: http://web.archive.org/web/20100104090147/http://fpmipa.upi.edu/staff/yudi/stop_words_list.txt
        public
             static List<string> stopWords
        {
            get
            {
                return ("yang di dan itu dengan untuk tidak ini dari dalam akan pada juga saya ke karena tersebut bisa ada mereka lebih "
           + "kata tahun sudah atau saat oleh menjadi orang ia telah adalah seperti sebagai bahwa dapat para harus namun kita dua "
           + "satu masih hari hanya mengatakan kepada kami setelah melakukan lalu belum lain dia kalau terjadi banyak menurut anda "
           + "hingga tak baru beberapa ketika saja jalan sekitar secara dilakukan sementara tapi sangat hal sehingga seorang bagi besar "
           + "lagi selama antara waktu sebuah jika sampai jadi terhadap tiga serta pun salah merupakan atas sejak membuat baik memiliki "
           + "kembali selain tetapi pertama kedua memang pernah apa mulai sama tentang bukan agar semua sedang kali kemudian hasil sejumlah juta "
           + "persen sendiri katanya demikian masalah mungkin umum setiap bulan bagian bila lainnya terus luar cukup termasuk sebelumnya bahkan wib "
           + "tempat perlu menggunakan memberikan rabu sedangkan kamis langsung apakah pihak melalui diri mencapai minggu aku berada tinggi ingin sebelum "
           + "tengah kini the tahu bersama depan selasa begitu merasa berbagai mengenai maka jumlah masuk katanya mengalami sering ujar kondisi akibat "
           + "hubungan empat paling mendapatkan selalu lima meminta melihat sekarang mengaku mau kerja acara menyatakan masa proses tanpa selatan sempat adanya hidup datang senin rasa maupun "
           + "seluruh mantan lama jenis segera misalnya mendapat bawah jangan meski terlihat akhirnya jumat punya yakni terakhir kecil panjang badan juni of jelas jauh tentu semakin tinggal kurang "
           + "mampu posisi asal sekali sesuai sebesar berat dirinya memberi pagi sabtu ternyata mencari sumber ruang menunjukkan biasanya nama sebanyak utara berlangsung barat kemungkinan yaitu "
           + "berdasarkan sebenarnya cara utama pekan terlalu membawa kebutuhan suatu menerima penting tanggal bagaimana terutama tingkat awal sedikit nanti pasti muncul dekat lanjut ketiga biasa dulu "
           + "kesempatan ribu akhir membantu terkait sebab menyebabkan khusus bentuk ditemukan diduga mana ya kegiatan sebagian tampil hampir bertemu usai berarti keluar pula digunakan justru padahal "
           + "menyebutkan gedung apalagi program milik teman menjalani keputusan sumber a upaya mengetahui mempunyai berjalan menjelaskan b mengambil benar lewat belakang ikut barang meningkatkan kejadian "
           + "kehidupan keterangan penggunaan masing-masing menghadapi").Split(' ').ToList();
            }

        }

        public static List<char> punctuation
        {
            get
            {
                return new List<char>()
                {
                    ',', '.', '_', '"', ';', '/', '?', '!', '(', ')', '[', ']', '{', '}', '|', '~', '`', '-',
                    '@', '#', '$', '%', '^', '&', '*', '+', '='
                };
            }
        }

        public Dictionary<string, int> goodness
        {
            get
            {
                return new Dictionary<string, int>()
                {
                    { "Y", 5 }, //best
                    { "O", 4 }, //good
                    { "M", 3 }, //ok
                    { "L", 2 }, //low
                    { "X", 1 } //not sufficient
                };
            }
        }

        // private readonly string file = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), @"\xampp\wn-msa-all.tab");
        private readonly string file = "wn-msa-all.tab";

        public List<Tuple<int, string, int>> GetSemanticWord()
        {
            using (StreamReader sr = new StreamReader(this.file))
            {
                string[] rows = sr.ReadToEnd().Split('\n');
                if (rows.Length > 0)
                {
                    List<Tuple<int, string, int>> result = new List<Tuple<int, string, int>>();
                    foreach (var row in rows)
                    {
                        string[] part = row.Split('\t');
                        if (part.Length == 4)
                        {
                            if (part[1] == "I" || part[1] == "B") //select only Bahasa Indonesia or Bahasa in common (shared between Malay and Indo)
                            {
                                string idsub = part[0].Substring(0, part[0].IndexOf("-"));
                                int id;
                                if (Int32.TryParse(idsub, out id))
                                {
                                    result.Add(
                                        new Tuple<int, string, int>
                                        (id, part[3], this.goodness.Where(p => p.Key == part[2]).First().Value));
                                }
                            }
                        }
                    }
                    return result;
                }
                else return new List<Tuple<int, string, int>>();
            }
        }

        public List<Tuple<int, string, int>> GetSemanticWord(string filename)
        {
            using (StreamReader sr = new StreamReader(filename))
            {
                string[] rows = sr.ReadToEnd().Split('\n');
                if (rows.Length > 0)
                {
                    List<Tuple<int, string, int>> result = new List<Tuple<int, string, int>>();
                    foreach (var row in rows)
                    {
                        string[] part = row.Split('\t');
                        if (part.Length == 4)
                        {
                            if (part[1] == "I" || part[1] == "B") //select only Bahasa Indonesia or Bahasa in common (shared between Malay and Indo)
                            {
                                string idsub = part[0].Substring(0, part[0].IndexOf("-"));
                                int id;
                                if (Int32.TryParse(idsub, out id))
                                {
                                    result.Add(
                                        new Tuple<int, string, int>
                                        (id, part[3], this.goodness.Where(p => p.Key == part[2]).First().Value));
                                }
                            }
                        }
                    }
                    return result;
                }
                else return new List<Tuple<int, string, int>>();
            }
        }

        public void PrintDictionary()
        {
            var semantic = this.GetSemanticWord();
            Console.Write("ID");
            Console.Write("\t");
            Console.Write("Word/Phrase");
            Console.Write("\t");
            Console.Write("Score");
            Console.WriteLine();
            foreach (var item in semantic)
            {
                Console.Write(item.Item1);
                Console.Write("\t");
                Console.Write(item.Item2);
                Console.Write("\t");
                Console.Write(item.Item3);
                Console.WriteLine();
            }
            Console.ReadLine();
        }

        public void InsertToDb(List<int> semanticRelations, int docNo, Dictionary<int, string> words, bool isNazief)
        {
            using (PlagarismDbEntities dbContent = new PlagarismDbEntities())
            {
                Guid docid = Guid.NewGuid();
                Document doc = new Document();
                doc.id = docid;
                doc.docname = "document" + docNo;
                doc.wordcount = words.Count;
                doc.detectedwordcount = semanticRelations.Count;
                doc.isnazief = isNazief;
                dbContent.Documents.Add(doc);

                if (semanticRelations.Count != 0)
                {
                    foreach (var relation in semanticRelations)
                    {
                        Calculation calc = new Calculation();
                        calc.docid = docid;
                        calc.value = relation;
                        dbContent.Calculations.Add(calc);
                    }
                }

                if (words.Count != 0)
                {
                    foreach (var word in words)
                    {
                        SimilarWord sim = new SimilarWord();
                        sim.docid = docid;
                        sim.pairid = Guid.NewGuid();
                        sim.word = word.Value;
                        sim.dictionaryid = word.Key;
                        dbContent.SimilarWords.Add(sim);
                    }
                }
                dbContent.SaveChanges();
            }
        }

        public List<int> GetSemanticRelation(List<string> words, int docNo, out List<string> vocabList,
            out Dictionary<int, string> results)
        {
            results = new Dictionary<int, string>();
            vocabList = new List<string>();
            List<int> ids = new List<int>();
            var semantic = this.GetSemanticWord();
            int count = words.Count;
            int current = 1;
            int excluded = 0;
            consoleln = 0;
            foreach (var word in words)
            {
                var matches = semantic.Where(p => p.Item2.ToLower() == word.ToLower()); //search word in dictionary
                if (matches.Count() != 0)
                {
                    if (!stopWords.Contains(word.ToLower()))
                    {
                        matches = matches.OrderByDescending(p => p.Item3).ToList(); //select highest score in word relationship
                        ids.Add(matches.First().Item1); //add the ID
                                                        //add the word to vocab list
                        vocabList.Add(matches.First().Item2);
                        foreach (var match in matches)
                        {
                            //ids.Add(match.Item1); //add the ID
                            //                      //add the word to vocab list
                            //vocabList.Add(match.Item2);

                            var sem = semantic.Where(q => q.Item1 == match.Item1 && q.Item2 != match.Item2);
                            if (sem.Count() != 0)
                            {
                                int max = sem.Max(p => p.Item3);
                                if (!results.ContainsKey(match.Item1))
                                    results.Add(match.Item1, match.Item2);
                            }
                        }
                    }
                    else excluded++;
                }
                Console.SetCursorPosition(0, consoleln);
                Console.WriteLine("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t");
                Console.SetCursorPosition(0, consoleln);
                Console.WriteLine("STATUS: Processing words in document " + docNo + " of 2: " + current + "/" + count +
                ". Excluded stop words: " + excluded);

                current++;
            }
            consoleln++;
            //this.InsertToDb(ids, docNo, results);
            return ids;
        }

        public List<int> GetSemanticRelation(List<string> words, int docNo, out List<string> vocabList,
            out Dictionary<int, string> results, string filename)
        {
            results = new Dictionary<int, string>();
            vocabList = new List<string>();
            List<int> ids = new List<int>();
            var semantic = this.GetSemanticWord(filename);
            int count = words.Count;
            int current = 1;
            int excluded = 0;
            consoleln = 0;
            foreach (var word in words)
            {
                var matches = semantic.Where(p => p.Item2.ToLower() == word.ToLower()); //search word in dictionary
                if (matches.Count() != 0)
                {
                    if (!stopWords.Contains(word.ToLower()))
                    {
                        matches = matches.OrderByDescending(p => p.Item3).ToList(); //select highest score in word relationship
                        ids.Add(matches.First().Item1); //add the ID
                                                        //add the word to vocab list
                        vocabList.Add(matches.First().Item2);
                        foreach (var match in matches)
                        {
                            //ids.Add(match.Item1); //add the ID
                            //                      //add the word to vocab list
                            //vocabList.Add(match.Item2);

                            var sem = semantic.Where(q => q.Item1 == match.Item1 && q.Item2 != match.Item2);
                            if (sem.Count() != 0)
                            {
                                int max = sem.Max(p => p.Item3);
                                if (!results.ContainsKey(match.Item1))
                                    results.Add(match.Item1, match.Item2);
                            }
                        }
                    }
                    else excluded++;
                }
                //Console.SetCursorPosition(0, consoleln);
                //Console.WriteLine("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t");
                //Console.SetCursorPosition(0, consoleln);
                //Console.WriteLine("STATUS: Processing words in document " + docNo + " of 2: " + current + "/" + count +
                //". Excluded stop words: " + excluded);

                current++;
            }
            consoleln++;
            //this.InsertToDb(ids, docNo, results);
            return ids;
        }

        public List<int> GetSemanticRelation(List<string> words, int docNo, int totalDoc, out List<string> vocabList,
          out Dictionary<int, string> results, Stemmer stem)
        {
            results = new Dictionary<int, string>();
            vocabList = new List<string>();
            List<int> ids = new List<int>();
            var semantic = this.GetSemanticWord();
            int count = words.Count;
            int current = 1;
            int excluded = 0;
            consoleln = 0;

            foreach (var oldword in words)
            {
                string word = stem.PerformNaziefStemming(oldword);

                var matches = semantic.Where(p => p.Item2.ToLower() == word.ToLower()); //search word in dictionary
                if (matches.Count() != 0)
                {
                    if (!stopWords.Contains(word.ToLower()))
                    {
                        matches = matches.OrderByDescending(p => p.Item3).ToList(); //select highest score in word relationship
                                                                                    //var match = matches.First();
                                                                                    //ids.Add(match.Item1); //add the ID
                                                                                    //add the word to vocab list
                                                                                    //vocabList.Add(match.Item2);
                        foreach (var match in matches)
                        {
                            ids.Add(match.Item1); //add the ID
                                                  //add the word to vocab list
                            vocabList.Add(match.Item2);

                            var sem = semantic.Where(q => q.Item1 == match.Item1 && q.Item2 != match.Item2);
                            if (sem.Count() != 0)
                            {
                                int max = sem.Max(p => p.Item3);
                                if (!results.ContainsKey(match.Item1))
                                    results.Add(match.Item1, match.Item2);
                            }
                        }
                    }
                    else excluded++;
                }
                Console.SetCursorPosition(0, consoleln);
                Console.WriteLine("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t");
                Console.SetCursorPosition(0, consoleln);
                Console.WriteLine("STATUS: Processing words in document " + docNo + " of " + totalDoc + ": " + current + "/" + count +
                ". Excluded stop words: " + excluded);

                current++;
            }
            consoleln++;
            //this.InsertToDb(ids, docNo, results, true);
            return ids;
        }

        public List<int> GetSemanticRelation(List<string> words, int docNo, int totalDoc, out List<string> vocabList,
          out Dictionary<int, string> results, string filename, Stemmer stem)
        {
            results = new Dictionary<int, string>();
            vocabList = new List<string>();
            List<int> ids = new List<int>();
            var semantic = this.GetSemanticWord(filename);
            int count = words.Count;
            int current = 1;
            int excluded = 0;
            consoleln = 0;

            foreach (var oldword in words)
            {
                string word = stem.PerformNaziefStemming(oldword);

                var matches = semantic.Where(p => p.Item2.ToLower() == word.ToLower()); //search word in dictionary
                if (matches.Count() != 0)
                {
                    if (!stopWords.Contains(word.ToLower()))
                    {
                        matches = matches.OrderByDescending(p => p.Item3).ToList(); //select highest score in word relationship
                        var match = matches.First();
                        ids.Add(match.Item1); //add the ID
                                              //add the word to vocab list
                        vocabList.Add(match.Item2);
                        //foreach (var match in matches)
                        //{
                        //ids.Add(match.Item1); //add the ID
                        //                      //add the word to vocab list
                        //vocabList.Add(match.Item2);

                        var sem = semantic.Where(q => q.Item1 == match.Item1 && q.Item2 != match.Item2);
                        if (sem.Count() != 0)
                        {
                            int max = sem.Max(p => p.Item3);
                            if (!results.ContainsKey(match.Item1))
                                results.Add(match.Item1, match.Item2);
                        }
                        //}
                    }
                    else excluded++;
                }
                //Console.SetCursorPosition(0, consoleln);
                //Console.WriteLine("\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t");
                //Console.SetCursorPosition(0, consoleln);
                //Console.WriteLine("STATUS: Processing words in document " + docNo + " of " + totalDoc + ": " + current + "/" + count +
                //". Excluded stop words: " + excluded);

                current++;
            }
            consoleln++;
            //this.InsertToDb(ids, docNo, results, true);
            return ids;
        }


    }
}
