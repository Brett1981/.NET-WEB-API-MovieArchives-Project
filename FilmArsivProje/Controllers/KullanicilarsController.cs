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
using System.Web.Http.Cors;
using System.Security.Claims;

namespace FilmArsivProje.Controllers
{
    [Authorize(Roles ="Admin")]
    public class KullanicilarsController : ApiController
    {
        HttpResponseMessage response;
        public KullanicilarsController()
        {
                db.Configuration.ProxyCreationEnabled = false;    
        }
        private FilmProjesiDatabaseEntities db = new FilmProjesiDatabaseEntities();

        // GET: api/Kullanicilars
        public IQueryable<Kullanicilar> GetKullanicilar()
        {
            return db.Kullanicilar;
        }

        // GET: api/Kullanicilars/5
        [ResponseType(typeof(Kullanicilar))]
        public IHttpActionResult GetKullanicilar(int id)
        {
            Kullanicilar kullanicilar = db.Kullanicilar.Find(id);
            if (kullanicilar == null)
            {
                return NotFound();
            }

            return Ok(kullanicilar);
        }

        // PUT: api/Kullanicilars/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutKullanicilar(int id, Kullanicilar kullanicilar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != kullanicilar.kullaniciid)
            {
                return BadRequest();
            }

            db.Entry(kullanicilar).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KullanicilarExists(id))
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

        // POST: api/Kullanicilars
        [ResponseType(typeof(Kullanicilar))]
        public HttpResponseMessage PostKullanicilar(Kullanicilar kullanicilar)
        {
            try
            {
                var check = db.Kullanicilar.FirstOrDefault(x => x.kullaniciadi == kullanicilar.kullaniciadi);

                if (!ModelState.IsValid)
                {
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Boş alan bıraktınız veya bir hata oluştu.");
                    return response;
                }
                else if (check != null)
                {
                    response = Request.CreateResponse(HttpStatusCode.InternalServerError, "Böyle bir kullanici adi bulunmaktadir.");
                    return response;
                }


                else
                {
                    db.Kullanicilar.Add(kullanicilar);
                    db.SaveChanges();
                    response = Request.CreateResponse(HttpStatusCode.OK, "Kayit basarilidir.");
                    return response;
                }
            }
            catch(Exception )
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, " Kayıt anında bir hata oluştu.");
                return response;
            }
        }
        [Route("api/PostIsCheckKullanicilar")]
        public Kullanicilar PostIsCheckKullanicilar(Kullanicilar kullanicilar)
        {
            var check = db.Kullanicilar.FirstOrDefault(x => x.kullaniciadi == kullanicilar.kullaniciadi && x.kullanicisifre == kullanicilar.kullanicisifre);
            if(check!=null)
            {
                
                return check;
            }
            else
            {
                return null;
            }
        }

        // DELETE: api/Kullanicilars/5
        [ResponseType(typeof(Kullanicilar))]
        public IHttpActionResult DeleteKullanicilar(int id)
        {
            Kullanicilar kullanicilar = db.Kullanicilar.Find(id);
            if (kullanicilar == null)
            {
                return NotFound();
            }

            db.Kullanicilar.Remove(kullanicilar);
            db.SaveChanges();

            return Ok(kullanicilar);
        }
   
       

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool KullanicilarExists(int id)
        {
            return db.Kullanicilar.Count(e => e.kullaniciid == id) > 0;
        }
    }
}