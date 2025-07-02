using System.Text.RegularExpressions;

while (true){
    
    Console.WriteLine("Digite uma senha forte:");
    string senha = Console.ReadLine();
    var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()+=_\-{}\[\]:;""'?<>,.]).{7,16}$");
    if (regex.IsMatch(senha)){
        Console.WriteLine("Sucesso");
        break;
    }else{
        Console.WriteLine("Senha fraca!");
        Thread.Sleep(1500);
        Console.Clear();
    }
}

