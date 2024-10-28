using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sexshop_TutsiPop.Models;

namespace Sexshop_TutsiPop.Data
{
    public class Sexshop_TutsiPopContext : DbContext
    {
        public Sexshop_TutsiPopContext (DbContextOptions<Sexshop_TutsiPopContext> options)
            : base(options)
        {
        }
        public DbSet<Sexshop_TutsiPop.Models.entregasInfo> entregasInfo { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.detalleventaInfo> detalleventasInfo { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.detallepedidoInfo> detallepedidoInfo { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.productosInfo> productosInfo { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.pedidosinfo> pedidoinfos { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.ProveedoresDireccion> ProveedoresDireccion { get; set; } 
        public DbSet<Sexshop_TutsiPop.Models.usuarios> usuarios { get; set; } = default!;
        public DbSet<Sexshop_TutsiPop.Models.estado_pedido> estado_pedido { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.categorias> categorias { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.metodos_pago> metodos_pago { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.direcciones> direcciones { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.Proveedores> proveedores { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.productos> productos { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.pedidos> pedidos { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.detalle_pedido> detalle_pedido { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.clientes> clientes { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.empleados> empleados { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.ventas> ventas { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.detalle_venta> detalle_venta { get; set; }
        public DbSet<Sexshop_TutsiPop.Models.entregas> entregas { get; set; }


    }
}
