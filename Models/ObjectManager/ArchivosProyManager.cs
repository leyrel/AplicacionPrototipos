using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Prototipos.Models.DB;
using Prototipos.Models.ViewModels;
using Prototipos.Models;

namespace Prototipos.Models.ObjectManager
{
    public class ArchivosProyManager
    {
        private PrototiposEntities entity = new PrototiposEntities();

        public ArchivosProyManager() { }

        public ArchivosProyManager(int id)
        {
            ProyectoActual = entity.tPrototipos.First(p => p.IdPrototipo == id);
        }

        public tPrototipo ProyectoActual { set; get; }
        public tPrototipoArchivo ArchivoActual { set; get; }

        public IEnumerable<tPrototipoArchivo> archProy(int id)
        {
            var query = from c in entity.tPrototipoArchivos
                        where c.IdPrototipo == id
                        select c;
            return query.ToList();
        }

        public bool TieneArchProy(int IdPrototipo)
        {
            return (from t in entity.tPrototipoArchivos where t.IdPrototipo == IdPrototipo select t).Any();
        }

        public void AddArch(int id)
        {
            tPrototipoArchivo arch = new tPrototipoArchivo();
            arch.IdPrototipo = id;
            arch.FechaAdd = DateTime.Now;
            var context = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain);
            var principal = UserPrincipal.FindByIdentity(context, HttpContext.Current.User.Identity.Name);
            arch.UsuarioAdd = String.Concat(principal.GivenName, " ", principal.Surname);

            entity.tPrototipoArchivos.AddObject(arch);
            entity.SaveChanges();
        }

        public void SaveArch(int id, int ultArchId, HttpPostedFileBase uploadFile, string descrip)
        {
            byte[] tempFile = new byte[uploadFile.ContentLength];
            uploadFile.InputStream.Read(tempFile, 0, uploadFile.ContentLength);

            tPrototipoArchivo arch = entity.tPrototipoArchivos.First(m => m.IdPrototipo == id && m.IdArch == ultArchId);
            arch.Nombre = new FileInfo(uploadFile.FileName).Name;
            arch.Tipo = uploadFile.ContentType;
            decimal len = uploadFile.ContentLength;
            arch.Lenght = len;
            arch.Contenido = tempFile;
            arch.Descripcion = descrip;
            entity.tPrototipoArchivos.ApplyCurrentValues(arch);

            entity.SaveChanges();
        }

        public void DelArch(int id)
        {
            var archivo = (from ar in entity.tPrototipoArchivos
                           where ar.IdPrototipo == ProyectoActual.IdPrototipo && ar.IdArch == id
                           select ar).First();
            entity.tPrototipoArchivos.DeleteObject(archivo);
            entity.SaveChanges();
        }
    }
}