using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBOAnalysis.Models
{
    public class DocumentRoot
    {
        public DocumentList documents { get; set; }
    }

    public class DocumentList
    {
        public List<Document> document { get; set; }
    }

    public class Document
    {
        public string id { get; set; }
        public List<object> occurrence { get; set; }
        public string cuid { get; set; }
        public string name { get; set; }
        public int folderId { get; set; }
        public string scheduled { get; set; }
    }
}
