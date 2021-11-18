using System;
using System.Collections.Generic;

namespace MvvmCrossApp.Core.Models
{
    public class PagedResult<T> where T : class
    {
        public int TotalFilas { get; set; }
        public int Pagina { get; set; }
        public int TamanioPagina { get; set; }
        public IEnumerable<T> Resultados { get; set; }
    }
}
