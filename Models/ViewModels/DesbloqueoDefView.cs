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
    [Bind(Exclude = "IdPrototipo, IdFase, IdDeficiencia, FechaRegistroDesbloqueo, IdUsuarioDesbloqueo, FechaRegistroResolucion")]
    public class DesbloqueoDefView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public DesbloqueoDefView() { }

        public DesbloqueoDefView(int id)
        {
            DeficienciaActual = entity.tDeficiencias.First(p => p.IdDeficiencia == id);
        }
        public tDeficiencia DeficienciaActual { get; set; }

        [ScaffoldColumn(false)]
        public int IdPrototipo { get { return DeficienciaActual.IdPrototipo; } set { DeficienciaActual.IdPrototipo = value; } }

        [ScaffoldColumn(false)]
        public int IdFase { get { return DeficienciaActual.IdFase; } set { DeficienciaActual.IdFase = value; } }

        [ScaffoldColumn(false)]
        public int IdDeficiencia { get { return DeficienciaActual.IdDeficiencia; } set { DeficienciaActual.IdDeficiencia = value; } }

        [Display(Name = "Fecha deficiencia:")]
        public DateTime Fecha { get { return DeficienciaActual.Fecha; } set { DeficienciaActual.Fecha = value; } }

        [Display(Name = "Usuario creación:")]
        public string IdUsuarioCreador { get { return DeficienciaActual.IdUsuarioCreador; } set { DeficienciaActual.IdUsuarioCreador = value; } }

        [Display(Name = "Descripción:")]
        public string Descripcion { get { return DeficienciaActual.Descripcion; } set { DeficienciaActual.Descripcion = value; } }

        [Display(Name = "Limitaciones:")]
        public string Limitaciones { get { return DeficienciaActual.Limitaciones; } set { DeficienciaActual.Limitaciones = value; } }

        [Display(Name = "Bloqueo del empleo del prototipo:")]
        public bool Bloqueo { get { return DeficienciaActual.Bloqueo; } set { DeficienciaActual.Bloqueo = value; } }

        [Required(ErrorMessage = "Tiene que especificar la fecha de desbloqueo")]
        [Display(Name = "Fecha de desbloqueo: (dd-mm-aaaa)")]
        [DisplayFormat(DataFormatString = "{dd-mm-yy}")]
        public DateTime? FechaDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaRegistroDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioDesbloqueo { get; set; }

        [Display(Name = "Usuario bloqueo:")]
        public string IdUsuarioBloqueo { get { return DeficienciaActual.IdUsuarioBloqueo; } set { DeficienciaActual.IdUsuarioBloqueo = value; } }

        [Required(ErrorMessage = "Tiene que añadir las razones para el desbloqueo")]
        [Display(Name = "Razones para el desbloqueo:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string RazonDesbloqueo { get; set; }

        [Display(Name = "Fecha resolución:")]
        public DateTime? FechaResolucion { get { return DeficienciaActual.FechaResolucion; } set { DeficienciaActual.FechaResolucion = value; } }

        [ScaffoldColumn(false)]
        public DateTime? FechaRegistroResolucion { get; set; }

        [Display(Name = "Usuario resolución:")]
        public string IdUsuarioResolucion { get { return DeficienciaActual.IdUsuarioResolucion; } set { DeficienciaActual.IdUsuarioResolucion = value; } }

        [Display(Name = "Observaciones resolución:")]
        public string ObservacionesResolucion { get { return DeficienciaActual.ObservacionesResolucion; } set { DeficienciaActual.ObservacionesResolucion = value; } }
    }
}