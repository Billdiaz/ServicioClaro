using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ServiciosClaro.Models
{
    public class Cuenta
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60)]
        public string Usuario { get; set; }

        [Required]
        [StringLength(30)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Clave { get; set; }
    }
}