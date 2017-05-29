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
    [Bind(Exclude = "IdPrototipo, Proyecto, Estado, IdUsuarioCierre")]
    public class CierreProyectoView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public CierreProyectoView() { }

        public CierreProyectoView(int id)
        {
            ProyectoActual = entity.tPrototipos.First(m => m.IdPrototipo == id);
            IdUsuarioCreador = ProyectoActual.IdUsuarioCreador;
            FechaCreacion = ProyectoActual.FechaCreacion;
            Responsable = ProyectoActual.Responsable;
            Configuracion = ProyectoActual.Configuracion;
            Descripcion = ProyectoActual.Descripcion;
            Observaciones = ProyectoActual.Observaciones;
            FechaCierre = ProyectoActual.FechaCierre;
            Cierre = ProyectoActual.Cierre;
        }

        public tPrototipo ProyectoActual { set; get; }

        [ScaffoldColumn(false)]
        public int IdPrototipo { get { return ProyectoActual.IdPrototipo; } set { ProyectoActual.IdPrototipo = value; } }

        [Display(Name = "Fecha creación:")]
        public DateTime FechaCreacion { get { return ProyectoActual.FechaCreacion; } set { ProyectoActual.FechaCreacion = value; } }

        [Display(Name = "Usuario creación:")]
        public string IdUsuarioCreador { get { return ProyectoActual.IdUsuarioCreador; } set { ProyectoActual.IdUsuarioCreador = value; } }

        [ScaffoldColumn(false)]
        public string Proyecto { get { return ProyectoActual.Proyecto; } set { ProyectoActual.Proyecto = value; } }

        [Display(Name = "Responsable I+D:")]
        public string Responsable { get { return ProyectoActual.Responsable; } set { ProyectoActual.Responsable = value; } }

        [Display(Name = "Configuración inicial del montaje:")]
        [DataType(DataType.MultilineText)]
        public string Configuracion { get { return ProyectoActual.Configuracion; } set { ProyectoActual.Configuracion = value; } }

        [Display(Name = "Descripción:")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get { return ProyectoActual.Descripcion; } set { ProyectoActual.Descripcion = value; } }

        [Display(Name = "Observaciones:")]
        [DataType(DataType.MultilineText)]
        public string Observaciones { get { return ProyectoActual.Observaciones; } set { ProyectoActual.Observaciones = value; } }

        [ScaffoldColumn(false)]
        public int Estado { get; set; }

        [Required(ErrorMessage = "Tiene que especificar una fecha de cierre")]
        [Display(Name = "Fecha cierre: (dd-mm-aaaa)")]
        [DisplayFormat(DataFormatString = "{dd-mm-yy}")]
        public DateTime? FechaCierre { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioCierre { get; set; }

        [Required(ErrorMessage = "Tiene que especificar observaciones de cierre del expediente")]
        [Display(Name = "Observaciones cierre del expediente:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Cierre { get; set; }
    }
}