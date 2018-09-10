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
    public class BuHaftakiFilmlerController : ApiController
    {
        private FilmProjesiDatabaseEntities db = new FilmProjesiDatabaseEntities();
        public BuHaftakiFilmlerController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        // GET: api/BuHaftakiFilmler
        public IQueryable<BuHaftakiFilmler> GetBuHaftakiFilmler()
        {
            return db.BuHaftakiFilmler;
        }

        // GET: api/BuHaftakiFilmler/5
        [ResponseType(typeof(BuHaftakiFilmler))]
        public IHttpActionResult GetBuHaftakiFilmler(int id)
        {
            BuHaftakiFilmler buHaftakiFilmler = db.BuHaftakiFilmler.Find(id);
            if (buHaftakiFilmler == null)
            {
                return NotFound();
            }

            return Ok(buHaftakiFilmler);
        }

        // PUT: api/BuHaftakiFilmler/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBuHaftakiFilmler(int id,BuHaftakiFilmler buHaftakiFilmler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != buHaftakiFilmler.filmid)
            {
                return BadRequest();
            }

            db.Entry(buHaftakiFilmler).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuHaftakiFilmlerExists(id))
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

        // POST: api/BuHaftakiFilmler
        [ResponseType(typeof(BuHaftakiFilmler))]
        public IHttpActionResult PostBuHaftakiFilmler(BuHaftakiFilmler buHaftakiFilmler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BuHaftakiFilmler.Add(buHaftakiFilmler);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = buHaftakiFilmler.filmid }, buHaftakiFilmler);
        }

        // DELETE: api/BuHaftakiFilmler/5
        [ResponseType(typeof(BuHaftakiFilmler))]
        public IHttpActionResult DeleteBuHaftakiFilmler(int id)
        {
            BuHaftakiFilmler buHaftakiFilmler = db.BuHaftakiFilmler.Find(id);
            if (buHaftakiFilmler == null)
            {
                return NotFound();
            }

            db.BuHaftakiFilmler.Remove(buHaftakiFilmler);
            db.SaveChanges();

            return Ok(buHaftakiFilmler);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BuHaftakiFilmlerExists(int id)
        {
            return db.BuHaftakiFilmler.Count(e => e.filmid == id) > 0;
        }

       
        
    }
}