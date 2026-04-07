namespace BibliotecaDoZE;

using System;
using System.Collections.Generic;
using System.Linq;

public class Leitor
{
    private static int _nextId = 1;
    private static readonly HashSet<string> _cpfsCadastrados = new(StringComparer.OrdinalIgnoreCase);

    private string _cpf;
    private string _nome;
    private int _idade;

    public int Id { get; }

    public string Cpf
    {
        get => _cpf;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("CPF não pode ser nulo ou vazio.");

            var cpfTrim = value.Trim();
            if (_cpf != null && _cpf.Equals(cpfTrim, StringComparison.OrdinalIgnoreCase)) return;

            if (_cpfsCadastrados.Contains(cpfTrim))
                throw new InvalidOperationException("CPF já cadastrado.");

            if (_cpf != null) _cpfsCadastrados.Remove(_cpf);

            _cpf = cpfTrim;
            _cpfsCadastrados.Add(_cpf);
        }
    }

    public string Nome
    {
        get => _nome;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Nome não pode ser nulo ou vazio.");
            _nome = value.Trim();
        }
    }

    public int Idade
    {
        get => _idade;
        set
        {
            if (value < 0)
                throw new ArgumentException("Idade não pode ser negativa.");
            _idade = value;
        }
    }

    public List<Livro> Livros { get; } = new();

    public Leitor(string cpf, string nome, int idade)
    {
        Id = _nextId++;
        Cpf = cpf;
        Nome = nome;
        Idade = idade;
    }

    public void LiberarCpf()
    {
        if (_cpf != null)
        {
            _cpfsCadastrados.Remove(_cpf);
        }
    }

    public override string ToString() => $"[{Id}] {Nome} (CPF: {Cpf}, Idade: {Idade}) - Livros: {Livros.Count}";
}

// O programa principal
internal class Program
{
    private static readonly List<Leitor> _leitores = new();

    private static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Biblioteca Do ZE ===");
            Console.WriteLine("1) Cadastrar leitor");
            Console.WriteLine("2) Listar todos os leitores e seus livros");
            Console.WriteLine("3) Editar leitor");
            Console.WriteLine("4) Excluir leitor");
            Console.WriteLine("5) Incluir livro para um leitor");
            Console.WriteLine("6) Editar um livro de um leitor");
            Console.WriteLine("7) Remover um livro de um leitor");
            Console.WriteLine("8) Doar um livro para outro leitor");
            Console.WriteLine("9) Listar um leitor específico e seus livros");
            Console.WriteLine("10) Pesquisar por um livro e mostrar o leitor");
            Console.WriteLine("0) Sair");
            Console.Write("Opção: ");

            var opcao = Console.ReadLine();
            Console.WriteLine();

            switch (opcao)
            {
                case "1": CadastrarLeitor(); break;
                case "2": ListarLeitores(); break;
                case "3": EditarLeitor(); break;
                case "4": ExcluirLeitor(); break;
                case "5": IncluirLivro(); break;
                case "6": EditarLivro(); break;
                case "7": RemoverLivro(); break;
                case "8": DoarLivro(); break;
                case "9": ListarLeitorEspecifico(); break;
                case "10": PesquisarLivro(); break;
                case "0": return;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }
    }

    private static void CadastrarLeitor()
    {
        try
        {
            var cpf = LerTexto("CPF do leitor");
            var nome = LerTexto("Nome do leitor");
            var idadeTexto = LerTexto("Idade do leitor");
            
            if (!int.TryParse(idadeTexto, out int idade))
                throw new ArgumentException("A idade informada não é um número válido.");

            var leitor = new Leitor(cpf, nome, idade);
            _leitores.Add(leitor);
            Console.WriteLine($"Leitor cadastrado: {leitor}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao cadastrar leitor: {ex.Message}");
        }
    }

    private static void ListarLeitores()
    {
        if (!_leitores.Any())
        {
            Console.WriteLine("Não há leitores cadastrados.");
            return;
        }

        foreach (var leitor in _leitores)
        {
            Console.WriteLine(leitor);
            ExibirLivros(leitor);
        }
    }

    private static void EditarLeitor()
    {
        var leitor = SelecionarLeitor("Digite o CPF do leitor que deseja editar");
        if (leitor == null) return;

        try
        {
            var novoNome = LerTexto($"Novo nome para '{leitor.Nome}' (pressione Enter para manter)");
            if (!string.IsNullOrWhiteSpace(novoNome))
            {
                leitor.Nome = novoNome;
            }

            var novaIdadeTexto = LerTexto($"Nova idade para '{leitor.Nome}' (atual: {leitor.Idade} - pressione Enter para manter)");
            if (!string.IsNullOrWhiteSpace(novaIdadeTexto))
            {
                if (!int.TryParse(novaIdadeTexto, out int novaIdade))
                    throw new ArgumentException("A idade informada não é um número válido.");
                leitor.Idade = novaIdade;
            }

            Console.WriteLine("Leitor atualizado com sucesso.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao editar leitor: {ex.Message}");
        }
    }

    private static void ExcluirLeitor()
    {
        var leitor = SelecionarLeitor("Digite o CPF do leitor que deseja excluir");
        if (leitor == null) return;

        Console.Write($"Tem certeza que deseja excluir '{leitor.Nome}'? (s/N): ");
        var confirmacao = Console.ReadLine();
        if (confirmacao?.Trim().ToLower() == "s")
        {
            leitor.LiberarCpf();
            _leitores.Remove(leitor);
            Console.WriteLine("Leitor removido.");
        }
        else
        {
            Console.WriteLine("Operação cancelada.");
        }
    }

    private static void IncluirLivro()
    {
        var leitor = SelecionarLeitor("Digite o CPF do leitor que receberá o livro");
        if (leitor == null) return;

        try
        {
            var isbn = LerTexto("ISBN");
            var titulo = LerTexto("Título");
            var subtitulo = LerTexto("Subtítulo");
            var escritor = LerTexto("Escritor");
            var editora = LerTexto("Editora");
            var genero = LerTexto("Gênero");
            
            var anoTexto = LerTexto("Ano de publicação");
            if (!int.TryParse(anoTexto, out int ano))
                throw new ArgumentException("O ano informado não é um número válido.");

            var tipoCapa = LerTexto("Tipo da capa");
            
            var paginasTexto = LerTexto("Número de páginas");
            if (!int.TryParse(paginasTexto, out int numeroDePaginas))
                throw new ArgumentException("O número de páginas informado não é numérico.");

            var livro = new Livro(isbn, titulo, subtitulo, escritor, editora, genero, ano, tipoCapa, numeroDePaginas);
            leitor.Livros.Add(livro);
            Console.WriteLine("Livro adicionado ao leitor.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao incluir livro: {ex.Message}");
        }
    }

    private static void EditarLivro()
    {
        var leitor = SelecionarLeitor("Digite o CPF do leitor que possui o livro");
        if (leitor == null) return;

        var livro = SelecionarLivro(leitor, "Escolha o livro para editar");
        if (livro == null) return;

        try
        {
            var novoTitulo = LerTexto($"Título (atual: {livro.Titulo}) - pressione Enter para manter");
            var novoSubtitulo = LerTexto($"Subtítulo (atual: {livro.Subtitulo}) - pressione Enter para manter");
            var novoEscritor = LerTexto($"Escritor (atual: {livro.Escritor}) - pressione Enter para manter");
            var novaEditora = LerTexto($"Editora (atual: {livro.Editora}) - pressione Enter para manter");
            var novoGenero = LerTexto($"Gênero (atual: {livro.Genero}) - pressione Enter para manter");
            var novoAnoTexto = LerTexto($"Ano de publicação (atual: {livro.AnoPublicacao}) - pressione Enter para manter");
            var novoTipoDaCapa = LerTexto($"Tipo da Capa (atual: {livro.TipoDaCapa}) - pressione Enter para manter");
            var novoNumeroPagsTexto = LerTexto($"Número de páginas (atual: {livro.NumeroDePaginas}) - pressione Enter para manter");

            if (!string.IsNullOrWhiteSpace(novoTitulo)) livro.Titulo = novoTitulo;
            if (!string.IsNullOrWhiteSpace(novoSubtitulo)) livro.Subtitulo = novoSubtitulo;
            if (!string.IsNullOrWhiteSpace(novoEscritor)) livro.Escritor = novoEscritor;
            if (!string.IsNullOrWhiteSpace(novaEditora)) livro.Editora = novaEditora;
            if (!string.IsNullOrWhiteSpace(novoGenero)) livro.Genero = novoGenero;
            
            if (!string.IsNullOrWhiteSpace(novoAnoTexto))
            {
                if (!int.TryParse(novoAnoTexto, out int novoAno)) throw new ArgumentException("O ano informado não é um número válido.");
                livro.AnoPublicacao = novoAno;
            }

            if (!string.IsNullOrWhiteSpace(novoTipoDaCapa)) livro.TipoDaCapa = novoTipoDaCapa;
            
            if (!string.IsNullOrWhiteSpace(novoNumeroPagsTexto))
            {
                if (!int.TryParse(novoNumeroPagsTexto, out int pags)) throw new ArgumentException("O número de páginas informado não é válido.");
                livro.NumeroDePaginas = pags;
            }

            Console.WriteLine("Livro atualizado.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao editar livro: {ex.Message}");
        }
    }

    private static void RemoverLivro()
    {
        var leitor = SelecionarLeitor("Digite o CPF do leitor que possui o livro a ser removido");
        if (leitor == null) return;

        var livro = SelecionarLivro(leitor, "Escolha o livro para remover");
        if (livro == null) return;

        leitor.Livros.Remove(livro);
        Console.WriteLine("Livro removido do leitor.");
    }

    private static void DoarLivro()
    {
        var origem = SelecionarLeitor("Digite o CPF do leitor que está doando o livro");
        if (origem == null) return;

        var livro = SelecionarLivro(origem, "Escolha o livro a ser doado");
        if (livro == null) return;

        var destino = SelecionarLeitor("Digite o CPF do leitor que receberá o livro");
        if (destino == null) return;

        if (destino == origem)
        {
            Console.WriteLine("Não é possível doar para o mesmo leitor.");
            return;
        }

        origem.Livros.Remove(livro);
        destino.Livros.Add(livro);

        Console.WriteLine($"Livro '{livro.Titulo}' doado de {origem.Nome} para {destino.Nome}.");
    }

    private static void ListarLeitorEspecifico()
    {
        var leitor = SelecionarLeitor("Digite o CPF do leitor que deseja visualizar");
        if (leitor == null) return;

        Console.WriteLine(leitor);
        ExibirLivros(leitor);
    }

    private static void PesquisarLivro()
    {
        var termo = LerTexto("Título ou parte do título do livro");
        if (string.IsNullOrWhiteSpace(termo))
        {
            Console.WriteLine("Termo inválido.");
            return;
        }

        var encontrados = _leitores
            .SelectMany(l => l.Livros.Select(livro => (Leitor: l, Livro: livro)))
            .Where(x => x.Livro.Titulo.Contains(termo, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (!encontrados.Any())
        {
            Console.WriteLine("Nenhum livro encontrado.");
            return;
        }

        foreach (var (leitor, livro) in encontrados)
        {
            Console.WriteLine($"{livro} -> Leitor: {leitor.Nome} (ID: {leitor.Id})");
        }
    }

    private static void ExibirLivros(Leitor leitor)
    {
        if (!leitor.Livros.Any())
        {
            Console.WriteLine("  (sem livros)");
            return;
        }

        for (var i = 0; i < leitor.Livros.Count; i++)
        {
            Console.WriteLine($"  {i + 1}) {leitor.Livros[i]}");
        }
    }

    private static Leitor? SelecionarLeitor(string prompt)
    {
        if (!_leitores.Any())
        {
            Console.WriteLine("Não há leitores cadastrados.");
            return null;
        }

        Console.WriteLine(prompt + ":");
        foreach (var leitor in _leitores)
        {
            Console.WriteLine(leitor);
        }

        var cpf = LerTexto("CPF do leitor");
        var selecionado = _leitores.FirstOrDefault(l => string.Equals(l.Cpf, cpf.Trim(), StringComparison.OrdinalIgnoreCase));

        if (selecionado == null)
        {
            Console.WriteLine("Leitor não encontrado.");
        }

        return selecionado;
    }

    private static Livro? SelecionarLivro(Leitor leitor, string prompt)
    {
        if (!leitor.Livros.Any())
        {
            Console.WriteLine("Este leitor não possui livros.");
            return null;
        }

        Console.WriteLine(prompt + ":");
        for (var i = 0; i < leitor.Livros.Count; i++)
        {
            Console.WriteLine($"  {i + 1}) {leitor.Livros[i]}");
        }

        var escolhaTexto = LerTexto("Número do livro");
        if (!int.TryParse(escolhaTexto, out int escolha) || escolha < 1 || escolha > leitor.Livros.Count)
        {
            Console.WriteLine("Escolha inválida.");
            return null;
        }

        return leitor.Livros[escolha - 1];
    }

    private static string LerTexto(string prompt)
    {
        Console.Write(prompt + ": ");
        return Console.ReadLine() ?? string.Empty;
    }
}
