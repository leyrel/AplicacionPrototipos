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
    [Bind(Exclude = "IdPrototipo, IdFase, IdDeficiencia, FechaDesbloqueo, FechaRegistroDesbloqueo, IdUsuarioDesbloqueo, IdUsuarioBloqueo, RazonDesbloqueo, FechaRegistroResolucion, IdUsuarioResolucion")]
    public class ResolucionDefView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public ResolucionDefView() { }

        public ResolucionDefView(int id)
        {
            DeficienciaActual = entity.tDeficiencias.First(p => p.IdDeficiencia == id);
        }
        public tDeficiencia DeficienciaActual { get; set; }

        public bool DeficienciaBloq(int idDef)
        {
            return (from d in entity.tDeficiencias where d.IdDeficiencia == idDef && d.Bloqueo == true select d).Any();
        }

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

        [Display(Name = "Bloqueo:")]
        public bool Bloqueo { get { return DeficienciaActual.Bloqueo; } set { DeficienciaActual.Bloqueo = value; } }

        [ScaffoldColumn(false)]
        public DateTime? FechaDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? FechaRegistroDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioBloqueo { get { return DeficienciaActual.IdUsuarioBloqueo; } set { DeficienciaActual.IdUsuarioBloqueo = value; } }

        [ScaffoldColumn(false)]
        public string RazonDesbloqueo { get; set; }

        [Required(ErrorMessage = "Tiene que especificar la fecha de resolución")]
        [Display(Name = "Fecha de resolución: (dd-mm-aaaa)")]
        [DisplayFormat(DataFormatString = "{dd-mm-yy}")]
        public DateTime? FechaResolucion { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaRegistroResolucion { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioResolucion { get; set; }

        [Required(ErrorMessage = "Tiene que añadir observaciones")]
        [Display(Name = "Observaciones:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string ObservacionesResolucion { get; set; }
    }
}