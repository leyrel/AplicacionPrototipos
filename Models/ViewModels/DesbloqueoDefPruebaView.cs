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
    [Bind(Exclude = "IdPrototipo, IdPrueba, IdDeficiencia, FechaRegistroDesbloqueo, IdUsuarioDesbloqueo, FechaRegistroResolucion")]
    public class DesbloqueoDefPruebaView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public DesbloqueoDefPruebaView() { }

        public DesbloqueoDefPruebaView(int id)
        {
            DefPruebaActual = entity.tDeficienciaPruebas.First(p => p.IdDeficiencia == id);
        }
        public tDeficienciaPrueba DefPruebaActual { get; set; }

        [ScaffoldColumn(false)]
        public int IdPrototipo { get { return DefPruebaActual.IdPrototipo; } set { DefPruebaActual.IdPrototipo = value; } }

        [ScaffoldColumn(false)]
        public int IdPrueba { get { return DefPruebaActual.IdPrueba; } set { DefPruebaActual.IdPrueba = value; } }

        [ScaffoldColumn(false)]
        public int IdDeficiencia { get { return DefPruebaActual.IdDeficiencia; } set { DefPruebaActual.IdDeficiencia = value; } }

        [Display(Name = "Fecha deficiencia:")]
        public DateTime Fecha { get { return DefPruebaActual.Fecha; } set { DefPruebaActual.Fecha = value; } }

        [Display(Name = "Usuario creación:")]
        public string IdUsuarioCreador { get { return DefPruebaActual.IdUsuarioCreador; } set { DefPruebaActual.IdUsuarioCreador = value; } }

        [Display(Name = "Descripción:")]
        public string Descripcion { get { return DefPruebaActual.Descripcion; } set { DefPruebaActual.Descripcion = value; } }

        [Display(Name = "Limitaciones:")]
        public string Limitaciones { get { return DefPruebaActual.Limitaciones; } set { DefPruebaActual.Limitaciones = value; } }

        [Display(Name = "Bloqueo del empleo del prototipo:")]
        public bool Bloqueo { get { return DefPruebaActual.Bloqueo; } set { DefPruebaActual.Bloqueo = value; } }

        [Required(ErrorMessage = "Tiene que especificar la fecha de desbloqueo")]
        [Display(Name = "Fecha de desbloqueo: (dd-mm-aaaa)")]
        [DisplayFormat(DataFormatString = "{dd-mm-yy}")]
        public DateTime? FechaDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaRegistroDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioDesbloqueo { get; set; }

        [Display(Name = "Usuario bloqueo:")]
        public string IdUsuarioBloqueo { get { return DefPruebaActual.IdUsuarioBloqueo; } set { DefPruebaActual.IdUsuarioBloqueo = value; } }

        [Required(ErrorMessage = "Tiene que añadir las razones para el desbloqueo")]
        [Display(Name = "Razones para el desbloqueo:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string RazonDesbloqueo { get; set; }

        [Display(Name = "Fecha resolución:")]
        public DateTime? FechaResolucion { get { return DefPruebaActual.FechaResolucion; } set { DefPruebaActual.FechaResolucion = value; } }

        [ScaffoldColumn(false)]
        public DateTime? FechaRegistroResolucion { get; set; }

        [Display(Name = "Usuario resolución:")]
        public string IdUsuarioResolucion { get { return DefPruebaActual.IdUsuarioResolucion; } set { DefPruebaActual.IdUsuarioResolucion = value; } }

        [Display(Name = "Observaciones resolución:")]
        public string ObservacionesResolucion { get { return DefPruebaActual.ObservacionesResolucion; } set { DefPruebaActual.ObservacionesResolucion = value; } }
    }
}