namespace BibliotecaDoZE;

using System;
using System.Collections.Generic;
using System.Linq;

public class Leitor
{
    private static int _nextId = 1;

    public int Id { get; }
    public string Cpf { get; init; }
    public string Nome { get; set; }
    public List<Livro> Livros { get; } = new();

    public Leitor(string cpf, string nome)
    {
        Id = _nextId++;
        Cpf = cpf;
        Nome = nome;
    }

    public override string ToString() => $"[{Id}] {Nome} (CPF: {Cpf}) - Livros: {Livros.Count}";
}

internal class Program
{
    private static readonly List<Leitor> _leitores = new();

    private static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Biblioteca DoZE ===");
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
        var cpf = LerTexto("CPF do leitor");
        if (string.IsNullOrWhiteSpace(cpf))
        {
            Console.WriteLine("CPF inválido. Operação cancelada.");
            return;
        }

        if (_leitores.Exists(l => l.Cpf == cpf.Trim()))
        {
            Console.WriteLine("CPF já cadastrado. Operação cancelada.");
            return;
        }

        var nome = LerTexto("Nome do leitor");
        if (string.IsNullOrWhiteSpace(nome))
        {
            Console.WriteLine("Nome inválido. Operação cancelada.");
            return;
        }

        var leitor = new Leitor(cpf.Trim(), nome.Trim());
        _leitores.Add(leitor);
        Console.WriteLine($"Leitor cadastrado: {leitor}");
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

        var novoNome = LerTexto($"Novo nome para '{leitor.Nome}' (pressione Enter para manter)");
        if (!string.IsNullOrWhiteSpace(novoNome))
        {
            leitor.Nome = novoNome.Trim();
            Console.WriteLine("Nome atualizado com sucesso.");
        }
        else
        {
            Console.WriteLine("Nenhuma alteração feita.");
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

        var titulo = LerTexto("Título");
        var autor = LerTexto("Autor");
        var ano = LerInt("Ano de publicação");
        var genero = LerTexto("Gênero");

        if (string.IsNullOrWhiteSpace(titulo) || string.IsNullOrWhiteSpace(autor) || ano <= 0 || string.IsNullOrWhiteSpace(genero))
        {
            Console.WriteLine("Dados incompletos. Operação cancelada.");
            return;
        }

        leitor.Livros.Add(new Livro(titulo.Trim(), autor.Trim(), Convert.ToInt32(ano), genero.Trim()));
        Console.WriteLine("Livro adicionado ao leitor.");
    }

    private static void EditarLivro()
    {
        var leitor = SelecionarLeitor("Digite o CPF do leitor que possui o livro");
        if (leitor == null) return;

        var livro = SelecionarLivro(leitor, "Escolha o livro para editar");
        if (livro == null) return;

        var novoTitulo = LerTexto($"Título (atual: {livro.Titulo}) - pressione Enter para manter");
        var novoAutor = LerTexto($"Autor (atual: {livro.Autor}) - pressione Enter para manter");
        var novoAno = LerInt($"Ano de publicação (atual: {livro.AnoPublicacao}) - pressione Enter para manter", allowEmpty: true);
        var novoGenero = LerTexto($"Gênero (atual: {livro.Genero}) - pressione Enter para manter");

        if (!string.IsNullOrWhiteSpace(novoTitulo)) livro.Titulo = novoTitulo.Trim();
        if (!string.IsNullOrWhiteSpace(novoAutor)) livro.Autor = novoAutor.Trim();
        if (novoAno.HasValue && novoAno.Value > 0) livro.AnoPublicacao = novoAno.Value;
        if (!string.IsNullOrWhiteSpace(novoGenero)) livro.Genero = novoGenero.Trim();

        Console.WriteLine("Livro atualizado.");
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
        var selecionado = _leitores.FirstOrDefault(l => l.Cpf == cpf.Trim());

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

        var escolha = LerInt("Número do livro");
        if (escolha < 1 || escolha > leitor.Livros.Count)
        {
            Console.WriteLine("Escolha inválida.");
            return null;
        }

        return leitor.Livros[Convert.ToInt32(escolha) - 1];
    }

    private static string LerTexto(string prompt)
    {
        Console.Write(prompt + ": ");
        return Console.ReadLine() ?? string.Empty;
    }

    private static int? LerInt(string prompt, bool allowEmpty = false)
    {
        Console.Write(prompt + ": ");
        var input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input))
        {
            return allowEmpty ? null : 0;
        }

        return int.TryParse(input.Trim(), out var valor) ? valor : 0;
    }
}
