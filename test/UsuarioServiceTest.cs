using AutoMapper;
using api.Senhas;
using api.Usuarios;
using Microsoft.Extensions.Configuration;
using Moq;
using app.Repositorios.Interfaces;
using app.Services;
using app.Services.Interfaces;
using System.Collections.Generic;
using test.Stub;
using test.Fixtures;
using app.Entidades;
using Xunit.Abstractions;
using Xunit.Microsoft.DependencyInjection.Abstracts;
using Microsoft.Extensions.Options;
using auth;
using System.Threading.Tasks;
using app.Configuracoes;

namespace test
{
    public class UsuarioServiceTest : TestBed<Base>, IDisposable
    {
        readonly AppDbContext dbContext;
        readonly Mock<IMapper> mapper;
        readonly Mock<IUsuarioRepositorio> usuarioRepositorio;
        readonly Mock<IPerfilRepositorio> perfilRepositorio;
        readonly Mock<IEmailService> emailService;
        readonly AuthService authService;
        readonly Mock<IOptions<AuthConfig>> authConfig;
        readonly IUsuarioService usuarioServiceMock;

        public UsuarioServiceTest(ITestOutputHelper testOutputHelper, Base fixture) : base(testOutputHelper, fixture)
        {
            dbContext = fixture.GetService<AppDbContext>(testOutputHelper)!;

            mapper = new Mock<IMapper>();
            usuarioRepositorio = new Mock<IUsuarioRepositorio>();
            perfilRepositorio = new Mock<IPerfilRepositorio>();
            emailService = new Mock<IEmailService>();
            var senhaConfig = new SenhaConfig();
            authConfig = new Mock<IOptions<AuthConfig>>();
            authService = new AuthService(authConfig.Object);

            usuarioServiceMock = new UsuarioService(
                usuarioRepositorio.Object, perfilRepositorio.Object,
                mapper.Object, emailService.Object,
                Options.Create(senhaConfig), dbContext, authService, authConfig.Object);
        }

        [Fact]
        public async Task CadastrarUsuarioDnit_QuandoUsuarioDnitForPassado_DeveCadastrarUsuarioDnitComSenhaEncriptografada()
        {
            UsuarioStub usuarioStub = new();
            var usuarioDTO = usuarioStub.RetornarUsuarioDnitDTO();

            var clonedUsuarioDTO = new UsuarioDTO
            {
                Nome = usuarioDTO.Nome,
                Senha = usuarioDTO.Senha,
                Email = usuarioDTO.Email,
                UfLotacao = usuarioDTO.UfLotacao,
            };

            mapper.Setup(x => x.Map<UsuarioDTO>(It.IsAny<UsuarioDTO>())).Returns((UsuarioDTO _) => clonedUsuarioDTO);

            await usuarioServiceMock.CadastrarUsuarioDnit(usuarioDTO);
            usuarioRepositorio.Verify(x => x.CadastrarUsuarioDnit(It.IsAny<UsuarioDTO>()), Times.Once);
            Assert.NotEqual(usuarioDTO.Senha, clonedUsuarioDTO.Senha);
        }

        [Fact]
        public void CadastrarUsuarioTerceiro_QuandoUsuarioTerceiroForPassado_DeveCadastrarUsuarioTerceiroComSenhaEncriptografada()
        {
            UsuarioStub usuarioStub = new();
            var usuarioTerceiro = usuarioStub.RetornarUsuarioTerceiro();

            var senhaAntesDaEncriptografia = usuarioTerceiro.Senha;

            mapper.Setup(x => x.Map<UsuarioTerceiro>(It.IsAny<UsuarioDTO>())).Returns(usuarioTerceiro);

            usuarioServiceMock.CadastrarUsuarioTerceiro(usuarioStub.RetornarUsuarioTerceiroDTO());

            usuarioRepositorio.Verify(x => x.CadastrarUsuarioTerceiro(It.IsAny<UsuarioTerceiro>()), Times.Once);

            Assert.NotEqual(senhaAntesDaEncriptografia, usuarioTerceiro.Senha);
        }

        [Fact]
        public async Task CadastrarUsuarioDnit_QuandoUsuarioDnitJaExistenteForPassado_DeveLancarExececaoFalandoQueEmailJaExiste()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            mapper.Setup(x => x.Map<UsuarioDTO>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);
            usuarioRepositorio.Setup(x => x.CadastrarUsuarioDnit(It.IsAny<UsuarioDTO>())).Throws(new InvalidOperationException("Email já cadastrado."));

            var cadastrarUsuario = async () => await usuarioServiceMock.CadastrarUsuarioDnit(usuarioStub.RetornarUsuarioDnitDTO());

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(cadastrarUsuario);
        }

        [Fact]
        public void CadastrarUsuarioTerceiro_QuandoUsuarioTerceiroJaExistenteForPassado_DeveLancarExececaoFalandoQueEmalJaExiste()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioTerceiro = usuarioStub.RetornarUsuarioTerceiro();

            mapper.Setup(x => x.Map<UsuarioTerceiro>(It.IsAny<UsuarioDTO>())).Returns(usuarioTerceiro);
            usuarioRepositorio.Setup(x => x.CadastrarUsuarioTerceiro(It.IsAny<UsuarioTerceiro>())).Throws(new InvalidOperationException("Email já cadastrado."));

            var cadastrarUsuario = () => usuarioServiceMock.CadastrarUsuarioTerceiro(usuarioStub.RetornarUsuarioTerceiroDTO());

            var exception = Assert.Throws<InvalidOperationException>(cadastrarUsuario);

            Assert.Equal("Email já cadastrado.", exception.Message);
        }

        [Fact]
        public async Task RecuperarSenha_QuandoUsuarioExistir_DeveEnviarEmailDeRecuperacaoDeSenha()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDnitDTO = usuarioStub.RetornarUsuarioDnitDTO();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            var usuarioRetorno = usuarioStub.RetornarUsuarioDnitBanco();

            mapper.Setup(x => x.Map<UsuarioDTO>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);

            usuarioRepositorio.Setup(x => x.InserirDadosRecuperacao(It.IsAny<string>(), It.IsAny<int>()));
            usuarioRepositorio.Setup(x => x.ObterUsuarioPorEmail(It.IsAny<string>())).Returns(usuarioRetorno);

            await usuarioServiceMock.RecuperarSenha(usuarioDnitDTO);

            emailService.Verify(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task RecuperarSenha_QuandoUsuarioNaoExistir_DeveLancarException()
        {
            var usuarioStub = new UsuarioStub();
            var usuarioDnitDTO = usuarioStub.RetornarUsuarioDnitDTO();
            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();

            mapper.Setup(x => x.Map<UsuarioDTO>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);

            usuarioRepositorio.Setup(x => x.InserirDadosRecuperacao(It.IsAny<string>(), It.IsAny<int>()));
            usuarioRepositorio.Setup(x => x.ObterUsuarioPorEmail(It.IsAny<string>())).Returns(value: null);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await usuarioServiceMock.RecuperarSenha(usuarioDnitDTO));
        }

        [Fact]
        public void TrocaSenha_QuandoUuidForValido_DeveTrocarSenha()
        {
            var usuarioStub = new UsuarioStub();
            var redefinicaoSenhaStub = new RedefinicaoSenhaStub();
            var emailRedefinicaoSenha = "usuarioTeste@gmail.com";

            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();
            var redefinicaoSenha = redefinicaoSenhaStub.ObterRedefinicaoSenha();

            mapper.Setup(x => x.Map<UsuarioDTO>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);
            mapper.Setup(x => x.Map<RedefinicaoSenhaModel>(It.IsAny<RedefinicaoSenhaDTO>())).Returns(redefinicaoSenha);

            usuarioRepositorio.Setup(x => x.InserirDadosRecuperacao(It.IsAny<string>(), It.IsAny<int>()));
            usuarioRepositorio.Setup(x => x.ObterUsuarioPorEmail(It.IsAny<string>())).Returns(value: null);

            usuarioRepositorio.Setup(x => x.ObterEmailRedefinicaoSenha(It.IsAny<string>())).Returns(emailRedefinicaoSenha);

            usuarioServiceMock.TrocaSenha(redefinicaoSenhaStub.ObterRedefinicaoSenhaDTO());

            emailService.Verify(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            usuarioRepositorio.Verify(x => x.RemoverUuidRedefinicaoSenha(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task TrocaSenha_QuandoUuidNaoForValido_DeveLancarException()
        {
            var usuarioStub = new UsuarioStub();
            var redefinicaoSenhaStub = new RedefinicaoSenhaStub();

            var usuarioDNIT = usuarioStub.RetornarUsuarioDnit();
            var redefinicaoSenha = redefinicaoSenhaStub.ObterRedefinicaoSenha();

            mapper.Setup(x => x.Map<UsuarioDTO>(It.IsAny<UsuarioDTO>())).Returns(usuarioDNIT);
            mapper.Setup(x => x.Map<RedefinicaoSenhaModel>(It.IsAny<RedefinicaoSenhaDTO>())).Returns(redefinicaoSenha);

            usuarioRepositorio.Setup(x => x.InserirDadosRecuperacao(It.IsAny<string>(), It.IsAny<int>()));
            usuarioRepositorio.Setup(x => x.ObterUsuarioPorEmail(It.IsAny<string>())).Returns(value: null);

            usuarioRepositorio.Setup(x => x.ObterEmailRedefinicaoSenha(It.IsAny<string>())).Returns(value: null);

            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await usuarioServiceMock.TrocaSenha(redefinicaoSenhaStub.ObterRedefinicaoSenhaDTO()));

            emailService.Verify(x => x.EnviarEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            usuarioRepositorio.Verify(x => x.RemoverUuidRedefinicaoSenha(It.IsAny<string>()), Times.Never);
        }

        public new void Dispose()
        {
            dbContext.RemoveRange(dbContext.Usuario);
            dbContext.RemoveRange(dbContext.PerfilPermissoes);
            dbContext.RemoveRange(dbContext.Perfis);
            dbContext.SaveChanges();
        }
    }
}
