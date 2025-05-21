using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


var endereco = IPAddress.Any;
const int porta = 9000;
var listener = new TcpListener(endereco, porta);
listener.Start();
Console.WriteLine($"Servidor TCP ouvindo na porta {porta}...");
bool continuar = true;


var cliente = await listener.AcceptTcpClientAsync();
using var stream = cliente.GetStream();

var t1 = Receber(cliente);
var t2 = Enviar(cliente);

await Task.WhenAll(t1, t2);
Console.WriteLine("Fim");


async Task Enviar(TcpClient cliente)
{
    while(continuar)
    {
        var msg = Console.ReadLine();
        if (!string.IsNullOrEmpty(msg))
            await EnviaMensagem(cliente, msg);

        if(msg == "sair")
        {
            continuar = false;
            break;
        }
    }
}

async Task Receber(TcpClient cliente)
{
    while(continuar)
    {
        var msg = await RecebeMensagem(cliente);
        if(msg == "sair")
        {
            continuar = false;
            break;
        }
        if(!string.IsNullOrEmpty(msg)) 
            Console.WriteLine($"Cliente: {msg}");
    }
}

async Task EnviaMensagem(TcpClient cliente, string mensagem)
{
    var dados = Encoding.UTF8.GetBytes(mensagem);
    await stream.WriteAsync(dados, 0, dados.Length);
}

async Task<string> RecebeMensagem(TcpClient cliente)
{
    var buffer = new byte[1024];
    int lidos = await stream.ReadAsync(buffer, 0, buffer.Length);
    return Encoding.UTF8.GetString(buffer, 0, lidos);
}