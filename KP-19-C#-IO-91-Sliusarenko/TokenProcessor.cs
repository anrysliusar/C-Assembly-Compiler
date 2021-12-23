using System;
using System.Collections.Generic;
using System.Linq;

namespace KR
{
    public class TokenProcessor
    {
        private readonly List<TokenPattern> _allTokenPatterns = new()
        {
            TokenPattern.GT, TokenPattern.INT, TokenPattern.LT, TokenPattern.CHAR, TokenPattern.MAIN, TokenPattern.VOID,
            TokenPattern.COLON, TokenPattern.COMMA, TokenPattern.EQUAL, TokenPattern.FLOAT, TokenPattern.DOUBLE, TokenPattern.MODULO,
            TokenPattern.RETURN, TokenPattern.STRING, TokenPattern.CС_BRACE, TokenPattern.OС_BRACE, TokenPattern.ADDITION,
             TokenPattern.DIVISION, TokenPattern.MULTIPLY, TokenPattern.BITWISE_OR,
            TokenPattern.CR_BRACKET, TokenPattern.OR_BRACKET, TokenPattern.QUEST_MARK, TokenPattern.SEMICOLON, TokenPattern.BITWISE_AND,
            TokenPattern.BITWISE_XOR,  TokenPattern.SUBTRACTION
        };



        public TokenPattern MatchItemWithTokenPattern(string item)
        {
            foreach (var pattern in _allTokenPatterns)
            {
                if (pattern.Field.Equals(item))
                {
                    return pattern;
                }

                if (CheckIfTextSeemsLikeNum(item))
                {
                    if (char.IsLetter(item[0]))
                    {
                        return TokenPattern.VARIABLE;
                    }
                    return TokenPattern.CONST_INT;
                }
                if (item.Contains(".") && CheckIfTextSeemsLikeDigit(item.Replace(".", ""))) return TokenPattern.CONST_FLOAT;
            }

            return item.Length == 0 ? null : TokenPattern.INVALID;
        }
        
        private bool CheckIfTextSeemsLikeNum(string text)
        {
            return text.Split("").SelectMany(s => s.ToCharArray()).All(char.IsDigit);
        }
        private bool CheckIfTextSeemsLikeDigit(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}