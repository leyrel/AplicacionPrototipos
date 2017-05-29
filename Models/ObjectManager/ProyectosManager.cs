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
    public class ProyectosManager
    {
        private PrototiposEntities entity = new PrototiposEntities();

        public ProyectosManager() { }

        public ProyectosManager(int id)
        {
            ProyectoActual = entity.tPrototipos.First(p => p.IdPrototipo == id);
        }

        public tPrototipo ProyectoActual { set; get; }

        public List<tSistema> tSistemas
        {
            get
            {
                return entity.tSistemas.OrderBy(s => s.Orden).ToList();
            }
        }

        public IEnumerable<v_defUsuario> DefsFases(int id)
        {
            var query = from d in entity.v_defUsuarios
                        where d.IdPrototipo == id && d.FechaResolucion == null
                        select d;
            return query.OrderByDescending(d => d.Fecha).ToList();
        }

        public IEnumerable<v_defPruebaUsuario> DefsPruebas(int id)
        {
            var query = from d in entity.v_defPruebaUsuarios
                        where d.IdPrototipo == id && d.FechaResolucion == null
                        select d;
            return query.OrderByDescending(d => d.Fecha).ToList();
        }

        public IEnumerable<v_defUsuario> DefsFasesBloq(int id)
        {
            var query = from d in entity.v_defUsuarios
                        where d.IdPrototipo == id && d.Bloqueo == "Sí"
                        select d;
            return query.OrderByDescending(d => d.Fecha).ToList();
        }

        public IEnumerable<v_defPruebaUsuario> DefsPruebasBloq(int id)
        {
            var query = from d in entity.v_defPruebaUsuarios
                        where d.IdPrototipo == id && d.Bloqueo == "Sí"
                        select d;
            return query.OrderByDescending(d => d.Fecha).ToList();
        }

        public IEnumerable<v_faseUsuario> FasesBloq(int id)
        {
            var query = from d in entity.v_faseUsuarios
                        where d.IdPrototipo == id && d.BloqueoGrua == "Sí"
                        select d;
            return query.OrderByDescending(d => d.FechaInsercion).ToList();
        }

        public IEnumerable<v_pruebaUsuario> PruebasBloq(int id)
        {
            var query = from d in entity.v_pruebaUsuarios
                        where d.IdPrototipo == id && d.BloqueoGrua == "Sí"
                        select d;
            return query.OrderByDescending(d => d.FechaPrueba).ToList();
        }

        public IEnumerable<tTratamientoPieza> TratamientoPiezas(int id)
        {
            var query = from p in entity.tTratamientoPiezas
                        where p.IdFase == id
                        select p;
            return query.ToList();
        }

        public IEnumerable<v_faseUsuario> fasesProy(int idProy)
        {
            var query = from p in entity.v_faseUsuarios
                        where p.IdPrototipo == idProy
                        select p;
            return query.ToList();
        }

        public List<v_pruebaUsuario> v_pruebaUsuarios
        {
            get
            {
                return entity.v_pruebaUsuarios.ToList();
            }
        }

        public List<v_faseUsuario> v_faseUsuarios
        {
            get
            {
                return entity.v_faseUsuarios.ToList();
            }
        }

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

        public bool ExisteProyecto(string Proyect)
        {
            return (from p in entity.tPrototipos where p.Proyecto == Proyect select p).Any();
        }

        public bool ExisteFaseInicial(int idProy)
        {
            return (from f in entity.tFases where f.IdPrototipo == idProy && f.EsInicial == true select f).Any();
        }

        public bool ExisteFaseSinDesmontar(int idProy)
        {
            return (from f in entity.tFases where f.IdPrototipo == idProy && f.Desmontaje == false select f).Any();
        }

        public bool ExisteDesmontajeFinal(int idProy)
        {
            return (from f in entity.tFases where f.IdPrototipo == idProy && f.DesmontajeFinal == true select f).Any();
        }

        public bool TieneTratPiezas(int idFase)
        {
            return (from t in entity.tTratamientoPiezas where t.IdFase == idFase select t).Any();
        }

        public bool TieneDefFase(int idFase)
        {
            return (from df in entity.tDeficiencias where df.IdFase == idFase select df).Any();
        }

        public bool TieneDefFaseSinResolver(int idFase)
        {
            return (from df in entity.tDeficiencias where df.IdFase == idFase && df.FechaResolucion == null select df).Any();
        }

        public bool TieneGruaBloqueada(int idFase)
        {
            return (from f in entity.tFases where f.IdFase == idFase && f.BloqueoGrua == true select f).Any();
        }

        public bool TieneDefPrueba(int idPrueba)
        {
            return (from dp in entity.tDeficienciaPruebas where dp.IdPrueba == idPrueba select dp).Any();
        }

        public bool ProtBloqueado(int idProy)
        {
            return (from d in entity.tDeficiencias where d.IdPrototipo == idProy && d.Bloqueo == true select d).Any();
        }

        public bool ProtBloqueadoPru(int idProy)
        {
            return (from dp in entity.tDeficienciaPruebas where dp.IdPrototipo == idProy && dp.Bloqueo == true select dp).Any();
        }

        public bool TieneArchProy(int idProy)
        {
            return (from t in entity.tPrototipoArchivos where t.IdPrototipo == idProy select t).Any();
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

        public bool EsUsuarioCorrecto(int idDef)
        {
            var user = HttpContext.Current.User.Identity.Name;
            return (from d in entity.tDeficiencias where d.IdDeficiencia == idDef && d.IdUsuarioCreador == user select d).Any();
        }

        public bool EsUsuarioCorrectoPru(int idDef)
        {
            var user = HttpContext.Current.User.Identity.Name;
            return (from d in entity.tDeficienciaPruebas where d.IdDeficiencia == idDef && d.IdUsuarioCreador == user select d).Any();
        }

        public bool EsUsuarioCorrectoGrua(int idFas)
        {
            var user = HttpContext.Current.User.Identity.Name;
            return (from d in entity.tFases where d.IdFase == idFas && d.IdUsuario == user select d).Any();
        }

        public bool EsUsuarioCorrectoGruaPrueba(int idPru)
        {
            var user = HttpContext.Current.User.Identity.Name;
            return (from d in entity.tPruebas where d.IdPrueba == idPru && d.IdUsuario == user select d).Any();
        }

        public bool EsResponsable(int idProy)
        {
            var user = HttpContext.Current.User.Identity.Name;
            return (from p in entity.tPrototipos where p.IdPrototipo == idProy && p.Responsable == user select p).Any();
        }

        public bool IsOtrosSelected(int[] selectedSistemas)
        {
            return (from s in selectedSistemas
                    where s == 29
                    select s).Any();
        }

        public bool IsSistemaSelected(int idSistema, int[] selectedSistemas)
        {
            return (from s in selectedSistemas
                    where s == idSistema
                    select s).Any();
        }

        public bool IsDefSistemaExist(int idDef, int idSistema)
        {
            return (from s in entity.tSistemas
                    join a in entity.tFaseAfectas on s.IdSistema equals a.IdSistema
                    where s.IdSistema == idSistema
                    select a).Any(d => d.IdDeficienciaFas == idDef);
        }

        public bool IsDefPruSistemaExist(int idDefPru, int idSistema)
        {
            return (from s in entity.tSistemas
                    join a in entity.tPruebaAfectas on s.IdSistema equals a.IdSistema
                    where s.IdSistema == idSistema
                    select a).Any(d => d.IdDeficienciaPru == idDefPru);
        }

        public void Add(ProyectoView proy)
        {
            DB.tPrototipo tPrototipos = new DB.tPrototipo();
            tPrototipos.FechaCreacion = DateTime.Now;
            var user = HttpContext.Current.User.Identity.Name;
            tPrototipos.IdUsuarioCreador = user;
            tPrototipos.Proyecto = proy.Proyecto;
            tPrototipos.Responsable = proy.Responsable;
            tPrototipos.Configuracion = proy.Configuracion;
            tPrototipos.Descripcion = proy.Descripcion;
            tPrototipos.Observaciones = proy.Observaciones;
            tPrototipos.Estado = 1;
            tPrototipos.FechaCierre = null;
            tPrototipos.IdUsuarioCierre = null;
            tPrototipos.Cierre = null;

            entity.tPrototipos.AddObject(tPrototipos);
            entity.SaveChanges();
        }

        public void EditProy(EditarProyectoView editProy)
        {
            tPrototipo tPrototipos = entity.tPrototipos.First(m => m.IdPrototipo == editProy.IdPrototipo);
            editProy.ProyectoActual.IdPrototipo = editProy.IdPrototipo;
            editProy.ProyectoActual.FechaCreacion = editProy.FechaCreacion;
            editProy.ProyectoActual.IdUsuarioCreador = editProy.IdUsuarioCreador;
            editProy.ProyectoActual.FechaCreacion = editProy.FechaCreacion;
            editProy.ProyectoActual.Proyecto = editProy.Proyecto;
            editProy.ProyectoActual.Responsable = editProy.Responsable;
            editProy.ProyectoActual.Configuracion = editProy.Configuracion;
            editProy.ProyectoActual.Descripcion = editProy.Descripcion;
            editProy.ProyectoActual.Observaciones = editProy.Observaciones;
            editProy.ProyectoActual.Estado = editProy.Estado;

            entity.tPrototipos.ApplyCurrentValues(editProy.ProyectoActual);
            entity.SaveChanges();
        }

        public void AddFaseInicial(FaseView fase)
        {
            DB.tFase tFases = new DB.tFase();
            tFases.IdPrototipo = fase.IdPrototipo;
            tFases.EsInicial = true;
            tFases.FechaInsercion = DateTime.Now;
            tFases.FechaPuestaMarcha = fase.FechaPuestaMarcha;
            var user = HttpContext.Current.User.Identity.Name;
            tFases.IdUsuario = user;
            tFases.ConfiguracionMontaje = fase.ConfiguracionMontaje;
            tFases.Observaciones = fase.Observaciones;
            tFases.Deficiencia = fase.Deficiencia;
            tFases.Desmontaje = false;
            tFases.DesmontajeFinal = null;
            tFases.FechaDesmontaje = null;
            tFases.IdUsuarioDesmontaje = null;
            tFases.ObservacionesDesmontaje = null;
            tFases.ObservacionesTratamiento = null;
            tFases.BloqueoGrua = fase.BloqueoGrua;
            if (tFases.BloqueoGrua == true) { tFases.IdUsuarioBloqueoGrua = user; }
            else { tFases.IdUsuarioBloqueoGrua = null; }
            tFases.SituacionBloqueoGrua = fase.SituacionBloqueoGrua;

            entity.tFases.AddObject(tFases);
            entity.SaveChanges();
        }

        public void AddFaseDerivada(FaseView fase)
        {
            DB.tFase tFases = new DB.tFase();
            tFases.IdPrototipo = fase.IdPrototipo;
            tFases.EsInicial = false;
            tFases.FechaInsercion = DateTime.Now;
            tFases.FechaPuestaMarcha = fase.FechaPuestaMarcha;
            var user = HttpContext.Current.User.Identity.Name;
            tFases.IdUsuario = user;
            tFases.ConfiguracionMontaje = fase.ConfiguracionMontaje;
            tFases.Observaciones = fase.Observaciones;
            tFases.Deficiencia = fase.Deficiencia;
            tFases.Desmontaje = false;
            tFases.DesmontajeFinal = null;
            tFases.FechaDesmontaje = null;
            tFases.IdUsuarioDesmontaje = null;
            tFases.ObservacionesDesmontaje = null;
            tFases.ObservacionesTratamiento = null;
            tFases.BloqueoGrua = fase.BloqueoGrua;
            if (tFases.BloqueoGrua == true) { tFases.IdUsuarioBloqueoGrua = user; }
            else { tFases.IdUsuarioBloqueoGrua = null; }
            tFases.SituacionBloqueoGrua = fase.SituacionBloqueoGrua;

            entity.tFases.AddObject(tFases);
            entity.SaveChanges();
        }

        public void EditFase(EditarFaseView editFase)
        {
            tFase tFases = entity.tFases.First(m => m.IdFase == editFase.IdFase);
            editFase.FaseActual.IdPrototipo = editFase.IdPrototipo;
            editFase.FaseActual.IdFase = editFase.IdFase;
            editFase.FaseActual.EsInicial = editFase.EsInicial;
            editFase.FaseActual.FechaInsercion = editFase.FechaInsercion;
            editFase.FaseActual.FechaPuestaMarcha = editFase.FechaPuestaMarcha;
            editFase.FaseActual.IdUsuario = editFase.IdUsuario;
            editFase.FaseActual.ConfiguracionMontaje = editFase.ConfiguracionMontaje;
            editFase.FaseActual.Observaciones = editFase.Observaciones;
            editFase.FaseActual.Deficiencia = editFase.Deficiencia;
            editFase.FaseActual.Desmontaje = editFase.Desmontaje;
            var user = HttpContext.Current.User.Identity.Name;
            if (editFase.BloqueoGrua == true) { editFase.FaseActual.IdUsuarioBloqueoGrua = user; }
            else { editFase.FaseActual.IdUsuarioBloqueoGrua = null; }
            editFase.FaseActual.SituacionBloqueoGrua = editFase.SituacionBloqueoGrua;

            entity.tFases.ApplyCurrentValues(editFase.FaseActual);
            entity.SaveChanges();
        }

        public void AddDeficiencia(DeficienciaView deficiencia)
        {
            DB.tDeficiencia tdeficiencia = new DB.tDeficiencia();
            tdeficiencia.IdPrototipo = deficiencia.IdPrototipo;
            tdeficiencia.IdFase = deficiencia.IdFase;
            tdeficiencia.Fecha = DateTime.Now;
            var user = HttpContext.Current.User.Identity.Name;
            tdeficiencia.IdUsuarioCreador = user;
            tdeficiencia.Descripcion = deficiencia.Descripcion;
            tdeficiencia.Limitaciones = deficiencia.Limitaciones;
            tdeficiencia.Bloqueo = deficiencia.Bloqueo;
            if (tdeficiencia.Bloqueo == true) { tdeficiencia.IdUsuarioBloqueo = user; }
            else { tdeficiencia.IdUsuarioBloqueo = null; }

            entity.tDeficiencias.AddObject(tdeficiencia);
            entity.SaveChanges();
        }

        public void DeficienciaSi(int idFase)
        {
            tFase tFase = entity.tFases.First(f => f.IdFase == idFase);
            var query2 = (from d in entity.tDeficiencias
                          where d.IdFase == idFase
                          select d).FirstOrDefault();
            if (tFase.Deficiencia == false && TieneDefFase(idFase) == true)
            {
                tFase.Deficiencia = true;
                entity.tFases.ApplyCurrentValues(tFase);
                entity.SaveChanges();
            }
        }

        public void AddSistema(int id, string otros, int[] selectedSistemas)
        {
            foreach (var sistema in tSistemas)
            {
                if (IsSistemaSelected(sistema.IdSistema, selectedSistemas))
                {
                    DB.tFaseAfecta faseAfecta = new DB.tFaseAfecta();
                    faseAfecta.IdDeficienciaFas = id;
                    faseAfecta.IdSistema = sistema.IdSistema;
                    faseAfecta.Fecha = DateTime.Now;

                    if (!IsDefSistemaExist(id, sistema.IdSistema))
                    {
                        if (sistema.IdSistema == 29) { faseAfecta.Otros = otros; }
                        else { faseAfecta.Otros = null; }

                        entity.tFaseAfectas.AddObject(faseAfecta);
                        entity.SaveChanges();
                    }
                    else if (sistema.IdSistema == 29)
                    {
                        tFaseAfecta tFaseAf = entity.tFaseAfectas.First(m => m.IdDeficienciaFas == id && m.IdSistema == sistema.IdSistema);
                        if (tFaseAf.Otros != otros)
                        {
                            faseAfecta.Otros = otros;
                            entity.tFaseAfectas.ApplyCurrentValues(faseAfecta);
                            entity.SaveChanges();
                        }
                    }
                }
                else
                {
                    if (IsDefSistemaExist(id, sistema.IdSistema))
                    {
                        tFaseAfecta tFaseAf = entity.tFaseAfectas.First(m => m.IdDeficienciaFas == id && m.IdSistema == sistema.IdSistema);
                        entity.tFaseAfectas.DeleteObject(tFaseAf);
                        entity.SaveChanges();
                    }
                }
            }
        }

        public void AddSistemaPrueba(int id, string otros, int[] selectedSistemas)
        {
            foreach (var sistema in tSistemas)
            {
                if (IsSistemaSelected(sistema.IdSistema, selectedSistemas))
                {
                    DB.tPruebaAfecta pruebaAfecta = new DB.tPruebaAfecta();
                    pruebaAfecta.IdDeficienciaPru = id;
                    pruebaAfecta.IdSistema = sistema.IdSistema;
                    pruebaAfecta.Fecha = DateTime.Now;

                    if (!IsDefPruSistemaExist(id, sistema.IdSistema))
                    {
                        if (sistema.IdSistema == 29) { pruebaAfecta.Otros = otros; }
                        else { pruebaAfecta.Otros = null; }

                        entity.tPruebaAfectas.AddObject(pruebaAfecta);
                        entity.SaveChanges();
                    }
                    else if (sistema.IdSistema == 29)
                    {
                        tPruebaAfecta tPruebaAf = entity.tPruebaAfectas.First(m => m.IdDeficienciaPru == id && m.IdSistema == sistema.IdSistema);
                        if (tPruebaAf.Otros != otros)
                        {
                            pruebaAfecta.Otros = otros;
                            entity.tPruebaAfectas.ApplyCurrentValues(pruebaAfecta);
                            entity.SaveChanges();
                        }
                    }
                }
                else
                {
                    if (IsDefPruSistemaExist(id, sistema.IdSistema))
                    {
                        tPruebaAfecta tPruebaAf = entity.tPruebaAfectas.First(m => m.IdDeficienciaPru == id && m.IdSistema == sistema.IdSistema);
                        entity.tPruebaAfectas.DeleteObject(tPruebaAf);
                        entity.SaveChanges();
                    }
                }
            }
        }

        public void AddDesmontaje(DesmontajeView faseDes)
        {
            tFase tFases = entity.tFases.First(m => m.IdFase == faseDes.IdFase);
            faseDes.FaseActual.IdPrototipo = faseDes.IdPrototipo;
            faseDes.FaseActual.EsInicial = faseDes.EsInicial;
            faseDes.FaseActual.FechaInsercion = faseDes.FechaInsercion;
            faseDes.FaseActual.FechaPuestaMarcha = faseDes.FechaPuestaMarcha;
            faseDes.FaseActual.IdUsuario = faseDes.IdUsuario;
            faseDes.FaseActual.ConfiguracionMontaje = faseDes.ConfiguracionMontaje;
            faseDes.FaseActual.Observaciones = faseDes.Observaciones;
            faseDes.FaseActual.Deficiencia = faseDes.Deficiencia;
            faseDes.FaseActual.Desmontaje = true;
            faseDes.FaseActual.DesmontajeFinal = faseDes.DesmontajeFinal;
            faseDes.FaseActual.FechaDesmontaje = faseDes.FechaDesmontaje;
            var user = HttpContext.Current.User.Identity.Name;
            faseDes.FaseActual.IdUsuarioDesmontaje = user;
            faseDes.FaseActual.ObservacionesDesmontaje = faseDes.ObservacionesDesmontaje;
            faseDes.FaseActual.ObservacionesTratamiento = faseDes.ObservacionesTratamiento;
            faseDes.FaseActual.BloqueoGrua = faseDes.BloqueoGrua;
            faseDes.FaseActual.IdUsuarioBloqueoGrua = faseDes.IdUsuarioBloqueoGrua;
            faseDes.FaseActual.SituacionBloqueoGrua = faseDes.SituacionBloqueoGrua;

            if (faseDes.FechaDesbloqueoGrua == DateTime.MinValue) { faseDes.FaseActual.FechaDesbloqueoGrua = null; }
            else { faseDes.FaseActual.FechaDesbloqueoGrua = faseDes.FechaDesbloqueoGrua; }
            if (faseDes.FechaRegistroDesbloqueoGrua == DateTime.MinValue) { faseDes.FaseActual.FechaRegistroDesbloqueoGrua = null; }
            else { faseDes.FaseActual.FechaRegistroDesbloqueoGrua = faseDes.FechaRegistroDesbloqueoGrua; }

            faseDes.FaseActual.IdUsuarioDesbloqueoGrua = faseDes.IdUsuarioDesbloqueoGrua;
            faseDes.FaseActual.RazonDesbloqueoGrua = faseDes.RazonDesbloqueoGrua;

            entity.tFases.ApplyCurrentValues(faseDes.FaseActual);
            entity.SaveChanges();
        }

        public void AddDesmontajeConBloqueoGrua(DesmontajeView faseDes)
        {
            tFase tFases = entity.tFases.First(m => m.IdFase == faseDes.IdFase);
            faseDes.FaseActual.IdPrototipo = faseDes.IdPrototipo;
            faseDes.FaseActual.EsInicial = faseDes.EsInicial;
            faseDes.FaseActual.FechaInsercion = faseDes.FechaInsercion;
            faseDes.FaseActual.FechaPuestaMarcha = faseDes.FechaPuestaMarcha;
            faseDes.FaseActual.IdUsuario = faseDes.IdUsuario;
            faseDes.FaseActual.ConfiguracionMontaje = faseDes.ConfiguracionMontaje;
            faseDes.FaseActual.Observaciones = faseDes.Observaciones;
            faseDes.FaseActual.Deficiencia = faseDes.Deficiencia;
            faseDes.FaseActual.Desmontaje = true;
            faseDes.FaseActual.DesmontajeFinal = faseDes.DesmontajeFinal;
            faseDes.FaseActual.FechaDesmontaje = faseDes.FechaDesmontaje;
            var user = HttpContext.Current.User.Identity.Name;
            faseDes.FaseActual.IdUsuarioDesmontaje = user;
            faseDes.FaseActual.ObservacionesDesmontaje = faseDes.ObservacionesDesmontaje;
            faseDes.FaseActual.ObservacionesTratamiento = faseDes.ObservacionesTratamiento;
            faseDes.FaseActual.BloqueoGrua = false;
            faseDes.FaseActual.IdUsuarioBloqueoGrua = null;
            faseDes.FaseActual.SituacionBloqueoGrua = null;
            faseDes.FaseActual.FechaDesbloqueoGrua = faseDes.FechaDesmontaje;
            faseDes.FaseActual.FechaRegistroDesbloqueoGrua = DateTime.Now;
            faseDes.FaseActual.IdUsuarioDesbloqueoGrua = user;
            faseDes.FaseActual.RazonDesbloqueoGrua = "Se desbloquea al desmontar la puesta en marcha";

            entity.tFases.ApplyCurrentValues(faseDes.FaseActual);
            entity.SaveChanges();
        }

        public void EditarDeficiencia(EditarDefView editDef)
        {
            tDeficiencia tDeficiencias = entity.tDeficiencias.First(m => m.IdDeficiencia == editDef.IdDeficiencia);
            editDef.DeficienciaActual.IdPrototipo = editDef.IdPrototipo;
            editDef.DeficienciaActual.IdFase = editDef.IdFase;
            editDef.DeficienciaActual.Fecha = editDef.Fecha;
            editDef.DeficienciaActual.IdUsuarioCreador = editDef.IdUsuarioCreador;
            editDef.DeficienciaActual.Descripcion = editDef.Descripcion;
            editDef.DeficienciaActual.Limitaciones = editDef.Limitaciones;
            editDef.DeficienciaActual.Bloqueo = editDef.Bloqueo;
            var user = HttpContext.Current.User.Identity.Name;
            if (tDeficiencias.Bloqueo == false && editDef.Bloqueo == true) { editDef.DeficienciaActual.IdUsuarioBloqueo = user; }
            else { editDef.DeficienciaActual.IdUsuarioBloqueo = editDef.IdUsuarioBloqueo; }

            entity.tDeficiencias.ApplyCurrentValues(editDef.DeficienciaActual);
            entity.SaveChanges();
        }

        public void EditarDeficienciaPrueba(EditarDefPruebaView editDefPru)
        {
            tDeficienciaPrueba tDeficienciaPruebas = entity.tDeficienciaPruebas.First(m => m.IdDeficiencia == editDefPru.IdDeficiencia);
            editDefPru.DefPruebaActual.IdPrototipo = editDefPru.IdPrototipo;
            editDefPru.DefPruebaActual.IdPrueba = editDefPru.IdPrueba;
            editDefPru.DefPruebaActual.Fecha = editDefPru.Fecha;
            editDefPru.DefPruebaActual.IdUsuarioCreador = editDefPru.IdUsuarioCreador;
            editDefPru.DefPruebaActual.Descripcion = editDefPru.Descripcion;
            editDefPru.DefPruebaActual.Limitaciones = editDefPru.Limitaciones;
            editDefPru.DefPruebaActual.Bloqueo = editDefPru.Bloqueo;
            var user = HttpContext.Current.User.Identity.Name;
            if (tDeficienciaPruebas.Bloqueo == false && editDefPru.Bloqueo == true) { editDefPru.DefPruebaActual.IdUsuarioBloqueo = user; }
            else { editDefPru.DefPruebaActual.IdUsuarioBloqueo = editDefPru.IdUsuarioBloqueo; }

            entity.tDeficienciaPruebas.ApplyCurrentValues(editDefPru.DefPruebaActual);
            entity.SaveChanges();
        }

        public void ResolverDeficiencia(ResolucionDefView resDef)
        {
            tDeficiencia tDeficiencias = entity.tDeficiencias.First(m => m.IdDeficiencia == resDef.IdDeficiencia);
            resDef.DeficienciaActual.IdPrototipo = resDef.IdPrototipo;
            resDef.DeficienciaActual.IdFase = resDef.IdFase;
            resDef.DeficienciaActual.Fecha = resDef.Fecha;
            resDef.DeficienciaActual.IdUsuarioCreador = resDef.IdUsuarioCreador;
            resDef.DeficienciaActual.Descripcion = resDef.Descripcion;
            resDef.DeficienciaActual.Limitaciones = resDef.Limitaciones;
            resDef.DeficienciaActual.Bloqueo = resDef.Bloqueo;
            resDef.DeficienciaActual.IdUsuarioBloqueo = resDef.IdUsuarioBloqueo;
            resDef.DeficienciaActual.FechaResolucion = resDef.FechaResolucion;
            resDef.DeficienciaActual.FechaRegistroResolucion = DateTime.Now;
            var user = HttpContext.Current.User.Identity.Name;
            resDef.DeficienciaActual.IdUsuarioResolucion = user;
            resDef.DeficienciaActual.ObservacionesResolucion = resDef.ObservacionesResolucion;

            entity.tDeficiencias.ApplyCurrentValues(resDef.DeficienciaActual);
            entity.SaveChanges();
        }

        public void DesbloquearDeficiencia(DesbloqueoDefView desDef)
        {
            tDeficiencia tDeficiencias = entity.tDeficiencias.First(m => m.IdDeficiencia == desDef.IdDeficiencia);
            desDef.DeficienciaActual.IdPrototipo = desDef.IdPrototipo;
            desDef.DeficienciaActual.IdFase = desDef.IdFase;
            desDef.DeficienciaActual.Fecha = desDef.Fecha;
            desDef.DeficienciaActual.IdUsuarioCreador = desDef.IdUsuarioCreador;
            desDef.DeficienciaActual.Descripcion = desDef.Descripcion;
            desDef.DeficienciaActual.Limitaciones = desDef.Limitaciones;
            desDef.DeficienciaActual.Bloqueo = false;
            desDef.DeficienciaActual.FechaDesbloqueo = desDef.FechaDesbloqueo;
            desDef.DeficienciaActual.FechaRegistroDesbloqueo = DateTime.Now;
            var user = HttpContext.Current.User.Identity.Name;
            desDef.DeficienciaActual.IdUsuarioDesbloqueo = user;
            desDef.DeficienciaActual.IdUsuarioBloqueo = desDef.IdUsuarioBloqueo;
            desDef.DeficienciaActual.RazonDesbloqueo = desDef.RazonDesbloqueo;
            desDef.DeficienciaActual.FechaResolucion = desDef.FechaResolucion;
            desDef.DeficienciaActual.IdUsuarioResolucion = desDef.IdUsuarioResolucion;
            desDef.DeficienciaActual.ObservacionesResolucion = desDef.ObservacionesResolucion;

            entity.tDeficiencias.ApplyCurrentValues(desDef.DeficienciaActual);
            entity.SaveChanges();
        }

        public void DesbloquearGrua(DesbloqueoGruaView desGrua)
        {
            tFase tFases = entity.tFases.First(m => m.IdFase == desGrua.IdFase);
            desGrua.FaseActual.IdPrototipo = desGrua.IdPrototipo;
            desGrua.FaseActual.IdFase = desGrua.IdFase;
            desGrua.FaseActual.EsInicial = desGrua.EsInicial;
            desGrua.FaseActual.FechaInsercion = desGrua.FechaInsercion;
            desGrua.FaseActual.FechaPuestaMarcha = desGrua.FechaPuestaMarcha;
            desGrua.FaseActual.IdUsuario = desGrua.IdUsuario;
            desGrua.FaseActual.ConfiguracionMontaje = desGrua.ConfiguracionMontaje;
            desGrua.FaseActual.Observaciones = desGrua.Observaciones;
            desGrua.FaseActual.Deficiencia = desGrua.Deficiencia;
            desGrua.FaseActual.Desmontaje = desGrua.Desmontaje;
            desGrua.FaseActual.BloqueoGrua = false;
            desGrua.FaseActual.IdUsuarioBloqueoGrua = desGrua.IdUsuarioBloqueoGrua;
            desGrua.FaseActual.SituacionBloqueoGrua = null;
            desGrua.FaseActual.FechaDesbloqueoGrua = desGrua.FechaDesbloqueoGrua;
            desGrua.FaseActual.FechaRegistroDesbloqueoGrua = DateTime.Now;
            var user = HttpContext.Current.User.Identity.Name;
            desGrua.FaseActual.IdUsuarioDesbloqueoGrua = user;
            desGrua.FaseActual.RazonDesbloqueoGrua = desGrua.RazonDesbloqueoGrua;

            entity.tFases.ApplyCurrentValues(desGrua.FaseActual);
            entity.SaveChanges();
        }

        public void AddPrueba(PruebaView prueba)
        {
            DB.tPrueba tPruebas = new DB.tPrueba();
            tPruebas.IdPrototipo = prueba.IdPrototipo;
            tPruebas.FechaPrueba = prueba.FechaPrueba;
            tPruebas.FechaRegistroPrueba = DateTime.Now;
            var user = HttpContext.Current.User.Identity.Name;
            tPruebas.IdUsuario = user;
            tPruebas.Prueba = prueba.Prueba;
            tPruebas.Deficiencia = prueba.Deficiencia;
            tPruebas.BloqueoGrua = prueba.BloqueoGrua;
            if (tPruebas.BloqueoGrua == true) { tPruebas.IdUsuarioBloqueoGrua = user; }
            else { tPruebas.IdUsuarioBloqueoGrua = null; }
            tPruebas.SituacionBloqueoGrua = prueba.SituacionBloqueoGrua;

            entity.tPruebas.AddObject(tPruebas);
            entity.SaveChanges();
        }

        public void EditPrueba(EditarPruebaView editPru)
        {
            tPrueba tPruebas = entity.tPruebas.First(m => m.IdPrueba == editPru.IdPrueba);
            editPru.PruebaActual.IdPrototipo = editPru.IdPrototipo;
            editPru.PruebaActual.IdPrueba = editPru.IdPrueba;
            editPru.PruebaActual.FechaPrueba = editPru.FechaPrueba;
            editPru.PruebaActual.FechaRegistroPrueba = editPru.FechaRegistroPrueba;
            editPru.PruebaActual.IdUsuario = editPru.IdUsuario;
            editPru.PruebaActual.Prueba = editPru.Prueba;
            editPru.PruebaActual.Deficiencia = editPru.Deficiencia;
            editPru.PruebaActual.BloqueoGrua = editPru.BloqueoGrua;
            var user = HttpContext.Current.User.Identity.Name;
            if (editPru.BloqueoGrua == true) { editPru.PruebaActual.IdUsuarioBloqueoGrua = user; }
            else { editPru.PruebaActual.IdUsuarioBloqueoGrua = null; }
            editPru.PruebaActual.SituacionBloqueoGrua = editPru.SituacionBloqueoGrua;

            entity.tPruebas.ApplyCurrentValues(editPru.PruebaActual);
            entity.SaveChanges();
        }

        public void AddDefPrueba(DefPruebaView defPrueba)
        {
            DB.tDeficienciaPrueba tdefPruebas = new DB.tDeficienciaPrueba();
            tdefPruebas.IdPrototipo = defPrueba.IdPrototipo;
            tdefPruebas.IdPrueba = defPrueba.IdPrueba;
            tdefPruebas.Fecha = DateTime.Now;
            var user = HttpContext.Current.User.Identity.Name;
            tdefPruebas.IdUsuarioCreador = user;
            tdefPruebas.Descripcion = defPrueba.Descripcion;
            tdefPruebas.Limitaciones = defPrueba.Limitaciones;
            tdefPruebas.Bloqueo = defPrueba.Bloqueo;
            if (tdefPruebas.Bloqueo == true) { tdefPruebas.IdUsuarioBloqueo = user; }
            else { tdefPruebas.IdUsuarioBloqueo = null; }

            entity.tDeficienciaPruebas.AddObject(tdefPruebas);
            entity.SaveChanges();
        }

        public void DefPruebaSi(int idPrueba)
        {
            tPrueba tPrueba = entity.tPruebas.First(f => f.IdPrueba == idPrueba);
            var query2 = (from d in entity.tDeficienciaPruebas
                          where d.IdPrueba == idPrueba
                          select d).FirstOrDefault();
            if (tPrueba.Deficiencia == false && TieneDefPrueba(idPrueba) == true)
            {
                tPrueba.Deficiencia = true;
                entity.tPruebas.ApplyCurrentValues(tPrueba);
                entity.SaveChanges();
            }
        }

        public void ResolverDefPrueba(ResolucionDefPruebaView resDefPrueba)
        {
            tDeficienciaPrueba tDefPruebas = entity.tDeficienciaPruebas.First(m => m.IdDeficiencia == resDefPrueba.IdDeficiencia);
            resDefPrueba.DefPruebaActual.IdPrototipo = resDefPrueba.IdPrototipo;
            resDefPrueba.DefPruebaActual.IdPrueba = resDefPrueba.IdPrueba;
            resDefPrueba.DefPruebaActual.Fecha = resDefPrueba.Fecha;
            resDefPrueba.DefPruebaActual.IdUsuarioCreador = resDefPrueba.IdUsuarioCreador;
            resDefPrueba.DefPruebaActual.Descripcion = resDefPrueba.Descripcion;
            resDefPrueba.DefPruebaActual.Limitaciones = resDefPrueba.Limitaciones;
            resDefPrueba.DefPruebaActual.Bloqueo = resDefPrueba.Bloqueo;
            resDefPrueba.DefPruebaActual.IdUsuarioBloqueo = resDefPrueba.IdUsuarioBloqueo;
            resDefPrueba.DefPruebaActual.FechaResolucion = resDefPrueba.FechaResolucion;
            resDefPrueba.DefPruebaActual.FechaRegistroResolucion = DateTime.Now;
            var user = HttpContext.Current.User.Identity.Name;
            resDefPrueba.DefPruebaActual.IdUsuarioResolucion = user;
            resDefPrueba.DefPruebaActual.ObservacionesResolucion = resDefPrueba.ObservacionesResolucion;

            entity.tDeficienciaPruebas.ApplyCurrentValues(resDefPrueba.DefPruebaActual);
            entity.SaveChanges();
        }

        public void DesbloquearDefPrueba(DesbloqueoDefPruebaView desDefPrueba)
        {
            tDeficienciaPrueba tDeficienciaPruebas = entity.tDeficienciaPruebas.First(m => m.IdDeficiencia == desDefPrueba.IdDeficiencia);
            desDefPrueba.DefPruebaActual.IdPrototipo = desDefPrueba.IdPrototipo;
            desDefPrueba.DefPruebaActual.IdPrueba = desDefPrueba.IdPrueba;
            desDefPrueba.DefPruebaActual.Fecha = desDefPrueba.Fecha;
            desDefPrueba.DefPruebaActual.IdUsuarioCreador = desDefPrueba.IdUsuarioCreador;
            desDefPrueba.DefPruebaActual.Descripcion = desDefPrueba.Descripcion;
            desDefPrueba.DefPruebaActual.Limitaciones = desDefPrueba.Limitaciones;
            desDefPrueba.DefPruebaActual.Bloqueo = false;
            desDefPrueba.DefPruebaActual.FechaDesbloqueo = desDefPrueba.FechaDesbloqueo;
            desDefPrueba.DefPruebaActual.FechaRegistroDesbloqueo = DateTime.Now;
            var user = HttpContext.Current.User.Identity.Name;
            desDefPrueba.DefPruebaActual.IdUsuarioDesbloqueo = user;
            desDefPrueba.DefPruebaActual.IdUsuarioBloqueo = desDefPrueba.IdUsuarioBloqueo;
            desDefPrueba.DefPruebaActual.RazonDesbloqueo = desDefPrueba.RazonDesbloqueo;

            entity.tDeficienciaPruebas.ApplyCurrentValues(desDefPrueba.DefPruebaActual);
            entity.SaveChanges();
        }

        public void DesbloquearGruaPrueba(DesbloqueoGruaPruebaView desGruapru)
        {
            tPrueba tPruebas = entity.tPruebas.First(m => m.IdPrueba == desGruapru.IdPrueba);
            desGruapru.PruebaActual.IdPrototipo = desGruapru.IdPrototipo;
            desGruapru.PruebaActual.IdPrueba = desGruapru.IdPrueba;
            desGruapru.PruebaActual.FechaPrueba = desGruapru.FechaPrueba;
            desGruapru.PruebaActual.FechaRegistroPrueba = desGruapru.FechaRegistroPrueba;
            desGruapru.PruebaActual.IdUsuario = desGruapru.IdUsuario;
            desGruapru.PruebaActual.Prueba = desGruapru.Prueba;
            desGruapru.PruebaActual.Deficiencia = desGruapru.Deficiencia;
            desGruapru.PruebaActual.BloqueoGrua = false;
            desGruapru.PruebaActual.IdUsuarioBloqueoGrua = desGruapru.IdUsuarioBloqueoGrua;
            desGruapru.PruebaActual.SituacionBloqueoGrua = null;
            desGruapru.PruebaActual.FechaDesbloqueoGrua = desGruapru.FechaDesbloqueoGrua;
            desGruapru.PruebaActual.FechaRegistroDesbloqueoGrua = DateTime.Now;
            var user = HttpContext.Current.User.Identity.Name;
            desGruapru.PruebaActual.IdUsuarioDesbloqueoGrua = user;
            desGruapru.PruebaActual.RazonDesbloqueoGrua = desGruapru.RazonDesbloqueoGrua;

            entity.tPruebas.ApplyCurrentValues(desGruapru.PruebaActual);
            entity.SaveChanges();
        }

        public void Cierre(CierreProyectoView cierreProy)
        {
            tPrototipo prototipo = entity.tPrototipos.First(p => p.IdPrototipo == cierreProy.IdPrototipo);

            ProyectoActual.FechaCreacion = cierreProy.FechaCreacion;
            ProyectoActual.IdUsuarioCreador = cierreProy.IdUsuarioCreador;
            ProyectoActual.Proyecto = cierreProy.Proyecto;
            ProyectoActual.Responsable = cierreProy.Responsable;
            ProyectoActual.Configuracion = cierreProy.Configuracion;
            ProyectoActual.Descripcion = cierreProy.Descripcion;
            ProyectoActual.Observaciones = cierreProy.Observaciones;
            ProyectoActual.Estado = 0;
            ProyectoActual.FechaCierre = cierreProy.FechaCierre;
            var userCierre = HttpContext.Current.User.Identity.Name;
            ProyectoActual.IdUsuarioCierre = userCierre;
            ProyectoActual.Cierre = cierreProy.Cierre;

            entity.tPrototipos.ApplyCurrentValues(ProyectoActual);
            entity.SaveChanges();
        }

        public void AddPieza(PiezaView pieza)
        {
            DB.tTratamientoPieza tTratPieza = new DB.tTratamientoPieza();
            tTratPieza.IdFase = pieza.IdFase;
            tTratPieza.Articulo = pieza.Articulo;
            tTratPieza.Accion = pieza.Accion;

            entity.tTratamientoPiezas.AddObject(tTratPieza);
            entity.SaveChanges();
        }
    }
}