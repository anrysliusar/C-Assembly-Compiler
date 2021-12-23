using System;
using System.Collections.Generic;
using System.Text;

namespace KR
{
    public class Lexer
    {
        private readonly string _textWithIncomingC;
        private List<string> _separatedTextOfIncomingC;
        private List<dynamic> _tokensExpressedInEnums;
        private List<object> _tokenVals;

        private List<string> _patternsSymb = new()
        {
            TokenPattern.ADDITION.Field, TokenPattern.SEMICOLON.Field, TokenPattern.OR_BRACKET.Field,
            TokenPattern.CR_BRACKET.Field,
            TokenPattern.OС_BRACE.Field, TokenPattern.CС_BRACE.Field, TokenPattern.EQUAL.Field,
            TokenPattern.SUBTRACTION.Field,
            TokenPattern.COLON.Field, TokenPattern.QUEST_MARK.Field, TokenPattern.COMMA.Field, TokenPattern.LT.Field,
            TokenPattern.GT.Field,
            TokenPattern.MODULO.Field, TokenPattern.BITWISE_AND.Field, TokenPattern.BITWISE_OR.Field,
            TokenPattern.BITWISE_XOR.Field,
            TokenPattern.MULTIPLY.Field, TokenPattern.DIVISION.Field
        };

        public Lexer(string textWithIncomingC)
        {
            _textWithIncomingC = textWithIncomingC;
        }

        public Dictionary<string, List<object>> ExtractTokens()
        {
            FileSplitter();
            TokenProcessor processor = new TokenProcessor();
            _tokensExpressedInEnums = new List<dynamic>();
            _tokenVals = new List<object>();


            foreach (string item in _separatedTextOfIncomingC)
            {
                if (!TokenPattern.INVALID.Equals(processor.MatchItemWithTokenPattern(item)))
                {
                    _tokensExpressedInEnums.Add(processor.MatchItemWithTokenPattern(item));
                    _tokenVals.Add(item);
                }
                else if (TokenPattern.INVALID.Equals(processor.MatchItemWithTokenPattern(item)))
                {
                    var amount = 0;
                    int index;
                    for (index = 0; index < item.Length; index++)
                    {
                        if (!TokenPattern.INVALID.Equals(processor.MatchItemWithTokenPattern(item[index].ToString())))
                        {
                            _tokensExpressedInEnums.Add(processor.MatchItemWithTokenPattern(item[index].ToString()));
                            _tokenVals.Add(item[index].ToString());
                            amount++;
                        }

                        if (!TokenPattern.INVALID.Equals(processor.MatchItemWithTokenPattern(item.Substring(index))))
                        {
                            _tokensExpressedInEnums.Add(processor.MatchItemWithTokenPattern(item.Substring(index)));
                            _tokenVals.Add(item.Substring(index));
                            amount += item.Substring(index).Length;
                        }
                    }

                    if (amount != item.Length) throw new UnsuspectedTokenException(_tokenVals, index);
                }
            }


            for (int i = 0; i < _tokensExpressedInEnums.Count - 1; i++)
            {
                if (_tokensExpressedInEnums[i].Equals(TokenPattern.VARIABLE) &&
                    _tokensExpressedInEnums[i + 1].Equals(TokenPattern.OR_BRACKET))
                {
                    _tokensExpressedInEnums[i] = TokenPattern.FUNCTION;
                }
                else if (_tokensExpressedInEnums[i].Equals(TokenPattern.EQUAL) &&
                         _tokensExpressedInEnums[i + 1].Equals(TokenPattern.EQUAL))
                {
                    _tokensExpressedInEnums[i] = TokenPattern.EQUAL_L;
                    _tokenVals[i] = TokenPattern.EQUAL_L.Field;
                    _tokensExpressedInEnums.RemoveAt(i + 1);
                    _tokenVals.RemoveAt(i + 1);
                }
                else if (_tokensExpressedInEnums[i].Equals(TokenPattern.NOT) &&
                         _tokensExpressedInEnums[i + 1].Equals(TokenPattern.EQUAL))
                {
                    _tokensExpressedInEnums[i] = TokenPattern.NOT_EQUAL;
                    _tokenVals[i] = TokenPattern.NOT_EQUAL.Field;
                    _tokensExpressedInEnums.RemoveAt(i + 1);
                    _tokenVals.RemoveAt(i + 1);
                }
                else if (_tokensExpressedInEnums[i].Equals(TokenPattern.LT) &&
                         _tokensExpressedInEnums[i + 1].Equals(TokenPattern.EQUAL))
                {
                    _tokensExpressedInEnums[i] = TokenPattern.LTE;
                    _tokenVals[i] = TokenPattern.LTE.Field;
                    _tokensExpressedInEnums.RemoveAt(i + 1);
                    _tokenVals.RemoveAt(i + 1);
                }
                else if (_tokensExpressedInEnums[i].Equals(TokenPattern.GT) &&
                         _tokensExpressedInEnums[i + 1].Equals(TokenPattern.EQUAL))
                {
                    _tokensExpressedInEnums[i] = TokenPattern.GTE;
                    _tokenVals[i] = TokenPattern.GTE.Field;
                    _tokensExpressedInEnums.RemoveAt(i + 1);
                    _tokenVals.RemoveAt(i + 1);
                }
                else if (_tokensExpressedInEnums[i].Equals(TokenPattern.GT) &&
                         _tokensExpressedInEnums[i + 1].Equals(TokenPattern.GT))
                {
                    _tokensExpressedInEnums[i] = TokenPattern.RIGHT_SHIFT;
                    _tokenVals[i] = TokenPattern.RIGHT_SHIFT.Field;
                    _tokensExpressedInEnums.RemoveAt(i + 1);
                    _tokenVals.RemoveAt(i + 1);
                }
                else if (_tokensExpressedInEnums[i].Equals(TokenPattern.LT) &&
                         _tokensExpressedInEnums[i + 1].Equals(TokenPattern.LT))
                {
                    _tokensExpressedInEnums[i] = TokenPattern.LEFT_SHIFT;
                    ;
                    _tokenVals[i] = TokenPattern.RIGHT_SHIFT.Field;
                    _tokensExpressedInEnums.RemoveAt(i + 1);
                    _tokenVals.RemoveAt(i + 1);
                }
                else if (_tokensExpressedInEnums[i].Equals(TokenPattern.BITWISE_AND) &&
                         _tokensExpressedInEnums[i + 1].Equals(TokenPattern.BITWISE_AND))
                {
                    _tokensExpressedInEnums[i] = TokenPattern.AND;
                    _tokenVals[i] = TokenPattern.AND.Field;
                    _tokensExpressedInEnums.RemoveAt(i + 1);
                    _tokenVals.RemoveAt(i + 1);
                }
                else if (_tokensExpressedInEnums[i].Equals(TokenPattern.BITWISE_OR) &&
                         _tokensExpressedInEnums[i + 1].Equals(TokenPattern.BITWISE_OR))
                {
                    _tokensExpressedInEnums[i] = TokenPattern.OR;
                    _tokenVals[i] = TokenPattern.OR;
                    _tokensExpressedInEnums.RemoveAt(i + 1);
                    _tokenVals.RemoveAt(i + 1);
                }
            }

            Dictionary<string, List<dynamic>> res = new Dictionary<string, List<dynamic>>();
            res.Add("tokenVals", _tokenVals);
            res.Add("tokensExpressedInEnums", _tokensExpressedInEnums);
            return res;
        }

        private string CommentHandler()
        {
            var incomingCWithoutComments = new StringBuilder();
            var flag = true;

            string[] split = _textWithIncomingC.Split("");
            for (var index = 0; index < split.Length; index++)
            {
                var symb = split[index];
                if (symb.Equals(TokenPattern.DIVISION.Field) &&
                    _textWithIncomingC[index + 1].ToString().Equals(TokenPattern.MULTIPLY.Field) &&
                    index != _textWithIncomingC.Length - 1)
                    flag = false;

                else if (symb.Equals(TokenPattern.DIVISION.Field) &&
                         _textWithIncomingC[index - 1].ToString().Equals(TokenPattern.MULTIPLY.Field)) flag = true;
                else if (flag)
                {
                    if (_patternsSymb.Contains(symb)) incomingCWithoutComments.Append(" ").Append(symb).Append(" ");
                    else if (symb.Equals("!")) incomingCWithoutComments.Append(symb).Append(" ");
                    else incomingCWithoutComments.Append(symb);
                }
            }

            return incomingCWithoutComments.ToString();
        }

        public void FileSplitter()
        {
            var separatedIncomingC = new List<string>();
            foreach (string item in CommentHandler().Split(" "))
            {
                if (item.Equals("")) continue;
                if (item.Contains(TokenPattern.OR_BRACKET.Field + TokenPattern.CR_BRACKET.Field))
                {
                    separatedIncomingC.AddRange(item
                        .Replace(TokenPattern.OR_BRACKET.Field + TokenPattern.CR_BRACKET.Field,
                            " " + TokenPattern.OR_BRACKET.Field + " " + TokenPattern.CR_BRACKET.Field + " ")
                        .Split(" "));
                }

                if (item.Length > 1)
                {
                    if (item.Contains(TokenPattern.ADDITION.Field))
                    {
                        separatedIncomingC.AddRange(item
                            .Replace(TokenPattern.ADDITION.Field, " " + TokenPattern.ADDITION.Field + " ")
                            .Split(" "));
                    }

                    if (item.Contains(TokenPattern.SEMICOLON.Field))
                    {
                        separatedIncomingC.AddRange(item
                            .Replace(TokenPattern.SEMICOLON.Field, " " + TokenPattern.SEMICOLON.Field + " ")
                            .Split(" "));
                    }

                    if (item.Contains(TokenPattern.OС_BRACE.Field))
                    {
                        separatedIncomingC.AddRange(item
                            .Replace(TokenPattern.OС_BRACE.Field, " " + TokenPattern.OС_BRACE.Field + " ")
                            .Split(" "));
                    }

                    if (item.Contains(TokenPattern.CС_BRACE.Field))
                    {
                        separatedIncomingC.AddRange(item
                            .Replace(TokenPattern.CС_BRACE.Field, " " + TokenPattern.CС_BRACE.Field + " ")
                            .Split(" "));
                    }
                }
                else if (item.Contains(TokenPattern.MAIN.Field))
                {
                    separatedIncomingC.AddRange(item
                        .Replace(TokenPattern.MAIN.Field, TokenPattern.MAIN.Field + " ")
                        .Split(" "));
                }

                else if (item.Contains(TokenPattern.RETURN.Field))
                {
                    separatedIncomingC.AddRange(item.Replace(TokenPattern.RETURN.Field, " " + TokenPattern.RETURN.Field)
                        .Split(" "));
                }
                else separatedIncomingC.Add(item);
            }

            _separatedTextOfIncomingC = separatedIncomingC;
        }
    }
}