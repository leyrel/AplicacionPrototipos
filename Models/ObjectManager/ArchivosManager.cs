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
    public class ArchivosManager
    {
        private PrototiposEntities entity = new PrototiposEntities();

        public ArchivosManager() { }

        public ArchivosManager(int id)
        {
            FaseActual = entity.tFases.First(f => f.IdFase == id);
        }

        public tFase FaseActual { set; get; }
        public tFaseArchivo ArchivoActual { set; get; }

        public IEnumerable<tFaseArchivo> archFase(int id)
        {
            var query = from c in entity.tFaseArchivos
                        where c.IdFase == id
                        select c;
            return query.ToList();
        }

        public bool TieneArchFase(int idFase)
        {
            return (from t in entity.tFaseArchivos where t.IdFase == idFase select t).Any();
        }

        public void AddArch(int id)
        {
            tFaseArchivo arch = new tFaseArchivo();
            arch.IdFase = id;
            arch.FechaAdd = DateTime.Now;
            var context = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain);
            var principal = UserPrincipal.FindByIdentity(context, HttpContext.Current.User.Identity.Name);
            arch.UsuarioAdd = String.Concat(principal.GivenName, " ", principal.Surname);

            entity.tFaseArchivos.AddObject(arch);
            entity.SaveChanges();
        }

        public void SaveArch(int id, int ultArchId, HttpPostedFileBase uploadFile, string descrip)
        {
            byte[] tempFile = new byte[uploadFile.ContentLength];
            uploadFile.InputStream.Read(tempFile, 0, uploadFile.ContentLength);

            tFaseArchivo arch = entity.tFaseArchivos.First(m => m.IdFase == id && m.IdArch == ultArchId);
            arch.Nombre = new FileInfo(uploadFile.FileName).Name;
            arch.Tipo = uploadFile.ContentType;
            decimal len = uploadFile.ContentLength;
            arch.Lenght = len;
            arch.Contenido = tempFile;
            arch.Descripcion = descrip;
            entity.tFaseArchivos.ApplyCurrentValues(arch);

            entity.SaveChanges();
        }

        public void DelArch(int id)
        {
            var archivo = (from ar in entity.tFaseArchivos
                           where ar.IdFase == FaseActual.IdFase && ar.IdArch == id
                           select ar).First();
            entity.tFaseArchivos.DeleteObject(archivo);
            entity.SaveChanges();
        }
    }
}