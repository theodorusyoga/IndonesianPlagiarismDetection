using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTETIPlagiarismDetection.Models
{
    public class Document
    {
        public Guid DocumentId { get; set; }
        public string Content { get; set; }
        public string Author1 { get; set; }
        public string Author2 { get; set; }
        public string Author3 { get; set; }
        public string DocumentTitle { get; set; }
        public double SimilarityLevel { get; set; }
        public int Year { get; set; }
    }
}