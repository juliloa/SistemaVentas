using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;
using System.ComponentModel.DataAnnotations;

namespace Sexshop_TutsiPop.Models
{
    public class usuarios
    {
        [Key]public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string contrasenna { get; set; }
        public string token { get; set; }
        public bool confirmado { get; set; }
        public bool restablecer { get; set; }
        public string confirmar_contrasenna { get; set; }

    }
}
