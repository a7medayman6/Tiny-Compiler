using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TinyCompiler
{
    public class SyntaxAnalyser
    {
        public static List<String> tempParameters = new List<String>();
        public static Dictionary<string, int> FunctionsData = new Dictionary<string, int>();

        static int TokenIndex = 0;
        static int TokenStreamLength;
        static List<Token> TokenStream;
        public static Node root;
        public static Node Parse(List<Token> Tokens)
        {
            TokenIndex = 0;
            TokenStream = Tokens;
            TokenStreamLength = TokenStream.Count;

            root = Program();

            return root;
        }

        /* Program → ProgramFunctionStatement Main_Function */
        private static Node Program()
        {
            Node node = new Node("Program");

            node.children.Add(Program_Function_Statement());

            if (TokenIndex < TokenStreamLength)
                node.children.Add(Main_Function());
            else
                Errors.ParserError_List.Add("missing main function");

            return node;
        }
        /* Main_Function → Data_Type main ( ) Function_Body */
        private static Node Main_Function()
        {
            // create the node
            Node node = new Node("Main_Function");

            List<String> errors = new List<string>();

            // Data type
            int currentTokenIndex = TokenIndex;

            Node datat = Data_Type();

            if (datat != null)
                node.children.Add(datat);
            else
            {
                TokenIndex = currentTokenIndex;
                errors.Add("Missing Datatype in Main Function.");
            }

            // main
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Main)
            {
                node.children.Add(match(Token_Class.Main));

                // (
                if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.LParanthesis)
                    node.children.Add(match(Token_Class.LParanthesis));
                else
                    errors.Add("Missing Left Pranthesis in Main Function.");

                // )
                if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.RParanthesis)
                    node.children.Add(match(Token_Class.RParanthesis));
                else
                    errors.Add("Missing Right Pranthesis in Main Function.");

                // Funciton body
                currentTokenIndex = TokenIndex;

                Node funBody = Function_Body();

                if (funBody != null)
                    node.children.Add(funBody);
                else
                {
                    TokenIndex = currentTokenIndex;
                    errors.Add("Missing Function Body of The Main Function.");
                }

                // add the errors only if the main derictive exists
                if (errors.Count != 0)
                    Errors.ParserError_List.AddRange(errors);

                return node;
            }
            else
                return null;
        }

        /* ProgramFunctionStatement → Function_Statement ProgramFunctionStatement | ɛ */
        private static Node Program_Function_Statement()
        {
            // create the node
            Node node = new Node("Program_Function_Statement");

            int currentTokenIndex = TokenIndex;

            Node fnSt = Function_Statement();

            // the ɛ choice
            // if there isn't any function statements, reset the tokenIndex,
            if (fnSt == null)
                TokenIndex = currentTokenIndex;
            else    // Function_Statement ProgramFunctionStatement
            {
                // add the function statement to the Program_Function_Statement children
                node.children.Add(fnSt);
                // if there still tokens left
                if (TokenIndex < TokenStreamLength)
                    node.children.Add(Program_Function_Statement());
            }

            return node;
        }
        /* Function_Statement → Function_Declaration Function_Body */
        private static Node Function_Statement()
        {
            // create the node
            Node node = new Node("Function_Statement");

            Node fnDec = Function_Declaration();

            // if the fun declaration doesn't exist, the function statement is not valid; return null
            if (fnDec == null)
                return null;
            else // fn declration exist, add it and continue
            {
                node.children.Add(fnDec);
                // if there is tokens left, continue 
                if (TokenIndex < TokenStreamLength)
                    node.children.Add(Function_Body());
                else // no more tokens, so no function body
                    Errors.ParserError_List.Add("Function body is missing.");
            }
            return node;
        }
        /* Function_Declaration → Data_Type Function_Name ( Function_Parameters ) */
        private static Node Function_Declaration()
        {
            // create the node
            Node node = new Node("Function_Declaration");

            // Data_Type
            Node datat = Data_Type();

            // if it's the main function, return null and the Program() function will deal with it
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Main)
                return null;

            // if there is no data type; it might be a main function
            if (datat == null)
            {
                if (TokenIndex >= TokenStreamLength)
                    Errors.ParserError_List.Add("Error in Fucntion Declaration.");

                Errors.ParserError_List.Add("Missing Data Type in Function Declaration.");
            }
            else
                node.children.Add(datat);

            // Function_Name
            Node fnName = Function_Name();

            // if no fnName; error
            if (fnName == null)
            {
                Errors.ParserError_List.Add("Missing Function Name in Function Declaration.");
                tempParameters.Add("0");
            }
            else // if it exists, add it
            {
                node.children.Add(fnName);
                tempParameters.Add(fnName.children[0].Name);
            }

            // Pranthesis and Parameters

            // if there are more tokens
            if (TokenIndex < TokenStreamLength)
            {
                // if the curr token is l pranthesis (
                if (TokenStream[TokenIndex].token_type == Token_Class.LParanthesis)
                {
                    // ( is a terminal; match it
                    node.children.Add(match(Token_Class.LParanthesis));
                    // add the function parameters, if any.
                    node.children.Add(Function_Parameters());

                    // find the r pranthesis
                    if (TokenStream[TokenIndex].token_type == Token_Class.RParanthesis)
                    {
                        // ( is a terminal; match it
                        node.children.Add(match(Token_Class.RParanthesis));
                        return node;
                    }
                    else
                        Errors.ParserError_List.Add("Missing Right Paranthesis in Function Declaration");

                }
                else
                {
                    Errors.ParserError_List.Add("Missing Left Paranthesis in Function Declaration");

                    // add the function parameters, if any.
                    node.children.Add(Function_Parameters());

                    // find the r pranthesis
                    if (TokenStream[TokenIndex].token_type == Token_Class.RParanthesis)
                    {
                        // ( is a terminal; match it
                        node.children.Add(match(Token_Class.RParanthesis));
                        return node;
                    }
                    else
                        Errors.ParserError_List.Add("Missing Right Paranthesis in Function Declaration");
                }
            }
            else
                Errors.ParserError_List.Add("Error in Function Declaration.");

            return node;
        }
        /* Data_Type → int | float | string */
        private static Node Data_Type()
        {
            // create the node
            Node node = new Node("Data_Type");

            // if there is a current token and it is int, float, or string
            if (TokenIndex < TokenStreamLength)
                // int
                if (TokenStream[TokenIndex].token_type == Token_Class.Int)
                {
                    node.children.Add(match(Token_Class.Int));
                    return node;
                }
                // float
                else if (TokenStream[TokenIndex].token_type == Token_Class.Float)
                {
                    node.children.Add(match(Token_Class.Float));
                    return node;
                }
                // string
                else if (TokenStream[TokenIndex].token_type == Token_Class.String)
                {
                    node.children.Add(match(Token_Class.String));
                    return node;

                }


            // if didn't return node; error in Data_Type, return null
            Errors.ParserError_List.Add("Error in the data type.");

            return null;
        }
        /* Function_Name → identifier */
        private static Node Function_Name()
        {
            // create the node
            Node node = new Node("Function_Name");

            // if the curr token is and identifier
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Idenifier)
            {
                // function name is an identifier, terminal; match it
                node.children.Add(match(Token_Class.Idenifier));

                return node;
            }

            // if didn't find the function name (the identifier)
            Errors.ParserError_List.Add("Error in Function Name.");

            return null;
        }
        /* Function_Parameters → Data_Type Identifier More_Parameters | ɛ */
        private static Node Function_Parameters()
        {
            Node node = new Node("Function_Parameters");

            int currentTokenIndex = TokenIndex;

            // Data_Type
            Node datat = Data_Type();

            if (datat != null)
            {
                // add it and continue 
                tempParameters.Add("1");
                node.children.Add(datat);

                if (TokenIndex < TokenStream.Count)
                {
                    // Identifier
                    // if the curr token is an identifier (parameter name); match it
                    if (TokenStream[TokenIndex].token_type == Token_Class.Idenifier)
                        node.children.Add(match(Token_Class.Idenifier));

                    else
                        Errors.ParserError_List.Add("Missing Parameter Identifier in Function Declaration Parameters");
                    // More_Parameters
                    node.children.Add(More_Function_Parameters());
                    return node;
                }
                else
                    Errors.ParserError_List.Add("Error in function parameters, data type exists, but no identefier.");
            }
            // Identifier
            // if there is no Data_Type, but the curr token is an identifier; add the error and the parameter, and continue
            else if (TokenIndex < TokenStream.Count && TokenStream[TokenIndex].token_type == Token_Class.Idenifier)
            {
                // add the error
                Errors.ParserError_List.Add("Missing Data Type in Function Declaration Parameters");

                // add the parameter, and continue   
                node.children.Add(match(Token_Class.Idenifier));
                tempParameters.Add("1");
                // More_Parameters
                node.children.Add(More_Function_Parameters());

                return node;

            }
            // if there are no more tokens; error 
            else if (TokenIndex >= TokenStream.Count)
                Errors.ParserError_List.Add("Error in Function Parameters.");


            // reset the token index; the ɛ state
            TokenIndex = currentTokenIndex;
            return node;
        }
        /* More_Function_Parameters → , Data_Type Identifier More_Function_Parameters | ɛ */
        private static Node More_Function_Parameters(bool declaration = true)
        {
            Node node = new Node("More_Function_Parameters");

            int currentTokenIndex = TokenIndex;

            if (TokenIndex < TokenStreamLength)
            {
                bool moreParameters = false;
                bool missingComma = false;
                if (TokenStream[TokenIndex].token_type != Token_Class.Comma)
                    missingComma = true;

                // if the curr token is a comma; add it and continue
                if (TokenStream[TokenIndex].token_type == Token_Class.Comma)
                {
                    node.children.Add(match(Token_Class.Comma));
                    tempParameters.Add("1");
                    moreParameters = true;
                }
                else if (TokenStream[TokenIndex].token_type == Token_Class.Int || TokenStream[TokenIndex].token_type == Token_Class.Float
                        || TokenStream[TokenIndex].token_type == Token_Class.String)
                {
                    tempParameters.Add("1");
                    moreParameters = true;
                }

                if (moreParameters)
                {
                    if (missingComma)
                        Errors.ParserError_List.Add("Missing Comma in Function Declaration Parameters.");

                    tempParameters.Add("1");

                    if (declaration)
                    {
                        // find the Data_Type
                        Node datat = Data_Type();
                        if (datat == null)
                            Errors.ParserError_List.Add("Missing Data type in Function Declaration Parameters.");
                        else
                            node.children.Add(datat);
                    }

                    // if the curr token is an identifier; match and add it, else add an error
                    if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Idenifier)
                        node.children.Add(match(Token_Class.Idenifier));
                    else
                        Errors.ParserError_List.Add("Missing Parameter Identefier in Function Declaration Parameters");

                    // recurse to find if there are more parameters
                    node.children.Add(More_Function_Parameters());
                    return node;
                }
                // the ɛ state
                else
                    return null;

            }
            // function declaration not complete; error
            Errors.ParserError_List.Add("Error in Function Declaration.");
            // reset the token index
            TokenIndex = currentTokenIndex;
            return node;

        }
        /* Function_Body → { Statements Return_Statement } */
        private static Node Function_Body()
        {
            Node node = new Node("Function_Body");

            if (TokenIndex < TokenStreamLength)
            {
                // match {
                if (TokenStream[TokenIndex].token_type == Token_Class.LCurlyBracket)
                    node.children.Add(match(Token_Class.LCurlyBracket));
                else
                    Errors.ParserError_List.Add("Missing Left Curly Bracket at the begining of the Function Body.");

                // match statements
                node.children.Add(Statements());

                // match return statement
                if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Return)
                    node.children.Add(Retrun_Statement());
                else
                    Errors.ParserError_List.Add("Missing Return in Function Body.");

                // match }
                if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.RCurlyBracket)
                    node.children.Add(match(Token_Class.RCurlyBracket));
                else
                    Errors.ParserError_List.Add("Missing Right Curly Bracket in the ending of Function Body.");
            }
            else
                Errors.ParserError_List.Add("Error in Function Body.");

            return node;
        }
        /* Return_Statement → return Expression ; */
        private static Node Retrun_Statement()
        {
            Node node = new Node("Return_Statement");

            // return
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Return)
                node.children.Add(match(Token_Class.Return));
            else
                return null;

            // Expression
            int currentTokenIndex = TokenIndex;

            Node expr = Expression();

            if (expr != null)
                node.children.Add(expr);
            else
            {
                TokenIndex = currentTokenIndex;
                Errors.ParserError_List.Add("Missing expression in Return Statement.");
            }

            // semicolon ;
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Semicolon)
                node.children.Add(match(Token_Class.Semicolon));
            else
                Errors.ParserError_List.Add("Missing Semicolon in Return Statement.");

            return node;
        }

        /* Statements → Statement Statements | ɛ */
        private static Node Statements()
        {
            Node node = new Node("Statements");

            int currentTokenIndex = TokenIndex;

            Node state = Statement();

            if (state != null && state.children[0] != null)
            {
                node.children.Add(state);

                node.children.Add(Statements());
                return node;
            }
            // the ɛ state
            TokenIndex = currentTokenIndex;

            return null;
        }
        /*  Statement → Function_Call | Assignment_Statement | Declaration_Statement | Write_Statement | 
            Read_Statement | If_Statement | Repeat_Statement */
        private static Node Statement()
        {
            Node node = new Node("Statement");

            if (TokenIndex < TokenStreamLength)
            {
                // if the curr token is an identifier; then it might be function call or assignment statement
                if (TokenStream[TokenIndex].token_type == Token_Class.Idenifier)
                {
                    // if the next token is left pranthesis (; it's a function call
                    if (TokenIndex + 1 < TokenStreamLength && TokenStream[TokenIndex + 1].token_type == Token_Class.LParanthesis)
                    {
                        node.children.Add(Function_Call(false));
                        return node;
                    }
                    // if the next token is = or boleanop, it's assignment sattement
                    else if (TokenIndex + 1 < TokenStreamLength && TokenStream[TokenIndex + 1].token_type == Token_Class.AssignmentOp)
                    {
                        node.children.Add(Assignment_Statement());
                        return node;
                    }
                }
                // if the curr token is a Data_Type
                else if (TokenStream[TokenIndex].token_type == Token_Class.Int || TokenStream[TokenIndex].token_type == Token_Class.Float
                    || TokenStream[TokenIndex].token_type == Token_Class.String)
                {
                    node.children.Add(Declaration_Statement());
                    //test later
                    if (node == null)
                        TokenIndex++;

                    return node;
                }
                // if the curr token is write
                else if (TokenStream[TokenIndex].token_type == Token_Class.Write)
                {
                    node.children.Add(Write_Statement());
                    return node;
                }
                // if the curr token is read
                else if (TokenStream[TokenIndex].token_type == Token_Class.Read)
                {
                    node.children.Add(Read_Statement());
                    return node;
                }
                // if the curr token is if
                else if (TokenStream[TokenIndex].token_type == Token_Class.If)
                {
                    node.children.Add(If_Statement());
                    return node;
                }
                // if the curr token is repeat
                else if (TokenStream[TokenIndex].token_type == Token_Class.Repeat)
                {
                    node.children.Add(Repeat_Statement());
                    return node;
                }
                else
                    return null;
            }
            return null;
        }



        /* Identifier ( Parameters ) ; */
        private static Node Function_Call(bool isTerm)
        {
            // create the node
            Node node = new Node("Function_Call");

            if (TokenIndex < TokenStreamLength)
            {
                // check if the current token is an identifier 
                if (TokenStream[TokenIndex].token_type == Token_Class.Idenifier)
                    node.children.Add(match(Token_Class.Idenifier));
                else
                    Errors.ParserError_List.Add("Missing Function Name in Function Call.");

                // check for l pranthesis (
                if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.LParanthesis)
                {
                    node.children.Add(match(Token_Class.LParanthesis));
                    // get the parameters
                    node.children.Add(Parameters());
                }
                else
                {
                    Errors.ParserError_List.Add("Missing Left Pranthesis in Function Call.");
                }

                // check for r pranthesis
                if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.RParanthesis)
                    node.children.Add(match(Token_Class.RParanthesis));
                else
                    Errors.ParserError_List.Add("Missing Right Pranthesis in Function Call.");

                // no semicolon checking if the function call is a term
                if (!isTerm)
                {
                    // check for semicolon
                    if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Semicolon)
                        node.children.Add(match(Token_Class.Semicolon));
                    else
                        Errors.ParserError_List.Add("Missing Semicolon in Function Call.");
                }

                return node;
            }
            else
                Errors.ParserError_List.Add("Missing Function Name in Function Call.");

            return null;
        }
        /* Parameters → Expression More_Parameters | ɛ */
        private static Node Parameters()
        {
            // create the node
            Node node = new Node("Parameters");

            // check if the parameter is an expression
            int currentTokenIndex = TokenIndex;

            Node expr = Expression();
            if (expr != null)
            {
                node.children.Add(expr);
                node.children.Add(More_Parameters());
                return node;
            }

            return null;

        }
        /* More_Parameters → , Expression More_Parameters | ɛ */
        private static Node More_Parameters()
        {
            // create the node
            Node node = new Node("More_Parameters");

            // check if the curr token is comma
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Comma)
                node.children.Add(match(Token_Class.Comma));
            // ɛ state
            else
                return null;

            // check if the parameter is an expression
            int currentTokenIndex = TokenIndex;

            Node expr = Expression();
            if (expr != null)
                node.children.Add(expr);
            else
                Errors.ParserError_List.Add("Missing Expression in Function Call Parameters after a comma.");

            node.children.Add(More_Parameters());
            return node;
        }

        /* Assignment_Statement → := Expression */
        private static Node Assignment_Statement(bool isDecleared = false)
        {
            Node node = new Node("Assignment_Statement");

            if (isDecleared)
            {
                if (TokenIndex >= TokenStreamLength || TokenStream[TokenIndex].token_type != Token_Class.AssignmentOp)
                    return null;
            }
            else
            {
                if (TokenIndex + 1 >= TokenStreamLength || TokenStream[TokenIndex].token_type != Token_Class.Idenifier
                    || TokenStream[TokenIndex + 1].token_type != Token_Class.AssignmentOp)
                    return null;
            }

            // if it started with an identifier
            if (!isDecleared)
            {
                if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Idenifier)
                    node.children.Add(match(Token_Class.Idenifier));
                else
                    Errors.ParserError_List.Add("Missing Variable Name in Assignment Statement.");
            }

            // check for assignment op
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.AssignmentOp)
                node.children.Add(match(Token_Class.AssignmentOp));
            else
                Errors.ParserError_List.Add("Missing Assignment Operator in Assignment Statement.");

            // get the expression
            node.children.Add(Expression());


            // if it's not in a declaration, check for semicolon
            if (!isDecleared)
            {
                if (TokenStream[TokenIndex].token_type == Token_Class.Semicolon)
                    node.children.Add(match(Token_Class.Semicolon));
                else
                    Errors.ParserError_List.Add("Missing Semicolon");
            }

            return node;
        }



        /* If_Statement → if Condition_Statement then Statements Other_Conditions */
        private static Node If_Statement()
        {
            Node node = new Node("If_Statement");

            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.If)
                node.children.Add(match(Token_Class.If));
            else
                Errors.ParserError_List.Add("Missing the word if in If Statement.");

            int currentTokenIndex = TokenIndex;

            Node condState = Condition_Statement();

            if (condState.children[0] != null)
            {
                node.children.Add(condState);
            }
            else
            {
                TokenIndex = currentTokenIndex;
                Errors.ParserError_List.Add("Missing Condition Statement.");
            }

            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Then)
                node.children.Add(match(Token_Class.Then));
            else
                Errors.ParserError_List.Add("Missing the word then in If Statement.");

            node.children.Add(Statements());

            currentTokenIndex = TokenIndex;

            Node otherCondState = Other_Conditions();

            if (otherCondState != null)
                node.children.Add(otherCondState);
            else
                TokenIndex = currentTokenIndex;

            return node;

        }

        /* Condition_statement → Condition */
        private static Node Condition_Statement()
        {
            Node node = new Node("Condition_Statement");
            node.children.Add(Condition());
            return node;
        }

        /* Condition → identifier Condition_Operator Term More_Conditions */
        private static Node Condition()
        {
            Node node = new Node("Condition");

            // Identifier
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Idenifier)
                node.children.Add(match(Token_Class.Idenifier));
            else
                Errors.ParserError_List.Add("Missing Idenifier in Condition.");

            // Condition_Operator
            int currentTokenIndex = TokenIndex;

            Node condOp = Condition_Operator();

            if (condOp != null)
                node.children.Add(condOp);
            else
            {
                TokenIndex = currentTokenIndex;
                Errors.ParserError_List.Add("Missing Condition Operator in Condition.");
            }

            // Term 
            currentTokenIndex = TokenIndex;

            Node termNode = Term();

            if (termNode != null)
                node.children.Add(termNode);
            else
            {
                TokenIndex = currentTokenIndex;
                Errors.ParserError_List.Add("Missing Term in Condition.");
            }

            node.children.Add(More_Conditions());


            return node;

        }
        /* More_Conditions → Boolean_Operator condition | ɛ */
        private static Node More_Conditions()
        {
            Node node = new Node("More_Conditions");

            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.AndOp)
            {
                node.children.Add(match(Token_Class.AndOp));
                node.children.Add(Condition());
                return node;

            }
            else if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.OrOp)
            {
                node.children.Add(match(Token_Class.OrOp));
                node.children.Add(Condition());
                return node;
            }

            return null;
        }

        /* Term → number | identifier | Function_Call */
        private static Node Term()
        {
            Node node = new Node("Term");
            // check for constant
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Constant)
            {
                node.children.Add(match(Token_Class.Constant));
                return node;
            }
            else if (TokenStream[TokenIndex].token_type == Token_Class.Idenifier)
            {
                // check for function call
                if (TokenIndex + 1 < TokenStreamLength && (TokenStream[TokenIndex + 1].token_type == Token_Class.LParanthesis))
                    node.children.Add(Function_Call(true));
                // check for identifier
                else if (TokenIndex < TokenStreamLength)
                    node.children.Add(match(Token_Class.Idenifier));
                return node;
            }
            return null;
        }
        /* Condition_Operator → less_than | greater_than | not_equal | equal */
        private static Node Condition_Operator()
        {
            Node node = new Node("Condition_Operator");

            // check if <
            if (TokenStream[TokenIndex].token_type == Token_Class.LessThanOp)
            {
                node.children.Add(match(Token_Class.LessThanOp));
                return node;
            }
            // check if >
            else if (TokenStream[TokenIndex].token_type == Token_Class.GreaterThanOp)
            {
                node.children.Add(match(Token_Class.GreaterThanOp));
                return node;
            }
            // check if !=
            else if (TokenStream[TokenIndex].token_type == Token_Class.NotEqualOp)
            {
                node.children.Add(match(Token_Class.NotEqualOp));
                return node;
            }
            // checked if =
            else if (TokenStream[TokenIndex].token_type == Token_Class.EqualOp)
            {
                node.children.Add(match(Token_Class.EqualOp));
                return node;

            }

            // if nothing matched
            return null;
        }

        /* Other_Conditions → Else_if_Statement | Else_statement | end */
        private static Node Other_Conditions()
        {
            Node node = new Node("Other_Conditions");

            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Elseif)
                node.children.Add(Else_If_Statement());
            else if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Else)
                node.children.Add(Else_Statement());
            else if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.End)
                node.children.Add(match(Token_Class.End));
            else
                Errors.ParserError_List.Add("Missing end in Condition Statement.");

            return node;
        }

        /* Else_if_Statement → elseif Condition_statement then Statements other_conditions */
        private static Node Else_If_Statement()
        {
            Node node = new Node("Else_If_Statement");

            // elseif
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Elseif)
                node.children.Add(match(Token_Class.Elseif));
            else
                return null;


            // Condition statement
            int currentTokenIndex = TokenIndex;

            Node condState = Condition_Statement();

            if (condState != null)
                node.children.Add(condState);
            else
            {
                TokenIndex = currentTokenIndex;
                Errors.ParserError_List.Add("Missing Condition Statement in Else If Statement.");
            }

            // then
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Then)
                node.children.Add(match(Token_Class.Then));
            else
                Errors.ParserError_List.Add("Missing Then in Else If Statement.");

            // Statements
            currentTokenIndex = TokenIndex;

            Node states = Statements();

            if (states != null)
                node.children.Add(states);
            else
            {
                TokenIndex = currentTokenIndex;
                Errors.ParserError_List.Add("Missing Statements in Else If Statement.");
            }

            // Other Conditions
            currentTokenIndex = TokenIndex;

            Node otherConds = Other_Conditions();

            if (otherConds != null)
                node.children.Add(otherConds);
            else
            {
                TokenIndex = currentTokenIndex;
                Errors.ParserError_List.Add("Missing Statements in Else If Statement.");
            }

            return node;

        }
        /* Else_statement → else Statements end */
        private static Node Else_Statement()
        {
            Node node = new Node("Else_Statement");

            // else
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Else)
                node.children.Add(match(Token_Class.Else));
            else
                return null;

            // Statements
            int currentTokenIndex = TokenIndex;

            Node states = Statements();

            if (states != null)
                node.children.Add(states);
            else
            {
                TokenIndex = currentTokenIndex;
                Errors.ParserError_List.Add("Missing Statements in Else Statement.");
            }

            // end
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.End)
                node.children.Add(match(Token_Class.End));
            else
                Errors.ParserError_List.Add("Missing End in Else Statement.");

            return node;
        }

        /* Repeat_Statement → repeat Statements untill Condition_statement */
        private static Node Repeat_Statement()
        {
            Node node = new Node("Repeat_Statement");

            // repeat
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Repeat)
                node.children.Add(match(Token_Class.Repeat));
            else
                return null;

            // Statements
            int currentTokenIndex = TokenIndex;

            Node states = Statements();

            if (states != null)
                node.children.Add(states);
            else
            {
                TokenIndex = currentTokenIndex;
                Errors.ParserError_List.Add("Missing Statements in Repeat Statement.");
            }

            // untill
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Until)
                node.children.Add(match(Token_Class.Until));
            else
                Errors.ParserError_List.Add("Missing untill in Repeat Statement.");

            // Condition Statement
            currentTokenIndex = TokenIndex;

            Node condState = Condition_Statement();

            if (condState != null)
                node.children.Add(condState);
            else
            {
                TokenIndex = currentTokenIndex;
                Errors.ParserError_List.Add("Missing Condition Statement in Repeat Statement.");
            }

            return node;
        }

        /* Read_Statement → read identifier ; */
        private static Node Read_Statement()
        {
            Node node = new Node("Read_Statement");

            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Read)
                node.children.Add(match(Token_Class.Read));
            else
                return null;

            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Idenifier)
                node.children.Add(match(Token_Class.Idenifier));
            else
                Errors.ParserError_List.Add("Missing Identifier in Read Statement.");

            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Semicolon)
                node.children.Add(match(Token_Class.Semicolon));
            else
                Errors.ParserError_List.Add("Missing Semicolon in Read Statement.");

            return node;
        }

        /* Write_Statement → write Write_Rest ; */
        private static Node Write_Statement()
        {
            Node node = new Node("Write_Statement");

            // write
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Write)
                node.children.Add(match(Token_Class.Write));
            else
                return null;

            // Write_Rest
            int currentTokenIndex = TokenIndex;

            Node writeRest = Write_Rest();

            if (writeRest != null)
                node.children.Add(writeRest);
            else
            {
                TokenIndex = currentTokenIndex;
            }

            // semicolon
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Semicolon)
                node.children.Add(match(Token_Class.Semicolon));
            else
                Errors.ParserError_List.Add("Missing Semicolon in Write Statement.");

            return node;
        }

        /* Write_Rest → Expression | endl */
        private static Node Write_Rest()
        {
            Node node = new Node("Write_Rest");

            // endl
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Endl)
            {
                node.children.Add(match(Token_Class.Endl));
                return node;
            }

            // expression
            int currentTokenIndex = TokenIndex;

            Node expr = Expression();

            if (expr != null)
            {
                node.children.Add(expr);
                return node;
            }
            else
            {
                TokenIndex = currentTokenIndex;
                Errors.ParserError_List.Add("Missing Expression or endl in Write Statement.");
                return null;
            }
        }

        /* Declaration_Statement → Data_Type Identifier Declare_Rest1 Declare_Rest2 ; */
        private static Node Declaration_Statement()
        {
            Node node = new Node("Declaration_Statement");

            // Data type
            int currentTokenIndex = TokenIndex;

            Node datat = Data_Type();

            if (datat != null)
                node.children.Add(datat);
            else
            {
                TokenIndex = currentTokenIndex;
                return null;
            }

            // Identifier
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Idenifier)
                node.children.Add(match(Token_Class.Idenifier));
            else
                Errors.ParserError_List.Add("Missing Idenifier in Declaration Statement.");

            // Declare Rest1
            currentTokenIndex = TokenIndex;

            Node declareRest = Declare_Rest1();

            if (declareRest != null)
                node.children.Add(declareRest);
            else
                TokenIndex = currentTokenIndex;

            // Declare Rest2
            currentTokenIndex = TokenIndex;

            declareRest = Declare_Rest2();

            if (declareRest != null)
                node.children.Add(declareRest);
            else
                TokenIndex = currentTokenIndex;

            // semicolon
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Semicolon)
                node.children.Add(match(Token_Class.Semicolon));
            else
                Errors.ParserError_List.Add("Missing Semicolon in Declaration Statement.");

            return node;
        }


        /* Declare_Rest1 → , identifier Declare_Rest1 | ɛ */
        private static Node Declare_Rest1()
        {
            Node node = new Node("Declare_Rest1");

            // coma ,
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Comma)
                node.children.Add(match(Token_Class.Comma));
            else
                // ɛ state
                return null;

            // Identifier
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.Idenifier)
                node.children.Add(match(Token_Class.Idenifier));
            else
                Errors.ParserError_List.Add("Missing Idenifier in Declaration Statement.");

            // Declare Rest1
            int currentTokenIndex = TokenIndex;

            Node declareRest = Declare_Rest1();

            if (declareRest != null)
                node.children.Add(declareRest);
            else
                TokenIndex = currentTokenIndex;

            return node;
        }
        /* Declare_Rest2 → Assignment_Statement | ɛ */
        private static Node Declare_Rest2()
        {
            Node node = new Node("Declare_Rest2");

            // Assignment_Statement
            int currentTokenIndex = TokenIndex;

            Node assignmentState = Assignment_Statement(true);

            if (assignmentState != null)
                node.children.Add(assignmentState);
            else
            {
                TokenIndex = currentTokenIndex;
                // ɛ state
                return null;
            }
            return node;
        }
        /* Expression → String | Term | Equation */
        private static Node Expression()
        {
            Node node = new Node("Expression");

            // String
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.String)
            {
                node.children.Add(match(Token_Class.String));
                return node;
            }

            // equation
            int currentTokenIndex = TokenIndex;

            Node equ = Equation();

            if (equ != null)
            {
                node.children.Add(equ);
                return node;
            }
            else
                TokenIndex = currentTokenIndex;

            // term
            currentTokenIndex = TokenIndex;

            Node termNode = Term();

            if (termNode != null)
            {
                node.children.Add(termNode);
                return node;
            }
            else
                TokenIndex = currentTokenIndex;

            // if it's neither
            Errors.ParserError_List.Add("Not Valid Expression.");
            return null;
        }

        /* Equation → Term Op_Eq | ( Equation ) Operator_Equation  */
        private static Node Equation()
        {
            Node node = new Node("Equation");

            int currentTokenIndex;
            Node opEqu;

            // if it started with ( 
            if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.LParanthesis)
            {
                node.children.Add(match(Token_Class.LParanthesis));

                // Equation
                currentTokenIndex = TokenIndex;

                Node equ = Equation();
                if (equ != null)
                    node.children.Add(equ);
                else
                {
                    TokenIndex = currentTokenIndex;
                    Errors.ParserError_List.Add("Missing Equation Inside Pranthesis.");
                }

                // Right Prantheisis
                if (TokenIndex < TokenStreamLength && TokenStream[TokenIndex].token_type == Token_Class.RParanthesis)
                    node.children.Add(match(Token_Class.RParanthesis));

                else
                    Errors.ParserError_List.Add("Missing Right Pranthesis in Equation.");

                // Operator_Equation 
                currentTokenIndex = TokenIndex;

                opEqu = Operator_Equation();
                if (opEqu != null)
                    node.children.Add(opEqu);
                else
                    TokenIndex = currentTokenIndex;


                return node;
            }
            // if it started with a term
            currentTokenIndex = TokenIndex;

            Node termNode = Term();

            if (termNode != null)
                node.children.Add(termNode);

            else
            {
                TokenIndex = currentTokenIndex;
                Errors.ParserError_List.Add("Missing Term or Right Pranthesis in Equation.");
                return null;
            }

            // Operator_Equation 
            currentTokenIndex = TokenIndex;

            opEqu = Operator_Equation();
            if (opEqu != null)
                node.children.Add(opEqu);
            else
                TokenIndex = currentTokenIndex;

            return node;
        }

        /* Operator_Equation → Arthematic_Operator Equation Operator_Equation | ε */
        private static Node Operator_Equation()
        {
            Node node = new Node("Equation");

            // Arthematic_Operator 
            int currentTokenIndex = TokenIndex;

            // Arthematic_Operator
            Node arthOp = Arthematic_Operator();
            if (arthOp != null)
            {
                node.children.Add(arthOp);

                // Equation
                currentTokenIndex = TokenIndex;

                Node equ = Equation();
                if (equ != null)
                    node.children.Add(equ);
                else
                {
                    TokenIndex = currentTokenIndex;
                    Errors.ParserError_List.Add("Missing Equation After Arithmetic Operator.");
                }

                // Operator_Equation
                currentTokenIndex = TokenIndex;

                Node equOp = Operator_Equation();
                if (equOp != null)
                    node.children.Add(equOp);
                else
                    TokenIndex = currentTokenIndex;

                return node;

            }

            // if there is no Arithmetic Operator, go to ε state 
            return null;

        }

        /* Arthematic_Operator → plus | minus | divide | multiply */
        private static Node Arthematic_Operator()
        {
            Node node = new Node("Arthematic_Operator");

            // check if +
            if (TokenStream[TokenIndex].token_type == Token_Class.PlusOp)
            {
                node.children.Add(match(Token_Class.PlusOp));
                return node;
            }
            // check if -
            else if (TokenStream[TokenIndex].token_type == Token_Class.MinusOp)
            {
                node.children.Add(match(Token_Class.MinusOp));
                return node;
            }
            // check if /
            else if (TokenStream[TokenIndex].token_type == Token_Class.DivideOp)
            {
                node.children.Add(match(Token_Class.DivideOp));
                return node;
            }
            // checked if *
            else if (TokenStream[TokenIndex].token_type == Token_Class.MultiplyOp)
            {
                node.children.Add(match(Token_Class.MultiplyOp));
                return node;

            }

            // if nothing matched
            return null;
        }

        public static Node match(Token_Class ExpectedToken)
        {
            Token currentToken = TokenStream[TokenIndex];
            if (currentToken.token_type == ExpectedToken)
            {
                Node node = new Node(currentToken.lex);
                TokenIndex++;
                return node;
            }
            return null;
        }

        // Collect the nodes into a tree node 
        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.children.Count == 0)
                return tree;
            foreach (Node child in root.children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
