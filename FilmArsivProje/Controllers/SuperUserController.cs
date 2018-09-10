using FilmArsivProje.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FilmArsivProje.Controllers
{
    
    [Authorize(Roles ="Admin,SuperUser")]
    public class SuperUserController : ApiController
    {
        FilmProjesiDatabaseEntities db = new FilmProjesiDatabaseEntities();

        [Route("api/PostMovieReview")]
        [HttpPost]
        public IHttpActionResult PostMovieReview(FilmElestirileri movieReview)
        {
            var check = db.FilmElestirileri.FirstOrDefault(x => x.filmid == movieReview.filmid && x.kullaniciid == movieReview.kullaniciid);
            if (check != null)
            {
                return BadRequest(ModelState);
            }
            else
            {
                db.FilmElestirileri.Add(movieReview);
                db.SaveChanges();
                return Ok();
            }
        }
        [Route("api/GetFilmElestirim/{id}")]
        [HttpGet]
        public List<SuperUserElestirileri> GetFilmElestirim(int id)
        {
            List<SuperUserElestirileri> query = db.FilmElestirileri.Include("Kullanicilar").Where(y => y.kullaniciid == id).Select(u=>new SuperUserElestirileri
            {
                elestiriicerik = u.elestiriicerik,
                filmadi = u.ListeliFilmler.filmadi,
                kullaniciadi = u.Kullanicilar.kullaniciadi
            }).ToList();
            return query;
                    
        }
        [Route("api/PostElestiriVerileriGetir")]
        [HttpPost]
        public FilmElestirileri PostElestiriVerileriGetir(SuperUserElestirileri superkullanicielestirisi)
        {
            int filmid = db.ListeliFilmler.FirstOrDefault(x => x.filmadi == superkullanicielestirisi.filmadi).filmid;
            int kullaniciid = db.Kullanicilar.FirstOrDefault(x => x.kullaniciadi == superkullanicielestirisi.kullaniciadi).kullaniciid;
            FilmElestirileri filmelestirisi = db.FilmElestirileri.FirstOrDefault(x => x.kullaniciid == kullaniciid && x.filmid == filmid);
            return filmelestirisi;
        }
        [Route("api/PostElestiriVerileriGuncelle")]
        [HttpPost]
        public IHttpActionResult PutElestiriVerileriGuncelle(FilmElestirileri movieReview)
        {
            var check = db.FilmElestirileri.FirstOrDefault(x => x.filmid == movieReview.filmid && x.kullaniciid == movieReview.kullaniciid);
            if (check == null)
            {
                return BadRequest(ModelState);
            }
            else
            {
                check.elestiriicerik = movieReview.elestiriicerik;
                db.SaveChanges();
                return Ok();
            }
        }

    }
}
