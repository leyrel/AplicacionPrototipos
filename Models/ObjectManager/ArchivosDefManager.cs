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
    public class ArchivosDefManager
    {
        private PrototiposEntities entity = new PrototiposEntities();

        public ArchivosDefManager() { }

        public ArchivosDefManager(int id)
        {
            DeficienciaActual = entity.tDeficiencias.First(p => p.IdDeficiencia == id);
        }

        public tDeficiencia DeficienciaActual { set; get; }
        public tDeficienciaArchivo ArchivoActual { set; get; }

        public IEnumerable<tDeficienciaArchivo> archDef(int id)
        {
            var query = from c in entity.tDeficienciaArchivos
                        where c.IdDeficiencia == id
                        select c;
            return query.ToList();
        }

        public IEnumerable<v_defUsuario> defsFase(int idFase)
        {
            var query = from v in entity.v_defUsuarios
                        where v.IdFase == idFase
                        select v;
            return query.ToList();
        }

        public bool TieneArchDef(int IdDeficiencia)
        {
            return (from t in entity.tDeficienciaArchivos where t.IdDeficiencia == IdDeficiencia select t).Any();
        }

        public void AddArch(int id)
        {
            tDeficienciaArchivo arch = new tDeficienciaArchivo();
            arch.IdDeficiencia = id;
            arch.FechaAdd = DateTime.Now;
            var context = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain);
            var principal = UserPrincipal.FindByIdentity(context, HttpContext.Current.User.Identity.Name);
            arch.UsuarioAdd = String.Concat(principal.GivenName, " ", principal.Surname);

            entity.tDeficienciaArchivos.AddObject(arch);
            entity.SaveChanges();
        }

        public void SaveArch(int id, int ultArchId, HttpPostedFileBase uploadFile, string descrip)
        {
            byte[] tempFile = new byte[uploadFile.ContentLength];
            uploadFile.InputStream.Read(tempFile, 0, uploadFile.ContentLength);

            tDeficienciaArchivo arch = entity.tDeficienciaArchivos.First(m => m.IdDeficiencia == id && m.IdArch == ultArchId);
            arch.Nombre = new FileInfo(uploadFile.FileName).Name;
            arch.Tipo = uploadFile.ContentType;
            decimal len = uploadFile.ContentLength;
            arch.Lenght = len;
            arch.Contenido = tempFile;
            arch.Descripcion = descrip;
            entity.tDeficienciaArchivos.ApplyCurrentValues(arch);

            entity.SaveChanges();
        }

        public void DelArch(int id)
        {
            var archivo = (from ar in entity.tDeficienciaArchivos
                           where ar.IdDeficiencia == DeficienciaActual.IdDeficiencia && ar.IdArch == id
                           select ar).First();
            entity.tDeficienciaArchivos.DeleteObject(archivo);
            entity.SaveChanges();
        }
    }
}