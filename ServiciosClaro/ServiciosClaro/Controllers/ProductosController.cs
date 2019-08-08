using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ServiciosClaro;
using System.IO;

namespace ServiciosClaro.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private ServiciosClaroEntities db = new ServiciosClaroEntities();

        [AllowAnonymous]
        public ActionResult MImagen(int id)
        {
            try
            {
                var imagen = (from c in db.Productos
                              where c.Id == id
                              select c.Imagen).FirstOrDefault();

                return File(imagen, "Imagenes/jpg");
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        [AllowAnonymous]
        // GET: Productos
        public ActionResult Index()
        {
            return View(db.Productos.ToList());
        }

        [AllowAnonymous]
        // GET: Productos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        [Authorize(Roles = "Admin")]
        // GET: Productos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(ServiciosClaro.Models.Producto p, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                if (Foto != null && Foto.ContentLength > 0)
                {
                    byte[] datosImagen = null;

                    using (var img = new BinaryReader(Foto.InputStream))
                    {
                        datosImagen = img.ReadBytes(Foto.ContentLength);
                    }

                    p.Imagen = datosImagen;

                    db.Productos.Add(new Productos()
                    {
                        Producto = p.Nombre,
                        Descripcion = p.Descripcion,
                        Cantidad = p.Cantidad,
                        Precio = p.Precio,
                        Imagen = p.Imagen
                    });

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError("", "Complete los campos Correctamente");

            return View(p);
        }


        [Authorize(Roles = "Admin")]
        // GET: Productos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(Productos productos, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                if (Foto != null && Foto.ContentLength > 0)
                {

                    byte[] datosImagen = null;

                    using (var img = new BinaryReader(Foto.InputStream))
                    {
                        datosImagen = img.ReadBytes(Foto.ContentLength);
                    }

                    productos.Imagen = datosImagen;

                    db.Entry(productos).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {

                    var imagen = (from c in db.Productos
                                  where c.Id == productos.Id
                                  select c.Imagen).FirstOrDefault();

                    productos.Imagen = imagen;

                    db.Entry(productos).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                
                
                
            }
            return View(productos);
        }

        [Authorize(Roles = "Admin")]
        // GET: Productos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = db.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Productos productos = db.Productos.Find(id);
            db.Productos.Remove(productos);
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
