using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FilmArsivProje.Models
{
    public class BuHaftaFilmYorumYap
    {
        public Yorumlar yorumlar { get; set; }
        public int filmid { get; set; }
    }
}