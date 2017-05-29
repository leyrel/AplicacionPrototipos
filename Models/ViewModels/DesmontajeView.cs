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
    [Bind(Exclude = "IdPrototipo, IdFase, FechaInsercion, EsInicial, IdUsuario, Desmontaje, IdUsuarioDesmontaje, BloqueoGrua, IdUsuarioBloqueoGrua, SituacionBloqueoGrua, FechaDesbloqueoGrua, FechaRegistroDesbloqueoGrua, IdUsuarioDesbloqueoGrua, RazonDesbloqueoGrua")]
    public class DesmontajeView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public DesmontajeView() { }

        public DesmontajeView(int id)
        {
            FaseActual = entity.tFases.First(p => p.IdFase == id);
        }

        public tFase FaseActual { get; set; }

        public IEnumerable<tTratamientoPieza> TratamientoPiezas(int id)
        {
            var query = from p in entity.tTratamientoPiezas
                        where p.IdFase == id
                        select p;
            return query.ToList();
        }

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

        [Display(Name = "Usuario:")]
        public string IdUsuario { get { return FaseActual.IdUsuario; } set { FaseActual.IdUsuario = value; } }

        [Display(Name = "Configuración de montaje:")]
        [StringLength(500, ErrorMessage = "No puede exceder de 500 caracteres.")]
        [DataType(DataType.MultilineText)]
        public string ConfiguracionMontaje { get { return FaseActual.ConfiguracionMontaje; } set { FaseActual.ConfiguracionMontaje = value; } }

        [Display(Name = "Observaciones:")]
        [DataType(DataType.MultilineText)]
        public string Observaciones { get { return FaseActual.Observaciones; } set { FaseActual.Observaciones = value; } }

        [Display(Name = "Hay deficiencias en seguridad:")]
        public bool Deficiencia { get { return FaseActual.Deficiencia; } set { FaseActual.Deficiencia = value; } }

        [ScaffoldColumn(false)]
        public bool Desmontaje { get; set; }

        [Required(ErrorMessage = "Tiene que especificar si es desmontaje final o no")]
        [Display(Name = "Es desmontaje final:")]
        public bool DesmontajeFinal { get; set; }

        [Required(ErrorMessage = "Tiene que especificar la fecha de desmontaje")]
        [Display(Name = "Fecha desmontaje: (dd-mm-aaaa)")]
        [DisplayFormat(DataFormatString = "{dd-mm-yy}")]
        public DateTime? FechaDesmontaje { get; set; }

        [ScaffoldColumn(false)]
        public int IdUsuarioDesmontaje { get; set; }

        [Required(ErrorMessage = "Tiene que añadir observaciones de desmontaje")]
        [Display(Name = "Observaciones desmontaje:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string ObservacionesDesmontaje { get; set; }

        [Display(Name = "Observaciones tratamiento:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string ObservacionesTratamiento { get; set; }

        [ScaffoldColumn(false)]
        public bool BloqueoGrua { get { return FaseActual.BloqueoGrua; } set { FaseActual.BloqueoGrua = value; } }

        [ScaffoldColumn(false)]
        public string IdUsuarioBloqueoGrua { get { return FaseActual.IdUsuarioBloqueoGrua; } set { FaseActual.IdUsuarioBloqueoGrua = value; } }

        [ScaffoldColumn(false)]
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