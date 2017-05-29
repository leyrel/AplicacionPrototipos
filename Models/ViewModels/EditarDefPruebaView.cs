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
    [Bind(Exclude = "IdPrototipo, IdPrueba, IdDeficiencia, FechaDesbloqueo, FechaRegistroDesbloqueo, IdUsuarioDesbloqueo, RazonDesbloqueo, FechaResolucion, FechaRegistroResolucion, IdUsuarioResolucion, ObservacionesResolucion")]
    public class EditarDefPruebaView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public EditarDefPruebaView() { }

        public EditarDefPruebaView(int id)
        {
            DefPruebaActual = entity.tDeficienciaPruebas.First(p => p.IdDeficiencia == id);
        }

        public tDeficienciaPrueba DefPruebaActual { get; set; }

        public List<tSistema> tSistemas
        {
            get
            {
                return entity.tSistemas.ToList();
            }
        }

        public bool ExistePruebaAfecta(int idDefPru, int idSist)
        {
            return (from s in entity.tSistemas
                    join f in entity.tPruebaAfectas on s.IdSistema equals f.IdSistema
                    where s.IdSistema == idSist
                    select f).Any(y => y.IdDeficienciaPru == idDefPru);
        }

        public bool ExistePruebaOtros(int idDefPru)
        {
            return (from f in entity.tPruebaAfectas
                    where f.IdDeficienciaPru == idDefPru && f.Otros != null
                    select f).Any();
        }

        public bool DeficienciaPruebaBloq(int idDefPru)
        {
            return (from d in entity.tDeficienciaPruebas where d.IdDeficiencia == idDefPru && d.Bloqueo == true select d).Any();
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

        [Required(ErrorMessage = "Tiene que especificar la descripción")]
        [Display(Name = "Descripción:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres.")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get { return DefPruebaActual.Descripcion; } set { DefPruebaActual.Descripcion = value; } }

        [Required(ErrorMessage = "Tiene que especificar las limitaciones")]
        [Display(Name = "Limitaciones de uso (parciales):")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Limitaciones { get { return DefPruebaActual.Limitaciones; } set { DefPruebaActual.Limitaciones = value; } }

        [StringLength(100, ErrorMessage = "No puede exceder 100 caracteres")]
        public string Otros { get; set; }

        [Display(Name = "Bloqueo del empleo del prototipo:")]
        public bool Bloqueo { get { return DefPruebaActual.Bloqueo; } set { DefPruebaActual.Bloqueo = value; } }

        [ScaffoldColumn(false)]
        public DateTime? FechaDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? FechaRegistroDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioDesbloqueo { get; set; }

        [Display(Name = "Usuario bloqueo:")]
        public string IdUsuarioBloqueo { get { return DefPruebaActual.IdUsuarioBloqueo; } set { DefPruebaActual.IdUsuarioBloqueo = value; } }

        [ScaffoldColumn(false)]
        public string RazonDesbloqueo { get; set; }

        [ScaffoldColumn(false)]
        public DateTime? FechaResolucion { get; set; }

        [ScaffoldColumn(false)]
        public DateTime FechaRegistroResolucion { get; set; }

        [ScaffoldColumn(false)]
        public string IdUsuarioResolucion { get; set; }

        [ScaffoldColumn(false)]
        public string ObservacionesResolucion { get; set; }
    }
}