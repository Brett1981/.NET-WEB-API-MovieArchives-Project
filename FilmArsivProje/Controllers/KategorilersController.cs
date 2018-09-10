using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using FilmArsivProje.Models;

namespace FilmArsivProje.Controllers
{
    [Authorize(Roles = "Admin")]
    public class KategorilersController : ApiController
    {
        private FilmProjesiDatabaseEntities db = new FilmProjesiDatabaseEntities();

        public KategorilersController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        // GET: api/Kategorilers
        public IQueryable<Kategoriler> GetKategoriler()
        {
            return db.Kategoriler;
        }
       
        //public List<ListeliFilmler> GetKategoriFilmleriGetir(int kategoriid)
        //{
        //    List<int> kategorilifilmlersorgusu = db.ListeliFilmKategori.Where(x => x.kategoriid == kategoriid).Select(x=>x.filmid).ToList();
        //    List<ListeliFilmler> kategorilistelifilmler = new List<ListeliFilmler>();
        //    for (int i =0;i<kategorilifilmlersorgusu.Count;i++)
        //    {
        //        int numara = kategorilifilmlersorgusu.ElementAt(i);
        //        ListeliFilmler listelifilmler = db.ListeliFilmler.FirstOrDefault(x => x.filmid == numara );
        //        kategorilistelifilmler.Add(listelifilmler);
        //    }
          
          
        //    return kategorilistelifilmler;
        //}
        
        // GET: api/Kategorilers/5
        [ResponseType(typeof(Kategoriler))]
        public IHttpActionResult GetKategoriler(int id)
        {
            Kategoriler kategoriler = db.Kategoriler.Find(id);
            if (kategoriler == null)
            {
                return NotFound();
            }

            return Ok(kategoriler);
        }

        // PUT: api/Kategorilers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutKategoriler(int id, Kategoriler kategoriler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kategoriler.kategoriid)
            {
                return BadRequest();
            }

            db.Entry(kategoriler).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KategorilerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Kategorilers
        [ResponseType(typeof(Kategoriler))]
        public IHttpActionResult PostKategoriler(Kategoriler kategoriler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Kategoriler.Add(kategoriler);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = kategoriler.kategoriid }, kategoriler);
        }

        // DELETE: api/Kategorilers/5
        [ResponseType(typeof(Kategoriler))]
        public IHttpActionResult DeleteKategoriler(int id)
        {
            Kategoriler kategoriler = db.Kategoriler.Find(id);
            if (kategoriler == null)
            {
                return NotFound();
            }

            db.Kategoriler.Remove(kategoriler);
            db.SaveChanges();

            return Ok(kategoriler);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KategorilerExists(int id)
        {
            return db.Kategoriler.Count(e => e.kategoriid == id) > 0;
        }

        
    }
}