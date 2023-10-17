﻿using api.Usuarios;
using api.Senhas;

namespace app.Services.Interfaces
{
    public interface IUsuarioService
    {
        public bool ValidaLogin(UsuarioDTO usuarioDTO);
        public Task TrocaSenha(RedefinicaoSenhaDTO redefinirSenhaDto);
        public Task RecuperarSenha(UsuarioDTO usuarioDto);
        public Task CadastrarUsuarioDnit(UsuarioDTO usuarioDTO);
        public void CadastrarUsuarioTerceiro(UsuarioDTO usuarioDTO);
    }
}