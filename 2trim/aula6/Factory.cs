public abstract class Produto
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public abstract string ObterCategoria();
    public abstract decimal CalcularFrete();
}

public class Eletronico : Produto
{
    public float Consumo { get; set; }

    private string categoria = "Eletrônicos";
    public override string ObterCategoria()
    {
        return categoria;
    }
    public override decimal CalcularFrete()
    {
       return Preco * 0.05m;
    }
    
}
public class Livro : Produto
{
    public string Autor { get; set; }
    public int Paginas { get; set; }
    private string categoria = "Livros";
    public override string ObterCategoria()
    {
        return categoria;
    }
    public override decimal CalcularFrete() {
        if (Paginas > 300)
        {
            return 8;
        }
        return 5;
    }
}
public class Roupa : Produto
{
    public string Tamanho { get; set; }
    private string categoria = "Roupas";
    public override string ObterCategoria()
    {
        return categoria;
    }
    public override decimal CalcularFrete()
    {
       return 12.5m;
    }
}

public abstract class FabricaProduto
{
    public abstract Produto CriarProduto(string nome, decimal preco);
}

public class FabricaEletronicos : FabricaProduto
{
    public float consumo = 100;
    public override Produto CriarProduto(string nome, decimal preco)
    {
        return new Eletronico { Nome = nome, Preco = preco, Consumo = consumo };
    }
}

public class FabricaRoupa : FabricaProduto
{
    public string tamanho = "M";
    public override Produto CriarProduto(string nome, decimal preco)
    {
        return new Roupa { Nome = nome, Preco = preco, Tamanho = tamanho };
    }
}

public class FabricaLivro : FabricaProduto
{
    public string autor = "CriadorDoUniverso";
    public int paginas = 1217;
    public override Produto CriarProduto(string nome, decimal preco)
    {
        return new Livro { Nome = nome, Preco = preco, Autor = autor, Paginas = paginas };
    }
}
