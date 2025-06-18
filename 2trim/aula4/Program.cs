using System.Diagnostics;

var sw = Stopwatch.StartNew();
var initialMemory = GC.GetTotalMemory(true);

ImageProcessor.ProcessImages(500);

var finalMemory = GC.GetTotalMemory(true);
sw.Stop();

Console.WriteLine($"Memória inicial: {initialMemory / 1024.0:F2} KB");
Console.WriteLine($"Memória final: {finalMemory / 1024.0:F2} KB");
Console.WriteLine($"Diferença de memória: {(finalMemory - initialMemory) / 1024.0:F2} KB");
Console.WriteLine($"Coleções GC Gen0: {GC.CollectionCount(0)}");
Console.WriteLine($"Coleções GC Gen1: {GC.CollectionCount(1)}");
Console.WriteLine($"Coleções GC Gen2: {GC.CollectionCount(2)}");