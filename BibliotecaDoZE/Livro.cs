namespace BibliotecaDoZE
{
    public class Livro
    {
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int AnoPublicacao { get; set; }
        public string Genero { get; set; }

        public Livro(string titulo, string autor, int anoPublicacao, string genero)
        {
            Titulo = titulo;
            Autor = autor;
            AnoPublicacao = anoPublicacao;
            Genero = genero;
        }

        public override string ToString()
        {
            return $"{Titulo} por {Autor} ({AnoPublicacao}) - Gênero: {Genero}";
        }
    }
}