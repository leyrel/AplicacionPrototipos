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
    [Bind(Exclude = "IdPrototipo, IdFase, EsInicial, FechaInsercion, Deficiencia, DesmontajeFinal,  Desmontaje, FechaDesmontaje, IdUsuarioDesmontaje, ObservacionesDesmontaje, ObservacionesTratamiento, BloqueoGrua, FechaRegistroDesbloqueoGrua, IdUsuarioDesbloqueoGrua")]
    public class DesbloqueoGruaView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public DesbloqueoGruaView() { }

        public DesbloqueoGruaView(int id)
        {
            FaseActual = entity.tFases.First(p => p.IdFase == id);
        }

        public tFase FaseActual { get; set; }

        [ScaffoldColumn(false)]
        public int IdPrototipo { get { return FaseActual.IdPrototipo; } set { FaseActual.IdPrototipo = value; } }

        [ScaffoldColumn(false)]
        public int IdFase { get { return FaseActual.IdFase; } set { FaseActual.IdFase = value; } }

        [ScaffoldColumn(false)]
        public bool EsInicial { get { return FaseActual.EsInicial; } set { FaseActual.EsInicial = value; } }

        [ScaffoldColumn(false)]
        public DateTime FechaInsercion { get { return FaseActual.FechaInsercion; } set { FaseActual.FechaInsercion = value; } }

        [Display(Name = "Fecha puesta en marcha:")]
        public DateTime? FechaPuestaMarcha { get { return FaseActual.FechaPuestaMarcha; } set { FaseActual.FechaPuestaMarcha = value; } }

        [Display(Name = "Usuario creación:")]
        public string IdUsuario { get { return FaseActual.IdUsuario; } set { FaseActual.IdUsuario = value; } }

        [Display(Name = "Configuración de montaje:")]
        public string ConfiguracionMontaje { get { return FaseActual.ConfiguracionMontaje; } set { FaseActual.ConfiguracionMontaje = value; } }

        [Display(Name = "Observaciones:")]
        public string Observaciones { get { return FaseActual.Observaciones; } set { FaseActual.Observaciones = value; } }

        [ScaffoldColumn(false)]
        public bool Deficiencia { get { return FaseActual.Deficiencia; } set { FaseActual.Deficiencia = value; } }

        [ScaffoldColumn(false)]
        public bool DesmontajeFinal { get; set; }

        [ScaffoldColumn(false)]
        public bool? Desmontaje { get { return FaseActual.Desmontaje; } set { FaseActual.Desmontaje = value; } }

        [ScaffoldColumn(false)]
        public DateTime FechaDesmontaje { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioDesmontaje { get; set; }

        [ScaffoldColumn(false)]
        public string ObservacionesDesmontaje { get; set; }

        [ScaffoldColumn(false)]
        public string ObservacionesTratamiento { get; set; }

        [ScaffoldColumn(false)]
        public bool BloqueoGrua { get { return FaseActual.BloqueoGrua; } set { FaseActual.BloqueoGrua = value; } }

        [Display(Name = "Usuario bloqueo KX1953:")]
        public string IdUsuarioBloqueoGrua { get { return FaseActual.IdUsuarioBloqueoGrua; } set { FaseActual.IdUsuarioBloqueoGrua = value; } }

        [Display(Name = "Situación bloqueo KX1953:")]
        public string SituacionBloqueoGrua { get { return FaseActual.SituacionBloqueoGrua; } set { FaseActual.SituacionBloqueoGrua = value; } }

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