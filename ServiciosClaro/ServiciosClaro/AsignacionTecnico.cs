using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiciosClaro
{
    public class AsignacionTecnico
    {
        public int Id;
        private ServiciosClaroEntities db = new ServiciosClaroEntities();
        public int TrabajosPendientes;
        public int NumeroDePersonas;



        public int EmpleadoMenorTrabajos() {
             
            Empleados empleados = db.Empleados.ToList();
            
        }

        public int TurnoEspera()
        {


            return NumeroDePersonas;
        }

    }
}