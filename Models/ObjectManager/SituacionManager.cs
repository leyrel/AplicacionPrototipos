using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prototipos.Models.DB;
using Prototipos.Models.ViewModels;
using Prototipos.Models;

namespace Prototipos.Models.ObjectManager
{
    public class SituacionManager
    {
        private PrototiposEntities entity = new PrototiposEntities();

        public SituacionManager() { }

        public SituacionManager(int id)
        {
            ProyectoActual = entity.tPrototipos.First(p => p.IdPrototipo == id);
            var ult = (from t in entity.v_faseUsuarios
                       where t.IdPrototipo == id
                       orderby t.FechaInsercion descending
                       select t).FirstOrDefault();
            UltimaFase = ult;
        }

        public tPrototipo ProyectoActual { set; get; }
        public v_faseUsuario UltimaFase { set; get; }

        public List<v_defUsuario> v_defUsuarios
        {
            get
            {
                return entity.v_defUsuarios.ToList();
            }
        }

        public List<v_faseAfectaSistema> v_faseAfectaSistema
        {
            get
            {
                return entity.v_faseAfectaSistemas.ToList();
            }
        }

        public List<v_pruebaUsSemana> v_pruebaUsSemana
        {
            get
            {
                return entity.v_pruebaUsSemanas.ToList();
            }
        }

        public List<v_defPruebaUsuario> v_defPruebaUsuarios
        {
            get
            {
                return entity.v_defPruebaUsuarios.ToList();
            }
        }

        public List<v_pruebaAfectaSistema> v_pruebaAfectaSistema
        {
            get
            {
                return entity.v_pruebaAfectaSistemas.OrderBy(v => v.Orden).ToList();
            }
        }

        public List<tTratamientoPieza> tTratamientoPiezas
        {
            get
            {
                return entity.tTratamientoPiezas.ToList();
            }
        }

        public List<v_faseSinResSistema> v_faseSinResSistemas
        {
            get
            {
                return entity.v_faseSinResSistemas.OrderBy(f => f.Orden).ToList();
            }
        }

        public List<string> otrosFase(int idProt)
        {
            return (from t in entity.tFaseAfectas join d in entity.tDeficiencias on t.IdDeficienciaFas equals d.IdDeficiencia where d.IdPrototipo == idProt && t.Otros != null select t.Otros).ToList();
        }

        public List<string> otrosFaseSinRes(int idProt)
        {
            return (from t in entity.tFaseAfectas join d in entity.tDeficiencias on t.IdDeficienciaFas equals d.IdDeficiencia where d.IdPrototipo == idProt && t.Otros != null && d.FechaResolucion == null select t.Otros).ToList();
        }

        public List<v_pruebaSinResSistema> v_pruebaSinResSistemas
        {
            get
            {
                return entity.v_pruebaSinResSistemas.OrderBy(p => p.Orden).ToList();
            }
        }

        public List<string> otrosPrueba(int idProt)
        {
            return (from t in entity.tPruebaAfectas join d in entity.tDeficienciaPruebas on t.IdDeficienciaPru equals d.IdDeficiencia where d.IdPrototipo == idProt && t.Otros != null select t.Otros).ToList();
        }

        public List<string> otrosPruebaSinRes(int idProt)
        {
            return (from t in entity.tPruebaAfectas join d in entity.tDeficienciaPruebas on t.IdDeficienciaPru equals d.IdDeficiencia where d.IdPrototipo == idProt && t.Otros != null && d.FechaResolucion == null select t.Otros).ToList();
        }

        public bool TieneTratPiezas(int idFase)
        {
            return (from t in entity.tTratamientoPiezas where t.IdFase == idFase select t).Any();
        }

        public bool TieneDefBloq(int idProy)
        {
            return (from b in entity.tDeficiencias where b.IdPrototipo == idProy && b.Bloqueo == true && b.FechaResolucion == null select b).Any();
        }

        public bool TieneDefPruebaBloq(int idProy)
        {
            return (from b in entity.tDeficienciaPruebas where b.IdPrototipo == idProy && b.Bloqueo == true && b.FechaResolucion == null select b).Any();
        }

        public bool TieneFaseBloqGrua(int idProy)
        {
            return (from b in entity.tFases where b.IdPrototipo == idProy && b.BloqueoGrua == true && b.Desmontaje != true select b).Any();
        }

        public bool TienePruebaBloqGrua(int idProy)
        {
            return (from b in entity.tPruebas where b.IdPrototipo == idProy && b.BloqueoGrua == true select b).Any();
        }

        public bool TieneDefPrueba(int idPrueba)
        {
            return (from dp in entity.tDeficienciaPruebas where dp.IdPrueba == idPrueba select dp).Any();
        }

        public bool TieneDefNoResuelta(int idProy)
        {
            return (from r in entity.tDeficiencias where r.IdPrototipo == idProy && r.FechaResolucion == null select r).Any();
        }

        public bool TieneDefPruebaNoResuelta(int idProy)
        {
            return (from r in entity.tDeficienciaPruebas where r.IdPrototipo == idProy && r.FechaResolucion == null select r).Any();
        }

        public bool TieneDefNoResueltaSinBloq(int idProy)
        {
            return (from r in entity.tDeficiencias where r.IdPrototipo == idProy && r.FechaResolucion == null && r.Bloqueo == false select r).Any();
        }

        public bool TieneDefNoResueltaPruebaSinBloq(int idProy)
        {
            return (from r in entity.tDeficienciaPruebas where r.IdPrototipo == idProy && r.FechaResolucion == null && r.Bloqueo == false select r).Any();
        }

        public bool TienePruebasSemana(int idProy)
        {
            return (from r in entity.v_pruebaUsSemanas where r.IdPrototipo == idProy select r).Any();
        }

        public bool AfectaSistemasFase(int idProy)
        {
            return (from r in entity.v_faseDefSinResSistemas where r.IdPrototipo == idProy select r).Any();
        }

        public bool AfectaSistemasPrueba(int idProy)
        {
            return (from r in entity.v_pruebaDefSinResSistemas where r.IdPrototipo == idProy select r).Any();
        }

        public bool TieneArchFase(int idFase)
        {
            return (from t in entity.tFaseArchivos where t.IdFase == idFase select t).Any();
        }

        public bool TieneArchDefFase(int idDef)
        {
            return (from t in entity.tDeficienciaArchivos where t.IdDeficiencia == idDef select t).Any();
        }

        public bool TieneArchPrueba(int idPrueba)
        {
            return (from t in entity.tPruebaArchivos where t.IdPrueba == idPrueba select t).Any();
        }

        public bool TieneArchDefPru(int idDefPru)
        {
            return (from t in entity.tDeficienciaPruArchivos where t.IdDeficiencia == idDefPru select t).Any();
        }
    }
}