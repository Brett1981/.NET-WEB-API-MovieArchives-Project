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
    [Authorize(Roles ="Admin")]
    public class GelecekHaftakiFilmlersController : ApiController
    {
        public GelecekHaftakiFilmlersController()
        {
            db.Configuration.ProxyCreationEnabled = false;

        }

        private FilmProjesiDatabaseEntities db = new FilmProjesiDatabaseEntities();

        // GET: api/GelecekHaftakiFilmlers
        public IQueryable<GelecekHaftakiFilmler> GetGelecekHaftakiFilmler()
        {
            return db.GelecekHaftakiFilmler;
        }

        // GET: api/GelecekHaftakiFilmlers/5
        [ResponseType(typeof(GelecekHaftakiFilmler))]
        public IHttpActionResult GetGelecekHaftakiFilmler(int id)
        {
            GelecekHaftakiFilmler gelecekHaftakiFilmler = db.GelecekHaftakiFilmler.Find(id);
            if (gelecekHaftakiFilmler == null)
            {
                return NotFound();
            }

            return Ok(gelecekHaftakiFilmler);
        }

        // PUT: api/GelecekHaftakiFilmlers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGelecekHaftakiFilmler(int id, GelecekHaftakiFilmler gelecekHaftakiFilmler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gelecekHaftakiFilmler.filmid)
            {
                return BadRequest();
            }

            db.Entry(gelecekHaftakiFilmler).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GelecekHaftakiFilmlerExists(id))
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

        // POST: api/GelecekHaftakiFilmlers
        [ResponseType(typeof(GelecekHaftakiFilmler))]
        public IHttpActionResult PostGelecekHaftakiFilmler(GelecekHaftakiFilmler gelecekHaftakiFilmler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GelecekHaftakiFilmler.Add(gelecekHaftakiFilmler);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = gelecekHaftakiFilmler.filmid }, gelecekHaftakiFilmler);
        }

        // DELETE: api/GelecekHaftakiFilmlers/5
        [ResponseType(typeof(GelecekHaftakiFilmler))]
        public IHttpActionResult DeleteGelecekHaftakiFilmler(int id)
        {
            GelecekHaftakiFilmler gelecekHaftakiFilmler = db.GelecekHaftakiFilmler.Find(id);
            if (gelecekHaftakiFilmler == null)
            {
                return NotFound();
            }

            db.GelecekHaftakiFilmler.Remove(gelecekHaftakiFilmler);
            db.SaveChanges();

            return Ok(gelecekHaftakiFilmler);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GelecekHaftakiFilmlerExists(int id)
        {
            return db.GelecekHaftakiFilmler.Count(e => e.filmid == id) > 0;
        }
    }
}