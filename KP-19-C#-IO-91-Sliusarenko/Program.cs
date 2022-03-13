using System.Collections.Generic;
using System.IO;

namespace KR
{
    public class Program
    {
        private static void Main(string[] args)
        {
            // *Don't even try to run it*
            const string filenameIncome = "KP-19-C#-IO-91-Sliusarenko.c";
            const string filenameOutcome = "KP-19-C#-IO-91-Sliusarenko.asm";

            string codeFromFile =
                File.ReadAllText(
                    $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent}\\{filenameIncome}");

            Lexer lexer = new Lexer(codeFromFile);
            Dictionary<string, List<object>> tokens = lexer.ExtractTokens();

            List<object> tokenVals = tokens["tokenVals"];
            List<object> tokensExpressedInEnums = tokens["tokensExpressedInEnums"];

            Parser parser = new Parser(tokensExpressedInEnums, tokenVals);

            var tree = parser.Parse();

            Generator generator = new Generator(tree);
            File.WriteAllText($"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent}{filenameOutcome}",
                generator.Generate());
        }
    }
}
