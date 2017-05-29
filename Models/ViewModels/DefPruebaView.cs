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
    [Bind(Exclude = "IdDeficiencia, Fecha, IdUsuarioCreador, FechaDesbloqueo, FechaRegistroDesbloqueo, IdUsuarioDesbloqueo, IdUsuarioBloqueo, RazonDesbloqueo, FechaResolucion, FechaRegistroResolucion, IdUsuarioResolucion, ObservacionesResolucion")]
    public class DefPruebaView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public DefPruebaView() { }

        public DefPruebaView(int id)
        {
            PruebaActual = entity.tPruebas.First(p => p.IdPrueba == id);
        }
        public tPrueba PruebaActual { get; set; }

        public int IdPrototipo { get { return PruebaActual.IdPrototipo; } set { PruebaActual.IdPrototipo = value; } }

        public int IdPrueba { get { return PruebaActual.IdPrueba; } set { PruebaActual.IdPrueba = value; } }

        [ScaffoldColumn(false)]
        public int IdDeficiencia { get; set; }

        [ScaffoldColumn(false)]
        public DateTime Fecha { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioCreador { get; set; }

        [Required(ErrorMessage = "Tiene que especificar la descripción")]
        [Display(Name = "Descripción:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres.")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Tiene que especificar las limitaciones")]
        [Display(Name = "Limitaciones de uso (parciales):")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Limitaciones { get; set; }

        [StringLength(100, ErrorMessage = "No puede exceder 100 caracteres")]
        public string Otros { get; set; }

        [Required(ErrorMessage = "Tiene que especificar si se debe bloquear el prototipo o no")]
        [Display(Name = "Bloqueo del empleo del prototipo:")]
        public bool Bloqueo { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaRegistroDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioBloqueo { get; set; }

        [ScaffoldColumn(false)]
        public string RazonDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaResolucion { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaRegistroResolucion { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioResolucion { get; set; }

        [ScaffoldColumn(false)]
        public string ObservacionesResolucion { get; set; }
    }
}