using System.Diagnostics.Contracts;
using System.Reflection.PortableExecutable;
using System.Collections.Specialized;
using System.Reflection.Metadata.Ecma335;
using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace BatalhaRPG;

public class SimuladorCombate
{
    private static Random gerador = new Random(42);

    public static int CalcularDano(Personagem atacante, Personagem defensor)
    {
        if (!atacante.Vivo || !defensor.Vivo)
            return 0;

        // Dano base = Ataque - Defesa (mínimo 1)
        int danoBase = Math.Max(1, atacante.Ataque - defensor.Defesa);

        // Verificar crítico
        bool ehCritico = gerador.Next(0, 100) < atacante.ChanceCritico;

        if (ehCritico)
        {
            danoBase = (danoBase * atacante.MultCritico) / 100;
        }

        defensor.Vida -= danoBase;

        return danoBase;
    }

    public static int CalcularDanoVetorizado(ExercitoSIMD atacantes, ExercitoSIMD defensores, int i, int tam, Vector<int> prob)
    {
        int danoTotal = 0;

        var ataques = new Vector<int>(atacantes.Ataques, i);
        var defesas = new Vector<int>(defensores.Defesas, i);
        var chancesCritico = new Vector<int>(atacantes.ChancesCritico, i);
        var multCriticos = new Vector<int>(atacantes.MultCriticos, i);
        var vidas = new Vector<int>(defensores.Vidas, i);

        var danoBase = Vector.Max(Vector<int>.One, ataques - defesas);
        var danoCritico = danoBase * multCriticos / 100;

        var ehCritico = Vector.LessThan(prob, chancesCritico);

        var dano = Vector.ConditionalSelect(ehCritico, danoBase, danoCritico);  

        danoTotal += Vector.Dot(dano, Vector<int>.One);

        vidas -= danoBase;

        var vivo = Vector.GreaterThan(vidas, Vector<int>.Zero);
        vivo.CopyTo(defensores.Vivos, i);

        return danoTotal;
    }

    public static int SimularRodadaCombate(Personagem[] atacantes, Personagem[] defensores)
    {
        int danoTotal = 0;

        for (int i = 0; i < atacantes.Length && i < defensores.Length; i++)
        {
            danoTotal += CalcularDano(atacantes[i], defensores[i]);
        }        

        return danoTotal;
    }


    public static int SimularRodadaCombate(ExercitoSIMD atacantes, ExercitoSIMD defensores)
    {
        int danoTotal = 0;
        int tam = Vector<int>.Count;

        var pb = Enumerable.Range(0, tam).Select(i => gerador.Next(0, 100)).ToArray();
        var prob = new Vector<int>(pb);

        Random random = new Random();
        for (int i = 0; i < atacantes.Ataques.Length; i += tam)
        {
            danoTotal += CalcularDanoVetorizado(atacantes, defensores, i,tam,prob);
        }

        return danoTotal;
    }



    public static Personagem[] GerarExercito(int tamanho, string tipo)
    {
        Personagem[] exercito = new Personagem[tamanho];

        for (int i = 0; i < tamanho; i++)
        {
            if (tipo == "atacante")
            {
                exercito[i] = new Personagem
                {
                    Ataque = gerador.Next(80, 120),     // 80-119 ataque
                    Defesa = gerador.Next(20, 40),      // 20-39 defesa 
                    ChanceCritico = gerador.Next(15, 25), // 15-24% crítico
                    MultCritico = gerador.Next(180, 220), // 1.8x-2.2x crítico
                    Vida = gerador.Next(100, 150),
                };
            }
            else // defensor
            {
                exercito[i] = new Personagem
                {
                    Ataque = gerador.Next(60, 80),      // menos ataque
                    Defesa = gerador.Next(40, 70),      // mais defesa
                    ChanceCritico = gerador.Next(10, 20),
                    MultCritico = gerador.Next(150, 200),
                    Vida = gerador.Next(120, 180),      // mais vida
                };
            }
        }

        return exercito;
    }
}