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
    [Authorize]
    public class RecargasController : Controller
    {
        private ServiciosClaroEntities db = new ServiciosClaroEntities();
        //private AsignacionTecnico t = new AsignacionTecnico();

        public Empleados Empleado()
        {
            var estado = (from e in db.Empleados
                          where e.Estado == "Trabajando" && (e.Puesto == 1 || e.Puesto == 2)
                          select e);

            var arrayEmpleado = estado.ToArray();

            int cant = estado.Count();

            int[] cantTask = new int[cant];

            Empleados menor = new Empleados();

            int m = int.MinValue;

            for (int i = 0; i < cant; i++)
            {
                int b = arrayEmpleado[i].Id;

                var tareas = (from e in db.TareasEmpleados
                              where e.Empleado == b && e.Estado == "En Espera"
                              select e).ToArray();

                int a = tareas.Length;

                cantTask[i] = a;
            }
            for (int i = 0; i < cant; i++)
            {

                if (i == 0)
                {
                    menor = arrayEmpleado[0];
                    m = cantTask[0];
                }
                else
                {
                    if (cantTask[i] < m)
                    {
                        menor = arrayEmpleado[i];
                        m = cantTask[i];
                    }
                }
            }



            return menor;
        }

        // GET: Recargas
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var recargas = db.Recargas.Include(r => r.Clientes).Include(r => r.Tareas);
            return View(recargas.ToList());
        }

        [Authorize(Roles = "Cliente")]
        public ActionResult IndexC()
        {
            int id = Convert.ToInt32(Session["ID"]);

            var recargas = from t in db.Tareas join
                     r in db.Recargas on t.Id equals r.Tarea
                     where r.Cliente == id
                     select r;


            return View(recargas.ToList());
        }

        [Authorize(Roles = "Empleado")]
        public ActionResult IndexE()
        {
            int id = Convert.ToInt32(Session["ID"]);

            var recargas = from te in db.TareasEmpleados
                           join
      t in db.Tareas on te.Tarea equals t.Id
                           join
      r in db.Recargas on t.Id equals r.Tarea
                           where t.Id == 1 && te.Empleado == id && te.Estado == "En Espera" && r.Tarea == 1
                           select r;


            return View(recargas.ToList());
        }

        // GET: Recargas/Details/5
        [Authorize(Roles = "Admin,Cliente")]
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
        [Authorize(Roles = "Admin,Cliente")]
        public ActionResult Create()
        {
            ViewBag.Cliente = new SelectList(db.Clientes, "Id", "Nombre");
            ViewBag.Tarea = new SelectList(db.Tareas, "Id", "Tarea");
            return View();
        }

        // POST: Recargas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Cliente")]
        public ActionResult Create([Bind(Include = "Id,Lugar,Precio,Celular,Cliente,Tarea")] Recargas recargas)
        {
            if (ModelState.IsValid)
            {

                db.TareasEmpleados.Add(new TareasEmpleados()
                {
                    Tarea = 1,
                    Empleado = Empleado().Id,
                    Estado = "En Espera"
                });

                if (!User.IsInRole("Admin"))
                {
                    int cli = Convert.ToInt32(Session["ID"]);
                    recargas.Cliente = cli;
                }

                db.Recargas.Add(recargas);
                db.SaveChanges();

                if (User.IsInRole("Cliente"))
                {
                    return RedirectToAction("IndexC");
                }
                return RedirectToAction("Index");
            }

            ViewBag.Cliente = new SelectList(db.Clientes, "Id", "Nombre", recargas.Cliente);
            ViewBag.Tarea = new SelectList(db.Tareas, "Id", "Tarea", recargas.Tarea);
            return View(recargas);
        }

        // GET: Recargas/Edit/5
        [Authorize(Roles = "Admin,Empleado")]
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
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Empleado")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
