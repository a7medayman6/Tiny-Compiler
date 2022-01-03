using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TinyCompiler
{
    public enum Token_Class
    {
        If, Int, Float, String, Read, Write, Repeat, Until, Elseif, Else, Then, Return, Endl,
        Semicolon, Comma, LParanthesis, RParanthesis, LCurlyBracket, RCurlyBracket,
        EqualOp, LessThanOp, GreaterThanOp, NotEqualOp, AssignmentOp, AndOp, OrOp,
        PlusOp, MinusOp, MultiplyOp, DivideOp,
        Idenifier, Constant, Comment, SingleQuote, DoubleQuote, Main, End
    }

    public class Scanner
    {
        public List<Token> Tokens = new List<Token>();
        Dictionary<string, Token_Class> ReservedWords = new Dictionary<string, Token_Class>();
        Dictionary<string, Token_Class> Operators = new Dictionary<string, Token_Class>();

        public Scanner()
        {
            // List of Reserved Words
            ReservedWords.Add("if", Token_Class.If);
            ReservedWords.Add("int", Token_Class.Int);
            ReservedWords.Add("float", Token_Class.Float);
            ReservedWords.Add("string", Token_Class.String);
            ReservedWords.Add("read", Token_Class.Read);
            ReservedWords.Add("write", Token_Class.Write);
            ReservedWords.Add("repeat", Token_Class.Repeat);
            ReservedWords.Add("until", Token_Class.Until);
            ReservedWords.Add("elseif", Token_Class.Elseif);
            ReservedWords.Add("else", Token_Class.Else);
            ReservedWords.Add("then", Token_Class.Then);
            ReservedWords.Add("return", Token_Class.Return);
            ReservedWords.Add("endl", Token_Class.Endl);
            ReservedWords.Add("main", Token_Class.Main);
            ReservedWords.Add("end", Token_Class.End);

            // List of Operators
            Operators.Add(";", Token_Class.Semicolon);
            Operators.Add(",", Token_Class.Comma);
            Operators.Add("(", Token_Class.LParanthesis);
            Operators.Add(")", Token_Class.RParanthesis);
            Operators.Add("{", Token_Class.LCurlyBracket);
            Operators.Add("}", Token_Class.RCurlyBracket);

            Operators.Add("=", Token_Class.EqualOp);
            Operators.Add("<", Token_Class.LessThanOp);
            Operators.Add(">", Token_Class.GreaterThanOp);
            Operators.Add("<>", Token_Class.NotEqualOp);

            Operators.Add(":=", Token_Class.AssignmentOp);

            Operators.Add("+", Token_Class.PlusOp);
            Operators.Add("-", Token_Class.MinusOp);
            Operators.Add("/", Token_Class.DivideOp);
            Operators.Add("*", Token_Class.MultiplyOp);
            Operators.Add("&&", Token_Class.AndOp);
            Operators.Add("||", Token_Class.OrOp);

            Operators.Add("'", Token_Class.SingleQuote);
            Operators.Add("\"", Token_Class.DoubleQuote);
        }

        public void StartScanning(string SourceCode)
        {

            const string whiteSpace = " \r\n\t";
            SourceCode += " ";
            for (int i = 0; i < SourceCode.Length - 1; i++)
            {
                string currentLexeme = "";
                char currentLetter = SourceCode[i];

                // is it a white space ?
                if (whiteSpace.Contains(currentLetter))
                    continue;

                if (currentLetter == '–')
                    currentLexeme += currentLetter;

                // is it the start of an identifier or reserverd word ? 
                else if (char.IsLetter(currentLetter))
                {
                    // iterate and build up the current lexeme untill currentLetter is a white space
                    while (char.IsLetterOrDigit(currentLetter))
                    {
                        currentLexeme += currentLetter;
                        currentLetter = SourceCode[++i];
                    }
                    i--;
                }
                // is it the start of a comment ?
                else if (currentLetter == '/' && SourceCode[i + 1] == '*')
                {
                    currentLetter = SourceCode[i];
                    currentLetter = SourceCode[++i];
                    currentLetter = SourceCode[++i];
                    while (i + 2 < SourceCode.Length && currentLetter != '*' && SourceCode[i + 1] != '/')
                    {
                        currentLetter = SourceCode[++i];
                    }
                    currentLetter = SourceCode[++i];

                }
                // is it the start of a string ? 
                else if (currentLetter == '\"')
                {
                    currentLexeme += currentLetter;
                    currentLetter = SourceCode[++i];
                    while (currentLetter != '\"' && i < SourceCode.Length - 2)
                    {
                        currentLexeme += currentLetter;
                        currentLetter = SourceCode[++i];
                    }
                    currentLexeme += currentLetter;
                }


                // is it the start of a constant ? 
                else if (char.IsDigit(currentLetter))
                {
                    while (char.IsLetterOrDigit(currentLetter) || currentLetter == '.' && i < SourceCode.Length - 2)
                    {
                        currentLexeme += currentLetter;
                        currentLetter = SourceCode[++i];
                    }
                    i--;
                }
                // is it the start of an operator ?
                else
                {
                    currentLexeme += currentLetter;

                    if (i != SourceCode.Length - 1)
                    {
                        // is it a double char operator ? 
                        // in other words, is the lexeme one of those "<>" ":=" "&&" "||" ?

                        char nextLetter = SourceCode[i + 1];
                        if ((currentLetter == '<' && nextLetter == '>') || (currentLetter == ':' && nextLetter == '=')
                            || (currentLetter == '&' && nextLetter == '&') || (currentLetter == '|' && nextLetter == '|'))
                        {
                            currentLexeme += nextLetter;
                            i++;
                        }
                    }

                }
                // call Find TokenClass with the current lexeme
                FindTokenClass(currentLexeme);
            }
            Tiny_Compiler.TokenStream = Tokens;

        }

        void FindTokenClass(string Lex)
        {
            if (Lex == "")
                return;
            Token Tok = new Token();
            Tok.lex = Lex;


            // is the lex a reserved word ? 
            if (ReservedWords.ContainsKey(Lex))
            {
                Tok.token_type = ReservedWords[Lex];
                Tokens.Add(Tok);
            }

            // is the lex an identifier ?
            else if (isIdentifier(Lex))
            {
                Tok.token_type = Token_Class.Idenifier;
                Tokens.Add(Tok);
            }
            // is the lex a constant ?
            else if (isConstant(Lex))
            {
                Tok.token_type = Token_Class.Constant;
                Tokens.Add(Tok);
            }
            // is the lex a string ?
            else if (isString(Lex))
            {
                Tok.token_type = Token_Class.String;
                Tokens.Add(Tok);
            }
            // is the lex an operator ?
            else if (Operators.ContainsKey(Lex))
            {
                Tok.token_type = Operators[Lex];
                Tokens.Add(Tok);
            }
            else if (isComment(Lex))
            {
                Tok.token_type = Token_Class.Comment;
                Tokens.Add(Tok);
            }
            else
            {
                Errors.Error_List.Add(Lex);
            }

        }

        private bool isString(string lex)
        {
            var exp = new Regex("^\".*\"$");
            return exp.IsMatch(lex);
        }

        bool isIdentifier(string lex)
        {
            // Check if the lex is an identifier or not.
            var exp = new Regex(@"^[a-zA-Z][a-zA-Z0-9]*$", RegexOptions.Compiled);
            return exp.IsMatch(lex);
        }
        bool isComment(string lex)
        {
            if (lex.Length < 4)
                return false;
            if (lex[0] == '/' && lex[1] == '*' && lex[lex.Length - 2] == '*' && lex[lex.Length - 1] == '/')
                return true;
            return false;
        }
        bool isConstant(string lex)
        {
            // Check if the lex is a constant (Number) or not.
            var exp = new Regex(@"^[0-9](.[0-9]+)*$", RegexOptions.Compiled);
            return exp.IsMatch(lex);
        }
    }
}
