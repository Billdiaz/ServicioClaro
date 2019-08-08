using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServiciosClaro
{
    public class AsignacionTecnico
    {
        private ServiciosClaroEntities db = new ServiciosClaroEntities();

        public Empleados Empleado()
        {
            var estado = (from e in db.Empleados
                          where e.Estado == "Trabjando"
                          select e).ToList();

            var arrayEmpleado = estado.ToArray();

            int cant = estado.Count();

            int[] cantTask = new int[cant];

            Empleados menor = new Empleados();

            int m = int.MinValue;

            for (int i = 0; i < cant; i++)
            {
                var tareas = db.TareasEmpleados.Where(e => e.Empleado == arrayEmpleado[i].Id && e.Estado == "En Espera");
                cantTask[i] = tareas.Count();
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

    }
}