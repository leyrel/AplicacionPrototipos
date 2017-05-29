using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Prototipos.Models.DB;

namespace Prototipos.Models.ViewModels
{
    [Bind(Exclude = "IdPrototipo, FechaCreacion, IdUsuarioCreador, Estado, FechaCierre, IdUsuarioCierre, Cierre")]
    public class ProyectoView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public ProyectoView() { }

        [ScaffoldColumn(false)]
        public int IdPrototipo { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaCreacion { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioCreador { get; set; }

        [Required(ErrorMessage = "Tiene que especificar un nombre para el proyecto")]
        [Display(Name = "Nombre del proyecto:")]
        public string Proyecto { get; set; }

        [Required(ErrorMessage = "Tiene que especificar un responsable de I+D")]
        [Display(Name = "Responsable I+D:")]
        public string Responsable { get; set; }

        [Required(ErrorMessage = "Tiene que especificar la configuración original del montaje del prototipo")]
        [Display(Name = "Configuración inicial del montaje:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Configuracion { get; set; }

        [Display(Name = "Descripción del proyecto:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [Display(Name = "Observaciones:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Observaciones { get; set; }

        [ScaffoldColumn(false)]
        public int Estado { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? FechaCierre { get; set; }

        [ScaffoldColumn(false)]
        public int IdUsuarioCierre { get; set; }

        [ScaffoldColumn(false)]
        public string Cierre { get; set; }
    }
}