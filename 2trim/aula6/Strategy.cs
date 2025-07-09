public interface IEstrategiaPagamento
{
    bool ProcessarPagamento(decimal valor);
    string ObterDetalhespagamento();
}

public class PagamentoCartaoCredito : IEstrategiaPagamento
{
    private string _tipo = "Cartão de Crédito";
    private int _teto = 5000;
    private decimal _valor = 0;
    public string NumeroCartao = "";
    public string NomeTitular = "";
    public bool ProcessarPagamento(decimal valor)
    {
        _valor = valor;
        if (valor < _teto && valor >=0)
        {
            Console.WriteLine($"Pago via {_tipo}: {valor}");
            return true;
        }
        else
        {
            Console.WriteLine($"Pagamento ultrapassou {_teto} reais ou é negativo ({valor})");
            return false;
        }
    }

    public string ObterDetalhespagamento()
    {
        string lasts = "",res="";
        for (int i = NumeroCartao.Length - 1; i >= 0; i--) {
            lasts += NumeroCartao[i];
            if ((NumeroCartao.Length - 1) - i >= 3) break;   
        }
        for (int i = lasts.Length - 1; i >= 0; i--) {
            res += lasts[i];
            if ((lasts.Length - 1) - i >= 3) break;   
        }
        string txt = $"{_tipo}: {res}";
        return txt;
    }

}
public class PagamentoPayPal : IEstrategiaPagamento
{
    private string _tipo = "pay Pal";
    private int _teto = 0;
    public string EmailPayPal = "";
    private decimal _valor = 0;
    public bool ProcessarPagamento(decimal valor)
    {
        _valor = valor;
        if (valor >= _teto && EmailPayPal.Length > 0)
        {
            Console.WriteLine($"Pago via {_tipo}: {valor}");
            return true;
        }
        else
        {
            Console.WriteLine($"Valor negativo ou email nulo ({valor}) ({EmailPayPal})");
            return false;
        }
    }

    public string ObterDetalhespagamento()
    {
        string txt = $"{_tipo}: {EmailPayPal}";
        return txt;
    }
}
public class PagamentoPix : IEstrategiaPagamento
{
    private string _tipo = "Pix";
    private int _teto =0;
    public string ChavePix = "";
    private decimal _valor = 0;
    public bool ProcessarPagamento(decimal valor)
    {
        _valor = valor;
        if (valor >= _teto)
        {
            Console.WriteLine($"Pago via {_tipo}: {valor}");
            return true;
        }
        else
        {
            Console.WriteLine($"Valor negativo ({valor})");
            return false;
        }
    }

    public string ObterDetalhespagamento()
    {
        string txt = $"{_tipo}: {ChavePix}";
        return txt;
    }
}

public class ContextoPagamento
{
    public IEstrategiaPagamento _estrategiaPagamento;

    public void DefinirEstrategiaPagamento(IEstrategiaPagamento estrategia)
    {
        _estrategiaPagamento = estrategia;
    }

    public bool ExecutarPagamento(decimal valor)
    {
        return _estrategiaPagamento?.ProcessarPagamento(valor) ?? false;
    }
}
