namespace BibliotecaDoZE;

public class Program
{
    private static void Main(string[] args)
    {
        Livro livro1;
        livro1 = new Livro("O Senhor dos Anéis", "J.R.R. Tolkien", 1954, "Fantasia");
        Console.WriteLine(livro1);
    }
}