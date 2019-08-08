using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ServiciosClaro.Models;

namespace ServiciosClaro.Controllers
{
    [AllowAnonymous]
    public class CuentasController : Controller
    {
        // GET: Cuentas
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Login(Cuenta c, string returnUrl)
        {
            using (var db = new ServiciosClaroEntities())
            {
                bool valido = db.Cuentas.Any(e => e.Usuario == c.Usuario && e.Clave == c.Clave);

                if (valido)
                {
                    FormsAuthentication.SetAuthCookie(c.Usuario, false);

                    var res = (from r in db.Roles
                               join rc in db.RolCuentas on r.Id equals rc.Rol
                               join cl in db.Cuentas on rc.Cuenta equals cl.Id
                               where cl.Usuario == c.Usuario
                               select r.RolName).ToArray();

                    switch (res[0])
                    {
                        case "Admin":

                            var ida = (from a in db.Empleados
                                       join u in db.Cuentas on a.Id equals u.Id
                                       where u.Usuario == c.Usuario
                                       select a);

                            foreach (var item in ida)
                            {
                                Session["ID"] = item.Id;
                            }

                            if (string.IsNullOrEmpty(returnUrl))
                            {
                                return RedirectToAction("Index", "Empleados");
                            }
                            else
                            {
                                return Redirect(returnUrl);
                            }
                            
                            break;

                        case "Empleado":

                            //var ide = (from e in db.Empleados
                            //           join u in db.Login on e.IDUsuario equals u.ID
                            //           where u.Usuario == c.Usuario
                            //           select e);

                            //foreach (var item in ide)
                            //{
                            //    Session["ID"] = item.ID;
                            //}

                            //return RedirectToAction("Index", "Equipos_Reparacion");
                            break;

                        case "Cliente":

                            var idc = (from cl in db.Clientes
                                       join u in db.Cuentas on cl.Id equals u.Id
                                       where u.Usuario == c.Usuario
                                       select cl);

                            foreach (var item in idc)
                            {
                                Session["ID"] = item.Id;
                            }

                            if (string.IsNullOrEmpty(returnUrl))
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else
                            {
                                return Redirect(returnUrl);
                            }
                            
                            break;

                    }

                }

                ModelState.AddModelError("", "Usuario o contraseña incorrecta!");

            }


            return View();
        }


        public ActionResult Registrar()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Registrar(RegistrarCliente r)
        {

            using (var db = new ServiciosClaroEntities())
            {

                if (ModelState.IsValid)
                {
                    db.Cuentas.Add(new Cuentas()
                    {
                        Usuario = r.Usuario,
                        Clave = r.Clave
                    });

                    db.SaveChanges();

                    var idcuenta = from c in db.Cuentas
                                   where c.Usuario == r.Usuario && c.Clave == r.Clave
                                   select c.Id;

                    int id = int.MinValue;

                    foreach (var item in idcuenta)
                    {
                        id = item;
                    }

                    db.Clientes.Add(new Clientes()
                    {
                        Nombre = r.Nombre,
                        Direccion = r.Direccion,
                        Telefono = r.Telefono,
                        Email = r.Email,
                        Cuenta = id

                    });


                    db.RolCuentas.Add(new RolCuentas()
                    {
                        Cuenta = id,
                        Rol = 3
                    });


                    db.SaveChanges();

                    FormsAuthentication.SetAuthCookie(r.Usuario, false);
                    return RedirectToAction("Index", "Home");

                }

               

            }

            ModelState.AddModelError("", "Complete los campos correctamente");
            return View(r);

            
        }


        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}