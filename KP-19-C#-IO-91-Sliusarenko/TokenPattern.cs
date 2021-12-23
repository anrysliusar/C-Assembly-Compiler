using System;

namespace KR
{
    public class TokenPattern
    {
        public static readonly TokenPattern INT = new("int", () => "type_int");
        public static readonly TokenPattern FLOAT = new("float", () => "type_float");
        public static readonly TokenPattern DOUBLE = new("double", () => "type_double");
        public static readonly TokenPattern CHAR = new("char", () => "type_char");
        public static readonly TokenPattern STRING = new("String", () => "type_String");
        public static readonly TokenPattern VOID = new("void", () => "type_void");

        public static readonly TokenPattern DIVISION = new("/", () => "division");
        public static readonly TokenPattern ADDITION = new("+", () => "addition");
        public static readonly TokenPattern SUBTRACTION = new("-", () => "minus");
        public static readonly TokenPattern MULTIPLY = new("*", () => "multiply");

        public static readonly TokenPattern BITWISE_AND = new("&", () => "&");
        public static readonly TokenPattern BITWISE_OR = new("|", () => "|");
        public static readonly TokenPattern BITWISE_XOR = new("^", () => "^");
        public static readonly TokenPattern MODULO = new("%", () => "%");
        public static readonly TokenPattern LEFT_SHIFT = new("<<", () => "<<");
        public static readonly TokenPattern RIGHT_SHIFT = new(">>", () => ">>");
        public static readonly TokenPattern AND = new("&&", () => "&&");
        public static readonly TokenPattern OR = new("||", () => "||");

        public static readonly TokenPattern EQUAL = new("=", () => "equal");
        public static readonly TokenPattern LT = new("<", () => "<");
        public static readonly TokenPattern LTE = new("<=", () => "<=");
        public static readonly TokenPattern GT = new(">", () => ">");
        public static readonly TokenPattern GTE = new(">=", () => ">=");

        public static readonly TokenPattern OR_BRACKET = new("(", () => "open_round_bracket");
        public static readonly TokenPattern CR_BRACKET = new(")", () => "close_round_bracket");
        public static readonly TokenPattern OС_BRACE = new("{", () => "open_curly_brace");
        public static readonly TokenPattern CС_BRACE = new("}", () => "close_curly_brace");

        public static readonly TokenPattern COMMA = new(",", () => "comma");
        public static readonly TokenPattern QUEST_MARK = new("?", () => "question mark");
        public static readonly TokenPattern COLON = new(":", () => "colon");
        public static readonly TokenPattern SEMICOLON = new(";", () => "semicolon");

        public static readonly TokenPattern MAIN = new("main", () => "identifier");
        public static readonly TokenPattern RETURN = new("return", () => "return_keyword");

        public static readonly TokenPattern VARIABLE = new("variable", () => "variable");
        public static readonly TokenPattern CONST_INT = new("int_constant", () => "int_constant");
        public static readonly TokenPattern CONST_FLOAT = new("float_constant", () => "float_constant");

        public static readonly TokenPattern INVALID = new("invalid", () => "invalid");
        public static readonly TokenPattern FUNCTION = new("function", () => "function");
        public static readonly TokenPattern EQUAL_L = new("==", () => "logic equal");
        public static readonly TokenPattern NOT_EQUAL = new("!=", () => "not equal");
        public static readonly TokenPattern NOT = new("!", () => "not");

        public readonly string Field;
        private readonly Func<string> _valueOfIt;

        private TokenPattern(string field, Func<string> valueOfIt)
        {
            Field = field;
            _valueOfIt = valueOfIt;
        }

        public string ValueOfIt()
        {
            return _valueOfIt.Invoke();
        }
    }
}