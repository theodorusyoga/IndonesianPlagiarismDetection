//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fingerprint
{
    using System;
    using System.Collections.Generic;
    
    public partial class Document
    {
        public Document()
        {
            this.Calculations = new HashSet<Calculation>();
            this.Results = new HashSet<Result>();
            this.Results1 = new HashSet<Result>();
            this.SimilarWords = new HashSet<SimilarWord>();
        }
    
        public System.Guid id { get; set; }
        public string docname { get; set; }
        public int wordcount { get; set; }
        public int detectedwordcount { get; set; }
        public bool isnazief { get; set; }
    
        public virtual ICollection<Calculation> Calculations { get; set; }
        public virtual ICollection<Result> Results { get; set; }
        public virtual ICollection<Result> Results1 { get; set; }
        public virtual ICollection<SimilarWord> SimilarWords { get; set; }
    }
}