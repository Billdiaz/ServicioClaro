using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ServiciosClaro.Models
{
    public class RegistrarCliente
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

        [Required]
        [StringLength(30)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Clave")]
        [NotMapped]
        public string ConfimarClave { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Cliente")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(300)]
        [Display(Name = "Drección")]
        public string Direccion { get; set; }

        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Display(Name = "Correo Electrónico")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}