using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Transactions;
using System.Web.Http;
using FilmArsivProje.Models;
namespace FilmArsivProje.Controllers
{
    public class MyController : ApiController
    {
        private FilmProjesiDatabaseEntities db = new FilmProjesiDatabaseEntities();

        public MyController()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        //ANASAYFA FİLM ADLARI VE KATEGORİLER
        [Route("api/GetBuHaftakiFilmlerAnasayfa")]
        [HttpGet]
        public IEnumerable<BuHaftakiFilmler> GetBuHaftakiFilmlerAnasayfa()
        {
            return db.BuHaftakiFilmler.ToList();
        }
        [Route("api/GetGelecekHaftakiFilmlerAnasayfa")]
        [HttpGet]
        public List<GelecekHaftakiFilmler> GetHaftayaOlanFilmlerAnasayfa()
        {
            return db.GelecekHaftakiFilmler.ToList();
        }
        [Route("api/GetKategorilerAnasayfa")]
        [HttpGet]
        public List<Kategoriler> GetKategorilerAnasayfa()
        {
            return db.Kategoriler.ToList();
        }
        [Route("api/GetListeliFilmlerAnasayfa")]
        [HttpGet]
        public List<ListeliFilmler> GetListeliFilmlerAnasayfa()
        {
            return db.ListeliFilmler.ToList();
        }

        //FİLMNOTLARI
        [Route("api/PostFilmNot")]
        [HttpPost]
        public bool PostFilmNot(BuHaftakiFilmKullaniciNotu buhaftafilmlerikullanicinotu)
        {
            int filmid = Convert.ToInt32(buhaftafilmlerikullanicinotu.filmid);
            int filmnotsayisi;
            int puanortalamasi;
            BuHaftakiFilmler buhaftakinotverilenfilm = db.BuHaftakiFilmler.FirstOrDefault(x => x.filmid == filmid);
            int genel_not = Convert.ToInt32(buhaftakinotverilenfilm.genel_not);
            BuHaftakiFilmKullaniciNotu dahaoncenotvermidi = db.BuHaftakiFilmKullaniciNotu.FirstOrDefault(x => x.filmid == filmid && x.kullaniciid == buhaftafilmlerikullanicinotu.kullaniciid);
            int toplampuan;
            if (dahaoncenotvermidi != null)
            {
                dahaoncenotvermidi.filmnotu = buhaftafilmlerikullanicinotu.filmnotu;
                db.SaveChanges();
                toplampuan = Convert.ToInt32(db.BuHaftakiFilmKullaniciNotu.Where(x => x.filmid == filmid).Sum(x => x.filmnotu));
                filmnotsayisi = db.BuHaftakiFilmKullaniciNotu.Where(x => x.filmid == filmid).Count();
                buhaftakinotverilenfilm.genel_not = Convert.ToInt32(toplampuan) / (filmnotsayisi);
                db.SaveChanges();
                return true;
            }
            else
            {
                db.BuHaftakiFilmKullaniciNotu.Add(buhaftafilmlerikullanicinotu);
                db.SaveChanges();
                toplampuan = Convert.ToInt32(db.BuHaftakiFilmKullaniciNotu.Where(x => x.filmid == filmid).Sum(x => x.filmnotu));
                filmnotsayisi = db.BuHaftakiFilmKullaniciNotu.Where(x => x.filmid == filmid).Count();
                puanortalamasi = Convert.ToInt32(toplampuan) / (filmnotsayisi);
                buhaftakinotverilenfilm.genel_not = puanortalamasi;
                db.SaveChanges();
                return true;
            }
        }
        [Route("api/getFilmNotu/{filmid}")]
        [HttpGet]
        public int getFilmNotu(int filmid)
        {
            return Convert.ToInt32(db.BuHaftakiFilmler.FirstOrDefault(x => x.filmid == filmid).genel_not);

        }
        //KATEGORİLERE GÖRE FİLMLER
        [Route("api/GetKategoriFilmleriGetir/{kategoriid}")]
        [HttpGet]
        public List<ListeliFilmler> GetKategoriFilmleriGetir(int kategoriid)
        {
            var tableinclude = db.ListeliFilmKategori.Include("ListeliFilmler").Where(x => x.kategoriid == kategoriid).Select(x => x.ListeliFilmler).ToList();

            return tableinclude;
        }
        //OYUNCULAR
        [Route("api/GetListeliFilmOyunculari/{filmid}")]
        [HttpGet]
        public IEnumerable<string> GetListeliFilmOyunculari(int filmid)
        {
            return db.ListeliFilmOyuncu.Include("Oyuncular").Where(x => x.filmid == filmid).Select(x => x.Oyuncular.oyuncuadi);
        }

        [Route("api/GetListeliFilmOyuncusu/{oyuncuadi}")]
        [HttpGet]
        public Oyuncular GetListeliFilmOyuncusu(string oyuncuadi)
        {
            Oyuncular oyuncu = db.Oyuncular.FirstOrDefault(x => x.oyuncuadi == oyuncuadi);
            return oyuncu;
        }
        [Route("api/GetOyuncuFilmleriGetir/{oyuncuadi}")]
        [HttpGet]
        public IEnumerable<ListeliFilmler> GetOyuncuFilmleriGetir(string oyuncuadi)
        {
            int oyuncuid = db.Oyuncular.FirstOrDefault(x => x.oyuncuadi == oyuncuadi).oyuncuid;
            IEnumerable<ListeliFilmler> oyuncunun_oynadigi_filmler = db.ListeliFilmOyuncu.Where(x => x.oyuncuid == oyuncuid).Select(x => x.ListeliFilmler);
            return oyuncunun_oynadigi_filmler;
        }
        //YORUMLAR
        [Route("api/GetFilmYorumlariGetir/{filmid}")]
        [HttpGet]
        public List<FilmYorumlariGetir> GetFilmYorumlariGetir(int filmid)
        {
            var q = (from t in db.Yorumlar
                     join qw in db.BuHaftaFilmYorumlari on filmid equals qw.filmid
                     join sc in db.Kullanicilar on t.kullaniciid equals sc.kullaniciid
                     where qw.yorumid == t.yorumid
                     select new { t.yorum, t.yorumsaati, t.yorumtarihi, sc.kullaniciadi });
            var filmyorumlari = new List<FilmYorumlariGetir>();
            foreach (var t in q)
            {
                filmyorumlari.Add(new FilmYorumlariGetir()
                {
                    yorum = t.yorum,
                    yorumsaati = t.yorumsaati,
                    yorumtarihi = t.yorumtarihi,
                    kullaniciadi = t.kullaniciadi

                });
            }
            return filmyorumlari;
        }
        [Route("api/GetListFilmYorumlariGetir/{filmid}")]
        [HttpGet]
        public List<FilmYorumlariGetir> GetListFilmYorumlariGetir(int filmid)
        {
            var q = (from t in db.Yorumlar
                     join qw in db.ListeliFilmYorumları on filmid equals qw.filmid
                     join sc in db.Kullanicilar on t.kullaniciid equals sc.kullaniciid
                     where qw.yorumid == t.yorumid
                     select new { t.yorum, t.yorumsaati, t.yorumtarihi, sc.kullaniciadi });
            var filmyorumlari = new List<FilmYorumlariGetir>();
            foreach (var t in q)
            {
                filmyorumlari.Add(new FilmYorumlariGetir()
                {
                    yorum = t.yorum,
                    yorumsaati = t.yorumsaati,
                    yorumtarihi = t.yorumtarihi,
                    kullaniciadi = t.kullaniciadi

                });
            }
            return filmyorumlari;
        }

        [Route("api/PostYorumYap")]
        [HttpPost]
        public bool PostYorumYap([FromBody]BuHaftaFilmYorumYap buhaftafilmyorumu)
        {

            BuHaftaFilmYorumlari buhaftafilmyorumlari = new BuHaftaFilmYorumlari();
            buhaftafilmyorumu.yorumlar.yorumtarihi = DateTime.Now.ToString("d");
            buhaftafilmyorumu.yorumlar.yorumsaati = DateTime.Now.ToShortTimeString();
            db.Yorumlar.Add(buhaftafilmyorumu.yorumlar);
            db.SaveChanges();
            buhaftafilmyorumlari.filmid = buhaftafilmyorumu.filmid;
            buhaftafilmyorumlari.yorumid = buhaftafilmyorumu.yorumlar.yorumid;
            db.BuHaftaFilmYorumlari.Add(buhaftafilmyorumlari);
            db.SaveChanges();
            return true;
        }
        [Route("api/PostListeliFilmYorumuYap")]
        [HttpPost]
        public bool PostListeliFilmYorumuYap([FromBody]BuHaftaFilmYorumYap listelifilminyorumu)
        {

            ListeliFilmYorumları listelifilmyorumlaritablosu = new ListeliFilmYorumları();
            listelifilminyorumu.yorumlar.yorumtarihi = DateTime.Now.ToString("d");
            listelifilminyorumu.yorumlar.yorumsaati = DateTime.Now.ToShortTimeString();
            db.Yorumlar.Add(listelifilminyorumu.yorumlar);
            db.SaveChanges();
            listelifilmyorumlaritablosu.filmid = listelifilminyorumu.filmid;
            listelifilmyorumlaritablosu.yorumid = listelifilminyorumu.yorumlar.yorumid;
            db.ListeliFilmYorumları.Add(listelifilmyorumlaritablosu);
            db.SaveChanges();
            return true;
        }
        //KULLANİCİ KAYİT
        [Route("api/PostKullaniciKayit")]
        [HttpPost]
        public bool PostKullanicilar(Kullanicilar kullanicilar)
        {
            var check = db.Kullanicilar.FirstOrDefault(x => x.kullaniciadi == kullanicilar.kullaniciadi);

            if (!ModelState.IsValid)
            {
                return false;
            }
            else if (check != null)
            {
                return false;
            }
            else
            {
                kullanicilar.kullaniciyetki = "User";
                db.Kullanicilar.Add(kullanicilar);
                db.SaveChanges();
                return true;
            }
           
        }

        //KULLANİCİ BİLGİLERİ
        [HttpGet]
        [Route("api/GetUserClaims")]
        public Kullanicilar GetUserClaims()
        {
            var identityClaims = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identityClaims.Claims;
            Kullanicilar model = new Kullanicilar()
            {
                kullaniciadi = identityClaims.FindFirst("kullaniciadi").Value,
                kullaniciid = Convert.ToInt32(identityClaims.FindFirst("kullaniciid").Value)
            };
            return model;
        }
        //HABERLERİ GETİR
        [Route("api/GetHaberleriGetir")]
        [HttpGet]
        public IQueryable<Haberler> GetHaberleriGetir()
        {
            return db.Haberler;
        }
        //HABER İCERİĞİNİ GETİR 
        [Route("api/GetHaberIcerikGetir/{id}")]
        [HttpGet]
        public IHttpActionResult GetHaberIcerikGetir(int id)
        {
            Haberler habericerik = db.Haberler.Find(id);
            if (habericerik == null)
            {
                return NotFound();
            }

            return Ok(habericerik);
        }
        //BU HAFTA FİLMLERİ GETİR
        [Route("api/GetBuHaftakiFilmler/{id}")]
        [HttpGet]
        public IHttpActionResult GetBuHaftakiFilmler(int id)
        {
            BuHaftakiFilmler buHaftakiFilmler = db.BuHaftakiFilmler.Find(id);
            if (buHaftakiFilmler == null)
            {
                return NotFound();
            }

            return Ok(buHaftakiFilmler);
        }
        //GELECEK FİLMLER
        [Route("api/GetGelecekHaftakiFilmler/{id}")]
        [HttpGet]
        public IHttpActionResult GetGelecekHaftakiFilmler(int id)
        {
            GelecekHaftakiFilmler gelecekHaftakiFilmler = db.GelecekHaftakiFilmler.Find(id);
            if (gelecekHaftakiFilmler == null)
            {
                return NotFound();
            }

            return Ok(gelecekHaftakiFilmler);
        }
        //LİSTELİ FİLMLER
        [Route("api/GetListeliFilmler/{id}")]
        [HttpGet]
        public IHttpActionResult GetListeliFilmler(int id)
        {
            ListeliFilmler listeliFilmler = db.ListeliFilmler.Find(id);
            if (listeliFilmler == null)
            {
                return NotFound();
            }

            return Ok(listeliFilmler);
        }
        //YÜZDE HESAPLAMA
        [Route("api/GetKategorilereGöreFilmSayiYuzdeleri")]
        [HttpGet]
        public List<KategorilereGöreFilmYüzdeleri> GetKategorilereGöreFilmSayiYuzdeleri()
        {
            List<KategorilereGöreFilmYüzdeleri> filmyuzdeligi = new List<KategorilereGöreFilmYüzdeleri>();
            var kategorifilmleri = db.Kategoriler.ToList();
            int film_sayisi = Convert.ToInt32(db.Kategoriler.Sum(x => x.kategorifilmsayisi));
           
            for(int i=0;i<kategorifilmleri.Count;i++)
                {
                filmyuzdeligi.Add(new KategorilereGöreFilmYüzdeleri { kategoriadi = kategorifilmleri[i].kategoriadi, kategorifilmyuzdesi = Convert.ToInt32((kategorifilmleri[i].kategorifilmsayisi * 100)/film_sayisi) });
                }
                    return filmyuzdeligi;
        }
        //ANASAYFA SLİDER LİSTELEME
        [Route("api/GetSlider")]
        [HttpGet]
        public IEnumerable<Slider> GetSlider()
        {   
            var query= db.Slider.ToList();


            return query;
        }
        [Route("api/GetSliderIcerik/{id}")]
        [HttpGet]
        public List<SliderIcerik> GetSliderIcerik(int id)
        {
            return db.SliderIcerik.Where(x=>x.sliderid==id).ToList();
        }
        [Route("api/GetSliderVerileriGetir/{id}")]
        [HttpGet]
        public Slider GetSliderVerileriGetir(int id)
        {
            var query= db.Slider.FirstOrDefault(x=>x.sliderid==id);
            return query;
        }
        //ANASAYFA FİLM ELESTİRİLERİ LİSTELEME
        [Route("api/GetSuperUserFilmElestirileri")]
        [HttpGet]
        public List<SuperUserElestirileri> GetSuperUserFilmElestirileri()
        {
            List<SuperUserElestirileri> superuserelestiri = db.FilmElestirileri.Select(u => new SuperUserElestirileri
            {
                elestiriid = u.elestiriid,
                filmadi = u.ListeliFilmler.filmadi,
                kullaniciadi = u.Kullanicilar.kullaniciadi,
                filmresim=u.ListeliFilmler.filmresim
            }).ToList();
            return superuserelestiri;
        }
        // FİLM ELESTİRİ ICERİGİ
        [Route("api/GetSuperUserFilmElestiriIcerik/{id}")]
        [HttpGet]
        public SuperUserElestirileri GetSuperUserFilmElestiriIcerik(int id)
        {
            SuperUserElestirileri superuserelestiri = db.FilmElestirileri.Select(u => new SuperUserElestirileri
            {
                elestiriid = u.elestiriid,
                filmadi = u.ListeliFilmler.filmadi,
                kullaniciadi = u.Kullanicilar.kullaniciadi,
                filmresim = u.ListeliFilmler.filmresim,
                elestiriicerik=u.elestiriicerik
            }).FirstOrDefault(x=>x.elestiriid==id);
            return superuserelestiri;
        }
        [Route("api/GetListeliFilmlerinSayisi")]
        [HttpGet]
        public int GetListeliFilmlerinSayisi()
        {
            return db.ListeliFilmler.Count();
        }
        [Route("api/GetListeliFilmKayitlari/{pageNumber}/{pageCount}")]
        [HttpGet]
        public IQueryable<ListeliFilmler> GetListeliFilmKayitlari(int pageNumber, int pageCount)
        {
            if (pageNumber == 1)
            {
                var pagedProductQuery = db.ListeliFilmler.OrderBy(x => x.filmadi).Take(pageCount);
                return pagedProductQuery;
            }
            else
            {
                var pagedProductQuery = db.ListeliFilmler.OrderBy(x => x.filmadi).Skip((pageNumber*pageCount)-pageCount).Take(pageCount);
                return pagedProductQuery;
            }
        }

    }

}
