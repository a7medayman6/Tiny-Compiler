using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TinyCompiler
{
    public static class Tiny_Compiler
    {
        public static Scanner TinyScanner = new Scanner();

        public static List<string> Lexemes = new List<string>();
        public static List<Token> TokenStream = new List<Token>();
        public static Node Start_Compiling(string SourceCode) 
        {
            // Start the Scanner
            TinyScanner.StartScanning(SourceCode);

            // Start the parser Parser
            return SyntaxAnalyser.Parse(Tiny_Compiler.TokenStream);

            //  TO DO ..
            //  Sematic Analysis
        }
    }
}
