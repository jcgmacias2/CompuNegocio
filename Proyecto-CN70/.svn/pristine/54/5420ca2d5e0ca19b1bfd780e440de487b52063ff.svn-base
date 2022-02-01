using Aprovi.Business.Helpers;
using Aprovi.Data.Core;
using Aprovi.Data.Models;
using Aprovi.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aprovi.Business.Services
{
    public abstract class UsuarioService : IUsuarioService
    {
        private IUnitOfWork _UOW;
        private IUsuariosRepository _users;
        private IAplicacionesRepository _app;

        public UsuarioService(IUnitOfWork unitOfWork)
        {
            _UOW = unitOfWork;
            _users = _UOW.Usuarios;
            _app = _UOW.Aplicaciones;
        }

        public Usuario Add(Usuario user)
        {
            Criptografia crypto;
            string key;

            try
            {
                //Debo cifrar la contraseña antes de agregar al usuario
                crypto = new Criptografia();
                //La llave para el cifrado se encuentra en el archivo de configuración bajo el key "Copyright"
                key = _app.ReadSetting("Copyright").ToString();

                user.contraseña = crypto.Cipher(user.contraseña, key);
                _users.Add(user);
                _UOW.Save();
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Usuario Find(int idUser)
        {
            try
            {
                return _users.Find(idUser);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Usuario Find(string name)
        {
            try
            {
                return _users.Find(name);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Usuario> List()
        {
            try
            {
                return _users.List().Where(u => u.activo).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Usuario> WithNameLike(string name)
        {
            try
            {
                return _users.WithNameLike(name).Where(u => u.activo).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Usuario Update(Usuario user)
        {

            Criptografia crypto;
            string key;

            try
            {
                var local = _users.Find(user.idUsuario);
                local.activo = user.activo;
                //Si la contraseña tiene un '=' entonces esta encriptada
                if (user.contraseña.Contains('='))
                    local.contraseña = user.contraseña;
                else
                {
                    //Debo cifrar la contraseña antes de agregar al usuario
                    crypto = new Criptografia();
                    //La llave para el cifrado se encuentra en el archivo de configuración bajo el key "Copyright"
                    key = _app.ReadSetting("Copyright").ToString();

                    local.contraseña = crypto.Cipher(user.contraseña, key);
                }

                local.nombreCompleto = user.nombreCompleto;
                local.nombreDeUsuario = user.nombreDeUsuario;
                local.descuento = user.descuento;
                local.ComisionesPorUsuarios = user.ComisionesPorUsuarios;

                _users.Update(local);
                _UOW.Save();

                return local;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(Usuario user)
        {
            try
            {
                _users.Remove(user.idUsuario);
                _UOW.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CanDelete(Usuario user)
        {
            try
            {
                return _users.CanDelete(user.idUsuario);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Usuario GetApiDefault()
        {
            try
            {
                //El sistema se empaquetara con un usuario administrativo "Aprovi", el cual tendrá privilegios totales sobre la configuración.
                return _users.Find("Aprovi");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
