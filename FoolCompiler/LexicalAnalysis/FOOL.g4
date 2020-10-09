grammar FOOL;

@header {
using System.Collections.Generic;
}
@lexer::members {
   public List<String> lexicalErrors = new List<String>();
}

/*------------------------------------------------------------------
 * PARSER RULES
 *------------------------------------------------------------------*/
  
prog   : exp SEMIC													#singleExp
       | let IN (exp SEMIC | stms)                                  #letInExp
       | (classdec)+ let? IN (exp SEMIC | stms)                     #classExp
       ;

let    : LET (dec SEMIC)+;

classdec : CLASS ID (EXTENDS ID)? (LPAR vardec (COMMA vardec)* RPAR)? (CLPAR met* CRPAR)? SEMIC
         ;

letVar : LET (varasm)+ ;

vardec  : type ID ;

varasm  : vardec ASM (exp | NULL) ;

fun    : (VOID | type) ID LPAR ( vardec ( COMMA vardec)* )? RPAR  CLPAR (letVar IN)? (exp SEMIC | stms) CRPAR
       ;

dec   : varasm
      | fun
      ;

met : fun
    ;
         
   
type   : INT  
        | BOOL
        | ID
      ;

exp   : left=term ((PLUS | MINUS) right=exp)?
      ;
   
term  : left=factor ((TIMES | DIV) right=term)?
      ;
   
factor : left=value ( logicoperator=(EQ|GREATERTHAN|LESSERTHAN|GREATEREQUAL|LESSEREQUAL|AND|OR) right=factor)?
      ;     
   
value  : (MINUS)? INTEGER                                                                            #intVal
      |  (NOT)? ( TRUE | FALSE )                                                                     #boolVal
      |  LPAR exp RPAR                                                                               #baseExp
      |  IF cond=exp THEN CLPAR thenBranch=exp SEMIC CRPAR (ELSE CLPAR elseBranch=exp SEMIC CRPAR)?  #ifExp
      |  (MINUS| NOT)? ID                                                                            #varId
      |  functioncall                                                                                #funExp
      |  ID DOT functioncall                                                                         #methExp
      |  NEW ID LPAR (exp (COMMA exp)* )? RPAR                                                       #newExp
      ;

functioncall : ID LPAR (exp (COMMA exp)* )? RPAR ;

stm :  ID ASM (NULL | exp)                                                                        #asmStm
	  | IF LPAR cond=exp RPAR THEN CLPAR thenBranch=stms CRPAR (ELSE CLPAR elseBranch=stms CRPAR)?  #ifStm
	  ;

stms :  (stm SEMIC)+ ;

/*------------------------------------------------------------------
 * LEXER RULES
 *------------------------------------------------------------------*/
SEMIC  : ';' ;
COLON  : ':' ;
COMMA  : ',' ;
EQ     : '==' ;
ASM    : '=' ;
PLUS   : '+' ;
MINUS  : '-' ;
TIMES  : '*' ;
DIV    : '/' ;
TRUE   : 'true' ;
FALSE  : 'false' ;
LPAR   : '(' ;
RPAR   : ')' ;
CLPAR  : '{' ;
CRPAR  : '}' ;
IF     : 'if' ;
THEN   : 'then' ;
ELSE   : 'else' ;
LET    : 'let' ;
IN     : 'in' ;
VAR    : 'var' ;
FUN    : 'fun' ;
INT    : 'int' ;
BOOL   : 'bool' ;

//ADDED FOR PROJECT.

OR             : '||' ;
AND            : '&&' ;
NOT            : 'not' ;
GREATERTHAN    : '>' ;
LESSERTHAN     : '<' ;
GREATEREQUAL   : '>=' ;
LESSEREQUAL    : '<=' ;


VOID            : 'void';
CLASS           : 'class';
THIS            : 'this';
NEW             : 'new';
DOT             : '.';
EXTENDS         : 'extends';
NULL            : 'null';


//Numbers
fragment DIGIT : '0'..'9';    
INTEGER       : DIGIT+;

//IDs
fragment CHAR  : 'a'..'z' |'A'..'Z' ;
ID              : CHAR (CHAR | DIGIT)* ;

//ESCAPED SEQUENCES
WS              : (' '|'\t'|'\n'|'\r')-> skip;
LINECOMENTS    : '//' (~('\n'|'\r'))* -> skip;
BLOCKCOMENTS    : '/*'( ~('/'|'*')|'/'~'*'|'*'~'/'|BLOCKCOMENTS)* '*/' -> skip;