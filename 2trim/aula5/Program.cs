using System;
using System.Diagnostics;
using BatalhaRPG;
using System.Numerics;

namespace RPG;

class Program
{
    static void Main()
    {
        TestarPerformanceCompleta();
    }


    public static void TestarPerformanceCompleta()
    {
        int[] tamanhosExercito = {10_000, 50_000, 100_000, 500_000, 1_000_000 }; 

        Console.WriteLine("=== BENCHMARK DE SISTEMA DE COMBATE ===");
        Console.WriteLine($"SIMD Suportado: {Vector.IsHardwareAccelerated}");
        Console.WriteLine($"Elementos por Vetor: {Vector<int>.Count}");
        Console.WriteLine();

        foreach (int tamanho in tamanhosExercito)
        {
            Console.WriteLine($"Testando exércitos de {tamanho:N0} personagens:");

            // Testar versão original
            // Testar versão SIMD
            // Calcular speedup
            // Mostrar DPS (danos por segundo)

            int tamanhoExercito = tamanho; // Meio milhão de personagens!

            Personagem[] atacantes = SimuladorCombate.GerarExercito(tamanhoExercito, "atacante");
            Personagem[] defensores = SimuladorCombate.GerarExercito(tamanhoExercito, "defensor");

            Console.WriteLine($"Exércitos: {tamanhoExercito:N0} vs {tamanhoExercito:N0}");

            Stopwatch cronometro = Stopwatch.StartNew();
            int danoTotalRodada = SimuladorCombate.SimularRodadaCombate(atacantes, defensores);
            cronometro.Stop();

            Console.WriteLine($"\nDano total causado: {danoTotalRodada:N0}");
            Console.WriteLine($"Tempo sem SIMD: {cronometro.ElapsedMilliseconds}ms");
            Console.WriteLine($"DPS (danos por segundo): {danoTotalRodada * 1000 / Math.Max(1, cronometro.ElapsedMilliseconds):N0}");


            var exercitoAtacantesSIMD = new ExercitoSIMD(atacantes);
            var exercitoDefensoresSIMD = new ExercitoSIMD(defensores);

            cronometro = Stopwatch.StartNew();
            int danoTotalRodadaSIMD = SimuladorCombate.SimularRodadaCombate(exercitoAtacantesSIMD, exercitoDefensoresSIMD);
            cronometro.Stop();

            Console.WriteLine($"\nDano total causado: {danoTotalRodadaSIMD:N0}");
            Console.WriteLine($"Tempo com SIMD: {cronometro.ElapsedMilliseconds}ms");
            Console.WriteLine($"DPS (danos por segundo): {danoTotalRodadaSIMD * 1000 / Math.Max(1, cronometro.ElapsedMilliseconds):N0}");
        }
    }
}