using api;
using api.Empresa;
using api.Usuarios;
using app.Entidades;


namespace app.Services.Interfaces
{
    public interface IEmpresaService
    {
        Task CadastrarEmpresa(Empresa empresa);
        Empresa? VisualizarEmpresa(string empresaid);
        Task DeletarEmpresa(string empresaid);
        
        Task<Empresa?> EditarEmpresa(string empresaid, Empresa empresa);
        Task<ListaPaginada<EmpresaModel>> ListarEmpresas(int pageIndex, int pageSize, string? nome = null, string? cnpj = null);
        Task<ListaPaginada<UsuarioModel>> ListarUsuarios(string cnpj, int pageIndex, int pageSize, string? nome = null);
        Task AdicionarUsuario(int usuarioid, string empresaid);
        Task RemoverUsuario(int usuarioid, string empresaid);
    }
}