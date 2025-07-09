public class SistemaECommerce
{
    public static void Main()
    {
        // 1. Configurar sistema usando Singleton
        var configuracao = GerenciadorConfiguracao.Instancia;

        // 2. Criar produtos usando Factory
        var fabricaEletronicos = new FabricaEletronicos();
        var smartphone = fabricaEletronicos.CriarProduto("iPhone", 999.99m);

        // 3. Aplicar decoradores
        var smartphoneComGarantia = new DecoradorGarantia(smartphone, 12);
        var produtoFinal = new DecoradorFreteExpresso(smartphoneComGarantia);

        // 4. Criar pedido e adicionar observadores
        var pedido = new Pedido();
        pedido.Inscrever(new NotificadorEmail());
        pedido.Inscrever(new NotificadorSMS());

        // 5. Processar pagamento usando Strategy
        var contextoPagamento = new ContextoPagamento();
        contextoPagamento.DefinirEstrategiaPagamento(new PagamentoCartaoCredito());

        //
        
        Console.WriteLine(contextoPagamento._estrategiaPagamento.ObterDetalhespagamento());

        pedido.Status = "pago";
        Console.WriteLine(pedido.Status);
	}
}
