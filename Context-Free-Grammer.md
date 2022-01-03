## Conte-t Free Grammar

- Program → Program_Function_Statement Main_Function
	
	- Main_Function → Data_Type main ( ) Function_Body

	- Program_Function_Statement → Function_Statement Program_Function_Statement | ɛ

		- Function_Statement → Function_Declaration Function_Body

			- Function_Declaration → Data_Type Function_Name ( Function_Parameters )

				- Datatype → int | float | string

				- Function_Name → identifier

				- Function_Parameters → Data_Type Identifier More_Parameters | ɛ

					- More_Function_Parameters → , Data_Type Identifier More_Function_Parameters | ɛ

			- Function_Body → { Statements Return_Statement }

				- Statements → State Statements | ɛ

					- Statement → Function_Call | Assignment_Statement | Declaration_Statement | Write_Statement | Read_Statement | If_Statement | Repeat_Statement

						- Function_Call → Identifier ( Parameters) ;

							- Parameters → Expression More_Parameters | ɛ

								- More_Parameters → , Expression More_Parameters | ɛ

						- Assignment_Statement → Identifier := Expression

							- Expression → String | Term | Equation

								- Term → number | identifier | Function_Call

								- Equation → Term Operator_Equation | (Equation) Operator_Equation 

									- Operator_Equation → Arthematic_Operator Equation Operator_Equation | ε

										- Arthematic_Operator → plus | minus | divide | multiply 

						- Declaration_Statement → Data_Type Identifier Declare_Rest1 Declare_Rest2 ;

							- Declare_Rest1 → , identifier Declare_Rest1 | ɛ

							- Declare_Rest2 → Assignment_Statement | ɛ

						- Write_Statement → write Write_Rest ;

							- Write_Rest → Expression | endl

						- Read_Statement → read identifier ;

						- If_Statement → if Condition_Statement then Statements Other_Conditions
							
							- Condition_statement → Condition

								- Condition → identifier Condition_Operator Term More_Conditions

									- Condition_Operator → less_than | greater_than | not_equal | equal

									- More_Conditions → and Condition | or Condition| ɛ

							- Other_Conditions → Else_if_Statement | Else_statement | end


								- Else_if_Statement → elseif Condition_statement then Statements Other_Conditions

								- Else_statement → else Statements end

						- Repeat_Statement → repeat Statements untill Condition_statement

						- Return_Statement → return Expression ;

