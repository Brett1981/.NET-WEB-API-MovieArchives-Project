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
    public class FilmElestirilerisController : ApiController
    {
        private FilmProjesiDatabaseEntities db = new FilmProjesiDatabaseEntities();

        // GET: api/FilmElestirileris
        public IQueryable<FilmElestirileri> GetFilmElestirileri()
        {
            return db.FilmElestirileri;
        }

        // GET: api/FilmElestirileris/5
        [ResponseType(typeof(FilmElestirileri))]
        public IHttpActionResult GetFilmElestirileri(int id)
        {
            FilmElestirileri filmElestirileri = db.FilmElestirileri.Find(id);
            if (filmElestirileri == null)
            {
                return NotFound();
            }

            return Ok(filmElestirileri);
        }

        // PUT: api/FilmElestirileris/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFilmElestirileri(int id, FilmElestirileri filmElestirileri)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != filmElestirileri.elestiriid)
            {
                return BadRequest();
            }

            db.Entry(filmElestirileri).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FilmElestirileriExists(id))
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

        // POST: api/FilmElestirileris
        [ResponseType(typeof(FilmElestirileri))]
        public IHttpActionResult PostFilmElestirileri(FilmElestirileri filmElestirileri)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.FilmElestirileri.Add(filmElestirileri);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = filmElestirileri.elestiriid }, filmElestirileri);
        }

        // DELETE: api/FilmElestirileris/5
        [ResponseType(typeof(FilmElestirileri))]
        public IHttpActionResult DeleteFilmElestirileri(int id)
        {
            FilmElestirileri filmElestirileri = db.FilmElestirileri.Find(id);
            if (filmElestirileri == null)
            {
                return NotFound();
            }

            db.FilmElestirileri.Remove(filmElestirileri);
            db.SaveChanges();

            return Ok(filmElestirileri);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FilmElestirileriExists(int id)
        {
            return db.FilmElestirileri.Count(e => e.elestiriid == id) > 0;
        }
    }
}