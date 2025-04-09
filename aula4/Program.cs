using System;
using System.Threading;
using Project;


Random rd = new Random();
int id = 0;
string[] pratos = ["executivo","italiano","especial"];
List<Pedido> pedidos = new List<Pedido>();
Dictionary<int, int> ids = new Dictionary<int, int>();
object lockPedidos = new object(), lockConsole = new object(), lockDict = new object(), lockId = new object();
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
    ids[Thread.CurrentThread.ManagedThreadId] = qtChefs;
	while (true){
        lock (lockPedidos){
            if (pedidos.Count == 0) continue;
        }
        Pedido pedido = new Pedido(-1,"nada");
        if (pedidos[0].doing == false){
            lock (lockPedidos){
                pedido = pedidos[0];
                pedido.doing = true;
            }
            switch (pedido.nome){
                case "executivo":
                    pedido.doing = true;
                    lock (lockDict) {
                        if (porcoes["rice"] < 1) prepareItem("rice");
                        if (porcoes["meat"] < 1) prepareItem("meat");
                        lock (lockConsole){
                            Console.ForegroundColor = ConsoleColor.Red;
                            WriteLine($"[Chef {ids[Thread.CurrentThread.ManagedThreadId]}] Inicio da Preparação do Pedido {pedido.id}");
                            Console.ResetColor();
                        }

                        porcoes["rice"]--;
                        porcoes["meat"]--;
                    }
                    Thread.Sleep(1000);
                    Thread.Sleep(1000);
                    lock (lockConsole){
                        Console.ForegroundColor = ConsoleColor.Red;
                        WriteLine($"[Chef {ids[Thread.CurrentThread.ManagedThreadId]}] Fim da Preparação do Pedido {pedido.id}");
                        Console.ResetColor();
                    }

                    break;
                case "italiano":
                    pedido.doing = true;
                    lock (lockDict) {
                        if (porcoes["pasta"] < 1) prepareItem("pasta");
                        if (porcoes["sauce"] < 1) prepareItem("sauce");
                        lock (lockConsole){
                            Console.ForegroundColor = ConsoleColor.Red;
                            WriteLine($"[Chef {ids[Thread.CurrentThread.ManagedThreadId]}] Inicio da Preparação do Pedido {pedido.id}");
                            Console.ResetColor();
                        }

                        porcoes["pasta"]--;
                        porcoes["sauce"]--;
                    }
                    Thread.Sleep(1000);
                    Thread.Sleep(1000);

                    lock (lockConsole){
                        Console.ForegroundColor = ConsoleColor.Red;
                        WriteLine($"[Chef {ids[Thread.CurrentThread.ManagedThreadId]}] Fim da Preparação do Pedido {pedido.id}");
                        Console.ResetColor();
                    }
                    break;
                case "especial":
                    pedido.doing = true;
                    lock (lockDict) {
                        if (porcoes["rice"] < 1) prepareItem("rice");
                        if (porcoes["meat"] < 1) prepareItem("meat");
                        if (porcoes["sauce"] < 1) prepareItem("sauce");
                        lock (lockConsole){
                            Console.ForegroundColor = ConsoleColor.Red;
                            WriteLine($"[Chef {ids[Thread.CurrentThread.ManagedThreadId]}] Inicio da Preparação do Pedido {pedido.id}");
                            Console.ResetColor();
                        }
                        porcoes["rice"]--;
                        porcoes["sauce"]--;
                        porcoes["meat"]--;
                    }
                    Thread.Sleep(1000);
                    Thread.Sleep(1000);
                    Thread.Sleep(1000);
                    lock (lockConsole){
                        Console.ForegroundColor = ConsoleColor.Red;
                        WriteLine($"[Chef {ids[Thread.CurrentThread.ManagedThreadId]}] Fim da Preparação do Pedido {pedido.id}");
                        Console.ResetColor();
                    }
                    break;
        }
        }
        lock (lockPedidos){
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
        WriteLine($"[Chef {ids[Thread.CurrentThread.ManagedThreadId]}] Iniciando produção de {item}");
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
        WriteLine($"[Chef {ids[Thread.CurrentThread.ManagedThreadId]}] Finalizou produção de {item}. Estoque atualizado: {porcoes[item]} unidades");
        Console.ResetColor();
    }
}

void garcom(){
    qtGarcoms ++;
    ids[Thread.CurrentThread.ManagedThreadId] = qtGarcoms;
	while (true){
        int time = (int) (rd.NextDouble()*1+10);
        int current_id = 0;
        lock (lockId){
            id++;
            current_id = id;
        }

        string nome = pratos[(int)(rd.NextDouble()*3)];

        lock (lockPedidos) {
            pedidos.Add(new Pedido(current_id,nome));
        
            Pedido pedido = new Pedido(0,"nenhum");
            foreach (Pedido ped in pedidos){
                if (ped.id == current_id){
                    pedido = ped;
                    break;
                }
            }
            string lista = "";
            lock (lockPedidos){
                foreach (Pedido ped in pedidos){
                    lista += ped.id+" ";
                }
            }
            lock (lockConsole){
                
                Console.ForegroundColor = ConsoleColor.Blue;
                WriteLine($"[Garçom {ids[Thread.CurrentThread.ManagedThreadId]}] - Envio de Pedido {current_id}: Prato {pedido.nome} [{lista}]");
                Console.ResetColor();
            }
        }      

        

        Thread.Sleep(time*1000);
    }
};

for (int i = 0 ;i < 2;i++){
    var a = Task.Run(garcom);
}
for (int i = 0 ;i < 3;i++){
    var a = Task.Run(chef);
}
void nothing(){
    while(true){}
}
var k = Task.Run(nothing);
k.Wait();

WriteLine("Thread finalizada. Continuando a execução...");