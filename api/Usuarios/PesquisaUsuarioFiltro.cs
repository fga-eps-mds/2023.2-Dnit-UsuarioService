namespace api.Usuarios
{
    public class PesquisaUsuarioFiltro
    {
        public int Pagina { get; set; } = 1;
        public int ItemsPorPagina { get; set; } = 50;
        public string? Nome { get; set; }
        public UF? UfLotacao { get; set; }
        
        // string? perfilNome { get; set; }
        // id? perfilId { get; set; }
    }
}