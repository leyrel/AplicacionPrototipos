using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prototipos.Models.DB;
using Prototipos.Models.ViewModels;
using Prototipos.Models.ObjectManager;

namespace Prototipos.Controllers
{
    public class FilesController : Controller
    {
        PrototiposEntities dre = new PrototiposEntities();

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult ProyAdjuntar(int id)
        {
            ArchivosProyManager archProyManager = new ArchivosProyManager(id);
            var proyecto = archProyManager.ProyectoActual;
            ViewBag.Message = " al proyecto ";
            ViewBag.Message2 = proyecto.Proyecto;
            ViewBag.Message3 = "Seleccione el archivo a adjuntar:";
            ViewBag.idProy = id;
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(archProyManager);
        }

        // POST: /Files/ProyAdjuntar
        [HttpPost]
        public ActionResult ProyAdjuntar(HttpPostedFileBase uploadFile, int id, string descrip)
        {
            ArchivosProyManager archProyManager = new ArchivosProyManager(id);
            if (descrip.Length > 50)
            {
                TempData["ErrorMessage"] = "La descripción no puede tener más de 50 caracteres";
                return RedirectToAction("ProyAdjuntar", new { id = id });
            }
            else
            {
                archProyManager.AddArch(id);
                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    var ultArch = (from a in dre.tPrototipoArchivos
                                   where a.IdPrototipo == id
                                   orderby a.FechaAdd descending
                                   select a).First();
                    int ultArchId = ultArch.IdArch;
                    archProyManager.SaveArch(id, ultArchId, uploadFile, descrip);
                }
                return RedirectToAction("GestionProyecto", "Home", new { id = id });
            }
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult ProyAdjuntarYCerrar(int id)
        {
            ArchivosProyManager archProyManager = new ArchivosProyManager(id);
            var proyecto = archProyManager.ProyectoActual;
            ViewBag.Message = " al proyecto ";
            ViewBag.Message2 = proyecto.Proyecto;
            ViewBag.Message3 = "Seleccione el archivo a adjuntar:";
            ViewBag.idProy = id;
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(archProyManager);
        }

        // POST: /Files/ProyAdjuntarYCerrar
        [HttpPost]
        public ActionResult ProyAdjuntarYCerrar(HttpPostedFileBase uploadFile, int id, string descrip)
        {
            ArchivosProyManager archProyManager = new ArchivosProyManager(id);
            if (descrip.Length > 50)
            {
                TempData["ErrorMessage"] = "La descripción no puede tener más de 50 caracteres";
                return RedirectToAction("ProyAdjuntar", new { id = id });
            }
            else
            {
                archProyManager.AddArch(id);
                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    var ultArch = (from a in dre.tPrototipoArchivos
                                   where a.IdPrototipo == id
                                   orderby a.FechaAdd descending
                                   select a).First();
                    int ultArchId = ultArch.IdArch;
                    archProyManager.SaveArch(id, ultArchId, uploadFile, descrip);
                }
                return View("Close");
            }
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult VerArchivoProy(int id)
        {
            var archivo = dre.tPrototipoArchivos.First(m => m.IdArch == id);

            MemoryStream ms = new MemoryStream(archivo.Contenido, 0, 0, true, true);
            Response.ContentType = archivo.Tipo;
            Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(archivo.Nombre));
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
            return new FileStreamResult(Response.OutputStream, archivo.Tipo);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult EliminarArchivoProy(int id)
        {
            var archivo = dre.tPrototipoArchivos.First(m => m.IdArch == id);
            var proy = dre.tPrototipos.First(p => p.IdPrototipo == archivo.IdPrototipo);

            var context = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain);
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            var usuario = UserPrincipal.FindByIdentity(context, user);
            var nombreUsuario = String.Concat(usuario.GivenName, " ", usuario.Surname);

            if (archivo.UsuarioAdd == nombreUsuario || proy.IdUsuarioCreador == user || proy.Responsable == user)
            {
                ViewBag.idArchivo = archivo.IdArch;
                ViewBag.IdProy = archivo.IdPrototipo;

                return View();
            }
            else
            {
                TempData["ErrorMessage"] = "Solo puede eliminar el archivo el que lo ha subido o el creador o responsable del proyecto";
                return RedirectToAction("GestionProyecto", "Home", new { id = archivo.IdPrototipo });
            }

        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DeleteFileProy(int id)
        {
            var archivo = dre.tPrototipoArchivos.First(m => m.IdArch == id);
            int idPrototipo = archivo.IdPrototipo;
            ArchivosProyManager archProyManager = new ArchivosProyManager(idPrototipo);
            archProyManager.DelArch(id);

            return RedirectToAction("GestionProyecto", "Home", new { id = idPrototipo });
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult MontajeAdjuntar(int id)
        {
            ArchivosManager archManager = new ArchivosManager(id);
            var proyecto = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == archManager.FaseActual.IdPrototipo);
            ViewBag.Message = " al montaje del proyecto ";
            ViewBag.Message2 = proyecto.Proyecto;
            ViewBag.Message3 = "Seleccione el archivo a adjuntar:";
            ViewBag.idProy = archManager.FaseActual.IdPrototipo;
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(archManager);
        }

        // POST: /Files/MontajeAdjuntar
        [HttpPost]
        public ActionResult MontajeAdjuntar(HttpPostedFileBase uploadFile, int id, string descrip)
        {
            ArchivosManager archManager = new ArchivosManager(id);
            if (descrip.Length > 50)
            {
                TempData["ErrorMessage"] = "La descripción no puede tener más de 50 caracteres";
                return RedirectToAction("MontajeAdjuntar", new { id = id });
            }
            else
            {
                archManager.AddArch(id);
                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    var ultArch = (from a in dre.tFaseArchivos
                                   where a.IdFase == id
                                   orderby a.FechaAdd descending
                                   select a).First();
                    int ultArchId = ultArch.IdArch;
                    archManager.SaveArch(id, ultArchId, uploadFile, descrip);
                }
                return RedirectToAction("MontajeArchivos", new { id = id });
            }
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult MontajeArchivos(int id)
        {
            ArchivosManager archManager = new ArchivosManager(id);
            if (archManager.TieneArchFase(id))
            {
                ViewBag.idFase = id;
                ViewBag.idProy = archManager.FaseActual.IdPrototipo;
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
                return View(archManager);
            }
            else
            {
                TempData["ErrorMessage"] = "Este montaje no tiene archivos adjuntos";
                return RedirectToAction("Montaje", "Home", new { id = archManager.FaseActual.IdPrototipo });
            }
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult VerArchivo(int id)
        {
            var archivo = dre.tFaseArchivos.First(m => m.IdArch == id);

            MemoryStream ms = new MemoryStream(archivo.Contenido, 0, 0, true, true);
            Response.ContentType = archivo.Tipo;
            Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(archivo.Nombre));
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
            return new FileStreamResult(Response.OutputStream, archivo.Tipo);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult EliminarArchivoFase(int id)
        {
            var archivo = dre.tFaseArchivos.First(m => m.IdArch == id);
            var fase = dre.tFases.First(m => m.IdFase == archivo.IdFase);
            var proy = dre.tPrototipos.First(p => p.IdPrototipo == fase.IdPrototipo);

            var context = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain);
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            var usuario = UserPrincipal.FindByIdentity(context, user);
            var nombreUsuario = String.Concat(usuario.GivenName, " ", usuario.Surname);

            if (archivo.UsuarioAdd == nombreUsuario || fase.IdUsuario == user || proy.Responsable == user)
            {
                ViewBag.idArchivo = archivo.IdArch;
                ViewBag.idFase = archivo.IdFase;
                ViewBag.IdProy = fase.IdPrototipo;
            }
            else
            {
                TempData["ErrorMessage"] = "Solo puede eliminar el archivo el que lo ha subido, el creador del montaje o el responsable del proyecto";
                return RedirectToAction("MontajeArchivos", new { id = archivo.IdFase });
            }

            return View();
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DeleteFileFase(int id)
        {
            var archivo = dre.tFaseArchivos.First(m => m.IdArch == id);
            int idFase = archivo.IdFase;
            ArchivosManager archManager = new ArchivosManager(idFase);
            archManager.DelArch(id);

            return RedirectToAction("MontajeArchivos", new { id = idFase });
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DefsFase(int id)
        {
            ArchivosDefManager archDefManager = new ArchivosDefManager();
            var fase = dre.tFases.FirstOrDefault(f => f.IdFase == id);
            ViewBag.idProy = fase.IdPrototipo;
            ViewBag.idFase = id;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.Message = " del montaje";

            return View(archDefManager);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult EditarDefFase(int id)
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
                    return RedirectToAction("DefsFase", new { id = editDef.IdFase });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No se pueden editar deficiencias resueltas. Cree una nueva.";
                return RedirectToAction("DefsFase", new { id = editDef.IdFase });
            }
        }

        // POST: /Files/EditarDefFase
        [HttpPost]
        public ActionResult EditarDefFase(EditarDefView editDef, int[] selectedSistemas)
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
                    return RedirectToAction("DefsFase", new { id = editDef.IdFase });
                }
            }

            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(editDef);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DefArchivos(int id)
        {
            ArchivosDefManager archDefManager = new ArchivosDefManager(id);
            if (archDefManager.TieneArchDef(id))
            {
                ViewBag.idProy = archDefManager.DeficienciaActual.IdPrototipo;
                ViewBag.idFase = archDefManager.DeficienciaActual.IdFase;
                ViewBag.idDef = id;
                ViewBag.Message = " de la deficiencia";
                return View(archDefManager);
            }
            else
            {
                TempData["ErrorMessage"] = "Esta deficiencia no tiene archivos adjuntos";
                return RedirectToAction("DefsFase", new { id = archDefManager.DeficienciaActual.IdFase });
            }
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DefAdjuntar(int id)
        {
            ArchivosDefManager archDefManager = new ArchivosDefManager(id);
            var fase = dre.tFases.FirstOrDefault(f => f.IdFase == archDefManager.DeficienciaActual.IdFase);
            ViewBag.Message = " a la deficiencia";
            ViewBag.Message3 = "Seleccione el archivo a adjuntar:";
            ViewBag.idProy = archDefManager.DeficienciaActual.IdPrototipo;
            ViewBag.idFase = fase.IdFase;
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(archDefManager);
        }

        // POST: /Files/DefAdjuntar
        [HttpPost]
        public ActionResult DefAdjuntar(HttpPostedFileBase uploadFile, int id, string descrip)
        {
            ArchivosDefManager archDefManager = new ArchivosDefManager(id);
            if (descrip.Length > 50)
            {
                TempData["ErrorMessage"] = "La descripción no puede tener más de 50 caracteres";
                return RedirectToAction("DefAdjuntar", new { id = id });
            }
            else
            {
                archDefManager.AddArch(id);
                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    var ultArch = (from a in dre.tDeficienciaArchivos
                                   where a.IdDeficiencia == id
                                   orderby a.FechaAdd descending
                                   select a).First();
                    int ultArchId = ultArch.IdArch;
                    archDefManager.SaveArch(id, ultArchId, uploadFile, descrip);
                }
                return RedirectToAction("DefArchivos", new { id = id });
            }
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult VerArchivoDef(int id)
        {
            var archivo = dre.tDeficienciaArchivos.First(m => m.IdArch == id);

            MemoryStream ms = new MemoryStream(archivo.Contenido, 0, 0, true, true);
            Response.ContentType = archivo.Tipo;
            Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(archivo.Nombre));
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
            return new FileStreamResult(Response.OutputStream, archivo.Tipo);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult EliminarArchivoDef(int id)
        {
            var archivo = dre.tDeficienciaArchivos.First(m => m.IdArch == id);
            var def = dre.tDeficiencias.First(m => m.IdDeficiencia == archivo.IdDeficiencia);
            var proy = dre.tPrototipos.First(p => p.IdPrototipo == def.IdPrototipo);

            var context = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain);
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            var usuario = UserPrincipal.FindByIdentity(context, user);
            var nombreUsuario = String.Concat(usuario.GivenName, " ", usuario.Surname);

            if (archivo.UsuarioAdd == nombreUsuario || def.IdUsuarioCreador == user || proy.Responsable == user)
            {
                ViewBag.idArchivo = archivo.IdArch;
                ViewBag.IdDeficiencia = archivo.IdDeficiencia;
                ViewBag.IdProy = def.IdPrototipo;
                return View();
            }
            else
            {
                TempData["ErrorMessage"] = "Solo puede eliminar el archivo el que lo ha subido, el creador de la deficiencia o el responsable del proyecto";
                return RedirectToAction("DefArchivos", new { id = archivo.IdDeficiencia });
            }
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DeleteFileDef(int id)
        {
            var archivo = dre.tDeficienciaArchivos.First(m => m.IdArch == id);
            int IdDeficiencia = archivo.IdDeficiencia;
            ArchivosDefManager archDefManager = new ArchivosDefManager(IdDeficiencia);
            archDefManager.DelArch(id);

            return RedirectToAction("DefArchivos", new { id = IdDeficiencia });
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult PruebaAdjuntar(int id)
        {
            ArchivosPruManager archPruManager = new ArchivosPruManager(id);
            var proyecto = dre.tPrototipos.FirstOrDefault(f => f.IdPrototipo == archPruManager.PruebaActual.IdPrototipo);
            ViewBag.Message = " a la prueba del proyecto ";
            ViewBag.Message2 = proyecto.Proyecto;
            ViewBag.Message3 = "Seleccione el archivo a adjuntar:";
            ViewBag.idProy = archPruManager.PruebaActual.IdPrototipo;
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(archPruManager);
        }

        // POST: /Files/PruebaAdjuntar
        [HttpPost]
        public ActionResult PruebaAdjuntar(HttpPostedFileBase uploadFile, int id, string descrip)
        {
            ArchivosPruManager archPruManager = new ArchivosPruManager(id);
            if (descrip.Length > 50)
            {
                TempData["ErrorMessage"] = "La descripción no puede tener más de 50 caracteres";
                return RedirectToAction("PruebaAdjuntar", new { id = id });
            }
            else
            {
                archPruManager.AddArch(id);
                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    var ultArch = (from a in dre.tPruebaArchivos
                                   where a.IdPrueba == id
                                   orderby a.FechaAdd descending
                                   select a).First();
                    int ultArchId = ultArch.IdArch;
                    archPruManager.SaveArch(id, ultArchId, uploadFile, descrip);
                }
                return RedirectToAction("PruebaArchivos", new { id = id });
            }
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult PruebaArchivos(int id)
        {
            ArchivosPruManager archPruManager = new ArchivosPruManager(id);
            if (archPruManager.TieneArchPrueba(id))
            {
                ViewBag.idPrueba = id;
                ViewBag.idProy = archPruManager.PruebaActual.IdPrototipo;
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
                return View(archPruManager);
            }
            else
            {
                TempData["ErrorMessage"] = "Esta prueba no tiene archivos adjuntos";
                return RedirectToAction("GestionPruebas", "Home", new { id = archPruManager.PruebaActual.IdPrototipo });
            }
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult VerArchivoPrueba(int id)
        {
            var archivo = dre.tPruebaArchivos.First(m => m.IdArch == id);

            MemoryStream ms = new MemoryStream(archivo.Contenido, 0, 0, true, true);
            Response.ContentType = archivo.Tipo;
            Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(archivo.Nombre));
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
            return new FileStreamResult(Response.OutputStream, archivo.Tipo);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult EliminarArchivoPrueba(int id)
        {
            var archivo = dre.tPruebaArchivos.First(m => m.IdArch == id);
            var prueba = dre.tPruebas.First(m => m.IdPrueba == archivo.IdPrueba);
            var proy = dre.tPrototipos.First(p => p.IdPrototipo == prueba.IdPrototipo);

            var context = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain);
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            var usuario = UserPrincipal.FindByIdentity(context, user);
            var nombreUsuario = String.Concat(usuario.GivenName, " ", usuario.Surname);

            if (archivo.UsuarioAdd == nombreUsuario || prueba.IdUsuario == user || proy.Responsable == user)
            {
                ViewBag.idArchivo = archivo.IdArch;
                ViewBag.idPrueba = archivo.IdPrueba;
                ViewBag.IdProy = prueba.IdPrototipo;
                return View();
            }
            else
            {
                TempData["ErrorMessage"] = "Solo puede eliminar el archivo el que lo ha subido, el creador de la prueba o el responsable del proyecto";
                return RedirectToAction("PruebaArchivos", new { id = archivo.IdPrueba });
            }
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DeleteFilePrueba(int id)
        {
            var archivo = dre.tPruebaArchivos.First(m => m.IdArch == id);
            int idPrueba = archivo.IdPrueba;
            ArchivosPruManager archPruManager = new ArchivosPruManager(idPrueba);
            archPruManager.DelArch(id);

            return RedirectToAction("PruebaArchivos", new { id = idPrueba });
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DefsPrueba(int id)
        {
            ArchivosDefPruManager archDefPruManager = new ArchivosDefPruManager();
            var prueba = dre.tPruebas.FirstOrDefault(f => f.IdPrueba == id);
            ViewBag.idProy = prueba.IdPrototipo;
            ViewBag.idPrueba = id;
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.Message = " de la prueba";

            return View(archDefPruManager);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult EditarDefPrueba(int id)
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

                    ViewBag.otros = (from pru in dre.tPruebaAfectas
                                     where pru.IdDeficienciaPru == id && pru.IdSistema == 29
                                     select pru.Otros).FirstOrDefault();

                    ViewBag.ErrorMessage = TempData["ErrorMessage"];

                    return View(editDefPru);
                }
                else
                {
                    TempData["ErrorMessage"] = "Solo puede editar la deficiencia su creador o el responsable del proyecto";
                    return RedirectToAction("DefsPrueba", new { id = editDefPru.IdPrueba });
                }
            }
            else
            {
                TempData["ErrorMessage"] = "No se pueden editar deficiencias resueltas. Cree una nueva.";
                return RedirectToAction("DefsPrueba", new { id = editDefPru.IdPrueba });
            }
        }

        // POST: /Files/EditarDefPrueba
        [HttpPost]
        public ActionResult EditarDefPrueba(EditarDefPruebaView editDefPru, int[] selectedSistemas)
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

            ViewBag.otros = (from pru in dre.tPruebaAfectas
                             where pru.IdDeficienciaPru == editDefPru.IdDeficiencia && pru.IdSistema == 29
                             select pru.Otros).FirstOrDefault();
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
                    return RedirectToAction("DefsPrueba", new { id = editDefPru.IdPrueba });
                }
            }

            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View(editDefPru);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DefPruArchivos(int id)
        {
            ArchivosDefPruManager archDefPruManager = new ArchivosDefPruManager(id);
            if (archDefPruManager.TieneArchDefPru(id))
            {
                ViewBag.idDefPru = id;
                ViewBag.IdPrueba = archDefPruManager.DefPruActual.IdPrueba;
                ViewBag.idProy = archDefPruManager.DefPruActual.IdPrototipo;
                ViewBag.Message = " de la deficiencia";
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
                return View(archDefPruManager);
            }
            else
            {
                TempData["ErrorMessage"] = "Esta deficiencia no tiene archivos adjuntos";
                return RedirectToAction("DefsPrueba", new { id = archDefPruManager.DefPruActual.IdPrueba });
            }
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DefPruAdjuntar(int id)
        {
            ArchivosDefPruManager archDefPruManager = new ArchivosDefPruManager(id);
            var prueba = dre.tPruebas.FirstOrDefault(f => f.IdPrueba == archDefPruManager.DefPruActual.IdPrueba);
            ViewBag.Message = " a la deficiencia";
            ViewBag.Message3 = "Seleccione el archivo a adjuntar:";
            ViewBag.idProy = archDefPruManager.DefPruActual.IdPrototipo;
            ViewBag.idPrueba = prueba.IdPrueba;
            ViewBag.ErrorMessage = TempData["ErrorMessage"];

            return View(archDefPruManager);
        }

        // POST: /Files/DefPruAdjuntar
        [HttpPost]
        public ActionResult DefPruAdjuntar(HttpPostedFileBase uploadFile, int id, string descrip)
        {
            ArchivosDefPruManager archDefPruManager = new ArchivosDefPruManager(id);
            if (descrip.Length > 50)
            {
                TempData["ErrorMessage"] = "La descripción no puede tener más de 50 caracteres";
                return RedirectToAction("DefPruAdjuntar", new { id = id });
            }
            else
            {
                archDefPruManager.AddArch(id);
                if (uploadFile != null && uploadFile.ContentLength > 0)
                {
                    var ultArch = (from a in dre.tDeficienciaPruArchivos
                                   where a.IdDeficiencia == id
                                   orderby a.FechaAdd descending
                                   select a).First();
                    int ultArchId = ultArch.IdArch;
                    archDefPruManager.SaveArch(id, ultArchId, uploadFile, descrip);
                }
                return RedirectToAction("DefPruArchivos", new { id = id });
            }
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult VerArchivoDefPru(int id)
        {
            var archivo = dre.tDeficienciaPruArchivos.First(m => m.IdArch == id);

            MemoryStream ms = new MemoryStream(archivo.Contenido, 0, 0, true, true);
            Response.ContentType = archivo.Tipo;
            Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(archivo.Nombre));
            Response.Buffer = true;
            Response.Clear();
            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.End();
            return new FileStreamResult(Response.OutputStream, archivo.Tipo);
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult EliminarArchivoDefPru(int id)
        {
            var archivo = dre.tDeficienciaPruArchivos.First(m => m.IdArch == id);
            var defPru = dre.tDeficienciaPruebas.First(m => m.IdDeficiencia == archivo.IdDeficiencia);
            var proy = dre.tPrototipos.First(p => p.IdPrototipo == defPru.IdPrototipo);

            var context = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain);
            var user = System.Web.HttpContext.Current.User.Identity.Name;
            var usuario = UserPrincipal.FindByIdentity(context, user);
            var nombreUsuario = String.Concat(usuario.GivenName, " ", usuario.Surname);

            if (archivo.UsuarioAdd == nombreUsuario || defPru.IdUsuarioCreador == user || proy.Responsable == user)
            {
                ViewBag.idArchivo = archivo.IdArch;
                ViewBag.IdDeficiencia = archivo.IdDeficiencia;
                ViewBag.IdProy = defPru.IdPrototipo;
                return View();
            }
            else
            {
                TempData["ErrorMessage"] = "Solo puede eliminar el archivo el que lo ha subido, el creador de la deficiencia o el responsable del proyecto";
                return RedirectToAction("DefPruArchivos", new { id = archivo.IdDeficiencia });
            }
        }

        [Authorize(Roles = @"DOMINIO\GestionPrototipos")]
        public ActionResult DeleteFileDefPru(int id)
        {
            var archivo = dre.tDeficienciaPruArchivos.First(m => m.IdArch == id);
            int IdDeficiencia = archivo.IdDeficiencia;
            ArchivosDefPruManager archDefPruManager = new ArchivosDefPruManager(IdDeficiencia);
            archDefPruManager.DelArch(id);

            return RedirectToAction("DefPruArchivos", new { id = IdDeficiencia });
        }
    }
}

