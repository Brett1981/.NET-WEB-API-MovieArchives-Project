using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilmArsivProje.Models
{
    public class SuperUserElestirileri
    {
        public int elestiriid { get; set; }
        public string elestiriicerik { get; set; }

        public string filmadi { get; set; }

        public string kullaniciadi { get; set; }

        public string filmresim { get; set; }
    }
}