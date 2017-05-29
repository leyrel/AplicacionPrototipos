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
    [Bind(Exclude = "IdPrototipo, IdPrueba, FechaRegistroPrueba, FechaDesbloqueoGrua, FechaRegistroDesbloqueoGrua, IdUsuarioDesbloqueoGrua, RazonDesbloqueoGrua")]
    public class EditarPruebaView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public EditarPruebaView() { }

        public EditarPruebaView(int id)
        {
            PruebaActual = entity.tPruebas.First(p => p.IdPrueba == id);
        }

        public tPrueba PruebaActual { get; set; }

        public List<tSistema> tSistemas
        {
            get
            {
                return entity.tSistemas.ToList();
            }
        }

        public bool TieneDefPrueba(int idPrueba)
        {
            return (from dp in entity.tDeficienciaPruebas where dp.IdPrueba == idPrueba select dp).Any();
        }

        [ScaffoldColumn(false)]
        public int IdPrototipo { get { return PruebaActual.IdPrototipo; } set { PruebaActual.IdPrototipo = value; } }

        [ScaffoldColumn(false)]
        public int IdPrueba { get { return PruebaActual.IdPrueba; } set { PruebaActual.IdPrueba = value; } }

        [Required(ErrorMessage = "Tiene que especificar la fecha")]
        [Display(Name = "Fecha: (dd-mm-aaaa)")]
        [DisplayFormat(DataFormatString = "{dd-mm-yy}")]
        public DateTime? FechaPrueba { get { return PruebaActual.FechaPrueba; } set { PruebaActual.FechaPrueba = value; } }

        [ScaffoldColumn(false)]
        public DateTime FechaRegistroPrueba { get { return PruebaActual.FechaRegistroPrueba; } set { PruebaActual.FechaRegistroPrueba = value; } }

        [Display(Name = "Usuario creación:")]
        public string IdUsuario { get { return PruebaActual.IdUsuario; } set { PruebaActual.IdUsuario = value; } }

        [Required(ErrorMessage = "Tiene que describir la actividad realizada en el prototipo")]
        [Display(Name = "Actividad realizada en el prototipo:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Prueba { get { return PruebaActual.Prueba; } set { PruebaActual.Prueba = value; } }

        [Required(ErrorMessage = "Tiene que especificar si hay deficiencias o no")]
        [Display(Name = "Hay deficiencias en seguridad:")]
        public bool Deficiencia { get { return PruebaActual.Deficiencia; } set { PruebaActual.Deficiencia = value; } }

        [Required(ErrorMessage = "Tiene que especificar si bloquea la grua o no")]
        [Display(Name = "¿Bloquea la KX1953?")]
        public bool BloqueoGrua { get { return PruebaActual.BloqueoGrua; } set { PruebaActual.BloqueoGrua = value; } }

        [Display(Name = "Usuario bloqueo KX1953:")]
        public string IdUsuarioBloqueoGrua { get { return PruebaActual.IdUsuarioBloqueoGrua; } set { PruebaActual.IdUsuarioBloqueoGrua = value; } }

        [Display(Name = "Situación bloqueo KX1953:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres.")]
        [DataType(DataType.MultilineText)]
        public string SituacionBloqueoGrua { get { return PruebaActual.SituacionBloqueoGrua; } set { PruebaActual.SituacionBloqueoGrua = value; } }

        [ScaffoldColumn(false)]
        public DateTime FechaDesbloqueoGrua { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaRegistroDesbloqueoGrua { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioDesbloqueoGrua { get; set; }

        [ScaffoldColumn(false)]
        public string RazonDesbloqueoGrua { get; set; }
    }
}