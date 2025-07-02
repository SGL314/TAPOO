namespace BatalhaRPG;

public class ExercitoSIMD
{
	public int[] Ataques;
	public int[] Defesas;
	public int[] ChancesCritico;
	public int[] MultCriticos;
	public int[] Vidas;
	public int[] Vivos;
   
	public ExercitoSIMD(Personagem[] personagens)
	{
		int tamanho = personagens.Length;
		
    	Ataques = new int[tamanho];
    	Defesas = new int[tamanho];
    	ChancesCritico = new int[tamanho];
    	MultCriticos = new int[tamanho];
    	Vidas = new int[tamanho];
    	Vivos = new int[tamanho];

        for (int i = 0; i < tamanho; i++)
        {
            Ataques[i] = personagens[i].Ataque;
            Defesas[i] = personagens[i].Defesa;
            ChancesCritico[i] = personagens[i].ChanceCritico;
            MultCriticos[i] = personagens[i].MultCritico;
            Vidas[i] = personagens[i].Vida;
            Vivos[i] = personagens[i].Vivo ? 1 : 0;
        }
	}

}