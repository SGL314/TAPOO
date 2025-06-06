﻿using System.Security.AccessControl;
using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

// variáveis
char[,] grid = new char[10,10];
const string host = "127.0.0.1";
var endereco = IPAddress.Any;
const int porta = 9000;
//

var listener = new TcpListener(endereco, porta);
listener.Start();
bool continuar = true;


var cliente = await listener.AcceptTcpClientAsync();
using var stream = cliente.GetStream();

int player = 1;
int outroPlayer = 2;
int turno = 1;
Console.WriteLine($"Player{player} entrou no Jogo");
var t1 = Receber(cliente);
var t2 = Enviar(cliente);

await Task.WhenAll(t1, t2);
Console.WriteLine("Fim");

// game

async Task Print(bool showShips)
{
    Console.Write("   ");
    for (int c = 0; c < 10; c++) Console.Write($"{c} ");
    Console.WriteLine();
    for (int r = 0; r < 10; r++)
    {
        Console.Write($"{(char)('A' + r)}  ");
        for (int c = 0; c < 10; c++)
        {
            char cell = grid[r, c];
            Console.Write(!showShips && cell=='*' ? "~ " : $"{cell} ");
        }
        Console.WriteLine();
    }
}

async Task PlaceShipsRandomly(int n)
{
    //
    for (int i =0;i<n;i++){
        for (int j=0;j<n;j++){
            grid[i,j] = '~';
        }
    }
    //
    var rnd = new Random();
    int placed = 0;
    while (placed < n)
    {
        int r = rnd.Next(10), c = rnd.Next(10);
        if (grid[r,c] == '~')
        {
            grid[r,c] = '*';
            placed++;
        }
    }
}

async Task process(string msg){
    int r = -1, c = 0;
    bool marcado = false;
    for (int i = 0; i < 10; i++){
        c = 0;
        r++;
        for (int j = 0; j < 10; j++){
            if (msg == $"{(char)('A' + i)}{j}" || msg == $"{(char)('a' + i)}{j}"){
                i = 11;
                marcado = true;
                break;
            }
            c++;
        }
    }
    if (!marcado){
        Console.WriteLine($"Código '{msg}' não identificado");
        return;
    }else{
        turno = 1;
        await EnviaMensagem(cliente,"turno");
    }
    //
    if (IsShip(r,c)) MarkHit(r,c);
    else MarkMiss(r,c);

    Print(true);

    if (AreAllShipsSunk()){
        Console.WriteLine($"WIN Player{outroPlayer}");
        await EnviaMensagem(cliente,$"WIN Player{outroPlayer}");
        Thread.Sleep(1000);
        await EnviaMensagem(cliente,"sair");
    }
}

bool IsShip(int r,int c){
    if (grid[r,c] == '*') return true;
    return false;
}
void MarkHit(int r,int c){
    grid[r,c] = 'x';
    Console.WriteLine("HIT");
}
void MarkMiss(int r,int c){
    grid[r,c] = 'o';
    Console.WriteLine("MISS");
}
bool AreAllShipsSunk(){
    for (int i=0;i<10;i++){
        for (int j=0;j<10;j++){
            if (grid[i,j] == '*'){
                return false;
            }
        }
    }
    return true;
}
//

async Task Enviar(TcpClient cliente)
{
    PlaceShipsRandomly(10);
    Print(true);
    while (continuar)
    {
        Console.Write(":> ");
        var msg = Console.ReadLine();
        if (turno == player)
        {
            if (!string.IsNullOrEmpty(msg))
                await EnviaMensagem(cliente, msg);
            Print(true);
        }
        else if (msg == "sair")
        {
            continuar = false;
            break;
        }
        else
        {
            Console.WriteLine("Não é a sua vez de Jogar");
        }
    }
}

async Task EnviaMensagem(TcpClient cliente, string mensagem)
{
    var dados = Encoding.UTF8.GetBytes(mensagem);
    await stream.WriteAsync(dados, 0, dados.Length);
}

async Task Receber(TcpClient cliente)
{
    while(continuar)
    {
        var msg = await RecebeMensagem(cliente);
        if (msg == "sair")
        {
            continuar = false;
            break;
        }
        else if (msg == "turno")
        {
            if (turno == 1) turno = 2;
            else turno = 1;
        }
        else
        {
            if (!string.IsNullOrEmpty(msg))
            {

                Console.WriteLine($"Player{outroPlayer}: {msg}");
            }

            process(msg);
        }
    }
}
async Task<string> RecebeMensagem(TcpClient cliente)
{
    var buffer = new byte[1024];
    int lidos = await stream.ReadAsync(buffer, 0, buffer.Length);
    return Encoding.UTF8.GetString(buffer, 0, lidos);
}