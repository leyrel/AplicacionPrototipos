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
    public class ArchivosDefPruManager
    {
        private PrototiposEntities entity = new PrototiposEntities();

        public ArchivosDefPruManager() { }

        public ArchivosDefPruManager(int id)
        {
            DefPruActual = entity.tDeficienciaPruebas.First(p => p.IdDeficiencia == id);
        }

        public tDeficienciaPrueba DefPruActual { set; get; }
        public tDeficienciaArchivo ArchivoActual { set; get; }

        public IEnumerable<tDeficienciaPruArchivo> archDefPru(int id)
        {
            var query = from c in entity.tDeficienciaPruArchivos
                        where c.IdDeficiencia == id
                        select c;
            return query.ToList();
        }

        public IEnumerable<v_defPruebaUsuario> defsPru(int idPrueba)
        {
            var query = from p in entity.v_defPruebaUsuarios
                        where p.IdPrueba == idPrueba
                        select p;
            return query.ToList();
        }

        public bool TieneArchDefPru(int IdDeficienciaPru)
        {
            return (from t in entity.tDeficienciaPruArchivos where t.IdDeficiencia == IdDeficienciaPru select t).Any();
        }

        public void AddArch(int id)
        {
            tDeficienciaPruArchivo arch = new tDeficienciaPruArchivo();
            arch.IdDeficiencia = id;
            arch.FechaAdd = DateTime.Now;
            var context = new PrincipalContext(System.DirectoryServices.AccountManagement.ContextType.Domain);
            var principal = UserPrincipal.FindByIdentity(context, HttpContext.Current.User.Identity.Name);
            arch.UsuarioAdd = String.Concat(principal.GivenName, " ", principal.Surname);

            entity.tDeficienciaPruArchivos.AddObject(arch);
            entity.SaveChanges();
        }

        public void SaveArch(int id, int ultArchId, HttpPostedFileBase uploadFile, string descrip)
        {
            byte[] tempFile = new byte[uploadFile.ContentLength];
            uploadFile.InputStream.Read(tempFile, 0, uploadFile.ContentLength);

            tDeficienciaPruArchivo arch = entity.tDeficienciaPruArchivos.First(m => m.IdDeficiencia == id && m.IdArch == ultArchId);
            arch.Nombre = new FileInfo(uploadFile.FileName).Name;
            arch.Tipo = uploadFile.ContentType;
            decimal len = uploadFile.ContentLength;
            arch.Lenght = len;
            arch.Contenido = tempFile;
            arch.Descripcion = descrip;
            entity.tDeficienciaPruArchivos.ApplyCurrentValues(arch);

            entity.SaveChanges();
        }

        public void DelArch(int id)
        {
            var archivo = (from ar in entity.tDeficienciaPruArchivos
                           where ar.IdDeficiencia == DefPruActual.IdDeficiencia && ar.IdArch == id
                           select ar).First();
            entity.tDeficienciaPruArchivos.DeleteObject(archivo);
            entity.SaveChanges();
        }
    }
}