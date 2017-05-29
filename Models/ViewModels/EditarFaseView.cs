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
    [Bind(Exclude = "IdPrototipo, IdFase, EsInicial, FechaInsercion, DesmontajeFinal,  Desmontaje, FechaDesmontaje, IdUsuarioDesmontaje, ObservacionesDesmontaje, ObservacionesTratamiento, FechaDesbloqueoGrua, FechaRegistroDesbloqueoGrua, IdUsuarioDesbloqueoGrua, RazonDesbloqueoGrua")]
    public class EditarFaseView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public EditarFaseView() { }

        public EditarFaseView(int id)
        {
            FaseActual = entity.tFases.First(p => p.IdFase == id);
        }

        public tFase FaseActual { get; set; }

        public List<tSistema> tSistemas
        {
            get
            {
                return entity.tSistemas.ToList();
            }
        }

        public bool TieneDefFase(int idPrueba)
        {
            return (from d in entity.tDeficiencias where d.IdFase == IdFase select d).Any();
        }

        [ScaffoldColumn(false)]
        public int IdPrototipo { get { return FaseActual.IdPrototipo; } set { FaseActual.IdPrototipo = value; } }

        [ScaffoldColumn(false)]
        public int IdFase { get { return FaseActual.IdFase; } set { FaseActual.IdFase = value; } }

        [ScaffoldColumn(false)]
        public bool EsInicial { get { return FaseActual.EsInicial; } set { FaseActual.EsInicial = value; } }

        [ScaffoldColumn(false)]
        public DateTime FechaInsercion { get { return FaseActual.FechaInsercion; } set { FaseActual.FechaInsercion = value; } }

        [Required(ErrorMessage = "Tiene que especificar la fecha de puesta en marcha")]
        [Display(Name = "Fecha puesta en marcha: (dd-mm-aaaa)")]
        [DisplayFormat(DataFormatString = "{dd-mm-yy}")]
        public DateTime? FechaPuestaMarcha { get { return FaseActual.FechaPuestaMarcha; } set { FaseActual.FechaPuestaMarcha = value; } }

        [Display(Name = "Usuario creación:")]
        public string IdUsuario { get { return FaseActual.IdUsuario; } set { FaseActual.IdUsuario = value; } }

        [Required(ErrorMessage = "Tiene que especificar la descripción de la configuración del montaje")]
        [Display(Name = "Configuración de montaje:")]
        [StringLength(500, ErrorMessage = "No puede exceder de 500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string ConfiguracionMontaje { get { return FaseActual.ConfiguracionMontaje; } set { FaseActual.ConfiguracionMontaje = value; } }

        [Display(Name = "Observaciones:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Observaciones { get { return FaseActual.Observaciones; } set { FaseActual.Observaciones = value; } }

        [Required(ErrorMessage = "Tiene que especificar si hay deficiencias o no")]
        [Display(Name = "Hay deficiencias en seguridad:")]
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

        [Required(ErrorMessage = "Tiene que especificar si bloquea la grua o no")]
        [Display(Name = "¿Bloquea la KX1953?")]
        public bool BloqueoGrua { get { return FaseActual.BloqueoGrua; } set { FaseActual.BloqueoGrua = value; } }

        [Display(Name = "Usuario bloqueo KX1953:")]
        public string IdUsuarioBloqueoGrua { get { return FaseActual.IdUsuarioBloqueoGrua; } set { FaseActual.IdUsuarioBloqueoGrua = value; } }

        [Display(Name = "Situación bloqueo KX1953:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres.")]
        [DataType(DataType.MultilineText)]
        public string SituacionBloqueoGrua { get { return FaseActual.SituacionBloqueoGrua; } set { FaseActual.SituacionBloqueoGrua = value; } }

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