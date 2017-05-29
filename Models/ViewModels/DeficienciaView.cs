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
    public class DeficienciaView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public DeficienciaView() { }

        public DeficienciaView(int id)
        {
            FaseActual = entity.tFases.First(p => p.IdFase == id);
        }
        public tFase FaseActual { get; set; }

        public int IdPrototipo { get { return FaseActual.IdPrototipo; } set { FaseActual.IdPrototipo = value; } }

        public int IdFase { get { return FaseActual.IdFase; } set { FaseActual.IdFase = value; } }

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