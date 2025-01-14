using test.Fixtures;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Xunit.Abstractions;
using app.Services.Interfaces;
using api;
using System.Linq;
using System.Collections.Generic;
using app.Services;

namespace test
{
    public class PermissaoServiceTest : TestBed<Base>, IDisposable
    {
        private readonly IPermissaoService permissaoService;

        public PermissaoServiceTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            permissaoService = fixture.GetService<IPermissaoService>(testOutputHelper)!;
        }

        [Fact]
        public void ObterCategorias_DeveRetornarListaComCategoriasDePermissoes()
        {
            var categorias = permissaoService.ObterCategorias();
            Assert.NotEmpty(categorias);
        }
    }

}