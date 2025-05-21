using System.Collections.Concurrent;
using System.Text.Json;
using System.Globalization;
using System.Threading;

var criptomoedas = new List<string>
{
	"BTC",  // Bitcoin
	"ETH",  // Ethereum
	"LTC",  // Litecoin
	"BCH",  // Bitcoin Cash
	"XRP",  // Ripple
	"ADA",  // Cardano
	"DOT",  // Polkadot
	"LINK", // Chainlink
	"XLM",  // Stellar
	"DOGE"  // Dogecoin
};

ConcurrentDictionary<string, List<decimal>> dictionary = new ConcurrentDictionary<string, List<decimal>>();
foreach (var cripto in criptomoedas){
    dictionary[cripto] = new List<decimal>([0,0]);
}

static HttpClient CriarClienteHttp() {
	var cliente = new HttpClient {
    	Timeout = TimeSpan.FromSeconds(10)
	};
	cliente.DefaultRequestHeaders.Add("User-Agent", "MonitorCripto/1.0");
	cliente.DefaultRequestHeaders.Add("Accept", "application/json");
	return cliente;
}

async Task ObterEConverterCotacaoAsync(string simbolo, CancellationToken token) {
	var clienteHttp = CriarClienteHttp();
	var urlRequisicao = $"https://api.exchange.cryptomkt.com/api/3/public/price/rate?from={simbolo}&to=USDT";
	var resposta = await clienteHttp.GetAsync(urlRequisicao, token);
	resposta.EnsureSuccessStatusCode();

	var json = await resposta.Content.ReadAsStringAsync(token);

	using var documento = JsonDocument.Parse(json);
	if (documento.RootElement.TryGetProperty(simbolo, out var dadosMoeda)){
    	var precoString = dadosMoeda.GetProperty("price").GetString();
    	decimal precoAtual = decimal.Parse(precoString, CultureInfo.InvariantCulture);
        foreach (var cripto in criptomoedas){
            if (cripto == simbolo){
                dictionary[cripto][0] = dictionary[cripto][1];
                dictionary[cripto][1] = precoAtual;
                break;
            }
        }
    }
}

void ExibirResultadosNoConsole(string simbolo, decimal precoAtual, decimal precoAnterior) {
	var corOriginal = Console.ForegroundColor;
	Console.ForegroundColor = precoAtual > precoAnterior ? ConsoleColor.Green : ConsoleColor.Red;
	Console.WriteLine($"{simbolo}: ${precoAtual:N2} {(precoAtual > precoAnterior ? "↑" : "↓")}");
	Console.ForegroundColor = corOriginal;
}

async Task MonitorarTeclaEscAsync(CancellationTokenSource cts) {
	while (true) {
    	if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape) {
        	cts.Cancel();
        	break;
    	}
    	await Task.Delay(100);        
	}
}

async Task main(CancellationToken token){
    int i = 0;
    try{
        while (!token.IsCancellationRequested) {
            foreach (string cripto in criptomoedas){
                await ObterEConverterCotacaoAsync(cripto, token);
            }
            await Task.Delay(1000);
            Console.Clear();
            foreach (var cripto in criptomoedas){
                ExibirResultadosNoConsole(cripto, dictionary[cripto][1], dictionary[cripto][0]);
            }
        }
    }catch (OperationCanceledException ex){}
}

using var token = new CancellationTokenSource();

var a = main(token.Token);
var b = MonitorarTeclaEscAsync(token);

await Task.WhenAll([a, b]);

