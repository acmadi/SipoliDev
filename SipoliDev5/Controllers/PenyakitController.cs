using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SipoliDev5.Models;
using SipoliDev5.Models.ViewModels;

using System.Data.Entity.Infrastructure;

using Rotativa;

using PagedList;
using PagedList.Mvc;
using System.Threading.Tasks;

namespace SipoliDev5.Controllers
{
    [Authorize]
    public class PenyakitController : Controller
    {
        private EntitiesConnection db = new EntitiesConnection();
        private List<PenyakitView> listPenyakit = new List<PenyakitView>();
        private List<KelainanBawaanCons> listKelainan = new List<KelainanBawaanCons>();


        // GET: Penyakit
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult DaftarPenyakitIndex(string sortOrder, string searchString,
            string currentFilter, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Kelompok" ? "kelompok_desc" : "Kelompok";

            var j = 1;
            var data = (from p in db.Penyakit
                        join k in db.KelompokPenyakit on p.KelompokPenyakitID equals k.ID
                        select new
                        {
                            ID = p.ID,
                            Nama = p.Nama,
                            NamaIlmiah = p.NamaIlmiah,
                            KelompokPenyakitID = p.KelompokPenyakitID,
                            KelompokPenyakit = p.KelompokPenyakit
                        }).Distinct();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                data = data.Where(s => s.Nama.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    data = data.OrderByDescending(s => s.Nama);
                    break;
                case "Kelompok":
                    data = data.OrderBy(s => s.KelompokPenyakit);
                    break;
                case "kelompok_desc":
                    data = data.OrderByDescending(s => s.KelompokPenyakit);
                    break;
                default:
                    data = data.OrderBy(s => s.Nama);
                    break;
            }

            foreach (var item in data)
            {
                listPenyakit.Add(new PenyakitView
                {
                    No = j,
                    ID = item.ID,
                    Nama = item.Nama,
                    NamaIlmiah = item.NamaIlmiah,
                    KelompokPenyakit = item.KelompokPenyakit,
                    KelompokPenyakitID = item.KelompokPenyakitID,
                });
                j++;
            }

            return PartialView(listPenyakit.ToPagedList(page ?? 1, 10));
        }

        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult DataPenyakitReport()
        {
            return new ViewAsPdf("DataPenyakitReport","Empty",null);
        }

        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public PartialViewResult _DaftarPenyakitReport()
        {
            
            var j = 1;
            var data = (from p in db.Penyakit
                        join k in db.KelompokPenyakit on p.KelompokPenyakitID equals k.ID
                        select new
                        {
                            ID = p.ID,
                            Nama = p.Nama,
                            NamaIlmiah = p.NamaIlmiah,
                            KelompokPenyakitID = p.KelompokPenyakitID,
                            KelompokPenyakit = p.KelompokPenyakit
                        }).Distinct();


            foreach (var item in data)
            {
                listPenyakit.Add(new PenyakitView
                {
                    No = j,
                    ID = item.ID,
                    Nama = item.Nama,
                    NamaIlmiah = item.NamaIlmiah,
                    KelompokPenyakit = item.KelompokPenyakit,
                    KelompokPenyakitID = item.KelompokPenyakitID,
                });
                j++;
            }

            return PartialView(listPenyakit.ToList());
        }

        /**/
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult _DaftarPenyakitStat()
        {
            var data = (from r in db.RiwayatPenyakit
                       group r by r.PenyakitID into penyakitGroup
                       select new
                       {
                           PenyakitID = penyakitGroup.Key,
                           PasienCount = penyakitGroup.Count()
                       }).OrderByDescending(p => p.PasienCount).Take(5);

            var j = 1;
            List<PenyakitStat> listPenyakit = new List<PenyakitStat>();

            foreach (var item in data)
            {
                listPenyakit.Add(new PenyakitStat((int)j, item.PenyakitID, NamaPenyakit(item.PenyakitID), item.PasienCount));
                j++;
            }
            
            return PartialView(listPenyakit.ToList());

        }

        public string NamaPenyakit(int? p)
        {
            string namaPenyakit = db.Penyakit.Where(d => d.ID == p).Select(n => n.Nama).Single().ToString();
            return namaPenyakit;
        }

        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public PartialViewResult _DaftarKelainanReport()
        {
            var data = (from k in db.KelainanBawaan1
                        select new
                        {
                            ID = k.ID,
                            Nama = k.Nama,
                            NamaIlmiah = k.NamaIlmiah
                        }).Distinct();

            var j = 1;
            foreach (var item in data)
            {
                listKelainan.Add(new KelainanBawaanCons((int)j, item.ID, item.Nama, item.NamaIlmiah));
                j++;
            }

            return PartialView(listKelainan.ToList());
        }

        /**/
        [Authorize(Roles = "Admin,Staf,Pemeriksa")]
        public ActionResult _DaftarKelainanStat()
        {
            var data = (from k in db.KelainanBawaan
                        group k by k.KelainanBawaanID into kelainanGroup
                        select new
                        {
                            kelainanID = kelainanGroup.Key,
                            PasienCount = kelainanGroup.Count()
                        }).OrderByDescending(p => p.PasienCount).Take(5);

            var j = 1;
            List<KelainanStat> listKelainan = new List<KelainanStat>();

            foreach (var item in data)
            {
                listKelainan.Add(new KelainanStat((int)j, item.kelainanID, NamaKelainan(item.kelainanID), item.PasienCount));
                j++;
            }

            return PartialView(listKelainan.ToList());

        }

        public string NamaKelainan(int? p)
        {
            string NamaKelainan = db.KelainanBawaan1.Where(k => k.ID == p).Select(n => n.Nama).Single().ToString();
            return NamaKelainan;
        }

        public PartialViewResult KelainanIndex(int? page)
        {
            var data = (from k in db.KelainanBawaan1
                        select new
                        {
                            ID = k.ID,
                            Nama = k.Nama,
                            NamaIlmiah = k.NamaIlmiah
                        }).Distinct();

            var j = 1;
            foreach (var item in data)
            {
                listKelainan.Add(new KelainanBawaanCons((int)j, item.ID, item.Nama, item.NamaIlmiah));
                j++;
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return PartialView(listKelainan.ToPagedList(pageNumber, pageSize));
        }

        // GET: Penyakits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Penyakit penyakit = db.Penyakit.Find(id);
            if (penyakit == null)
            {
                return HttpNotFound();
            }
            return View(penyakit);
        }

        // GET: Penyakits/Create
        /*
        public ActionResult Create()
        {
            ViewBag.KelompokPenyakitID = new SelectList(db.KelompokPenyakit, "ID", "Nama");
            return View();
        }
         */ 

        public PartialViewResult _Create()
        {
            ViewBag.KelompokPenyakitID = new SelectList(db.KelompokPenyakit, "ID", "Nama");

            return PartialView();
        }


        // POST: Penyakits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Create([Bind(Include = "ID,Nama,NamaIlmiah,KelompokPenyakitID")] PenyakitView penyakit)
        {
            
            if (ModelState.IsValid)
            {
                Penyakit data = new Penyakit();
                data.ID = penyakit.ID;
                data.Nama = penyakit.Nama;
                data.NamaIlmiah = penyakit.NamaIlmiah;
                data.KelompokPenyakitID = penyakit.KelompokPenyakitID;

                db.Penyakit.Add(data);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KelompokPenyakitID = new SelectList(db.KelompokPenyakit, "ID", "Nama", penyakit.KelompokPenyakitID);
            return View(penyakit);
        }

       
        public PartialViewResult _CreateKelainan()
        {
            return PartialView();
        }

        // POST: KelainanBawaan1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CreateKelainan([Bind(Include = "ID,Nama,NamaIlmiah")] KelainanBawaan1 kelainanBawaan1)
        {
            if (ModelState.IsValid)
            {
                db.KelainanBawaan1.Add(kelainanBawaan1);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(kelainanBawaan1);
        }


        // GET: Penyakits/Edit/5
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var penyakit = db.Penyakit.Find(id);
            if (penyakit == null)
            {
                return HttpNotFound();
            }
            ViewBag.KelompokPenyakitID = new SelectList(db.KelompokPenyakit, "ID", "Nama", penyakit.KelompokPenyakitID);
            PenyakitCons penyakitEdit = new PenyakitCons((int)penyakit.ID, penyakit.Nama, penyakit.NamaIlmiah, (int)penyakit.KelompokPenyakitID);

            return View(penyakitEdit);
        }

        // POST: Penyakits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        /* COBA CONCURRENCY
        public async Task<ActionResult> Edit(
            [Bind(Include = "ID,Nama,NamaIlmiah,KelompokPenyakitID")] 
            Penyakit penyakit){
            try{
                if(ModelState.IsValid){
                    db.Entry(penyakit).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch(DbUpdateConcurrencyException ex){
                var entry = ex.Entries.Single();
                var clientValues = (Penyakit)entry.Entity;
                var databaseEntry = entry.GetDatabaseValues();

                if(databaseEntry == null){
                    ModelState.AddModelError(string.Empty,
                        "Tidak bisa mengubah data. Data penyakit sudah dihapus oleh pengguna lain.");
                }
                else{
                    var databaseValues = (Penyakit)databaseEntry.ToObject();

                    if(databaseValues.Nama != clientValues.Nama)
                        ModelState.AddModelError("Nama","Nama Sekarang: "
                            + databaseValues.Nama);
                    if(databaseValues.NamaIlmiah != clientValues.NamaIlmiah)
                        ModelState.AddModelError("NamaIlmiah", "Nama Ilmiah: "
                            + databaseValues.NamaIlmiah);
                    if (databaseValues.KelompokPenyakitID != clientValues.KelompokPenyakitID)
                        ModelState.AddModelError("KelompokPenyakitID", "Kelompok: "
                            + db.KelompokPenyakit.Find(databaseValues.KelompokPenyakitID));

                    ModelState.AddModelError(string.Empty, "Data yang anda edit sudah di modifikasi terlebih dahulu oleh pengguna lain. Operasi Edit dibatalkan.");
                    penyakit.RowVersion = databaseValues.RowVersion;
                }
            }
            catch(RetryLimitExceededException /* dex)
            {
                //Log the error
                ModelState.AddModelError(string.Empty,"Gagal menyimpan perubahan data. Jika error tetap terjadi, silahkan hubungi Administrator sistem.");
            }
            return View(penyakit);
        } 
         */

        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        public ActionResult Edit([Bind(Include = "ID,Nama,NamaIlmiah,KelompokPenyakitID")] Penyakit penyakit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(penyakit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KelompokPenyakitID = new SelectList(db.KelompokPenyakit, "ID", "Nama", penyakit.KelompokPenyakitID);
            return View(penyakit);
        }


        // GET: KelainanBawaan1/Edit/5
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        public ActionResult EditKelainan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var kelainanBawaan1 = db.KelainanBawaan1.Find(id);
            if (kelainanBawaan1 == null)
            {
                return HttpNotFound();
            }
            KelainanBawaanCons kelainanBawaanEdit = new KelainanBawaanCons(null, kelainanBawaan1.ID,
                kelainanBawaan1.Nama, kelainanBawaan1.NamaIlmiah);

            return View(kelainanBawaanEdit);
        }

        // POST: KelainanBawaan1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditKelainan([Bind(Include = "ID,Nama,NamaIlmiah")] KelainanBawaan1 kelainanBawaan1)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kelainanBawaan1).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kelainanBawaan1);
        }


        // GET: Penyakits/Delete/5
        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        public ActionResult Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var riwayatPenyakit = db.RiwayatPenyakit.Where(p => p.PenyakitID == id).ToList();
            var riwayatPenyakitKeluarga = db.RiwayatPenyakitKeluarga.Where(p => p.PenyakitID == id).ToList();
            if ((riwayatPenyakit != null) || ((riwayatPenyakitKeluarga != null)))
            {
                db.RiwayatPenyakit.RemoveRange(riwayatPenyakit);
                db.RiwayatPenyakitKeluarga.RemoveRange(riwayatPenyakitKeluarga);
                db.SaveChanges();
            }

            Penyakit penyakit = db.Penyakit.Find(id);
            if (penyakit == null)
            {
                return HttpNotFound();
            }
            db.Penyakit.Remove(penyakit);
            db.SaveChanges();

            return RedirectToAction("DaftarPenyakitIndex");
        }

        [Authorize(Roles = "Admin,Dokter,Pemeriksa")]
        public ActionResult DeleteKelainan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var kelainanBawaan = db.KelainanBawaan.Where(k => k.KelainanBawaanID == id);
            if (kelainanBawaan != null)
            {
                db.KelainanBawaan.RemoveRange(kelainanBawaan);
                db.SaveChanges();
            }

            KelainanBawaan1 kelainanBawaan1 = db.KelainanBawaan1.Find(id);
            if (kelainanBawaan1 == null)
            {
                return HttpNotFound();
            }
            db.KelainanBawaan1.Remove(kelainanBawaan1);
            db.SaveChanges();

            return RedirectToAction("Index");
        }



        // POST: Penyakits/Delete/5
        /*LAMA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Penyakit penyakit)
        {
            try
            {
                db.Entry(penyakit).State = EntityState.Deleted;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DataException) {
                //Log Error, uncomm dex variable, and add a line here
                ModelState.AddModelError(string.Empty, "Tidak bisa menghapus data. Coba lagi, jika terjadi Error kembali, silahkan hubungi Administrator sistem.");
                return View(penyakit);
            }
            
        }
         */


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
