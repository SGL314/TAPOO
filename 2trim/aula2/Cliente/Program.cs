

// using Internal;
using System.Threading;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Main
{
    public class Tempo {
        double horas, minutos, segundos, milesimos;
    }
    public class Temperater
    {
        public string unidade { get; set; }
        public string enviada { get; set; }
        public float valor { get; set; }
    }
    

    class ClienteHttp
    {
        static int porta = 5034;
        static async Task Main()
        {
            string tipoTemperatura = "", indicativo = "";
            bool sair = false;

            while (true)
            {
                Console.Clear();
                Console.Write("Escolha o tipo de temperatura a ser recebida sendo:\nK: Kelvin\nC: Celsius\nF: Fahrenheit\n:> ");
                string tipo = Console.ReadLine();
                
                switch (tipo)
                {
                    case "K":
                        tipoTemperatura = "kelvin";
                        sair = true;
                        indicativo = "K";
                        break;
                    case "F":
                        tipoTemperatura = "fahrenheit";
                        sair = true;
                        indicativo = "F";
                        break;
                    case "C":
                        tipoTemperatura = "celsius";
                        sair = true;
                        indicativo = "C";
                        break;
                    case "k":
                        tipoTemperatura = "kelvin";
                        sair = true;
                        indicativo = "K";
                        break;
                    case "f":
                        tipoTemperatura = "fahrenheit";
                        sair = true;
                        indicativo = "F";
                        break;
                    case "c":
                        Console.Clear();
                        tipoTemperatura = "celsius";
                        sair = true;
                        indicativo = "C";
                        break;
                    default:
                        Console.WriteLine("Coloque um tipo de temperatura correto !");
                        Thread.Sleep(2000);
                        break;
                }

                if (sair)
                {
                    Console.Clear();
                    Thread.Sleep(1000);
                    break;
                }
            }
            await Receive(tipoTemperatura, indicativo);
        }

        static async Task Receive(string tipo, string indicativo)
        {
            using var cliente = new HttpClient();
            float ultimo = 0, temperatura = 0;
            string classificacao = "SEM ALTERAÇÃO";
            Console.WriteLine("Temperaturas: ");

            while (true)
            {
                var resposta = await cliente.GetStringAsync($"http://localhost:{porta}/temperatura/{tipo}");
                var resultado = JsonSerializer.Deserialize<Temperater>(resposta);
                temperatura = resultado.valor;

                if (ultimo == 0) ultimo = temperatura;

                if (resultado.valor == ultimo)
                {
                    classificacao = "SEM ALTERAÇÃO";
                }
                else if (resultado.valor > ultimo)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    classificacao = "SUBIU";
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    classificacao = "DESCEU";
                }

                Console.WriteLine($"{resultado.valor}°{indicativo} | {classificacao} | {resultado.enviada}");
                Thread.Sleep(1000);
                ultimo = temperatura;
                Console.ResetColor();
            }
        }
    }

    class Main
    {
        public static void main(string[] args)
        {
            ClienteHttp clienteHttp = new ClienteHttp();
        }
    }
}






