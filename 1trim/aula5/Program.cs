using System;
using System.Threading;
namespace Fil{
    class Maine{
        static void Main(string[] args){
            Object lockConsole = new object(), lockGarfos = new object();

            List<Filosofo> filosofos = new List<Filosofo>();
            List<Garfo> garfos = new List<Garfo>();

            for (int i=0;i<5;i++){
                filosofos.Add(new Filosofo(i+1,lockGarfos,garfos));
                garfos.Add(new Garfo(i+1));
            }

            for (int i=0;i<5;i++){
                var a = Task.Run(filosofos[i].viver);
            }

            void nothing(){
                while(true){}
            }
            var k = Task.Run(nothing);
            k.Wait();

            void WriteLine(string str){
                string itens = "";
                Console.WriteLine(str+" | "+itens);

            }

            WriteLine("Thread finalizada. Continuando a execução...");
        }
    }
}






