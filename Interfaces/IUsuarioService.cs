using System.Collections.Generic;

namespace API_ISDb.Interfaces
{
    public interface IUsuarioService
    {
        Usuario CheckUser(Usuario _users);
        bool DeleteUsuario(int id);
        ICollection<Usuario> GetAll();
        Usuario GetUsuario(int id);
        string login(Usuario _users);
        Usuario PostUsuario(Usuario usuario);
        bool PutUsuario(Usuario usuario);
        string register(Usuario _users);
    }
}