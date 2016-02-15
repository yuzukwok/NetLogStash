grammar Logstashconfig;
options {
    language=CSharp;
}

/* Lexical rules */

INPUT : 'input';
FILTER : 'filter';
OUTPUT : 'output';

LBRACE : '{';
RBRACE : '}';
LPAREN : '(';
RPAREN : ')';
LBRACKET : '[';
RBRACKET : ']';

HASHROCKET : '=>';

GT : '>';
GE : '>=';
LT : '<';
LE : '<=';
EQ : '==';
NEQ : '!=';
BANG : '!';
COMA : ',';
IN : 'in';
NOT: 'not';
MATCH : '=~';
NOT_MATCH : '!~';

AND : 'and';
OR : 'or';
XOR : 'xor';
NAND : 'nand';

IF : 'if';
ELSE : 'else';

fragment NL : '\n';
fragment HASH : '#';
fragment FSLASH : '/';
fragment BSLASH : '\\';

// double and single quoted string support
fragment DQUOTE : '"';
fragment SQUOTE : '\'';
fragment DQ_STRING_ESC : BSLASH ["\\/bfnrt] ;
fragment SQ_STRING_ESC : BSLASH ['\\/bfnrt] ;
fragment DQ_STRING : DQUOTE (DQ_STRING_ESC | ~["\\])* DQUOTE ;
fragment SQ_STRING : SQUOTE (SQ_STRING_ESC | ~['\\])* SQUOTE ;
STRING : DQ_STRING | SQ_STRING ;
// TODO: (colin) verify REGEX validity, I am unsure about the original treetop grammar definition
fragment REGEX_ESC : BSLASH FSLASH ;
REGEX : FSLASH (REGEX_ESC | ~[FSLASH])* FSLASH ;
DECIMAL : '-'?[0-9]+('.'[0-9]+)? ;
IDENTIFIER : [a-zA-Z_][a-zA-Z_0-9-]* ;
// ignore whitespaces and comments
WS : [ \r\t\n\u000C]+ -> skip ;
COMMENT : HASH .+? (NL|EOF) -> skip ;
/* Grammar rules */
config : stage_declaration+ EOF ;
stage_declaration : ( INPUT | FILTER | OUTPUT ) stage_definition ;
stage_definition : LBRACE (plugin_declaration | stage_condition)* RBRACE ;
plugin_declaration : IDENTIFIER plugin_definition ;
plugin_definition : LBRACE plugin_attribute* RBRACE ;
plugin_attribute : IDENTIFIER HASHROCKET plugin_attribute_value ;
// TODO: (colin) verify hash requirement here
plugin_attribute_value : plugin_declaration | IDENTIFIER | STRING | DECIMAL | array | hash ;
stage_condition : IF logical_expression stage_definition (ELSE IF logical_expression stage_definition)* (ELSE stage_definition)? ;
logical_expression
 : logical_expression AND logical_expression
 | logical_expression OR logical_expression
 | logical_expression NAND logical_expression
 | logical_expression XOR logical_expression
 | compare_expression
 | in_expression
 | match_expression
 | negative_expression
 | LPAREN logical_expression RPAREN
 | rvalue
 ;
// TODO: (colin) why not also accept: BANG logical_expression - are parens necessary?
negative_expression : BANG LPAREN logical_expression RPAREN ;
compare_expression
 : rvalue GT rvalue
 | rvalue GE rvalue
 | rvalue LT rvalue
 | rvalue LE rvalue
 | rvalue EQ rvalue
 | rvalue NEQ rvalue
 ;
in_expression : rvalue NOT? IN rvalue ;
match_expression : rvalue (MATCH | NOT_MATCH) (STRING | REGEX) ;
// TODO: (colin) add method_call to rvalue
// TODO: (colin) per original treetop grammar, REGEX is also valid in rvalue, should we add it?
rvalue : STRING | DECIMAL | fieldref | array ;
fieldref : fieldref_element+ ;
fieldref_element : LBRACKET IDENTIFIER RBRACKET ;
// TODO: (colin) plugin_declaration are allowed in array per original treetop grammar and seems related
// TODO: (con't) to never-implemented channable codec. I am not allowing here.
array : LBRACKET  array_element (COMA array_element)*  RBRACKET;
array_element : IDENTIFIER | STRING | DECIMAL | array | hash ;

hash : LBRACE hash_element* RBRACE ;
hash_element : (DECIMAL | IDENTIFIER | STRING) HASHROCKET plugin_attribute_value ;