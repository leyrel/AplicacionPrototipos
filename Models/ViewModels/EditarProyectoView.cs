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
    [Bind(Exclude = "IdPrototipo, Estado, FechaCierre, IdUsuarioCierre, Cierre")]
    public class EditarProyectoView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public EditarProyectoView() { }

        public EditarProyectoView(int id)
        {
            ProyectoActual = entity.tPrototipos.First(p => p.IdPrototipo == id);
        }

        public tPrototipo ProyectoActual { get; set; }

        [ScaffoldColumn(false)]
        public int IdPrototipo { get { return ProyectoActual.IdPrototipo; } set { ProyectoActual.IdPrototipo = value; } }

        [Display(Name = "Fecha creación:")]
        public DateTime FechaCreacion { get { return ProyectoActual.FechaCreacion; } set { ProyectoActual.FechaCreacion = value; } }

        [Display(Name = "Usuario creación:")]
        public string IdUsuarioCreador { get { return ProyectoActual.IdUsuarioCreador; } set { ProyectoActual.IdUsuarioCreador = value; } }

        [Required(ErrorMessage = "Tiene que especificar un nombre para el proyecto")]
        [Display(Name = "Nombre del proyecto:")]
        public string Proyecto { get { return ProyectoActual.Proyecto; } set { ProyectoActual.Proyecto = value; } }

        [Required(ErrorMessage = "Tiene que especificar un responsable de I+D")]
        [Display(Name = "Responsable I+D:")]
        public string Responsable { get { return ProyectoActual.Responsable; } set { ProyectoActual.Responsable = value; } }

        [Required(ErrorMessage = "Tiene que especificar la configuración original del montaje del prototipo")]
        [Display(Name = "Configuración inicial del montaje:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Configuracion { get { return ProyectoActual.Configuracion; } set { ProyectoActual.Configuracion = value; } }

        [Display(Name = "Descripción del proyecto:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get { return ProyectoActual.Descripcion; } set { ProyectoActual.Descripcion = value; } }

        [Display(Name = "Observaciones:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Observaciones { get { return ProyectoActual.Observaciones; } set { ProyectoActual.Observaciones = value; } }

        [ScaffoldColumn(false)]
        public int Estado { get { return ProyectoActual.Estado; } set { ProyectoActual.Estado = value; } }

        [ScaffoldColumn(false)]
        public DateTime? FechaCierre { get; set; }

        [ScaffoldColumn(false)]
        public int IdUsuarioCierre { get; set; }

        [ScaffoldColumn(false)]
        public string Cierre { get; set; }
    }
}