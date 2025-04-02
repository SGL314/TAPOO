using System;
using System.Threading;
Random rd = new Random();

Thread chef = new Thread(() => {
	
});

Thread garcom = new Thread(() => {
	while (true){
        int time = (int) (rd.NextDouble()*10+1);
        Thread.Sleep(time);
        // Console.WriteLine("" + time);
    }
});

garcom.Start();

Console.WriteLine("Thread finalizada. Continuando a execução...");



