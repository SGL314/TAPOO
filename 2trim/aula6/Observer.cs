public interface IObservadorPedido
{
	void AoMudarStatusPedido(Pedido pedido, string novoStatus);
}

public interface IObservavel<T>
{
    void Inscrever(T sub);
}

public class NotificadorEmail : IObservadorPedido
{
    public void AoMudarStatusPedido(Pedido pedido, string novoStatus)
    {
        Console.WriteLine($"Email:\n  {pedido}\n  >>> status >>>{novoStatus}");
    }   
}
public class NotificadorSMS : IObservadorPedido
{
    public void AoMudarStatusPedido(Pedido pedido, string novoStatus)
    {
        Console.WriteLine($"SMS:\n  {pedido}\n  >>> status >>>{novoStatus}");
    }  
}

public class Pedido : IObservavel<IObservadorPedido>
{
    private List<IObservadorPedido> _observadores = new List<IObservadorPedido>();
    private string _status;

    public string Status
    {
        get => _status;
        set
        {
            _status = value;
            NotificarObservadores();
        }
    }

    public void Inscrever(IObservadorPedido observador)
    {
        _observadores.Add(observador);
    }

    private void NotificarObservadores()
    {
        foreach (var observador in _observadores)
        {
            observador.AoMudarStatusPedido(this, _status);
        }
    }

    public override string ToString()
    {
        return $"Pedido: {_status}";
    }
}



