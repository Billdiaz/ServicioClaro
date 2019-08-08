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
    public class TareasEmpleadosController : Controller
    {
        private ServiciosClaroEntities db = new ServiciosClaroEntities();

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TareasEmpleados tareasEmpleados = db.TareasEmpleados.Find(id);
            if (tareasEmpleados == null)
            {
                return HttpNotFound();
            }
            ViewBag.Empleado = new SelectList(db.Empleados, "Id", "Nombre", tareasEmpleados.Empleado);
            ViewBag.Tarea = new SelectList(db.Tareas, "Id", "Tarea", tareasEmpleados.Tarea);
            return View(tareasEmpleados);
        }

        // POST: TareasEmpleados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Tarea,Empleado,Estado")] TareasEmpleados tareasEmpleados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tareasEmpleados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexE", "Recargas", null);
            }
            ViewBag.Empleado = new SelectList(db.Empleados, "Id", "Nombre", tareasEmpleados.Empleado);
            ViewBag.Tarea = new SelectList(db.Tareas, "Id", "Tarea", tareasEmpleados.Tarea);
            return View(tareasEmpleados);
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
