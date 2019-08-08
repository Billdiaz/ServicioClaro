using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServiciosClaro.Report;

namespace ServiciosClaro.Controllers
{
    public class FacturasController : Controller
    {
        private ServiciosClaroEntities db = new ServiciosClaroEntities();
        // GET: Facturas
        public ActionResult Index(Recargas recarga)

        {
            RecargasReport recargasReport = new RecargasReport();
            byte[] abytes = recargasReport.PrepareReport(GetRecargas());



            return File(abytes, "application/pdf");
        }

        public List<Recargas> GetRecargas() {
            List<Recargas> recarga = db.Recargas.ToList();
            return recarga;
        }


    }



}