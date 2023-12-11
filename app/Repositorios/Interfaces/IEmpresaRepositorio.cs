using api;
using api.Usuarios;
using app.Entidades;
using System.Linq.Expressions;

namespace app.Repositorios.Interfaces
{
    public interface IEmpresaRepositorio
    {
        void CadastrarEmpresa(Empresa empresa);
        Empresa? VisualizarEmpresa(string empresaid);
        Task DeletarEmpresa(Empresa empresa);

        Task<List<Empresa>> ListarEmpresasSemPaginacao(Expression<Func<Empresa, bool>>? filter = null);
        public Task<Empresa?> ObterEmpresaPorCnpjAsync(string empresaid);
        Task<ListaPaginada<Empresa>> ListarEmpresas(int pageIndex, int pageSize, List<UF> ufs, string? nome = null, string? cnpj = null);
        Task<ListaPaginada<Usuario>> ListarUsuarios(string cnpj, PesquisaUsuarioFiltro filtro);
        void AdicionarUsuario(int usuarioid, string empresaid);
        void RemoverUsuario(int usuarioid, string empresaid);
    }

}