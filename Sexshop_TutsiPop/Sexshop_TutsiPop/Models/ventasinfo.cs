﻿using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class ventasinfo
    {
        [Key]
        public int id_venta { get; set; }

        [Required]
        public string cedula_cliente { get; set; }

        [Required]
        public string cedula_empleado { get; set; }

        [Required]
        public DateTime fecha_venta { get; set; } = DateTime.Now;

        public string metodo_pago { get; set; }
    }
}
