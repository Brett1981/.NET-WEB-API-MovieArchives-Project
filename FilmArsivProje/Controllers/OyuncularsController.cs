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
    public class OyuncularsController : ApiController
    {
        private FilmProjesiDatabaseEntities db = new FilmProjesiDatabaseEntities();
        public OyuncularsController()
        {
            db.Configuration.ProxyCreationEnabled = false;

        }
        
        

        // GET: api/Oyunculars
        public IQueryable<Oyuncular> GetOyuncular()
        {
            return db.Oyuncular;
        }

        // GET: api/Oyunculars/5
        [ResponseType(typeof(Oyuncular))]
        public IHttpActionResult GetOyuncular(int id)
        {
            Oyuncular oyuncular = db.Oyuncular.Find(id);
            if (oyuncular == null)
            {
                return NotFound();
            }

            return Ok(oyuncular);
        }

        // PUT: api/Oyunculars/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOyuncular(int id, Oyuncular oyuncular)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != oyuncular.oyuncuid)
            {
                return BadRequest();
            }

            db.Entry(oyuncular).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OyuncularExists(id))
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

        // POST: api/Oyunculars
        [ResponseType(typeof(Oyuncular))]
        public IHttpActionResult PostOyuncular(Oyuncular oyuncular)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Oyuncular.Add(oyuncular);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = oyuncular.oyuncuid }, oyuncular);
        }

        // DELETE: api/Oyunculars/5
        [ResponseType(typeof(Oyuncular))]
        public IHttpActionResult DeleteOyuncular(int id)
        {
            Oyuncular oyuncular = db.Oyuncular.Find(id);
            if (oyuncular == null)
            {
                return NotFound();
            }

            db.Oyuncular.Remove(oyuncular);
            db.SaveChanges();

            return Ok(oyuncular);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OyuncularExists(int id)
        {
            return db.Oyuncular.Count(e => e.oyuncuid == id) > 0;
        }
      

    }
}