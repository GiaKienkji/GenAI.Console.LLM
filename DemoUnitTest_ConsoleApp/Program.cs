using System.Text;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

public class Program
{
    static async Task Main()
    {
        string? calculatorPath = FindUpwardFile(AppContext.BaseDirectory, "Calculator.cs");

        if (calculatorPath == null)
        {
            Console.WriteLine("Calculator.cs not found");
            return;
        }

        string methodCode = await File.ReadAllTextAsync(calculatorPath, Encoding.UTF8);

        var prompt = $"""
        Write real xUnit tests for the following C# class.
        Do not use Moq.
        Just call the methods and assert the results.

        Code:
        {methodCode}
        """;

        var client = new HttpClient
        {
            Timeout = TimeSpan.FromMinutes(6)
        };

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", "lm-studio");

        var body = new
        {
            model = "qwen2.5-coder-7b-instruct",
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            max_tokens = 800,
            stream = false,
            temperature = 0.2
        };

        var json = System.Text.Json.JsonSerializer.Serialize(body);

        var resp = await client.PostAsync(
            "http://127.0.0.1:1234/v1/chat/completions",
            new StringContent(json, Encoding.UTF8, "application/json")
        );

        resp.EnsureSuccessStatusCode();

        var text = await resp.Content.ReadAsStringAsync();

        var raw = JObject.Parse(text)["choices"]![0]!["message"]!["content"]!.ToString();

        string unitTestCode = StripCodeFence(raw);

        string unitTestDir = Path.GetFullPath(
            Path.Combine(Path.GetDirectoryName(calculatorPath)!, "..", "UnitTest")
        );

        Directory.CreateDirectory(unitTestDir);

        string outFile = Path.Combine(unitTestDir, "UnitTest_Generated.cs");

        await File.WriteAllTextAsync(outFile, unitTestCode, Encoding.UTF8);

        Console.WriteLine($"Saved: {outFile}");
    }

    static string? FindUpwardFile(string start, string name, int max = 8)
    {
        var d = new DirectoryInfo(start);

        for (int i = 0; i < max && d != null; i++, d = d.Parent)
        {
            string c = Path.Combine(d.FullName, name);

            if (File.Exists(c))
                return c;
        }

        return null;
    }

    static string StripCodeFence(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return s;

        int a = s.IndexOf("```");

        if (a >= 0)
        {
            int b = s.IndexOf("```", a + 3);

            if (b > a)
                s = s.Substring(a + 3, b - a - 3);

            s = s.Replace("csharp", "").Replace("cs", "");
        }

        return s.Trim();
    }
}