using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
using System.Web.Http.Description;
using FilmArsivProje.Models;

namespace FilmArsivProje.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ListeliFilmlersController : ApiController
    {
        public ListeliFilmlersController()
        {
            db.Configuration.ProxyCreationEnabled = false;

        }
        private FilmProjesiDatabaseEntities db = new FilmProjesiDatabaseEntities();

        // GET: api/ListeliFilmlers
        public IQueryable<ListeliFilmler> GetListeliFilmler()
        {
            return db.ListeliFilmler;
        }

        // GET: api/ListeliFilmlers/5
        [ResponseType(typeof(ListeliFilmler))]
        public IHttpActionResult GetListeliFilmler(int id)
        {
            ListeliFilmler listeliFilmler = db.ListeliFilmler.Find(id);
            if (listeliFilmler == null)
            {
                return NotFound();
            }

            return Ok(listeliFilmler);
        }

        // PUT: api/ListeliFilmlers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutListeliFilmler(int id, ListeliFilmler listeliFilmler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != listeliFilmler.filmid)
            {
                return BadRequest();
            }

            db.Entry(listeliFilmler).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListeliFilmlerExists(id))
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

        // POST: api/ListeliFilmlers
        [ResponseType(typeof(ListeliFilmler))]
        public IHttpActionResult PostListeliFilmler(ListeliFilmler listeliFilmler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ListeliFilmler.Add(listeliFilmler);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = listeliFilmler.filmid }, listeliFilmler);
        }

        // DELETE: api/ListeliFilmlers/5
        [ResponseType(typeof(ListeliFilmler))]
        public IHttpActionResult DeleteListeliFilmler(int id)
        {
            ListeliFilmler listeliFilmler = db.ListeliFilmler.Find(id);
            if (listeliFilmler == null)
            {
                return NotFound();
            }

            db.ListeliFilmler.Remove(listeliFilmler);
            db.SaveChanges();

            return Ok(listeliFilmler);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ListeliFilmlerExists(int id)
        {
            return db.ListeliFilmler.Count(e => e.filmid == id) > 0;
        }
        [Authorize(Roles ="Admin")]
        //LİSTELİ FİLM VE KATEGORİYE EKLEME
        [Route("api/PostFilmKategoriKayıt")]
        [HttpPost]
        public HttpResponseMessage PostFilmKategoriKayıt([FromBody]ListeliFilmlerVeKategoriKayit listeliFilmlerVeKategoriKayit)
        {
            HttpResponseMessage response;

            if (!ModelState.IsValid)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Boş alan bıraktınız veya bir hata oluştu.");
                return response;
            }
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    using (FilmProjesiDatabaseEntities db = new FilmProjesiDatabaseEntities())
                    {
                        ListeliFilmKategori listeliFilmKategori = new ListeliFilmKategori();
                        db.ListeliFilmler.Add(listeliFilmlerVeKategoriKayit.listelifilm);
                        db.SaveChanges();
                        listeliFilmKategori.filmid = listeliFilmlerVeKategoriKayit.listelifilm.filmid;
                        listeliFilmKategori.kategoriid = listeliFilmlerVeKategoriKayit.kategoriid;
                        db.ListeliFilmKategori.Add(listeliFilmKategori);
                         db.SaveChanges();

                    }
                    ts.Complete();
                    response = Request.CreateResponse(HttpStatusCode.OK, "Başarılıdır.");
                    return response;
                }
            }
            catch (Exception )
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Bir hata oluştu.");
                return response;
            }


        }
        [Authorize(Roles = "Admin")]
        [Route("api/DeleteFilmKategori/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteFilmKategori(int id)
        {
            HttpResponseMessage response;
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    using (FilmProjesiDatabaseEntities db = new FilmProjesiDatabaseEntities())
                    {
                        ListeliFilmler listeliFilmler = db.ListeliFilmler.Find(id);
                        if (listeliFilmler == null)
                        {
                            response = Request.CreateResponse(HttpStatusCode.NotFound, "Bulunamadi.");
                        }
                        IQueryable<ListeliFilmKategori> filmkategori = db.ListeliFilmKategori.Where(x => x.filmid == id);
                        foreach (var item in filmkategori)
                        {
                            db.ListeliFilmKategori.Remove(item);
                        }
                        db.SaveChanges();
                        db.ListeliFilmler.Remove(listeliFilmler);
                        db.SaveChanges();
                     

                        
                    }
                    ts.Complete();
                    response = Request.CreateResponse(HttpStatusCode.OK, "Başarılıdır.");
                    return response;
                }
            }
            catch(Exception )
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Bir hata oluştu.");
                return response;
            }
        }

    }
}
//c# unit of work