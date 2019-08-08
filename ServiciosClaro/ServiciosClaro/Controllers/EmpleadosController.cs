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
    [Authorize(Roles = "Admin")]
    public class EmpleadosController : Controller
    {
        private ServiciosClaroEntities db = new ServiciosClaroEntities();

        // GET: Empleados
        public ActionResult Index()
        {
            var empleados = db.Empleados.Include(e => e.Cuentas).Include(e => e.Puestos);
            return View(empleados.ToList());
            
        }

        // GET: Empleados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            return View(empleados);
        }

        // GET: Empleados/Create
        public ActionResult Create()
        {
            ViewBag.Puesto = new SelectList(db.Puestos, "Id", "Puesto");
            return View();
        }

        // POST: Empleados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ServiciosClaro.Models.RegistrarEmpleado e)
        {
            if (ModelState.IsValid)
            {
                db.Cuentas.Add(new Cuentas()
                {
                    Usuario = e.Usuario,
                    Clave = e.Clave
                });

                db.SaveChanges();

                var idcuenta = from c in db.Cuentas
                               where c.Usuario == e.Usuario && c.Clave == e.Clave
                               select c.Id;

                int id = int.MinValue;

                foreach (var item in idcuenta)
                {
                    id = item;
                }

                db.Empleados.Add(new Empleados()
                {
                    Nombre = e.Nombre,
                    Telefono = e.Telefono,
                    Puesto = e.Puesto,
                    Email = e.Email,
                    Cedula = e.Cedula,
                    FechaContratacion = e.FechaContratacion,
                    Cuenta = id
                });

                db.RolCuentas.Add(new RolCuentas()
                {
                    Cuenta = id,
                    Rol = 2
                });


                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Complete los campos correctamente");
            ViewBag.Puesto = new SelectList(db.Puestos, "Id", "Puesto", e.Puesto);
            return View(e);
        }

        // GET: Empleados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }

            ViewBag.Cuenta = new SelectList(db.Cuentas, "Id", "Usuario", empleados.Cuenta);
            ViewBag.Puesto = new SelectList(db.Puestos, "Id", "Puesto", empleados.Puesto);
            return View(empleados);
        }

        // POST: Empleados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,Telefono,Puesto,Email,Cedula,FechaContratacion,Cuenta")] Empleados empleados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleados).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Cuenta = new SelectList(db.Cuentas, "Id", "Usuario", empleados.Cuenta);
            ViewBag.Puesto = new SelectList(db.Puestos, "Id", "Puesto", empleados.Puesto);
            return View(empleados);
        }

        // GET: Empleados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            return View(empleados);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Empleados empleados = db.Empleados.Find(id);
            db.Empleados.Remove(empleados);
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
