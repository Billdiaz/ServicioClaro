using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiciosClaro.Models
{
    public class Recarga
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Lugar { get; set; }

        [Required]
        public decimal Precio { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Celular { get; set; }

        [Required]
        public int Cliente { get; set; }

        public int Tarea { get; set; }

        public Clientes Clientes { get; set; }
        public Tareas Tareas { get; set; }
    }
}