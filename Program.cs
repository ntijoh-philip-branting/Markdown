using System;
using System.IO;
using static System.Console;
using System.Text.RegularExpressions;

partial class Program
{
    static void Main()
    {
        string path = @"./docs";
        string outputPath = @"./convertedHTML";
        string[] files = Directory.GetFiles(path);
        string parsedContent;
        Dictionary<string, string> metadata = [];


        if (Directory.Exists(outputPath))
        {
            Directory.Delete(outputPath, true);
            WriteLine($"Filepath '{outputPath}' was removed.");
        }

        if (!Directory.Exists(outputPath)){
            Directory.CreateDirectory(outputPath);
            WriteLine($"Filepath '{outputPath}' was created.");
        }

        foreach (string file in files)
        {
            WriteLine($"Processing file: {file}");

            // Example: Read the content of the file
            string content = File.ReadAllText(file);
            WriteLine($"File content: \n{content}");

            string FileNameWithoutExtension = Path.GetFileNameWithoutExtension(file); 
            string newFilePath = Path.Combine(outputPath, FileNameWithoutExtension + ".html"); 

            WriteLine($"New filepath: {newFilePath}");

            string[] lines = MyRegex().Split(content);

            if (lines[0] == "---")
            {
                int endMetadataIndex = -1;
                for (int i = 1; i < lines.Length; i++)
                {


                    if (lines[i] == "---")
                    {
                        endMetadataIndex = i;
                        break;
                    }

                var parts = lines[i].Split(new[] { ':' }, 2); 
                if (parts.Length == 2)
                {
                    string key = parts[0].Trim();
                    string value = parts[1].Trim(); 
                    metadata[key] = value; 
                }
            }

            if (endMetadataIndex != -1)
            {
                lines = lines.Skip(endMetadataIndex + 1).ToArray();
            }
        }

            
            WriteLine("Metadata:");
            foreach (var entry in metadata)
            {
                WriteLine($"{entry.Key}: {entry.Value}");
            }

            
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] == null || lines[i] == "\n" || lines[i] == ""){

                }
                else if (lines[i].StartsWith("## ")){
                    lines[i] = lines[i].Replace($"{lines[i]}", $"<h2>{lines[i][3..]}</h2>");
                } else if (lines[i].StartsWith("### ")){
                    lines[i] = lines[i].Replace($"{lines[i]}", $"<h3>{lines[i][4..]}</h3>");
                } else{
                    lines[i] = lines[i].Replace($"{lines[i]}", $"<p>{lines[i]}</p>");
                }

                    WriteLine($"Rad: {i} \n {lines[i]} \n");
                

                

            }


            parsedContent = string.Join("\n", lines);
            WriteLine("------------");
            WriteLine(parsedContent);

            File.WriteAllText(newFilePath, parsedContent);

            

            
        }
    }

    static string FilterComments(string content) {
            string pattern = @"<!--.*?-->";
            return Regex.Replace(content, pattern, string.Empty, RegexOptions.Singleline);
        }

    [GeneratedRegex(@"\r\n|\n|\r")]
    private static partial Regex MyRegex();
}