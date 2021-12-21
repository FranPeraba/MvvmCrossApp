using System.Collections.Generic;

namespace MvvmCrossApp.Core.Models
{
    public class Medicine
    {
        public string Nregistro { get; set; }
        public string Nombre { get; set; }
        public List<Document> Docs { get; set; }
    }
}