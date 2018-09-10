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
    public class YorumlarsController : ApiController
    {
        private FilmProjesiDatabaseEntities db = new FilmProjesiDatabaseEntities();
        public YorumlarsController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        

        // GET: api/Yorumlars
        public IQueryable<Yorumlar> GetYorumlar()
        {
            return db.Yorumlar;
        }

        // GET: api/Yorumlars/5
        [ResponseType(typeof(Yorumlar))]
        public IHttpActionResult GetYorumlar(int id)
        {
            Yorumlar yorumlar = db.Yorumlar.Find(id);
            if (yorumlar == null)
            {
                return NotFound();
            }

            return Ok(yorumlar);
        }

        // PUT: api/Yorumlars/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutYorumlar(int id, Yorumlar yorumlar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != yorumlar.yorumid)
            {
                return BadRequest();
            }

            db.Entry(yorumlar).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!YorumlarExists(id))
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

        // POST: api/Yorumlars
        [ResponseType(typeof(Yorumlar))]
        public bool PostYorumlar(Yorumlar yorumlar,int filmid)
        {
            BuHaftaFilmYorumlari filmyorumlari = new BuHaftaFilmYorumlari();
            yorumlar.yorumtarihi = DateTime.Now.ToString("d");
            yorumlar.yorumsaati = DateTime.Now.ToShortTimeString();
            if (!ModelState.IsValid)
            {
                return false;
            }

            //db.Yorumlar.Add(yorumlar);
            //db.SaveChanges();

            return true;
        }

    
        // DELETE: api/Yorumlars/5
        [ResponseType(typeof(Yorumlar))]
        public IHttpActionResult DeleteYorumlar(int id)
        {
            Yorumlar yorumlar = db.Yorumlar.Find(id);
            if (yorumlar == null)
            {
                return NotFound();
            }

            db.Yorumlar.Remove(yorumlar);
            db.SaveChanges();

            return Ok(yorumlar);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool YorumlarExists(int id)
        {
            return db.Yorumlar.Count(e => e.yorumid == id) > 0;
        }
    }
}