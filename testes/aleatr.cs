class Aleatr{
    public Aleatr(){
        Main();
    }

    private void Main() {
        Random rd = new Random();
        int[] lista = [1,2,3,4,5,6,7,8,9,11,12,13,14,15,16];
        Console.WriteLine($"{lista[rd.Next(lista.Length)]}");


    }
}