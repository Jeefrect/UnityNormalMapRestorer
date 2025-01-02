using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ColorDranek
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please drag and drop files onto this .exe file.");
                return;
            }

            foreach (string filePath in args)
            {
                ProcessImage(filePath);
            }
        }

        static void ProcessImage(string imagePath)
        {
            using (var image = Image.Load<Rgba32>(imagePath))
            {
                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        Rgba32 pixel = image[x, y];

                        pixel.R = pixel.A;
                        pixel.B = 255;
                        pixel.A = 255;

                        image[x, y] = pixel;
                    }
                }

                string outputPath = Path.Combine(Path.GetDirectoryName(imagePath)!, $"{Path.GetFileNameWithoutExtension(imagePath)}_Fixed.png");

                image.SaveAsPng(outputPath);
                Console.WriteLine($"Saved: {outputPath}");
            }
        }
    }
}
