using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ServiciosClaro;

namespace ServiciosClaro.Controllers
{
    public class RecargasController : Controller
    {
        private ServiciosClaroEntities db = new ServiciosClaroEntities();

        // GET: Recargas
        public ActionResult Index()
        {
            var recargas = db.Recargas.Include(r => r.Clientes).Include(r => r.Tareas);
            return View(recargas.ToList());
        }

        // GET: Recargas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recargas recargas = db.Recargas.Find(id);
            if (recargas == null)
            {
                return HttpNotFound();
            }
            return View(recargas);
        }

        // GET: Recargas/Create
        public ActionResult Create()
        {
            ViewBag.Cliente = new SelectList(db.Clientes, "Id", "Nombre");
            ViewBag.Tarea = new SelectList(db.Tareas, "Id", "Tarea");
            return View();
        }

        // POST: Recargas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Lugar,Precio,Celular,Cliente,Tarea")] Recargas recargas)
        {
            if (ModelState.IsValid)
            {
                db.Recargas.Add(recargas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Cliente = new SelectList(db.Clientes, "Id", "Nombre", recargas.Cliente);
            ViewBag.Tarea = new SelectList(db.Tareas, "Id", "Tarea", recargas.Tarea);
            return View(recargas);
        }

        // GET: Recargas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recargas recargas = db.Recargas.Find(id);
            if (recargas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cliente = new SelectList(db.Clientes, "Id", "Nombre", recargas.Cliente);
            ViewBag.Tarea = new SelectList(db.Tareas, "Id", "Tarea", recargas.Tarea);
            return View(recargas);
        }

        // POST: Recargas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Lugar,Precio,Celular,Cliente,Tarea")] Recargas recargas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recargas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cliente = new SelectList(db.Clientes, "Id", "Nombre", recargas.Cliente);
            ViewBag.Tarea = new SelectList(db.Tareas, "Id", "Tarea", recargas.Tarea);
            return View(recargas);
        }

        // GET: Recargas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recargas recargas = db.Recargas.Find(id);
            if (recargas == null)
            {
                return HttpNotFound();
            }
            return View(recargas);
        }

        // POST: Recargas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recargas recargas = db.Recargas.Find(id);
            db.Recargas.Remove(recargas);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
