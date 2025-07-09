public abstract class DecoradorProduto : Produto
{
	protected Produto _produto;
   
	public DecoradorProduto(Produto produto)
	{
    	    _produto = produto;
	}
   
	public override string ObterCategoria() => _produto.ObterCategoria();
	public override decimal CalcularFrete() => _produto.CalcularFrete();
}

public class DecoradorGarantia : DecoradorProduto
{
    private int _mesesGarantia;
    private int _cor;

    public DecoradorGarantia(Produto produto, int mesesGarantia) : base(produto)
    {
        _mesesGarantia = mesesGarantia;
        Preco = produto.Preco + (mesesGarantia * 10); // R$10 por mês
    }

}

public class DecoradorFreteExpresso : DecoradorProduto
{

    public DecoradorFreteExpresso(Produto produto) : base(produto)
    {
        Preco = produto.Preco + 15;
    }
}

public class DecoradorEmbalagemPresente : DecoradorProduto
{
    public string _cor;
    public DecoradorEmbalagemPresente(Produto produto, string cor) : base(produto)
    {
        _cor = cor;
        Preco = produto.Preco + 5;
    }
}



