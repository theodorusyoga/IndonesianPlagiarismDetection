using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Fingerprint;

namespace DTETIPlagiarismDetection
{
    public partial class _default : System.Web.UI.Page
    {
        Wordnet wn = new Wordnet();
        Stemmer stemmer;
        string dictfile;
        Pearson pr = new Pearson();
        protected void Page_Load(object sender, EventArgs e)
        {
            dictfile = Server.MapPath("/") + "/wn-msa-all.tab";
            stemmer = new Stemmer(wn.GetSemanticWord(dictfile).Select(p => p.Item2).ToList());
            if (!IsPostBack)
            {
                divresult.Visible = false;
                
                
            }
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            //hashing no. + hasil akhir
           

            List<double> jaccard = new List<double>();
            List<double> pearson = new List<double>();

            //tokenize 
            var lw1 = TFIDF.Tokenize(teks1.Text);
            var lw2 = TFIDF.Tokenize(teks2.Text);
            int hashno = Convert.ToInt32(Math.Ceiling((decimal)(lw1.Count+ lw2.Count)/2));
            MinHash mh = new MinHash(Math.Min(lw1.Count, lw2.Count), hashno);
            //vocab list
            var vl1 = new List<string>();
            var vl2 = new List<string>();
            //pasangan antara ID kamus dengan string yang dicocokkan
            Dictionary<int, string> res1 = new Dictionary<int, string>();
            Dictionary<int, string> res2 = new Dictionary<int, string>();
            //list ID kamus aja
          
            List<int> sr1 = wn.GetSemanticRelation(lw1, 1, 2, out vl1, out res1, dictfile, stemmer);
            List<int> sr2 = wn.GetSemanticRelation(lw2, 2, 2, out vl2, out res2, dictfile, stemmer);

            List<uint> hash1 = mh.GetMinHash(sr1);
            List<uint> hash2 = mh.GetMinHash(sr2);

            List<double> hashdb1 = new List<double>();
            List<double> hashdb2 = new List<double>();

            //convert uint to double
            string hr1 = string.Empty;
            string hr2 = string.Empty;

            //print code + id
            foreach (var item in res1)
            {
                hr1 += "<p>" + item.Value + ": " + item.Key.ToString() + "</p>";
            }

            foreach (var item in res2)
            {
                hr2 += "<p>" + item.Value + ": " + item.Key.ToString() + "</p>";
            }

            foreach (var item in hash1)
            {
                var cvt = Convert.ToDouble(item);
                hashdb1.Add(cvt);
            }
            foreach (var item in hash2)
            {
                var cvt = Convert.ToDouble(item);
                hashdb2.Add(Convert.ToDouble(cvt));
            }

            //final jaccard & pearson           
            var jacc = ((double)mh.Similarity(hash1, hash2));
            var jaccpercent = (double)jacc * 100;
            var jaccmin = (double)(jacc - 1);
            var pear = pr.Calc(hashdb1, hashdb2);

            jaccardResult.Text = jaccpercent + "%";
            pearsonResult.Text = (double)(100-pear) + "%";
            hashresult1.Text = hr1;
            hashresult2.Text = hr2;
            divresult.Visible = true;
        }
    }
}