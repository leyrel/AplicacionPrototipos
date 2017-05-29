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
    [Bind(Exclude = "IdPrototipo, IdPrueba, FechaRegistroPrueba, Deficiencia, BloqueoGrua, FechaRegistroDesbloqueoGrua, IdUsuarioDesbloqueoGrua")]
    public class DesbloqueoGruaPruebaView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public DesbloqueoGruaPruebaView() { }

        public DesbloqueoGruaPruebaView(int id)
        {
            PruebaActual = entity.tPruebas.First(p => p.IdPrueba == id);
        }

        public tPrueba PruebaActual { get; set; }

        [ScaffoldColumn(false)]
        public int IdPrototipo { get { return PruebaActual.IdPrototipo; } set { PruebaActual.IdPrototipo = value; } }

        [ScaffoldColumn(false)]
        public int IdPrueba { get { return PruebaActual.IdPrueba; } set { PruebaActual.IdPrueba = value; } }

        [Display(Name = "Fecha: (dd-mm-aaaa)")]
        public DateTime? FechaPrueba { get { return PruebaActual.FechaPrueba; } set { PruebaActual.FechaPrueba = value; } }

        [ScaffoldColumn(false)]
        public DateTime FechaRegistroPrueba { get { return PruebaActual.FechaRegistroPrueba; } set { PruebaActual.FechaRegistroPrueba = value; } }

        [Display(Name = "Usuario creación:")]
        public string IdUsuario { get { return PruebaActual.IdUsuario; } set { PruebaActual.IdUsuario = value; } }

        [Display(Name = "Actividad realizada en el prototipo:")]
        public string Prueba { get { return PruebaActual.Prueba; } set { PruebaActual.Prueba = value; } }

        [ScaffoldColumn(false)]
        public bool Deficiencia { get { return PruebaActual.Deficiencia; } set { PruebaActual.Deficiencia = value; } }

        [ScaffoldColumn(false)]
        public bool BloqueoGrua { get { return PruebaActual.BloqueoGrua; } set { PruebaActual.BloqueoGrua = value; } }

        [Display(Name = "Usuario bloqueo KX1953:")]
        public string IdUsuarioBloqueoGrua { get { return PruebaActual.IdUsuarioBloqueoGrua; } set { PruebaActual.IdUsuarioBloqueoGrua = value; } }

        [Display(Name = "Situación bloqueo KX1953:")]
        public string SituacionBloqueoGrua { get { return PruebaActual.SituacionBloqueoGrua; } set { PruebaActual.SituacionBloqueoGrua = value; } }

        [Required(ErrorMessage = "Tiene que especificar la fecha de desbloqueo")]
        [Display(Name = "Fecha de desbloqueo: (dd-mm-aaaa)")]
        [DisplayFormat(DataFormatString = "{dd-mm-yy}")]
        public DateTime? FechaDesbloqueoGrua { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaRegistroDesbloqueoGrua { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioDesbloqueoGrua { get; set; }

        [Display(Name = "Razón desbloqueo:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string RazonDesbloqueoGrua { get; set; }
    }
}