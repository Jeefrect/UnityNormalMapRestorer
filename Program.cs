using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace ColorDranek
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please drag and drop files or a folder onto this .exe file.");
                return;
            }

            var tasks = args.Select(async path =>
            {
                if (Directory.Exists(path))
                {
                    await ProcessFolderAsync(path);
                }
                else if (File.Exists(path))
                {
                    string fileName = Path.GetFileName(path).ToLower();

                    if (fileName.Contains("nm") || fileName.Contains("normal") || fileName.Contains("norm"))
                    {
                        string outputPath = Path.Combine(Path.GetDirectoryName(path)!,
                            $"{Path.GetFileNameWithoutExtension(path)}_Fixed.png");

                        try
                        {
                            await ProcessImageAsync(path, outputPath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to process {path}: {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid path: {path}");
                }
            });

            await Task.WhenAll(tasks);
        }

        static async Task ProcessFolderAsync(string folderPath)
        {
            string fixedFolderPath = folderPath + "_fixed";

            var tasks = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .Where(file => {
                    string fileName = Path.GetFileName(file).ToLower();
                    return fileName.Contains("nm") || fileName.Contains("normal") || fileName.Contains("norm");
                })
                .Select(async file =>
                {
                    string relativePath = Path.GetRelativePath(folderPath, file);
                    string outputPath = Path.Combine(fixedFolderPath, Path.ChangeExtension(relativePath, ".png"));
                    Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

                    try
                    {
                        await ProcessImageAsync(file, outputPath);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to process {file}: {ex.Message}");
                    }
                });

            await Task.WhenAll(tasks);
        }

        static async Task ProcessImageAsync(string inputPath, string outputPath)
        {
            using (var image = await Image.LoadAsync<Rgba32>(inputPath))
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

                await image.SaveAsPngAsync(outputPath);
                Console.WriteLine($"Saved: {outputPath}");
            }
        }
    }
}
