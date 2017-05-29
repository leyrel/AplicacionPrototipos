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
    [Bind(Exclude = "IdFase, EsInicial, FechaInsercion, IdUsuario, DesmontajeFinal,  Desmontaje, FechaDesmontaje, IdUsuarioDesmontaje, ObservacionesDesmontaje, ObservacionesTratamiento, IdUsuarioBloqueoGrua, FechaDesbloqueoGrua, FechaRegistroDesbloqueoGrua, IdUsuarioDesbloqueoGrua, RazonDesbloqueoGrua")]
    public class FaseView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public FaseView() { }

        public FaseView(int id)
        {
            ProyectoActual = entity.tPrototipos.First(p => p.IdPrototipo == id);
        }
        public tPrototipo ProyectoActual { get; set; }

        public int IdPrototipo { get { return ProyectoActual.IdPrototipo; } set { ProyectoActual.IdPrototipo = value; } }

        [ScaffoldColumn(false)]
        public int IdFase { get; set; }

        [ScaffoldColumn(false)]
        public bool? EsInicial { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaInsercion { get; set; }

        [Required(ErrorMessage = "Tiene que especificar la fecha de puesta en marcha")]
        [Display(Name = "Fecha puesta en marcha: (dd-mm-aaaa)")]
        [DisplayFormat(DataFormatString = "{dd-mm-yy}")]
        public DateTime? FechaPuestaMarcha { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuario { get; set; }

        [Required(ErrorMessage = "Tiene que especificar la descripción de la configuración del montaje")]
        [Display(Name = "Configuración de montaje:")]
        [StringLength(500, ErrorMessage = "No puede exceder de 500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string ConfiguracionMontaje { get; set; }

        [Display(Name = "Observaciones:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Observaciones { get; set; }

        [Required(ErrorMessage = "Tiene que especificar si hay deficiencias o no")]
        [Display(Name = "Hay deficiencias en seguridad:")]
        public bool Deficiencia { get; set; }

        [ScaffoldColumn(false)]
        public bool DesmontajeFinal { get; set; }

        [ScaffoldColumn(false)]
        public bool Desmontaje { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaDesmontaje { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioDesmontaje { get; set; }

        [ScaffoldColumn(false)]
        public string ObservacionesDesmontaje { get; set; }

        [ScaffoldColumn(false)]
        public string ObservacionesTratamiento { get; set; }

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