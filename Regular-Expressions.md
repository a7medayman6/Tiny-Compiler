# Tiny Programing Language Regular Expressions
______________________________________________

digit ::= 0|1|2|3|4......|9

letter ::= [a-z][A-Z]

Number ::= digit*.?digit*

String ::= "(letter|digit)*"

Reserved_Keywords ::=  int | float | string | read | write |
repeat | until | if | elseif | else | then | return | endl

Comment ::= /\*String\*\/

Identifier ::= letter (letter | digit)*

Term ::= Number | Identifier | Function_Call

Arithmatic_Operator ::= + | - | * | /

Equation ::=  (Term (Arithmatic_Operator (Equation | Term))+) | (\( Term Arithmatic_Operator ( Equation | Term) \)) (Arithmatic_Operator (Term | Equation))*

Expression ::= String | Term | Equation

Datatype ::= int | float | string

Condition_Operator ::= < | > | = | <>

Boolean_Operator ::= && | \|\|