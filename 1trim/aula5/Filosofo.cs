namespace Fil{
    class Filosofo{
        public int id;
        public int lanchei = 0;
        public string state = "pensando";
        public List<Garfo> garfos;
        public object lockGarfos;

        public Filosofo(int id,object lockGarfos, List<Garfo> garfos){
            this.id = id;
            this.garfos = garfos;
            this.lockGarfos = lockGarfos;
        }

        void WriteLine(string str,string cor){
            switch (cor){
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "white":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "yellow":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
            Console.WriteLine($"{str}");
        }

        public void showState(){
            if (this.state == "comendo"){
                WriteLine($"Filosofo {id}({lanchei}) está {state}","yellow");
            }else{
                WriteLine($"Filosofo {id}({lanchei}) está {state}","white");
            }
        }

        public void viver(){
            while (true){
                showState();
                Thread.Sleep(1000);
                int lockIt = 0;
                lock (lockGarfos){
                    foreach (Garfo garfo in garfos){
                        if (garfo.use == true) continue;
                        if (this.id != 5 && (garfo.id == id || garfo.id == id+1)){
                            lockIt++;
                        }else if (this.id == 5 && (garfo.id == id || garfo.id == 1)){
                            lockIt++;
                        }
                    }
                }
                if (lockIt == 2){
                    this.state = "comendo";
                    lock (lockGarfos){
                        foreach (Garfo garfo in garfos){
                            if (garfo.use == true) continue;
                            if (this.id != 5 && (garfo.id == id || garfo.id == id+1)){
                                WriteLine($"Filosofo {id} bloqueou o garfo {garfo.id}","red");
                                garfo.use = true;
                            }else if (this.id == 5 && (garfo.id == id || garfo.id == 1)){
                                WriteLine($"Filosofo {id} bloqueou o garfo {garfo.id}","red");
                                garfo.use = true;
                            }
                        }
                    }
                    this.lanchei ++;
                    showState();
                    Thread.Sleep(3000);
                    lock (lockGarfos){
                        foreach (Garfo garfo in garfos){
                            if (this.id != 5 && (garfo.id == id || garfo.id == id+1)){
                                WriteLine($"Filosofo {id} desbloqueou o garfo {garfo.id}","green");
                                garfo.use = false;
                            }else if (this.id == 5 && (garfo.id == id || garfo.id == 1)){
                                WriteLine($"Filosofo {id} desbloqueou o garfo {garfo.id}","green");
                                garfo.use = false;
                            }
                        }
                    }
                }
            }
        }
    }
}