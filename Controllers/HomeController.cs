using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prototipos.Models.DB;
using Prototipos.Models.ViewModels;
using Prototipos.Models.ObjectManager;

namespace Prototipos.Controllers
{
    public class HomeController : Controller
    {
        PrototiposEntities dre = new PrototiposEntities();


        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult Creacion()
        {
            ProyectoView proy = new ProyectoView();
            ViewBag.Message = " de un nuevo proyecto";

            var query1 = from u in dre.tUsuarios
                         where u.Departamento == "I+D"
                         select u;
            ViewBag.idUsuario = new SelectList(query1.AsEnumerable(), "idUsuario", "Usuario");

            return View(proy);
        }

        // POST: /Home/Creacion
        [HttpPost]
        public ActionResult Creacion(ProyectoView proy)
        {
            if (ModelState.IsValid)
            {
                ProyectosManager proyectosManager = new ProyectosManager();
                if (proyectosManager.ExisteProyecto(proy.Proyecto))
                {
                    ModelState.AddModelError("", "Ya existe un proyecto con ese nombre.");
                }
                else
                {
                    proyectosManager.Add(proy);
                    var query = from u in dre.tUsuarios
                                where u.Departamento == "I+D"
                                select u;
                    ViewBag.idUsuario = new SelectList(query.AsEnumerable(), "idUsuario", "Usuario");
                    TempData["SuccessMessage"] = "Proyecto creado con éxito";
                    return RedirectToAction("Creacion");
                }
            }
            ViewBag.Message = " de un nuevo proyecto";
            var query1 = from u in dre.tUsuarios
                         select u;
            ViewBag.idUsuario = new SelectList(query1.AsEnumerable(), "idUsuario", "Usuario");
            return View(proy);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult Gestion(string searchString)
        {
            ViewBag.Message = " de un proyecto abierto";
            var proyectos = from p in dre.tPrototipos
                            where p.Estado == 1
                            select p;
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            if (!String.IsNullOrEmpty(searchString))
            {
                proyectos = proyectos.Where(p => p.Proyecto.Contains(searchString));
            }

            proyectos = from p in proyectos
                        orderby p.FechaCreacion descending
                        select p;

            return View(proyectos);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult EditarProyecto(int id)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            EditarProyectoView editProy = new EditarProyectoView(id);
            if (editProy.IdUsuarioCreador == user)
            {
                string proyecto = editProy.ProyectoActual.Proyecto;
                ViewBag.Message = proyecto;

                string usCreador = editProy.IdUsuarioCreador;
                var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
                ViewBag.usCreador = usCre.Usuario;

                var query1 = from u in dre.tUsuarios
                             where u.Departamento == "I+D"
                             select u;
                ViewBag.idUsuario = new SelectList(query1.AsEnumerable(), "idUsuario", "Usuario", editProy.ProyectoActual.Responsable);

                return View(editProy);
            }
            else
            {
                TempData["ErrorMessage"] = "Solo puede editar el proyecto su creador";
                return RedirectToAction("Gestion");
            }
        }

        // POST: /Home/EditarProyecto
        [HttpPost]
        public ActionResult EditarProyecto(EditarProyectoView editProy)
        {
            if (ModelState.IsValid)
            {
                ProyectosManager proyectosManager = new ProyectosManager();
                proyectosManager.EditProy(editProy);
                var query = from u in dre.tUsuarios
                            where u.Departamento == "I+D"
                            select u;
                ViewBag.idUsuario = new SelectList(query.AsEnumerable(), "idUsuario", "Usuario");
                TempData["SuccessMessage"] = "Proyecto editado con éxito";
                return RedirectToAction("GestionProyecto", new { id = editProy.IdPrototipo });
            }
            string proyecto = editProy.ProyectoActual.Proyecto;
            ViewBag.Message = proyecto;

            string usCreador = editProy.IdUsuarioCreador;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            var query1 = from u in dre.tUsuarios
                         select u;
            ViewBag.idUsuario = new SelectList(query1.AsEnumerable(), "idUsuario", "Usuario");
            return View(editProy);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult GestionProyecto(int id)
        {
            ArchivosProyManager proy = new ArchivosProyManager(id);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = id;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            var cre = (from u in dre.tUsuarios
                       where u.IdUsuario == proy.ProyectoActual.IdUsuarioCreador
                       select u).FirstOrDefault();
            ViewBag.creador = cre.Usuario;

            var resp = (from u in dre.tUsuarios
                        where u.IdUsuario == proy.ProyectoActual.Responsable
                        select u).FirstOrDefault();
            ViewBag.responsable = resp.Usuario;

            int est = proy.ProyectoActual.Estado;
            if (est == 1)
                ViewBag.estado = "Abierto";
            else
                ViewBag.estado = "Cerrado";

            return View(proy);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult Montaje(int id)
        {
            ProyectosManager proy = new ProyectosManager(id);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.Message3 = "Puestas en marcha del proyecto";
            ViewBag.idProy = id;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(proy);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult MarchaInicial(int id)
        {
            ProyectosManager proy = new ProyectosManager(id);
            if (proy.ExisteFaseInicial(id))
            {
                TempData["ErrorMessage"] = "Ya existe un montaje inicial para ese proyecto";
                return RedirectToAction("Montaje", new { id = id });
            }
            else
            {
                FaseView fase = new FaseView(id);
                string proyecto = fase.ProyectoActual.Proyecto;
                ViewBag.Message = " del proyecto ";
                ViewBag.Message2 = proyecto;
                ViewBag.idProy = id;
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

                return View(fase);
            }
        }

        // POST: /Home/MarchaInicial
        [HttpPost]
        public ActionResult MarchaInicial(FaseView fase)
        {
            string proyecto = fase.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = fase.IdPrototipo;
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            if (ModelState.IsValid)
            {
                if (fase.BloqueoGrua == true && fase.SituacionBloqueoGrua == null)
                {
                    TempData["ErrorMessage"] = "Si la prueba bloquea la KX1953, debe describir su situación";
                }
                else
                {
                    ProyectosManager proyectosManager = new ProyectosManager();
                    proyectosManager.AddFaseInicial(fase);
                    TempData["SuccessMessage"] = "Montaje inicial creado con éxito";
                    if (fase.Deficiencia == true)
                    {
                        var query = (from f in dre.tFases
                                     where f.IdPrototipo == fase.IdPrototipo && f.EsInicial == true
                                     orderby f.FechaInsercion descending
                                     select f).FirstOrDefault();
                        int idFase = query.IdFase;
                        return RedirectToAction("NuevaDeficiencia", new { id = idFase });
                    }
                    return RedirectToAction("Montaje", new { id = fase.IdPrototipo });
                }
            }

            return View(fase);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult MarchaDerivadas(int id)
        {
            ProyectosManager proy = new ProyectosManager(id);
            if (proy.ExisteFaseSinDesmontar(id))
            {
                TempData["ErrorMessage"] = "No se pueden crear nuevos montajes porque el último está sin desmontar";
                return RedirectToAction("Montaje", new { id = id });
            }
            if (!proy.ExisteFaseInicial(id))
            {
                TempData["ErrorMessage"] = "Todavía no existe un montaje inicial";
                return RedirectToAction("Montaje", new { id = id });
            }
            else
            {
                FaseView fase = new FaseView(id);
                string proyecto = fase.ProyectoActual.Proyecto;
                ViewBag.Message = " del proyecto ";
                ViewBag.Message2 = proyecto;
                ViewBag.idProy = id;
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

                return View(fase);
            }
        }

        // POST: /Home/MarchaDerivadas
        [HttpPost]
        public ActionResult MarchaDerivadas(FaseView fase)
        {
            if (ModelState.IsValid)
            {
                if (fase.BloqueoGrua == true && fase.SituacionBloqueoGrua == null)
                {
                    TempData["ErrorMessage"] = "Si la prueba bloquea la KX1953, debe describir su situación";
                }
                else
                {
                    ProyectosManager proyectosManager = new ProyectosManager();
                    proyectosManager.AddFaseDerivada(fase);
                    TempData["SuccessMessage"] = "Montaje derivado creado con éxito";
                    if (fase.Deficiencia == true)
                    {
                        var query = (from f in dre.tFases
                                     where f.IdPrototipo == fase.IdPrototipo && f.EsInicial == false
                                     orderby f.FechaInsercion descending
                                     select f).FirstOrDefault();
                        int idFase = query.IdFase;
                        return RedirectToAction("NuevaDeficiencia", new { id = idFase });
                    }
                    return RedirectToAction("Montaje", new { id = fase.IdPrototipo });
                }
            }
            string proyecto = fase.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = fase.IdPrototipo;
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(fase);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult EditarFase(int id)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            EditarFaseView editFase = new EditarFaseView(id);
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == editFase.IdPrototipo);
            if (editFase.FaseActual.Desmontaje == false)
            {
                if (editFase.IdUsuario == user)
                {
                    string proyecto = proy.Proyecto;
                    ViewBag.Message = " del proyecto ";
                    ViewBag.Message2 = proyecto;
                    ViewBag.idProy = proy.IdPrototipo;
                    ViewBag.SuccessMessage = TempData["SuccessMessage"];
                    ViewBag.ErrorMessage = TempData["ErrorMessage"];


                    string usCreador = editFase.IdUsuario;
                    var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
                    ViewBag.usCreador = usCre.Usuario;

                    if (editFase.BloqueoGrua == true) { ViewBag.BloqueoGrua = "Sí"; }
                    else { ViewBag.BloqueoGrua = "No"; }

                    string usBloqueoGrua = editFase.IdUsuarioBloqueoGrua;
                    if (usBloqueoGrua != null)
                    {
                        var usBloqGrua = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usBloqueoGrua);
                        ViewBag.usBloqueoGrua = usBloqGrua.Usuario;
                    }

                    return View(editFase);
                }
                else
                {
                    TempData["ErrorMessage"] = "Solo puede editar la puesta en marcha su creador";
                    return RedirectToAction("Montaje", new { id = proy.IdPrototipo });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No se pueden editar puestas en marcha desmontadas";
                return RedirectToAction("Montaje", new { id = proy.IdPrototipo });
            }
        }

        // POST: /Home/EditarFase
        [HttpPost]
        public ActionResult EditarFase(EditarFaseView editFase)
        {
            if (ModelState.IsValid)
            {
                if (editFase.BloqueoGrua == true && editFase.SituacionBloqueoGrua == null)
                {
                    TempData["ErrorMessage"] = "Si la prueba bloquea la KX1953, debe describir su situación";
                }
                else
                {
                    ProyectosManager proyectosManager = new ProyectosManager();
                    var fasAntes = (from p in dre.tFases
                                    where p.IdFase == editFase.IdFase
                                    select p).FirstOrDefault();
                    if (fasAntes.Deficiencia == false && editFase.Deficiencia == true)
                    {
                        return RedirectToAction("NuevaDeficiencia", new { id = editFase.IdFase });
                    }
                    proyectosManager.EditFase(editFase);
                    TempData["SuccessMessage"] = "Fase editada con éxito";
                    return RedirectToAction("Montaje", new { id = editFase.IdPrototipo });
                }
            }
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == editFase.IdPrototipo);
            string proyecto = proy.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = editFase.IdPrototipo;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            string usCreador = editFase.IdUsuario;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            if (editFase.BloqueoGrua == true) { ViewBag.BloqueoGrua = "Sí"; }
            else { ViewBag.BloqueoGrua = "No"; }

            string usBloqueoGrua = editFase.IdUsuarioBloqueoGrua;
            if (usBloqueoGrua != null)
            {
                var usBloqGrua = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usBloqueoGrua);
                ViewBag.usBloqueoGrua = usBloqGrua.Usuario;
            }

            return View(editFase);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult NuevaDeficiencia(int id)
        {
            DeficienciaView deficiencia = new DeficienciaView(id);
            var fase = dre.tFases.FirstOrDefault(f => f.IdFase == id);
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == fase.IdPrototipo);
            string proyecto = proy.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = proy.IdPrototipo;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(deficiencia);
        }

        // POST: /Home/NuevaDeficiencia
        [HttpPost]
        public ActionResult NuevaDeficiencia(DeficienciaView deficiencia, int[] selectedSistemas)
        {
            var fase = dre.tFases.FirstOrDefault(f => f.IdFase == deficiencia.IdFase);
            if (ModelState.IsValid)
            {
                ProyectosManager proyectosManager = new ProyectosManager();
                if (selectedSistemas == null)
                {
                    TempData["ErrorMessage"] = "Debe especificar al menos un sistema de seguridad o limitador";
                }
                else if (proyectosManager.IsOtrosSelected(selectedSistemas) && deficiencia.Otros == null)
                {
                    TempData["ErrorMessage"] = "Si como sistema de seguridad elige 'Otros', debe especificarlo";
                }
                else
                {
                    proyectosManager.AddDeficiencia(deficiencia);
                    proyectosManager.DeficienciaSi(deficiencia.IdFase);

                    var query = (from d in dre.tDeficiencias
                                 where d.IdPrototipo == deficiencia.IdPrototipo && d.IdFase == deficiencia.IdFase
                                 orderby d.Fecha descending
                                 select d).FirstOrDefault();
                    int id = query.IdDeficiencia;
                    proyectosManager.AddSistema(id, deficiencia.Otros, selectedSistemas);

                    if (deficiencia.Bloqueo == true)
                    {
                        TempData["SuccessMessage"] = "¡Debe poner carteles de bloqueo a pie de prototipo y en mando control remoto!";
                    }

                    return RedirectToAction("DeficienciaOk", new { id = fase.IdFase });
                }
            }
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == fase.IdPrototipo);
            string proyecto = proy.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = fase.IdPrototipo;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(deficiencia);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DeficienciaOk(int id)
        {
            var fase = dre.tFases.FirstOrDefault(f => f.IdFase == id);
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == fase.IdPrototipo);
            ViewBag.idProy = proy.IdPrototipo;
            ViewBag.idFase = fase.IdFase;
            ViewBag.AvisoMessage = TempData["SuccessMessage"];

            return View();
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult Desmontaje(int id)
        {
            ProyectosManager proy = new ProyectosManager(id);
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.WarningMessage = TempData["WarningMessage"];
            if (proy.ExisteFaseSinDesmontar(id))
            {
                var fase = dre.tFases.FirstOrDefault(f => f.IdPrototipo == id && f.Desmontaje == false);
                int idFas = fase.IdFase;
                if (proy.TieneDefFaseSinResolver(idFas))
                {
                    TempData["ErrorMessage"] = "Antes de desmontar, es necesario resolver las deficiencias que tiene el montaje. ¡Recuerde copiar la información de las deficiencias si luego quiere añadir un nuevo montaje con deficiencias similares!";
                    return RedirectToAction("Resolucion", new { id = id });
                }
                else
                {
                    DesmontajeView faseDes = new DesmontajeView(idFas);
                    string proyecto = proy.ProyectoActual.Proyecto;
                    ViewBag.Message = " del proyecto ";
                    ViewBag.Message2 = proyecto;
                    ViewBag.Message3 = "Piezas del desmontaje";
                    ViewBag.idProy = faseDes.FaseActual.IdPrototipo;
                    ViewBag.idFase = faseDes.IdFase;

                    if (faseDes.FaseActual.EsInicial == true) { ViewBag.inicial = "Sí"; }
                    else { ViewBag.inicial = "No"; }

                    string us = faseDes.FaseActual.IdUsuario;
                    var usuario = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == us);
                    ViewBag.usuario = usuario.Usuario;

                    if (faseDes.FaseActual.Deficiencia == true) { ViewBag.deficiencia = "Sí"; }
                    else { ViewBag.deficiencia = "No"; }

                    return View(faseDes);
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No existe ningún montaje a desmontar";
                return RedirectToAction("Montaje", new { id = id });
            }
        }

        // POST: /Home/Desmontaje
        [HttpPost]
        public ActionResult Desmontaje(DesmontajeView faseDes)
        {
            ProyectosManager proy = new ProyectosManager(faseDes.IdPrototipo);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = faseDes.IdPrototipo;

            if (faseDes.FaseActual.EsInicial == true) { ViewBag.inicial = "Sí"; }
            else { ViewBag.inicial = "No"; }

            string us = faseDes.FaseActual.IdUsuario;
            var usuario = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == us);
            ViewBag.usuario = usuario.Usuario;

            if (faseDes.FaseActual.Deficiencia == true) { ViewBag.deficiencia = "Sí"; }
            else { ViewBag.deficiencia = "No"; }

            if (ModelState.IsValid)
            {
                if (proy.TieneTratPiezas(faseDes.IdFase) || faseDes.DesmontajeFinal == true || (faseDes.DesmontajeFinal == false && !proy.TieneTratPiezas(faseDes.IdFase) && faseDes.ObservacionesTratamiento != null))
                {
                    if (proy.TieneGruaBloqueada(faseDes.IdFase))
                    {
                        proy.AddDesmontajeConBloqueoGrua(faseDes);
                        TempData["SuccessMessage"] = "Desmontaje añadido con éxito";
                        return RedirectToAction("Montaje", new { id = faseDes.IdPrototipo });
                    }
                    proy.AddDesmontaje(faseDes);
                    TempData["SuccessMessage"] = "Desmontaje añadido con éxito";
                    return RedirectToAction("Montaje", new { id = faseDes.IdPrototipo });
                }
                else
                {
                    TempData["ErrorMessage"] = "Si no es el desmontaje final y no añade tratamiento de piezas, debe especificar en observaciones de tratamiento la razón";
                    return RedirectToAction("Desmontaje", new { id = faseDes.IdPrototipo });
                }
            }
            return View(faseDes);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult TratamientoPieza(int id)
        {
            PiezaView pieza = new PiezaView(id);
            var fase = dre.tFases.FirstOrDefault(f => f.IdFase == id);
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == fase.IdPrototipo);
            string proyecto = proy.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = proy.IdPrototipo;

            return View(pieza);
        }

        // POST: /Home/TratamientoPieza
        [HttpPost]
        public ActionResult TratamientoPieza(PiezaView pieza)
        {
            var fase = dre.tFases.FirstOrDefault(f => f.IdFase == pieza.IdFase);
            if (ModelState.IsValid)
            {
                ProyectosManager proyectosManager = new ProyectosManager();
                proyectosManager.AddPieza(pieza);
                TempData["SuccessMessage"] = "Pieza añadida con éxito";
                return RedirectToAction("Desmontaje", new { id = fase.IdPrototipo });
            }
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == fase.IdPrototipo);
            string proyecto = proy.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = fase.IdPrototipo;

            return View(pieza);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult Resolucion(int id)
        {
            ProyectosManager proy = new ProyectosManager(id);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.Message3 = "Deficiencias de puestas en marcha:";
            ViewBag.Message4 = "Deficiencias de pruebas:";
            ViewBag.idProy = id;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(proy);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult EditarDefResolucion(int id)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            EditarDefView editDef = new EditarDefView(id);
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == editDef.IdPrototipo);
            if (editDef.DeficienciaActual.FechaResolucion == null)
            {
                if (editDef.IdUsuarioCreador == user || proy.Responsable == user)
                {
                    string proyecto = proy.Proyecto;
                    ViewBag.Message = " del proyecto ";
                    ViewBag.Message2 = proyecto;
                    ViewBag.idProy = editDef.IdPrototipo;

                    string usCreador = editDef.IdUsuarioCreador;
                    var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
                    ViewBag.usCreador = usCre.Usuario;

                    if (editDef.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
                    else { ViewBag.Bloqueo = "No"; }

                    string usBloqueo = editDef.IdUsuarioBloqueo;
                    if (usBloqueo != null)
                    {
                        var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usBloqueo);
                        ViewBag.usBloqueo = usBloq.Usuario;
                    }

                    ViewBag.otros = (from fa in dre.tFaseAfectas
                                     where fa.IdDeficienciaFas == id && fa.IdSistema == 29
                                     select fa.Otros).FirstOrDefault();

                    ViewBag.ErrorMessage = TempData["ErrorMessage"];

                    return View(editDef);
                }
                else
                {
                    TempData["ErrorMessage"] = "Solo puede editar la deficiencia su creador o el responsable del proyecto";
                    return RedirectToAction("Resolucion", new { id = editDef.IdPrototipo });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No se pueden editar deficiencias resueltas. Cree una nueva.";
                return RedirectToAction("Resolucion", new { id = editDef.IdPrototipo });
            }
        }

        // POST: /Home/EditarDefResolucion
        [HttpPost]
        public ActionResult EditarDefResolucion(EditarDefView editDef, int[] selectedSistemas)
        {
            ProyectosManager proy = new ProyectosManager(editDef.IdPrototipo);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = editDef.IdPrototipo;

            string usCreador = editDef.IdUsuarioCreador;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            if (editDef.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
            else { ViewBag.Bloqueo = "No"; }

            string usBloqueo = editDef.IdUsuarioBloqueo;
            if (usBloqueo != null)
            {
                var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usBloqueo);
                ViewBag.usBloqueo = usBloq.Usuario;
            }

            ViewBag.otros = (from fa in dre.tFaseAfectas
                             where fa.IdDeficienciaFas == editDef.IdDeficiencia && fa.IdSistema == 29
                             select fa.Otros).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (selectedSistemas == null)
                {
                    TempData["ErrorMessage"] = "Debe especificar al menos un sistema de seguridad o limitador";
                }
                else if (proy.IsOtrosSelected(selectedSistemas) && editDef.Otros == null)
                {
                    TempData["ErrorMessage"] = "Si como sistema de seguridad elige 'Otros', debe especificarlo";
                }
                else
                {
                    proy.EditarDeficiencia(editDef);
                    proy.AddSistema(editDef.IdDeficiencia, editDef.Otros, selectedSistemas);

                    if (editDef.Bloqueo == true)
                    {
                        TempData["ErrorMessage"] = "¡Debe poner carteles de bloqueo a pie de prototipo y en mando control remoto!";
                    }

                    TempData["SuccessMessage"] = "Deficiencia editada con éxito";
                    return RedirectToAction("Resolucion", new { id = editDef.IdPrototipo });
                }
            }

            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(editDef);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult ResolucionDef(int id)
        {
            ResolucionDefView resDef = new ResolucionDefView(id);
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == resDef.IdPrototipo);
            if (resDef.DeficienciaBloq(resDef.IdDeficiencia))
            {
                return RedirectToAction("ProtBloqueado", new { id = resDef.IdDeficiencia });
            }
            string proyecto = proy.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = resDef.IdPrototipo;

            string usCreador = resDef.IdUsuarioCreador;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            if (resDef.DeficienciaActual.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
            else { ViewBag.Bloqueo = "No"; }

            var query = from a in dre.v_faseDefSinResSistemas
                        where a.IdDeficienciaFas == id
                        select a;
            ViewBag.Sistemas = new SelectList(query.AsEnumerable(), "IdSistema", "Nombre");

            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(resDef);
        }

        // POST: /Home/ResolucionDef
        [HttpPost]
        public ActionResult ResolucionDef(ResolucionDefView resDef)
        {
            ProyectosManager proy = new ProyectosManager(resDef.IdPrototipo);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = resDef.IdPrototipo;

            string usCreador = resDef.IdUsuarioCreador;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            if (resDef.DeficienciaActual.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
            else { ViewBag.Bloqueo = "No"; }

            var query = from a in dre.v_faseDefSinResSistemas
                        where a.IdDeficienciaFas == resDef.IdDeficiencia
                        select a;
            ViewBag.Sistemas = new SelectList(query.AsEnumerable(), "IdSistema", "Nombre");

            if (ModelState.IsValid)
            {
                if (resDef.FechaResolucion < resDef.Fecha.Date)
                {
                    TempData["ErrorMessage"] = "La fecha de resolución no puede ser anterior a la de creación de la deficiencia";
                }
                else
                {
                    proy.ResolverDeficiencia(resDef);
                    TempData["SuccessMessage"] = "Deficiencia resuelta con éxito";
                    return RedirectToAction("Resolucion", new { id = resDef.IdPrototipo });
                }
            }

            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(resDef);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult ProtBloqueado(int id)
        {
            ResolucionDefView resDef = new ResolucionDefView(id);
            ViewBag.idProy = resDef.IdPrototipo;
            ViewBag.idDeficiencia = id;
            ViewBag.Message = "Primero debe desbloquearla, ¿desea hacerlo ahora?";

            return View();
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult EditarDefResolucionPrueba(int id)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            EditarDefPruebaView editDefPru = new EditarDefPruebaView(id);
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == editDefPru.IdPrototipo);
            if (editDefPru.DefPruebaActual.FechaResolucion == null)
            {
                if (editDefPru.IdUsuarioCreador == user || proy.Responsable == user)
                {
                    string proyecto = proy.Proyecto;
                    ViewBag.Message = " del proyecto ";
                    ViewBag.Message2 = proyecto;
                    ViewBag.idProy = editDefPru.IdPrototipo;

                    string usCreador = editDefPru.IdUsuarioCreador;
                    var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
                    ViewBag.usCreador = usCre.Usuario;

                    if (editDefPru.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
                    else { ViewBag.Bloqueo = "No"; }

                    string usBloqueo = editDefPru.IdUsuarioBloqueo;
                    if (usBloqueo != null)
                    {
                        var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usBloqueo);
                        ViewBag.usBloqueo = usBloq.Usuario;
                    }

                    ViewBag.otros = (from fa in dre.tPruebaAfectas
                                     where fa.IdDeficienciaPru == id && fa.IdSistema == 29
                                     select fa.Otros).FirstOrDefault();

                    ViewBag.ErrorMessage = TempData["ErrorMessage"];

                    return View(editDefPru);
                }
                else
                {
                    TempData["ErrorMessage"] = "Solo puede editar la deficiencia su creador o el responsable del proyecto";
                    return RedirectToAction("Resolucion", new { id = editDefPru.IdPrototipo });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No se pueden editar deficiencias resueltas. Cree una nueva.";
                return RedirectToAction("Resolucion", new { id = editDefPru.IdPrototipo });
            }
        }

        // POST: /Home/EditarDefResolucionPrueba
        [HttpPost]
        public ActionResult EditarDefResolucionPrueba(EditarDefPruebaView editDefPru, int[] selectedSistemas)
        {
            ProyectosManager proy = new ProyectosManager(editDefPru.IdPrototipo);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = editDefPru.IdPrototipo;

            string usCreador = editDefPru.IdUsuarioCreador;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            if (editDefPru.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
            else { ViewBag.Bloqueo = "No"; }

            string usBloqueo = editDefPru.IdUsuarioBloqueo;
            if (usBloqueo != null)
            {
                var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usBloqueo);
                ViewBag.usBloqueo = usBloq.Usuario;
            }

            ViewBag.otros = (from fa in dre.tFaseAfectas
                             where fa.IdDeficienciaFas == editDefPru.IdDeficiencia && fa.IdSistema == 29
                             select fa.Otros).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (selectedSistemas == null)
                {
                    TempData["ErrorMessage"] = "Debe especificar al menos un sistema de seguridad o limitador";
                }
                else if (proy.IsOtrosSelected(selectedSistemas) && editDefPru.Otros == null)
                {
                    TempData["ErrorMessage"] = "Si como sistema de seguridad elige 'Otros', debe especificarlo";
                }
                else
                {
                    proy.EditarDeficienciaPrueba(editDefPru);
                    proy.AddSistemaPrueba(editDefPru.IdDeficiencia, editDefPru.Otros, selectedSistemas);

                    if (editDefPru.Bloqueo == true)
                    {
                        TempData["ErrorMessage"] = "¡Debe poner carteles de bloqueo a pie de prototipo y en mando control remoto!";
                    }

                    TempData["SuccessMessage"] = "Deficiencia editada con éxito";
                    return RedirectToAction("Resolucion", new { id = editDefPru.IdPrototipo });
                }
            }

            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(editDefPru);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult ResolucionDefPrueba(int id)
        {
            ResolucionDefPruebaView resDefPrueba = new ResolucionDefPruebaView(id);
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == resDefPrueba.IdPrototipo);
            string proyecto = proy.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = resDefPrueba.IdPrototipo;

            string usCreador = resDefPrueba.IdUsuarioCreador;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            if (resDefPrueba.DefPruebaActual.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
            else { ViewBag.Bloqueo = "No"; }

            var query = from a in dre.v_pruebaDefSinResSistemas
                        where a.IdDeficienciaPru == id
                        select a;
            ViewBag.Sistemas = new SelectList(query.AsEnumerable(), "IdSistema", "Nombre");

            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(resDefPrueba);
        }

        // POST: /Home/ResolucionDefPrueba
        [HttpPost]
        public ActionResult ResolucionDefPrueba(ResolucionDefPruebaView resDefPrueba)
        {
            ProyectosManager proy = new ProyectosManager(resDefPrueba.IdPrototipo);
            string proyecto = proy.ProyectoActual.Proyecto;
            if (resDefPrueba.DeficienciaBloq(resDefPrueba.IdDeficiencia))
            {
                return RedirectToAction("ProtBloqueadoPru", new { id = resDefPrueba.IdDeficiencia });
            }
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = resDefPrueba.IdPrototipo;

            string usCreador = resDefPrueba.IdUsuarioCreador;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            if (resDefPrueba.DefPruebaActual.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
            else { ViewBag.Bloqueo = "No"; }

            var query = from a in dre.v_pruebaDefSinResSistemas
                        where a.IdDeficienciaPru == resDefPrueba.IdDeficiencia
                        select a;
            ViewBag.Sistemas = new SelectList(query.AsEnumerable(), "IdSistema", "Nombre");

            if (ModelState.IsValid)
            {
                if (resDefPrueba.FechaResolucion < resDefPrueba.Fecha.Date)
                {
                    TempData["ErrorMessage"] = "La fecha de resolución no puede ser anterior a la de creación de la deficiencia";
                }
                else
                {
                    proy.ResolverDefPrueba(resDefPrueba);
                    TempData["SuccessMessage"] = "Deficiencia resuelta con éxito";
                    return RedirectToAction("Resolucion", new { id = resDefPrueba.IdPrototipo });
                }
            }

            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(resDefPrueba);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult ProtBloqueadoPru(int id)
        {
            ResolucionDefPruebaView resDefPrueba = new ResolucionDefPruebaView(id);
            ViewBag.idProy = resDefPrueba.IdPrototipo;
            ViewBag.idDeficiencia = id;
            ViewBag.Message = "Primero debe desbloquearla, ¿desea hacerlo ahora?";

            return View();
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult GestionPruebas(int id)
        {
            PruebaView prueba = new PruebaView(id);
            string proyecto = prueba.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.Message3 = "Pruebas del proyecto:";
            ViewBag.idProy = id;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ProyectosManager proy = new ProyectosManager(id);
            if (proy.ProtBloqueado(id) || proy.ProtBloqueadoPru(id))
            {
                ViewBag.ProtBloqueado = "EL PROTOTIPO ESTÁ BLOQUEADO. No debería hacer pruebas.";
            }

            return View(prueba);
        }

        // POST: /Home/GestionPruebas
        [HttpPost]
        public ActionResult GestionPruebas(PruebaView pru)
        {
            if (ModelState.IsValid)
            {
                if (pru.BloqueoGrua == true && pru.SituacionBloqueoGrua == null)
                {
                    TempData["ErrorMessage"] = "Si la prueba bloquea la KX1953, debe describir su situación";
                }
                else
                {
                    ProyectosManager proyectosManager = new ProyectosManager();
                    proyectosManager.AddPrueba(pru);
                    TempData["SuccessMessage"] = "Prueba creada con éxito";
                    if (pru.Deficiencia == true)
                    {
                        var query = (from p in dre.tPruebas
                                     where p.IdPrototipo == pru.IdPrototipo
                                     orderby p.FechaRegistroPrueba descending
                                     select p).FirstOrDefault();
                        int idPrueba = query.IdPrueba;
                        return RedirectToAction("NuevaDefPrueba", new { id = idPrueba });
                    }
                    return RedirectToAction("GestionPruebas", new { id = pru.IdPrototipo });
                }
            }

            string proyecto = pru.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.Message3 = "Pruebas del proyecto:";
            ViewBag.idProy = pru.IdPrototipo;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ProyectosManager proy = new ProyectosManager(pru.IdPrototipo);
            if (proy.ProtBloqueado(pru.IdPrototipo) || proy.ProtBloqueadoPru(pru.IdPrototipo))
            {
                ViewBag.ProtBloqueado = "EL PROTOTIPO ESTÁ BLOQUEADO. No debería hacer pruebas.";
                return RedirectToAction("Index");
            }
            return View(pru);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult EditarPrueba(int id)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            EditarPruebaView editPru = new EditarPruebaView(id);
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == editPru.IdPrototipo);
            if (editPru.IdUsuario == user)
            {
                string proyecto = proy.Proyecto;
                ViewBag.Message = " del proyecto ";
                ViewBag.Message2 = proyecto;
                ViewBag.idProy = proy.IdPrototipo;
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
                ViewBag.ErrorMessage = TempData["ErrorMessage"];

                string usCreador = editPru.IdUsuario;
                var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
                ViewBag.usCreador = usCre.Usuario;

                if (editPru.BloqueoGrua == true) { ViewBag.BloqueoGrua = "Sí"; }
                else { ViewBag.BloqueoGrua = "No"; }

                string usBloqueoGrua = editPru.IdUsuarioBloqueoGrua;
                if (usBloqueoGrua != null)
                {
                    var usBloqGrua = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usBloqueoGrua);
                    ViewBag.usBloqueoGrua = usBloqGrua.Usuario;
                }

                return View(editPru);
            }
            else
            {
                TempData["ErrorMessage"] = "Solo puede editar la prueba su creador";
                return RedirectToAction("GestionPruebas", new { id = proy.IdPrototipo });
            }
        }

        // POST: /Home/EditarPrueba
        [HttpPost]
        public ActionResult EditarPrueba(EditarPruebaView editPru)
        {
            if (ModelState.IsValid)
            {
                if (editPru.BloqueoGrua == true && editPru.SituacionBloqueoGrua == null)
                {
                    TempData["ErrorMessage"] = "Si la prueba bloquea la KX1953, debe describir su situación";
                }
                else
                {
                    ProyectosManager proyectosManager = new ProyectosManager();
                    var pruAntes = (from p in dre.tPruebas
                                    where p.IdPrueba == editPru.IdPrueba
                                    select p).FirstOrDefault();
                    if (pruAntes.Deficiencia == false && editPru.Deficiencia == true)
                    {
                        return RedirectToAction("NuevaDefPrueba", new { id = editPru.IdPrueba });
                    }
                    proyectosManager.EditPrueba(editPru);
                    TempData["SuccessMessage"] = "Prueba editada con éxito";
                    return RedirectToAction("GestionPruebas", new { id = editPru.IdPrototipo });
                }
            }
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == editPru.IdPrototipo);
            string proyecto = proy.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = editPru.IdPrototipo;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            string usCreador = editPru.IdUsuario;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            if (editPru.BloqueoGrua == true) { ViewBag.BloqueoGrua = "Sí"; }
            else { ViewBag.BloqueoGrua = "No"; }

            string usBloqueoGrua = editPru.IdUsuarioBloqueoGrua;
            if (usBloqueoGrua != null)
            {
                var usBloqGrua = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usBloqueoGrua);
                ViewBag.usBloqueoGrua = usBloqGrua.Usuario;
            }

            return View(editPru);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult NuevaDefPrueba(int id)
        {
            DefPruebaView defPrueba = new DefPruebaView(id);
            var prueba = dre.tPruebas.FirstOrDefault(f => f.IdPrueba == id);
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == prueba.IdPrototipo);
            string proyecto = proy.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = proy.IdPrototipo;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(defPrueba);
        }

        // POST: /Home/NuevaDeficiencia
        [HttpPost]
        public ActionResult NuevaDefPrueba(DefPruebaView defPrueba, int[] selectedSistemas)
        {
            var prueba = dre.tPruebas.FirstOrDefault(f => f.IdPrueba == defPrueba.IdPrueba);
            if (ModelState.IsValid)
            {
                ProyectosManager proyectosManager = new ProyectosManager();
                if (selectedSistemas == null)
                {
                    TempData["ErrorMessage"] = "Debe especificar al menos un sistema de seguridad o limitador";
                }
                else if (proyectosManager.IsOtrosSelected(selectedSistemas) && defPrueba.Otros == null)
                {
                    TempData["ErrorMessage"] = "Si como sistema de seguridad elige 'Otros', debe especificarlo";
                }
                else
                {
                    proyectosManager.AddDefPrueba(defPrueba);
                    proyectosManager.DefPruebaSi(defPrueba.IdPrueba);
                    var query = (from d in dre.tDeficienciaPruebas
                                 where d.IdPrototipo == defPrueba.IdPrototipo && d.IdPrueba == defPrueba.IdPrueba
                                 orderby d.Fecha descending
                                 select d).FirstOrDefault();
                    int id = query.IdDeficiencia;
                    proyectosManager.AddSistemaPrueba(id, defPrueba.Otros, selectedSistemas);

                    if (defPrueba.Bloqueo == true)
                    {
                        TempData["SuccessMessage"] = "¡Debe poner carteles de bloqueo a pie de prototipo y en mando control remoto!";
                    }
                    return RedirectToAction("DefPruebaOk", new { id = prueba.IdPrueba });
                }
            }
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == prueba.IdPrototipo);
            string proyecto = proy.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = prueba.IdPrototipo;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(defPrueba);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DefPruebaOk(int id)
        {
            var prueba = dre.tPruebas.FirstOrDefault(f => f.IdPrueba == id);
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == prueba.IdPrototipo);
            ViewBag.idProy = proy.IdPrototipo;
            ViewBag.idPrueba = prueba.IdPrueba;
            ViewBag.AvisoMessage = TempData["SuccessMessage"];

            return View();
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult Situacion(int id)
        {
            SituacionManager proy = new SituacionManager(id);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = id;

            return View(proy);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult ImprimirSituacion(int id)
        {
            SituacionManager proy = new SituacionManager(id);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = id;

            return View(proy);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult Tratamiento(int id)
        {
            ProyectosManager proy = new ProyectosManager(id);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = id;

            return View(proy);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult Desbloqueo(int id, string searchString)
        {
            ProyectosManager proy = new ProyectosManager(id);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.Message3 = "Deficiencias de puestas en marcha:";
            ViewBag.Message4 = "Deficiencias de pruebas:";
            ViewBag.Message5 = "Bloqueos de puestas en marcha:";
            ViewBag.Message6 = "Bloqueos de pruebas:";
            ViewBag.idProy = id;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(proy);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult EditarDefDesbloqueo(int id)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            EditarDefView editDef = new EditarDefView(id);
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == editDef.IdPrototipo);
            if (editDef.DeficienciaActual.FechaResolucion == null)
            {
                if (editDef.IdUsuarioCreador == user || proy.Responsable == user)
                {
                    string proyecto = proy.Proyecto;
                    ViewBag.Message = " del proyecto ";
                    ViewBag.Message2 = proyecto;
                    ViewBag.idProy = editDef.IdPrototipo;

                    string usCreador = editDef.IdUsuarioCreador;
                    var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
                    ViewBag.usCreador = usCre.Usuario;

                    if (editDef.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
                    else { ViewBag.Bloqueo = "No"; }

                    string usBloqueo = editDef.IdUsuarioBloqueo;
                    if (usBloqueo != null)
                    {
                        var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usBloqueo);
                        ViewBag.usBloqueo = usBloq.Usuario;
                    }

                    ViewBag.otros = (from fa in dre.tFaseAfectas
                                     where fa.IdDeficienciaFas == id && fa.IdSistema == 29
                                     select fa.Otros).FirstOrDefault();

                    ViewBag.ErrorMessage = TempData["ErrorMessage"];

                    return View(editDef);
                }
                else
                {
                    TempData["ErrorMessage"] = "Solo puede editar la deficiencia su creador o el responsable del proyecto";
                    return RedirectToAction("Desbloqueo", new { id = editDef.IdPrototipo });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No se pueden editar deficiencias resueltas. Cree una nueva.";
                return RedirectToAction("Desbloqueo", new { id = editDef.IdPrototipo });
            }
        }

        // POST: /Home/EditarDefDesbloqueo
        [HttpPost]
        public ActionResult EditarDefDesbloqueo(EditarDefView editDef, int[] selectedSistemas)
        {
            ProyectosManager proy = new ProyectosManager(editDef.IdPrototipo);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = editDef.IdPrototipo;

            string usCreador = editDef.IdUsuarioCreador;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            if (editDef.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
            else { ViewBag.Bloqueo = "No"; }

            string usBloqueo = editDef.IdUsuarioBloqueo;
            if (usBloqueo != null)
            {
                var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usBloqueo);
                ViewBag.usBloqueo = usBloq.Usuario;
            }

            ViewBag.otros = (from fa in dre.tFaseAfectas
                             where fa.IdDeficienciaFas == editDef.IdDeficiencia && fa.IdSistema == 29
                             select fa.Otros).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (selectedSistemas == null)
                {
                    TempData["ErrorMessage"] = "Debe especificar al menos un sistema de seguridad o limitador";
                }
                else if (proy.IsOtrosSelected(selectedSistemas) && editDef.Otros == null)
                {
                    TempData["ErrorMessage"] = "Si como sistema de seguridad elige 'Otros', debe especificarlo";
                }
                else
                {
                    proy.EditarDeficiencia(editDef);
                    proy.AddSistema(editDef.IdDeficiencia, editDef.Otros, selectedSistemas);

                    if (editDef.Bloqueo == true)
                    {
                        TempData["ErrorMessage"] = "¡Debe poner carteles de bloqueo a pie de prototipo y en mando control remoto!";
                    }

                    TempData["SuccessMessage"] = "Deficiencia editada con éxito";
                    return RedirectToAction("Desbloqueo", new { id = editDef.IdPrototipo });
                }
            }

            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(editDef);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult EditarDefDesbloqueoPrueba(int id)
        {
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            EditarDefPruebaView editDefPru = new EditarDefPruebaView(id);
            var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == editDefPru.IdPrototipo);
            if (editDefPru.DefPruebaActual.FechaResolucion == null)
            {
                if (editDefPru.IdUsuarioCreador == user || proy.Responsable == user)
                {
                    string proyecto = proy.Proyecto;
                    ViewBag.Message = " del proyecto ";
                    ViewBag.Message2 = proyecto;
                    ViewBag.idProy = editDefPru.IdPrototipo;

                    string usCreador = editDefPru.IdUsuarioCreador;
                    var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
                    ViewBag.usCreador = usCre.Usuario;

                    if (editDefPru.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
                    else { ViewBag.Bloqueo = "No"; }

                    string usBloqueo = editDefPru.IdUsuarioBloqueo;
                    if (usBloqueo != null)
                    {
                        var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usBloqueo);
                        ViewBag.usBloqueo = usBloq.Usuario;
                    }

                    ViewBag.otros = (from fa in dre.tPruebaAfectas
                                     where fa.IdDeficienciaPru == id && fa.IdSistema == 29
                                     select fa.Otros).FirstOrDefault();

                    ViewBag.ErrorMessage = TempData["ErrorMessage"];

                    return View(editDefPru);
                }
                else
                {
                    TempData["ErrorMessage"] = "Solo puede editar la deficiencia su creador o el responsable del proyecto";
                    return RedirectToAction("Desbloqueo", new { id = editDefPru.IdPrototipo });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No se pueden editar deficiencias resueltas. Cree una nueva.";
                return RedirectToAction("Desbloqueo", new { id = editDefPru.IdPrototipo });
            }
        }

        // POST: /Home/EditarDefDesbloqueoPrueba
        [HttpPost]
        public ActionResult EditarDefDesbloqueoPrueba(EditarDefPruebaView editDefPru, int[] selectedSistemas)
        {
            ProyectosManager proy = new ProyectosManager(editDefPru.IdPrototipo);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = editDefPru.IdPrototipo;

            string usCreador = editDefPru.IdUsuarioCreador;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            if (editDefPru.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
            else { ViewBag.Bloqueo = "No"; }

            string usBloqueo = editDefPru.IdUsuarioBloqueo;
            if (usBloqueo != null)
            {
                var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usBloqueo);
                ViewBag.usBloqueo = usBloq.Usuario;
            }

            ViewBag.otros = (from fa in dre.tFaseAfectas
                             where fa.IdDeficienciaFas == editDefPru.IdDeficiencia && fa.IdSistema == 29
                             select fa.Otros).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (selectedSistemas == null)
                {
                    TempData["ErrorMessage"] = "Debe especificar al menos un sistema de seguridad o limitador";
                }
                else if (proy.IsOtrosSelected(selectedSistemas) && editDefPru.Otros == null)
                {
                    TempData["ErrorMessage"] = "Si como sistema de seguridad elige 'Otros', debe especificarlo";
                }
                else
                {
                    proy.EditarDeficienciaPrueba(editDefPru);
                    proy.AddSistemaPrueba(editDefPru.IdDeficiencia, editDefPru.Otros, selectedSistemas);

                    if (editDefPru.Bloqueo == true)
                    {
                        TempData["ErrorMessage"] = "¡Debe poner carteles de bloqueo a pie de prototipo y en mando control remoto!";
                    }

                    TempData["SuccessMessage"] = "Deficiencia editada con éxito";
                    return RedirectToAction("Desbloqueo", new { id = editDefPru.IdPrototipo });
                }
            }

            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(editDefPru);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DesbloqueoDef(int id)
        {
            DesbloqueoDefView desDef = new DesbloqueoDefView(id);
            ProyectosManager proyManager = new ProyectosManager();
            if (proyManager.EsUsuarioCorrecto(id) || proyManager.EsResponsable(desDef.IdPrototipo))
            {
                var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == desDef.IdPrototipo);
                string proyecto = proy.Proyecto;
                ViewBag.Message = " del proyecto ";
                ViewBag.Message2 = proyecto;
                ViewBag.idProy = desDef.IdPrototipo;

                string usCreador = desDef.IdUsuarioCreador;
                var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
                ViewBag.usCreador = usCre.Usuario;

                string usuarioBloq = desDef.IdUsuarioBloqueo;
                if (usuarioBloq != null)
                {
                    var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usuarioBloq);
                    ViewBag.usuarioBloq = usBloq.Usuario;
                }

                string usuarioRes = desDef.IdUsuarioResolucion;
                var usRes = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usuarioRes);
                if (usRes == null) { ViewBag.usuarioRes = ""; }
                else { ViewBag.usuarioRes = usRes.Usuario; }

                if (desDef.DeficienciaActual.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
                else { ViewBag.Bloqueo = "No"; }

                var query = from a in dre.v_faseDefSinResSistemas
                            where a.IdDeficienciaFas == id
                            select a;
                ViewBag.Sistemas = new SelectList(query.AsEnumerable(), "IdSistema", "Nombre");

                return View(desDef);
            }
            else
            {
                TempData["ErrorMessage"] = "Una deficiencia sólo puede ser desbloqueada por el responsable del proyecto o por el que la creó";
                return RedirectToAction("Desbloqueo", new { id = desDef.IdPrototipo });
            }
        }

        // POST: /Home/DesbloqueoDef
        [HttpPost]
        public ActionResult DesbloqueoDef(DesbloqueoDefView desDef)
        {
            ProyectosManager proy = new ProyectosManager(desDef.IdPrototipo);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = desDef.IdPrototipo;

            string usCreador = desDef.IdUsuarioCreador;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            string usuarioBloq = desDef.IdUsuarioBloqueo;
            if (usuarioBloq != null)
            {
                var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usuarioBloq);
                ViewBag.usuarioBloq = usBloq.Usuario;
            }

            string usuarioRes = desDef.IdUsuarioResolucion;
            var usRes = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usuarioRes);
            if (usRes == null) { ViewBag.usuarioRes = ""; }
            else { ViewBag.usuarioRes = usRes.Usuario; }

            if (desDef.DeficienciaActual.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
            else { ViewBag.Bloqueo = "No"; }

            var query = from a in dre.v_faseDefSinResSistemas
                        where a.IdDeficienciaFas == desDef.IdDeficiencia
                        select a;
            ViewBag.Sistemas = new SelectList(query.AsEnumerable(), "IdSistema", "Nombre");

            if (ModelState.IsValid)
            {
                proy.DesbloquearDeficiencia(desDef);
                TempData["ErrorMessage"] = "Acuérdese de quitar carteles de bloqueo a pie de prototipo y en mando control remoto";
                return RedirectToAction("DesbloqueoConExito", new { id = desDef.IdPrototipo });
            }

            return View(desDef);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DesbloqueoDefPrueba(int id)
        {
            DesbloqueoDefPruebaView desDefPrueba = new DesbloqueoDefPruebaView(id);
            ProyectosManager proyManager = new ProyectosManager();
            if (proyManager.EsUsuarioCorrectoPru(id) || proyManager.EsResponsable(desDefPrueba.IdPrototipo))
            {
                var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == desDefPrueba.IdPrototipo);
                string proyecto = proy.Proyecto;
                ViewBag.Message = " del proyecto ";
                ViewBag.Message2 = proyecto;
                ViewBag.idProy = desDefPrueba.IdPrototipo;

                string usCreador = desDefPrueba.IdUsuarioCreador;
                var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
                ViewBag.usCreador = usCre.Usuario;

                string usuarioBloq = desDefPrueba.IdUsuarioBloqueo;
                if (usuarioBloq != null)
                {
                    var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usuarioBloq);
                    ViewBag.usuarioBloq = usBloq.Usuario;
                }

                string usuarioRes = desDefPrueba.IdUsuarioResolucion;
                var usRes = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usuarioRes);
                if (usRes == null) { ViewBag.usuarioRes = ""; }
                else { ViewBag.usuarioRes = usRes.Usuario; }

                if (desDefPrueba.DefPruebaActual.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
                else { ViewBag.Bloqueo = "No"; }

                var query = from a in dre.v_pruebaDefSinResSistemas
                            where a.IdDeficienciaPru == id
                            select a;
                ViewBag.Sistemas = new SelectList(query.AsEnumerable(), "IdSistema", "Nombre");

                return View(desDefPrueba);
            }
            else
            {
                TempData["ErrorMessage"] = "Una deficiencia sólo puede ser desbloqueada por el responsable del proyecto o por el que la creó";
                return RedirectToAction("Desbloqueo", new { id = desDefPrueba.IdPrototipo });
            }
        }

        // POST: /Home/DesbloqueoDefPrueba
        [HttpPost]
        public ActionResult DesbloqueoDefPrueba(DesbloqueoDefPruebaView desDefPrueba)
        {
            ProyectosManager proy = new ProyectosManager(desDefPrueba.IdPrototipo);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = desDefPrueba.IdPrototipo;

            string usCreador = desDefPrueba.IdUsuarioCreador;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            string usuarioBloq = desDefPrueba.IdUsuarioBloqueo;
            if (usuarioBloq != null)
            {
                var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usuarioBloq);
                ViewBag.usuarioBloq = usBloq.Usuario;
            }

            string usuarioRes = desDefPrueba.IdUsuarioResolucion;
            var usRes = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usuarioRes);
            if (usRes == null) { ViewBag.usuarioRes = ""; }
            else { ViewBag.usuarioRes = usRes.Usuario; }

            if (desDefPrueba.DefPruebaActual.Bloqueo == true) { ViewBag.Bloqueo = "Sí"; }
            else { ViewBag.Bloqueo = "No"; }

            var query = from a in dre.v_pruebaDefSinResSistemas
                        where a.IdDeficienciaPru == desDefPrueba.IdDeficiencia
                        select a;
            ViewBag.Sistemas = new SelectList(query.AsEnumerable(), "IdSistema", "Nombre");

            if (ModelState.IsValid)
            {
                proy.DesbloquearDefPrueba(desDefPrueba);
                TempData["ErrorMessage"] = "Acuérdese de quitar carteles de bloqueo a pie de prototipo y en mando control remoto";
                return RedirectToAction("DesbloqueoConExito", new { id = desDefPrueba.IdPrototipo });
            }

            return View(desDefPrueba);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DesbloqueoConExito(int id)
        {
            ProyectosManager proy = new ProyectosManager(id);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.idProy = id;
            ViewBag.AvisoMessage = TempData["ErrorMessage"];

            return View(proy);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DesbloqueoGruaFase(int id)
        {
            DesbloqueoGruaView desGrua = new DesbloqueoGruaView(id);
            ProyectosManager proyManager = new ProyectosManager();
            if (proyManager.EsUsuarioCorrectoGrua(id) || proyManager.EsResponsable(desGrua.IdPrototipo))
            {
                var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == desGrua.IdPrototipo);
                string proyecto = proy.Proyecto;
                ViewBag.Message = " del proyecto ";
                ViewBag.Message2 = proyecto;
                ViewBag.idProy = desGrua.IdPrototipo;

                string usCreador = desGrua.IdUsuario;
                var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
                ViewBag.usCreador = usCre.Usuario;

                string usuarioBloq = desGrua.IdUsuarioBloqueoGrua;
                if (usuarioBloq != null)
                {
                    var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usuarioBloq);
                    ViewBag.usBloqueoGrua = usBloq.Usuario;
                }

                return View(desGrua);
            }
            else
            {
                TempData["ErrorMessage"] = "Una puesta en marcha sólo puede ser desbloqueada por el responsable del proyecto o por el que la creó";
                return RedirectToAction("Desbloqueo", new { id = desGrua.IdPrototipo });
            }
        }

        // POST: /Home/DesbloqueoGruaFase
        [HttpPost]
        public ActionResult DesbloqueoGruaFase(DesbloqueoGruaView desGrua)
        {
            ProyectosManager proy = new ProyectosManager(desGrua.IdPrototipo);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = desGrua.IdPrototipo;

            string usCreador = desGrua.IdUsuario;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            string usuarioBloq = desGrua.IdUsuarioBloqueoGrua;
            if (usuarioBloq != null)
            {
                var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usuarioBloq);
                ViewBag.usBloqueoGrua = usBloq.Usuario;
            }

            if (ModelState.IsValid)
            {
                proy.DesbloquearGrua(desGrua);
                TempData["SuccessMessage"] = "Desbloqueo realizado con éxito";
                return RedirectToAction("Desbloqueo", new { id = desGrua.IdPrototipo });
            }

            return View(desGrua);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DesbloqueoGruaPrueba(int id)
        {
            DesbloqueoGruaPruebaView desGruaPru = new DesbloqueoGruaPruebaView(id);
            ProyectosManager proyManager = new ProyectosManager();
            if (proyManager.EsUsuarioCorrectoGruaPrueba(id) || proyManager.EsResponsable(desGruaPru.IdPrototipo))
            {
                var proy = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == desGruaPru.IdPrototipo);
                string proyecto = proy.Proyecto;
                ViewBag.Message = " del proyecto ";
                ViewBag.Message2 = proyecto;
                ViewBag.idProy = desGruaPru.IdPrototipo;

                string usCreador = desGruaPru.IdUsuario;
                var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
                ViewBag.usCreador = usCre.Usuario;

                string usuarioBloq = desGruaPru.IdUsuarioBloqueoGrua;
                if (usuarioBloq != null)
                {
                    var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usuarioBloq);
                    ViewBag.usBloqueoGrua = usBloq.Usuario;
                }

                return View(desGruaPru);
            }
            else
            {
                TempData["ErrorMessage"] = "Una prueba sólo puede ser desbloqueada por el responsable del proyecto o por el que la creó";
                return RedirectToAction("Desbloqueo", new { id = desGruaPru.IdPrototipo });
            }
        }

        // POST: /Home/DesbloqueoGruaPrueba
        [HttpPost]
        public ActionResult DesbloqueoGruaPrueba(DesbloqueoGruaPruebaView desGruaPru)
        {
            ProyectosManager proy = new ProyectosManager(desGruaPru.IdPrototipo);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.idProy = desGruaPru.IdPrototipo;

            string usCreador = desGruaPru.IdUsuario;
            var usCre = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCreador);
            ViewBag.usCreador = usCre.Usuario;

            string usuarioBloq = desGruaPru.IdUsuarioBloqueoGrua;
            if (usuarioBloq != null)
            {
                var usBloq = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usuarioBloq);
                ViewBag.usBloqueoGrua = usBloq.Usuario;
            }

            if (ModelState.IsValid)
            {
                proy.DesbloquearGruaPrueba(desGruaPru);
                TempData["SuccessMessage"] = "Desbloqueo realizado con éxito";
                return RedirectToAction("Desbloqueo", new { id = desGruaPru.IdPrototipo });
            }

            return View(desGruaPru);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult Cierre(string searchString)
        {
            ViewBag.Message = " de un proyecto abierto";
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            var proyectos = from p in dre.tPrototipos
                            where p.Estado == 1
                            select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                proyectos = proyectos.Where(p => p.Proyecto.Contains(searchString));
            }

            proyectos = from p in proyectos
                        orderby p.FechaCreacion descending
                        select p;

            return View(proyectos);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult CierreProyecto(int id)
        {
            ProyectosManager proyManager = new ProyectosManager(id);
            if (!proyManager.ExisteFaseSinDesmontar(id))
            {
                if (proyManager.ExisteDesmontajeFinal(id))
                {
                    CierreProyectoView proy = new CierreProyectoView(id);
                    string proyecto = proy.ProyectoActual.Proyecto;
                    var cre = (from u in dre.tUsuarios
                               where u.IdUsuario == proy.ProyectoActual.IdUsuarioCreador
                               select u).FirstOrDefault();
                    ViewBag.creador = cre.Usuario;
                    string resp = proy.ProyectoActual.Responsable;
                    var usuario = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == resp);
                    ViewBag.Responsable = usuario.Usuario;

                    ViewBag.Message = " del proyecto ";
                    ViewBag.Message2 = proyecto;
                    ViewBag.idProy = id;

                    return View(proy);
                }
                else
                {
                    TempData["ErrorMessage"] = "No se puede cerrar el proyecto porque no hay ningún montaje final";
                    return RedirectToAction("Cierre");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No se puede cerrar el proyecto porque existen montajes sin desmontar";
                return RedirectToAction("Cierre");
            }
        }

        [HttpPost]
        public ActionResult CierreProyecto(CierreProyectoView proy)
        {
            if (ModelState.IsValid)
            {
                ProyectosManager proyectosManager = new ProyectosManager(proy.ProyectoActual.IdPrototipo);

                proyectosManager.Cierre(proy);
                TempData["SuccessMessage"] = "Proyecto cerrado con éxito";
                return RedirectToAction("Cierre");
            }
            string proyecto = proy.ProyectoActual.Proyecto;
            var cre = (from u in dre.tUsuarios
                       where u.IdUsuario == proy.ProyectoActual.IdUsuarioCreador
                       select u).FirstOrDefault();
            ViewBag.creador = cre.Usuario;
            string resp = proy.ProyectoActual.Responsable;
            var usuario = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == resp);
            ViewBag.Responsable = usuario.Usuario;

            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            return View(proy);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult Informes(string searchString)
        {
            ViewBag.Message = " de proyectos creados";

            var proyectos = from p in dre.tPrototipos
                            select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                proyectos = proyectos.Where(p => p.Proyecto.Contains(searchString));
            }

            proyectos = from p in proyectos
                        orderby p.FechaCreacion descending
                        select p;

            return View(proyectos);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult InformeProyecto(int id)
        {
            ProyectosManager proy = new ProyectosManager(id);
            string proyecto = proy.ProyectoActual.Proyecto;
            ViewBag.Message = " del proyecto ";
            ViewBag.Message2 = proyecto;
            ViewBag.Message3 = " - Histórico del expediente";

            var cre = (from u in dre.tUsuarios
                       where u.IdUsuario == proy.ProyectoActual.IdUsuarioCreador
                       select u).FirstOrDefault();
            ViewBag.creador = cre.Usuario;

            var resp = (from u in dre.tUsuarios
                        where u.IdUsuario == proy.ProyectoActual.Responsable
                        select u).FirstOrDefault();
            ViewBag.responsable = resp.Usuario;

            int est = proy.ProyectoActual.Estado;
            if (est == 1)
                ViewBag.estado = "Abierto";
            else
                ViewBag.estado = "Cerrado";

            string usCierre = proy.ProyectoActual.IdUsuarioCierre;
            var usCi = dre.tUsuarios.FirstOrDefault(u => u.IdUsuario == usCierre);
            if (usCi == null)
                ViewBag.usCierre = "";
            else
                ViewBag.usCierre = usCi.Usuario;

            return View(proy);
        }
    }
}

