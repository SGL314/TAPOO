using System;
using System.Threading;
using Project;


Random rd = new Random();
int id = 0;
string[] pratos = ["executivo","italiano","especial"];
List<Pedido> pedidos = new List<Pedido>();
object lockObj = new object(), lockConsole = new object(), lockDict = new object();
int qtChefs = 0;
int qtGarcoms = 0;

Dictionary<string,int> porcoes = new Dictionary<string, int>{
    {"rice",0}, 
    {"meat",0}, 
    {"sauce",0},
    {"pasta",0} 
};

void chef(){
    qtChefs ++;
	while (true){
        lock (lockObj){
            if (pedidos.Count == 0) continue;
        }
        if (pedidos[0].doing == false){
            switch (pedidos[0].nome){
                case "executivo":
                    pedidos[0].doing = true;
                    lock (lockDict) {
                        if (porcoes["rice"] < 1) prepareItem("rice");
                        if (porcoes["meat"] < 1) prepareItem("meat");
                        lock (lockConsole){
                            Console.ForegroundColor = ConsoleColor.Red;
                            WriteLine($"[Chef] Inicio da Preparação do Pedido {pedidos[0].id}");
                            Console.ResetColor();
                        }

                        porcoes["rice"]--;
                        porcoes["meat"]--;
                    }
                    Thread.Sleep(1000);
                    Thread.Sleep(1000);
                    lock (lockConsole){
                        Console.ForegroundColor = ConsoleColor.Red;
                        WriteLine($"[Chef] Fim da Preparação do Pedido {pedidos[0].id}");
                        Console.ResetColor();
                    }

                    break;
                case "italiano":
                    pedidos[0].doing = true;
                    lock (lockDict) {
                        if (porcoes["pasta"] < 1) prepareItem("pasta");
                        if (porcoes["sauce"] < 1) prepareItem("sauce");
                        lock (lockConsole){
                            Console.ForegroundColor = ConsoleColor.Red;
                            WriteLine($"[Chef] Inicio da Preparação do Pedido {pedidos[0].id}");
                            Console.ResetColor();
                        }

                        porcoes["pasta"]--;
                        porcoes["sauce"]--;
                    }
                    Thread.Sleep(1000);
                    Thread.Sleep(1000);

                    lock (lockConsole){
                        Console.ForegroundColor = ConsoleColor.Red;
                        WriteLine($"[Chef] Fim da Preparação do Pedido {pedidos[0].id}");
                        Console.ResetColor();
                    }
                    break;
                case "especial":
                    pedidos[0].doing = true;
                    lock (lockDict) {
                        if (porcoes["rice"] < 1) prepareItem("rice");
                        if (porcoes["meat"] < 1) prepareItem("meat");
                        if (porcoes["sauce"] < 1) prepareItem("sauce");
                        lock (lockConsole){
                            Console.ForegroundColor = ConsoleColor.Red;
                            WriteLine($"[Chef] Inicio da Preparação do Pedido {pedidos[0].id}");
                            Console.ResetColor();
                        }
                        porcoes["rice"]--;
                        porcoes["pasta"]--;
                        porcoes["meat"]--;
                    }
                    Thread.Sleep(1000);
                    Thread.Sleep(1000);
                    Thread.Sleep(1000);
                    lock (lockConsole){
                        Console.ForegroundColor = ConsoleColor.Red;
                        WriteLine($"[Chef] Fim da Preparação do Pedido {pedidos[0].id}");
                        Console.ResetColor();
                    }
                    break;
        }
        }
        lock (lockObj){
            pedidos.RemoveAt(0);
        }
    }
};

void WriteLine(string str){
    string itens = "";
    itens += "r: "+porcoes["rice"];
    itens += " p: "+porcoes["pasta"];
    itens += " m: "+porcoes["meat"];
    itens += " s: "+porcoes["sauce"];
    Console.WriteLine(str+" | "+itens);

}

void prepareItem(string item){
    lock (lockConsole){
        Console.ForegroundColor = ConsoleColor.Green;
        WriteLine($"[Chef] Iniciando produção de {item}");
        Console.ResetColor();
    }

    Thread.Sleep(2000);
    lock (lockDict){
        switch (item){
            case "rice":
                porcoes["rice"] += 3;
                break;
            case "meat":
                porcoes["meat"] += 2;
                break;
            case "sauce":
                porcoes["sauce"] += 2;
                break;
            case "pasta":
                porcoes["pasta"] += 4;
                break;
        }
    }

    lock (lockConsole){
        Console.ForegroundColor = ConsoleColor.Green;
        WriteLine($"[Chef] Finalizou produção de {item}. Estoque atualizado: {porcoes[item]} unidades");
        Console.ResetColor();
    }
}

void garcom(){
    qtGarcoms ++;
	while (true){
        int time = (int) (rd.NextDouble()*10+1);
        id++;
        string nome = pratos[(int)(rd.NextDouble()*3)];        
        
        lock (lockConsole){
            Console.ForegroundColor = ConsoleColor.Blue;
            WriteLine($"[Garçom] - Envio de Pedido {id}: Prato {nome}");
            Console.ResetColor();
        }

        lock (lockObj) {
            pedidos.Add(new Pedido(id,nome));
        }

        Thread.Sleep(time*1000);
    }
};

for (int i = 0 ;i < 3;i++){
    var a = Task.Run(garcom);
}
for (int i = 0 ;i < 6;i++){
    var a = Task.Run(chef);
}
void nothing(){
    while(true){}
}
var k = Task.Run(nothing);
k.Wait();

WriteLine("Thread finalizada. Continuando a execução...");