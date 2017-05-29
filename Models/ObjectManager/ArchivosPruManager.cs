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
    public class ArchivosPruManager
    {
        private PrototiposEntities entity = new PrototiposEntities();

        public ArchivosPruManager() { }

        public ArchivosPruManager(int id)
        {
            PruebaActual = entity.tPruebas.First(f => f.IdPrueba == id);
        }

        public tPrueba PruebaActual { set; get; }
        public tPruebaArchivo ArchivoActual { set; get; }

        public IEnumerable<tPruebaArchivo> archPrueba(int id)
        {
            var query = from c in entity.tPruebaArchivos
                        where c.IdPrueba == id
                        select c;
            return query.ToList();
        }

        public bool TieneArchPrueba(int idPrueba)
        {
            return (from t in entity.tPruebaArchivos where t.IdPrueba == idPrueba select t).Any();
        }

        public void AddArch(int id)
        {
            tPruebaArchivo arch = new tPruebaArchivo();
            arch.IdPrueba = id;
            arch.FechaAdd = DateTime.Now;
            var context = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain);
            var principal = UserPrincipal.FindByIdentity(context, HttpContext.Current.User.Identity.Name);
            arch.UsuarioAdd = String.Concat(principal.GivenName, " ", principal.Surname);

            entity.tPruebaArchivos.AddObject(arch);
            entity.SaveChanges();
        }

        public void SaveArch(int id, int ultArchId, HttpPostedFileBase uploadFile, string descrip)
        {
            byte[] tempFile = new byte[uploadFile.ContentLength];
            uploadFile.InputStream.Read(tempFile, 0, uploadFile.ContentLength);

            tPruebaArchivo arch = entity.tPruebaArchivos.First(m => m.IdPrueba == id && m.IdArch == ultArchId);
            arch.Nombre = new FileInfo(uploadFile.FileName).Name;
            arch.Tipo = uploadFile.ContentType;
            decimal len = uploadFile.ContentLength;
            arch.Lenght = len;
            arch.Contenido = tempFile;
            arch.Descripcion = descrip;
            entity.tPruebaArchivos.ApplyCurrentValues(arch);

            entity.SaveChanges();
        }

        public void DelArch(int id)
        {
            var archivo = (from ar in entity.tPruebaArchivos
                           where ar.IdPrueba == PruebaActual.IdPrueba && ar.IdArch == id
                           select ar).First();
            entity.tPruebaArchivos.DeleteObject(archivo);
            entity.SaveChanges();
        }
    }
}