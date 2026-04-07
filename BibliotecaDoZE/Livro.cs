namespace BibliotecaDoZE
{
    using System;

    public class Livro
    {
        private string _isbn;
        private string _titulo;
        private string _subtitulo;
        private string _escritor;
        private string _editora;
        private string _genero;
        private int _anoPublicacao;
        private string _tipoDaCapa;
        private int _numeroDePaginas;

        public string Isbn
        {
            get => _isbn;
            init
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("ISBN não pode ser nulo ou vazio.");
                _isbn = value.Trim();
            }
        }

        public string Titulo
        {
            get => _titulo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Título não pode ser nulo ou vazio.");
                _titulo = value.Trim();
            }
        }

        public string Subtitulo
        {
            get => _subtitulo;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Subtítulo não pode ser nulo ou vazio.");
                _subtitulo = value.Trim();
            }
        }

        public string Escritor
        {
            get => _escritor;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Escritor não pode ser nulo ou vazio.");
                _escritor = value.Trim();
            }
        }

        public string Editora
        {
            get => _editora;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Editora não pode ser nula ou vazia.");
                _editora = value.Trim();
            }
        }

        public string Genero
        {
            get => _genero;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Gênero não pode ser nulo ou vazio.");
                _genero = value.Trim();
            }
        }

        public int AnoPublicacao
        {
            get => _anoPublicacao;
            set
            {
                int anoAtual = DateTime.Now.Year;
                if (value < 1970 || value > anoAtual)
                    throw new ArgumentException($"Ano de publicação deve ser entre 1970 e {anoAtual}.");
                _anoPublicacao = value;
            }
        }

        public string TipoDaCapa
        {
            get => _tipoDaCapa;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tipo da Capa não pode ser nulo ou vazio.");
                _tipoDaCapa = value.Trim();
            }
        }

        public int NumeroDePaginas
        {
            get => _numeroDePaginas;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Número de páginas não pode ser negativo.");
                _numeroDePaginas = value;
            }
        }

        public Livro(string isbn, string titulo, string subtitulo, string escritor, string editora, string genero, int anoPublicacao, string tipoDaCapa, int numeroDePaginas)
        {
            Isbn = isbn;
            Titulo = titulo;
            Subtitulo = subtitulo;
            Escritor = escritor;
            Editora = editora;
            Genero = genero;
            AnoPublicacao = anoPublicacao;
            TipoDaCapa = tipoDaCapa;
            NumeroDePaginas = numeroDePaginas;
        }

        public override string ToString()
        {
            return $"{Titulo} por {Escritor} ({AnoPublicacao}) - Gênero: {Genero} [ISBN: {Isbn}]";
        }
    }
}