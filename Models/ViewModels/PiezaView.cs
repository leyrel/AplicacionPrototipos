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
    [Bind(Exclude = "IdPieza")]
    public class PiezaView
    {
        private PrototiposEntities entity = new PrototiposEntities();
        public PiezaView() { }

        public PiezaView(int id)
        {
            FaseActual = entity.tFases.First(p => p.IdFase == id);
        }
        public tFase FaseActual { get; set; }

        public int IdFase { get { return FaseActual.IdFase; } set { FaseActual.IdFase = value; } }

        [ScaffoldColumn(false)]
        public int IdPieza { get; set; }

        [Required(ErrorMessage = "Tiene que especificar el artículo")]
        [Display(Name = "Artículo:")]
        [StringLength(500, ErrorMessage = "No puede exceder de 100 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Articulo { get; set; }

        [Required(ErrorMessage = "Tiene que especificar la acción")]
        [Display(Name = "Acción:")]
        [StringLength(2500, ErrorMessage = "No puede exceder 2500 caracteres")]
        [DataType(DataType.MultilineText)]
        public string Accion { get; set; }
    }
}