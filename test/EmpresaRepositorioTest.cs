using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Abstractions;
using test.Fixtures;
using app.Repositorios.Interfaces;
using app.Entidades;
using Xunit.Sdk;
using api.Perfis;
using System.Linq;
using System.Collections.Generic;
using api;
using api.Usuarios;
using System.Data.Common;

namespace test
{
    public class EmpresaRepositorioTest : TestBed<Base>, IDisposable
    {
        private readonly IEmpresaRepositorio repositorio;

        private readonly IUsuarioRepositorio repositorio_usuario;
        private readonly AppDbContext dbContext;
      
        public EmpresaRepositorioTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {   

            this.repositorio = fixture.GetService<IEmpresaRepositorio>
            (testOutputHelper)!;

            this.repositorio_usuario = fixture.GetService<IUsuarioRepositorio>
            (testOutputHelper)!;
            
            this.dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;
        }

        [Fact]
        public void
        DeletarEmpresa_QuandoEmpresaPassado_DeveRemoverDoBanco()
        {
         
            Empresa empresa = Stub.EmpresaStub.RetornarEmpresa();

            repositorio.DeletarEmpresa(empresa);

            dbContext.SaveChanges();

            var empresaDb = dbContext.Empresa.Find(empresa.RazaoSocial);

            Assert.Null(empresaDb);

        }

        [Fact]
        public void
        AdicionarEmpresa_QuandoEmpresaPassado_DeveRetornarEmpresa()
        {
         
            Empresa empresa = Stub.EmpresaStub.RetornarEmpresa();

            var empresaCadastrado = empresa;

            repositorio.CadastrarEmpresa(empresa);

            dbContext.SaveChanges();
            var empresaDb = dbContext.Empresa.FirstOrDefault(e => e.Cnpj == empresa.Cnpj);

            Assert.NotNull(empresaDb);
            Assert.Equal(empresaCadastrado.RazaoSocial,empresa.RazaoSocial );

        }

        [Fact]
        public async void ObterPerfilPorIdAsync_QuandoPerfilExiste_DeveRetornarPerfil()
        {
            var empresa = Stub.EmpresaStub.RetornarEmpresa();

            await repositorio.CadastrarEmpresa(empresa);

            dbContext.SaveChanges();

            var EmpresaRecuperado = await repositorio.ObterEmpresaPorIdAsync(empresa.Cnpj);

            Assert.NotNull(EmpresaRecuperado);
            Assert.Equal(EmpresaRecuperado.RazaoSocial, empresa.RazaoSocial);
        }

        [Fact]
        public void VisualizarEmpresa_QuandoColocadoCNPJ()
        {
            var empresa = Stub.EmpresaStub.RetornarEmpresa();

            var empresaVisualiza = repositorio.VisualizarEmpresa(empresa.Cnpj);

            Assert.Equal(empresaVisualiza.Cnpj,empresa.Cnpj);
        }

        [Fact]
        public async void ListarEmpresas_QuandoColocadoTamanho()
        {
            var lista = Stub.EmpresaStub.RetornaListaDeEmpresas();
            List<string> nomeLista = new();

            lista.ForEach(p => nomeLista.Add(p.RazaoSocial));

            lista.ForEach(p => repositorio.CadastrarEmpresa(p));

            dbContext.SaveChanges();

            var listaRetornada = await repositorio.ListarEmpresas(1,3);
            Assert.NotNull(listaRetornada);

            foreach(var item in lista)
            {
                Assert.Contains(item.RazaoSocial, nomeLista);
            }
        }

        // [Fact]
        // public async void AdicionarUsuario_QuandoPassado_DeveRetornarUsuario()
        // {
         
            

        //     Empresa empresa = Stub.EmpresaStub.RetornarEmpresa();

        //     Usuario usuario= Stub.EmpresaStub.RetornarUsuarioDnitBanco();
            
        //     var usuarioid = usuario.Id;
        //     dbContext.Add(usuario);

        //     dbContext.SaveChanges();
            
        //     await repositorio.AdicionarUsuario(usuario.Id, empresa.Cnpj);

        //     dbContext.SaveChanges();
        //     var empresaDb = dbContext.Empresa.FirstOrDefault(e => e.Usuarios == empresa.Usuarios);

        //     Assert.NotNull(empresaDb);
        //     // Assert.Equal(empresaCadastrado.RazaoSocial,empresa.RazaoSocial );

        // }

        // [Fact]
        // public async void ListarUsuarios_QuandoColocadoCnpj()
        // {
        //     var lista = Stub.EmpresaStub.RetornaListaDeEmpresas();
        //     List<string> nomeLista = new();

        //     lista.ForEach(p => nomeLista.Add(p.RazaoSocial));

        //     lista.ForEach(p => repositorio.CadastrarEmpresa(p));

        //     dbContext.SaveChanges();

        //     var listaRetornada = await repositorio.ListarEmpresas(1,3);
        //     Assert.NotNull(listaRetornada);

        //     foreach(var item in lista)
        //     {
        //         Assert.Contains(item.RazaoSocial, nomeLista);
        //     }
        // }

    }
}