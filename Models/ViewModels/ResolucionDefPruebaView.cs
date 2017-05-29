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
    [Bind(Exclude = "IdPrototipo, IdPrueba, IdDeficiencia, FechaDesbloqueo, FechaRegistroDesbloqueo, IdUsuarioDesbloqueo, IdUsuarioBloqueo, RazonDesbloqueo, FechaRegistroResolucion, IdUsuarioResolucion")]
    public class ResolucionDefPruebaView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public ResolucionDefPruebaView() { }

        public ResolucionDefPruebaView(int id)
        {
            DefPruebaActual = entity.tDeficienciaPruebas.First(p => p.IdDeficiencia == id);
        }
        public tDeficienciaPrueba DefPruebaActual { get; set; }

        public bool DeficienciaBloq(int idDef)
        {
            return (from d in entity.tDeficienciaPruebas where d.IdDeficiencia == idDef && d.Bloqueo == true select d).Any();
        }

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

        [Display(Name = "Bloqueo:")]
        public bool Bloqueo { get { return DefPruebaActual.Bloqueo; } set { DefPruebaActual.Bloqueo = value; } }

        [ScaffoldColumn(false)]
        public DateTime? FechaDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? FechaRegistroDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioBloqueo { get { return DefPruebaActual.IdUsuarioBloqueo; } set { DefPruebaActual.IdUsuarioBloqueo = value; } }

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