//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FilmArsivProje.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FilmElestirileri
    {
        public int elestiriid { get; set; }
        public string elestiriicerik { get; set; }
        public Nullable<int> filmid { get; set; }
        public Nullable<int> kullaniciid { get; set; }
    
        public virtual Kullanicilar Kullanicilar { get; set; }
        public virtual ListeliFilmler ListeliFilmler { get; set; }
    }
}