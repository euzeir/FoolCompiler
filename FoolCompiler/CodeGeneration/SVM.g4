grammar SVM;

@header {
using FoolCompiler.CodeGeneration;
using System.Collections.Generic;
}

@lexer::members {
public List<string> errors = new List<string>();
}

@parser::members {

    private List<int> _code = new List<int>();

    private Dictionary<string,int> _labelAdd = new Dictionary<string,int>();
    private Dictionary<int,string> _labelRef = new  Dictionary<int,string>();


    public List<int> GetCode() {return _code;}
}

/*------------------------------------------------------------------
 * PARSER RULES
 *------------------------------------------------------------------*/
  
assembly:
    ( PUSH n=NUMBER                 {   _code.Add(PUSH);
			                            _code.Add(int.Parse($n.text));     }

	  | PUSH l=LABEL                {   _code.Add(PUSH);
	    		                        _labelRef.Add(_code.Count, $l.text);
	    		                        _code.Add(0);                             }

	  | POP		                    {   _code.Add(POP);  }
	  | ADD		                    {   _code.Add(ADD);  }
	  | SUB		                    {   _code.Add(SUB);    }
	  | MULT	                    {   _code.Add(MULT);   }
	  | DIV		                    {   _code.Add(DIV);  }
	  | STOREW	                    {   _code.Add(STOREW);   }
	  | LOADW                       {   _code.Add(LOADW);    }
	  | l=LABEL COL                 {   _labelAdd.Add($l.text, _code.Count);   }
	  | BRANCH l=LABEL              {   _code.Add(BRANCH);
                                        _labelRef.Add(_code.Count, $l.text);
                                        _code.Add(0);                          } //WITHOUT 0 SEGMENTATION FAULT
	  | BRANCHEQ l=LABEL 			{	_code.Add(BRANCHEQ);
										_labelRef.Add(_code.Count, $l.text);
										_code.Add(0);  }
	  | BRANCHLESSEQ l=LABEL 		{	_code.Add(BRANCHLESSEQ);
										_labelRef.Add(_code.Count,$l.text);
										_code.Add(0); }
	  | JS                          {   _code.Add(JS);    }
	  | LOADRA                      {   _code.Add(LOADRA);   }
	  | STORERA                     {   _code.Add(STORERA);  }
	  | LOADRV                      {   _code.Add(LOADRV);   }
	  | STORERV                     {   _code.Add(STORERV);  }
	  | LOADFP                      {   _code.Add(LOADFP);   }
	  | STOREFP                     {   _code.Add(STOREFP);  }
	  | COPYFP                      {   _code.Add(COPYFP);   }
	  | LOADHP                      {   _code.Add(LOADHP);   }
	  | STOREHP                     {   _code.Add(STOREHP);  }
	  | PRINT                       {   _code.Add(PRINT);    }
	  | NEW                         {   _code.Add(NEW);      }
	  | LOADMETHOD                  {   _code.Add(LOADMETHOD);       }
	  | HALT                        {   _code.Add(HALT);     }
	  | l=LABEL                     {   _labelRef.Add(_code.Count, $l.text);
	                                    _code.Add(0);        }
	  | COPYTOPSTACK                {   _code.Add(COPYTOPSTACK);     }
	)* {
	        foreach (int refAdd in _labelRef.Keys) {
	            _code[refAdd] =  _labelAdd[_labelRef[refAdd]];
	        }
	   } ;


/*------------------------------------------------------------------
 * LEXER RULES
 *------------------------------------------------------------------*/
 
PUSH  	 : 'push' ; 	// pushes constant in the stack
POP	 : 'pop' ; 	// pops from stack
ADD	 : 'add' ;  	// add two values from the stack
SUB	 : 'sub' ;	// add two values from the stack
MULT	 : 'mult' ;  	// add two values from the stack
DIV	 : 'div' ;	// add two values from the stack
STOREW	 : 'sw' ; 	// store in the memory cell pointed by top the value next
LOADW	 : 'lw' ;	// load a value from the memory cell pointed by top
BRANCH	 : 'b' ;	// jump to label
BRANCHEQ : 'beq' ;	// jump to label if top == next
BRANCHLESSEQ:'bleq' ;	// jump to label if top <= next
JS	 : 'js' ;	// jump to instruction pointed by top of stack and store next instruction in ra
LOADRA	 : 'lra' ;	// load from ra
STORERA  : 'sra' ;	// store top into ra	 
LOADRV	 : 'lrv' ;	// load from rv
STORERV  : 'srv' ;	// store top into rv	 
LOADFP	 : 'lfp' ;	// load frame pointer in the stack
STOREFP	 : 'sfp' ;	// store top into frame pointer
COPYFP   : 'cfp' ;      // copy stack pointer into frame pointer
LOADHP	 : 'lhp' ;	// load heap pointer in the stack
STOREHP	 : 'shp' ;	// store top into heap pointer
PRINT	 : 'print' ;	// print top of stack       //NOT USED
HALT	 : 'halt' ;	// stop execution

// Object stuff
NEW      : 'new' ;   // allocates a memory in the heap
// Methods stuff
LOADMETHOD: 'loadm' ; //Load on the stack the address of the method of a class from the dispatch table
COPYTOPSTACK: 'cts'; // Makes a copy of the top of the stack and inserts it. Serves to access the dispatch table
                        // because to access a method of an object we use the address in the dispatch table
                        // and you need to duplicate it so then you can use it to continue accessing the object
                        // (e.g. to use the fields in the method)

COL	 : ':' ;
LABEL	 : ('a'..'z'|'A'..'Z')('a'..'z' | 'A'..'Z' | '0'..'9')* ;
NUMBER	 : '0' | ('-')?(('1'..'9')('0'..'9')*) ;

WHITESP  : ( '\t' | ' ' | '\r' | '\n' )+   -> channel(HIDDEN);

ERR   	 : . { errors.Add("Invalid char: " + Text); } -> channel(HIDDEN);