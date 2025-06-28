using System;
using System.Diagnostics;
using System;
using System.Buffers;

public struct PixelRGB
{
    public byte R, G, B;
    
    public PixelRGB(byte r, byte g, byte b)
    {
        R = r; G = g; B = b;
    }
  public static PixelRGB Average(PixelRGB a, PixelRGB b, PixelRGB c, PixelRGB d)
    {
        return new PixelRGB(
            (byte)((a.R + b.R + c.R + d.R) / 4),
            (byte)((a.G + b.G + c.G + d.G) / 4),
            (byte)((a.B + b.B + c.B + d.B) / 4)
        );
    }
}


public class ImageProcessor
{
    private const int IMAGE_WIDTH = 800;
    private const int IMAGE_HEIGHT = 600;
    private static int TOTAL_IMAGES;
    private static ArrayPool<PixelRGB> pool = ArrayPool<PixelRGB>.Shared;

    public static void ProcessImages(int totalImages)
    {
        TOTAL_IMAGES = totalImages;
        Console.WriteLine("Iniciando processamento de imagens (versão trivial)...");

        var stopwatch = Stopwatch.StartNew();
        int processedCount = 0;

        for (int imageIndex = 0; imageIndex < TOTAL_IMAGES; imageIndex++)
        {

            // Aplica filtro blur (cria novo array a cada operação)
            PixelRGB[] blurredImage = ApplyBlurFilter(GenerateSyntheticImage(imageIndex));

            // Simula salvamento
            SaveImage(blurredImage, $"processed_{imageIndex}.jpg");
            processedCount++;

            if (imageIndex % 50 == 0)
            {
                Console.WriteLine($"Processadas {imageIndex} imagens...");
            }
        }

        stopwatch.Stop();

        Console.WriteLine($"Processamento concluído!");
        Console.WriteLine($"Imagens processadas: {processedCount}");
        Console.WriteLine($"Tempo total: {stopwatch.ElapsedMilliseconds} ms");
        Console.WriteLine($"Tempo médio por imagem: {stopwatch.ElapsedMilliseconds / (double)processedCount:F2} ms");
    }

    private static PixelRGB[] GenerateSyntheticImage(int seed)
    {
        var image = pool.Rent(IMAGE_HEIGHT*IMAGE_WIDTH);
        var random = new Random(seed);

        for (int y = 0; y < IMAGE_HEIGHT; y++)
        {
            for (int x = 0; x < IMAGE_WIDTH; x++)
            {
                image[y*IMAGE_WIDTH + x] = new PixelRGB(
                    (byte)random.Next(256),
                    (byte)random.Next(256),
                    (byte)random.Next(256)
                );
            }
        }
        pool.Return(image);

        return image;
    }

    private static PixelRGB[] ApplyBlurFilter(PixelRGB[] original)
    {
        int height = IMAGE_HEIGHT;
        int width = IMAGE_WIDTH;

        // PROBLEMA: Nova alocação a cada chamada!
        var blurred = pool.Rent(IMAGE_WIDTH*IMAGE_HEIGHT);

        // Aplicação de blur 2x2 simples
        for (int y = 0; y < height - 1; y++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                blurred[y*IMAGE_WIDTH+x] = PixelRGB.Average(
                    original[y*(IMAGE_WIDTH)+ x],
                    original[y*(IMAGE_WIDTH)+ x + 1],
                    original[(y+1)*(IMAGE_WIDTH)+ x],
                    original[(y+1)*(IMAGE_WIDTH)+ x + 1]
                );
            }
        }

        pool.Return(blurred);
        pool.Return(original);

        return blurred;
    }

    private static void SaveImage(PixelRGB[] image, string filename)
    {
        Console.WriteLine($"Pseudo-salvamento de imagem: {filename}");
    }
}
