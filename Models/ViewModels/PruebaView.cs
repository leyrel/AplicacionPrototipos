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
    [Bind(Exclude = "IdPrototipo, IdPrueba, FechaRegistroPrueba, IdUsuario, IdUsuarioBloqueoGrua, FechaDesbloqueoGrua, FechaRegistroDesbloqueoGrua, IdUsuarioDesbloqueoGrua, RazonDesbloqueoGrua")]
    public class PruebaView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public PruebaView() { }

        public PruebaView(int id)
        {
            ProyectoActual = entity.tPrototipos.First(p => p.IdPrototipo == id);
        }

        public tPrototipo ProyectoActual { get; set; }

        public IEnumerable<v_pruebaUsuario> pruebaUs(int idProy)
        {
            var query = from p in entity.v_pruebaUsuarios
                        where p.IdPrototipo == idProy
                        select p;
            return query.ToList();
        }

        [ScaffoldColumn(false)]
        public int IdPrototipo { get { return ProyectoActual.IdPrototipo; } set { ProyectoActual.IdPrototipo = value; } }

        [ScaffoldColumn(false)]
        public int IdPrueba { get; set; }

        [Required(ErrorMessage = "Tiene que especificar la fecha")]
        [Display(Name = "Fecha: (dd-mm-aaaa)")]
        [DisplayFormat(DataFormatString = "{dd-mm-yy}")]
        public DateTime? FechaPrueba { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaRegistroPrueba { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuario { get; set; }

        [Required(ErrorMessage = "Tiene que describir la actividad realizada en el prototipo")]
        [Display(Name = "Actividad realizada en el prototipo:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Prueba { get; set; }

        [Required(ErrorMessage = "Tiene que especificar si hay deficiencias o no")]
        [Display(Name = "Hay deficiencias en seguridad:")]
        public bool Deficiencia { get; set; }

        [Required(ErrorMessage = "Tiene que especificar si bloquea la grua o no")]
        [Display(Name = "¿Bloquea la KX1953?")]
        public bool BloqueoGrua { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioBloqueoGrua { get; set; }

        [Display(Name = "Situación bloqueo KX1953:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres.")]
        [DataType(DataType.MultilineText)]
        public string SituacionBloqueoGrua { get; set; }

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