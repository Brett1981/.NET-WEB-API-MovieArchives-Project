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
    public class HaberlersController : ApiController
    {
        public HaberlersController()
        {
            db.Configuration.ProxyCreationEnabled = false;

        }
        private FilmProjesiDatabaseEntities db = new FilmProjesiDatabaseEntities();

        // GET: api/Haberlers
        public IQueryable<Haberler> GetHaberler()
        {
            return db.Haberler;
        }

        // GET: api/Haberlers/5
        [ResponseType(typeof(Haberler))]
        public IHttpActionResult GetHaberler(int id)
        {
            Haberler haberler = db.Haberler.Find(id);
            if (haberler == null)
            {
                return NotFound();
            }

            return Ok(haberler);
        }

        // PUT: api/Haberlers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutHaberler(int id, Haberler haberler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != haberler.haberid)
            {
                return BadRequest();
            }

            db.Entry(haberler).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HaberlerExists(id))
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

        // POST: api/Haberlers
        [ResponseType(typeof(Haberler))]
        public IHttpActionResult PostHaberler(Haberler haberler)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Haberler.Add(haberler);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = haberler.haberid }, haberler);
        }

        // DELETE: api/Haberlers/5
        [ResponseType(typeof(Haberler))]
        public IHttpActionResult DeleteHaberler(int id)
        {
            Haberler haberler = db.Haberler.Find(id);
            if (haberler == null)
            {
                return NotFound();
            }

            db.Haberler.Remove(haberler);
            db.SaveChanges();

            return Ok(haberler);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HaberlerExists(int id)
        {
            return db.Haberler.Count(e => e.haberid == id) > 0;
        }
    }
}