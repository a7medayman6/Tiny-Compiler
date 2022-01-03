using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinyCompiler
{
    public partial class TinyCompilerForm : Form
    {
        public TinyCompilerForm()
        {
            InitializeComponent();
        }

        private void Compile_btn_Click(object sender, EventArgs e)
        {
            errorList_t.Clear();
            parserErrors_t.Clear();

            string Code = tinyCode_t.Text;
         
            // START COMPILING THE SOURCE CODE
            // Returns the root of t he parsing tree
            Node root = Tiny_Compiler.Start_Compiling(Code);

            // Print the tokens ( the scanner output )
            PrintTokens();

            // Add the parse tree to the tree view in GUI
            ParseTree.Nodes.Add(SyntaxAnalyser.PrintParseTree(root));

            // Print the Scanner Errors
            PrintErrors();

            // Expand the tree nodes
            ParseTree.ExpandAll();
        }

        private void Clear_btn_Click(object sender, EventArgs e)
        {
            tokens_dgv.Rows.Clear();
            errorList_t.Clear();
            parserErrors_t.Clear();

            Errors.Error_List.Clear();
            Errors.ParserError_List.Clear();

            Tiny_Compiler.TokenStream.Clear();
            ParseTree.Nodes.Clear();
        }

        void PrintErrors()
        {
            for (int i = 0; i < Errors.Error_List.Count; i++)
            {
                errorList_t.Text += Errors.Error_List[i];
                errorList_t.Text += "\r\n";
            }
            for (int i = 0; i < Errors.ParserError_List.Count; i++)
            {
                parserErrors_t.Text += Errors.ParserError_List[i];
                parserErrors_t.Text += "\r\n";
                parserErrors_t.Text += "---------------------------------------------------------- \r\n";
            }
        }
        void PrintTokens()
        {
            for (int i = 0; i < Tiny_Compiler.TinyScanner.Tokens.Count; i++)
            {
                tokens_dgv.Rows.Add(Tiny_Compiler.TinyScanner.Tokens.ElementAt(i).lex, Tiny_Compiler.TinyScanner.Tokens.ElementAt(i).token_type);
            }
        }
    }
}
