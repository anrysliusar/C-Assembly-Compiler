using System;
using System.Collections.Generic;

namespace KR
{
    public class Parser
    {
        private List<dynamic> _tokensExpressedInEnums;
        private List<object> _tokenVals;

        public Parser(List<object> tokenVals, List<dynamic> tokensExpressedInEnums)
        {
            _tokensExpressedInEnums = new List<dynamic>();
            foreach (var token in tokensExpressedInEnums)
            {
                _tokensExpressedInEnums.Add((string) token);
            }

            _tokenVals = tokenVals;
        }


        public Dictionary<string, Dictionary<string, List<string>>> Parse()
        {
            for (var index = 0; index < _tokensExpressedInEnums.Count; index++)
            {
                var tokenPattern = _tokensExpressedInEnums[index];
                if (tokenPattern.Equals(TokenPattern.CONST_FLOAT))
                {
                    _tokenVals[index] = int.Parse((string) _tokenVals[index]);
                    _tokensExpressedInEnums[index] = TokenPattern.CONST_INT;
                }
                else if (tokenPattern.Equals(TokenPattern.CONST_INT))
                {
                    _tokenVals[index] = int.Parse((string) _tokenVals[index]);
                }
            }

            var operations = new List<TokenPattern>
            {
                TokenPattern.EQUAL_L, TokenPattern.AND, TokenPattern.OR, TokenPattern.NOT_EQUAL, TokenPattern.LT,
                TokenPattern.LTE, TokenPattern.GT, TokenPattern.GTE, TokenPattern.MODULO, TokenPattern.BITWISE_AND,
                TokenPattern.BITWISE_OR, TokenPattern.BITWISE_XOR, TokenPattern.LEFT_SHIFT, TokenPattern.RIGHT_SHIFT,
                TokenPattern.MULTIPLY, TokenPattern.DIVISION, TokenPattern.ADDITION, TokenPattern.SUBTRACTION,
                TokenPattern.QUEST_MARK, TokenPattern.COLON
            };


            for (var index = 0; index < _tokensExpressedInEnums.Count; index++)
            {
                var pattern = _tokensExpressedInEnums[index];
                var markedList = new List<string> {TokenPattern.MAIN.ValueOfIt(), TokenPattern.FUNCTION.ValueOfIt()};
                if (pattern.Equals(TokenPattern.INT) || pattern.Equals(TokenPattern.FLOAT))
                {
                    if (index == 0)
                    {
                        if (!markedList.Contains(_tokensExpressedInEnums[index + 1]))
                            throw new UnsuspectedTokenException(_tokenVals, 1);
                    }
                    else
                    {
                        var checkList = new List<string>
                        {
                            TokenPattern.VARIABLE.ValueOfIt(), TokenPattern.MAIN.ValueOfIt(),
                            TokenPattern.FUNCTION.ValueOfIt()
                        };
                        var signs = new List<string>
                        {
                            TokenPattern.COMMA.ValueOfIt(), TokenPattern.OR_BRACKET.ValueOfIt(),
                            TokenPattern.CС_BRACE.ValueOfIt(), TokenPattern.SEMICOLON.ValueOfIt(),
                            TokenPattern.OС_BRACE.ValueOfIt()
                        };
                        if (!(checkList.Contains(_tokensExpressedInEnums[index + 1]) &&
                              signs.Contains(_tokensExpressedInEnums[index - 1])))
                            throw new UnsuspectedTokenException(_tokenVals, index + 1);
                    }
                }
                else if (markedList.Contains(_tokensExpressedInEnums[index - 1]))
                {
                    var checkList = new List<string>
                    {
                        TokenPattern.INT.ValueOfIt(), TokenPattern.FLOAT.ValueOfIt(), TokenPattern.RETURN.ValueOfIt()
                    };
                    if (!checkList.Contains(_tokensExpressedInEnums[index - 1]) ||
                        !TokenPattern.OR_BRACKET.Equals(_tokensExpressedInEnums[index + 1]))
                        throw new UnsuspectedTokenException(_tokenVals, index + 1);
                }
                else if (TokenPattern.OR_BRACKET.Equals(pattern))
                {
                    var checkList = new List<string>
                    {
                        TokenPattern.CR_BRACKET.ValueOfIt(), TokenPattern.VARIABLE.ValueOfIt(),
                        TokenPattern.CONST_INT.ValueOfIt(), TokenPattern.INT.ValueOfIt(), TokenPattern.FLOAT.ValueOfIt()
                    };
                    if (!markedList.Contains(_tokensExpressedInEnums[index - 1]) ||
                        !checkList.Contains(_tokensExpressedInEnums[index + 1]))
                        throw new UnsuspectedTokenException(_tokenVals, index + 1);
                }
                else if (TokenPattern.CR_BRACKET.Equals(pattern))
                {
                    List<string> checkList = new List<string>
                    {
                        TokenPattern.OR_BRACKET.ValueOfIt(), TokenPattern.VARIABLE.ValueOfIt(),
                        TokenPattern.CONST_INT.ValueOfIt()
                    };
                    if (!checkList.Contains(_tokensExpressedInEnums[index - 1]) ||
                        !new List<string>
                        {
                            TokenPattern.OС_BRACE.ValueOfIt(), TokenPattern.SEMICOLON.ValueOfIt()
                        }.Contains(_tokensExpressedInEnums[index + 1]))
                        throw new UnsuspectedTokenException(_tokenVals, index + 1);
                }
                else if (TokenPattern.COMMA.Equals(pattern))
                {
                    List<string> checkList = new List<string>
                    {
                        TokenPattern.VARIABLE.ValueOfIt(), TokenPattern.CONST_INT.ValueOfIt()
                    };
                    List<string> typeVars = new List<string>
                    {
                        TokenPattern.INT.ValueOfIt(), TokenPattern.FLOAT.ValueOfIt(), TokenPattern.VARIABLE.ValueOfIt(),
                        TokenPattern.CONST_INT.ValueOfIt()
                    };
                    if (!checkList.Contains(_tokensExpressedInEnums[index - 1]) ||
                        !typeVars.Contains(_tokensExpressedInEnums[index + 1]))
                        throw new UnsuspectedTokenException(_tokenVals, index + 1);
                }
                else if (TokenPattern.RETURN.Equals(pattern))
                {
                    List<string> arr1 = new List<string>
                        {TokenPattern.SEMICOLON.ValueOfIt(), TokenPattern.OС_BRACE.ValueOfIt()};
                    List<string> arr2 = new List<string>
                    {
                        TokenPattern.VARIABLE.ValueOfIt(), TokenPattern.CONST_INT.ValueOfIt(),
                        TokenPattern.FUNCTION.ValueOfIt(),
                        TokenPattern.NOT.ValueOfIt()
                    };
                    if (!arr1.Contains(_tokensExpressedInEnums[index - 1]) ||
                        !arr2.Contains(_tokensExpressedInEnums[index + 1]))
                        throw new UnsuspectedTokenException(_tokenVals, index + 1);
                }
                else if (TokenPattern.EQUAL.Equals(pattern))
                {
                    List<string> checkList = new List<string>
                    {
                        TokenPattern.VARIABLE.ValueOfIt(), TokenPattern.CONST_INT.ValueOfIt(),
                        TokenPattern.FUNCTION.ValueOfIt(),
                        TokenPattern.NOT.ValueOfIt()
                    };
                    if (!TokenPattern.VARIABLE.ValueOfIt().Contains(_tokensExpressedInEnums[index - 1]) ||
                        !checkList.Contains(_tokensExpressedInEnums[index + 1]))
                        throw new UnsuspectedTokenException(_tokenVals, index + 1);
                }
                else if (TokenPattern.SEMICOLON.Equals(pattern))
                {
                    List<string> checkList = new List<string>
                    {
                        TokenPattern.VARIABLE.ValueOfIt(), TokenPattern.CONST_INT.ValueOfIt(),
                        TokenPattern.FUNCTION.ValueOfIt(),
                        TokenPattern.NOT.ValueOfIt()
                    };
                    List<string> arr2 = new List<string>
                    {
                        TokenPattern.RETURN.ValueOfIt(), TokenPattern.CС_BRACE.ValueOfIt(),
                        TokenPattern.INT.ValueOfIt(),
                        TokenPattern.FLOAT.ValueOfIt(), TokenPattern.VARIABLE.ValueOfIt()
                    };
                    if (!checkList.Contains(_tokensExpressedInEnums[index - 1]) ||
                        !arr2.Contains(_tokensExpressedInEnums[index + 1]))
                        throw new UnsuspectedTokenException(_tokenVals, index + 1);
                }
                else if (TokenPattern.OС_BRACE.ValueOfIt().Equals(pattern))
                {
                    List<string> arr1 = new List<string>
                    {
                        TokenPattern.RETURN.ValueOfIt(), TokenPattern.INT.ValueOfIt(), TokenPattern.FLOAT.ValueOfIt()
                    };
                    if (!TokenPattern.CR_BRACKET.Equals(_tokensExpressedInEnums[index - 1]) ||
                        !arr1.Equals(_tokensExpressedInEnums[index + 1]))
                        throw new UnsuspectedTokenException(_tokenVals, index + 1);
                }
                else if (TokenPattern.CС_BRACE.ValueOfIt().Equals(pattern))
                {
                    if (!TokenPattern.SEMICOLON.ValueOfIt().Equals(_tokensExpressedInEnums[index - 1]))
                        throw new UnsuspectedTokenException(_tokenVals, index + 1);
                }
                else if (new List<string> {TokenPattern.VARIABLE.ValueOfIt(), TokenPattern.CONST_INT.ValueOfIt()}
                    .Contains(pattern))
                {
                    if (!operations.Contains(_tokensExpressedInEnums[index - 1]))
                        throw new UnsuspectedTokenException(_tokenVals, index + 1);
                }
                else if (operations.Contains(pattern))
                {
                    List<string> arr1 = new List<string>
                    {
                        TokenPattern.CONST_INT.ValueOfIt(), TokenPattern.VARIABLE.ValueOfIt()
                    };
                    List<string> arr2 = new List<string>
                    {
                        TokenPattern.CONST_INT.ValueOfIt(), TokenPattern.VARIABLE.ValueOfIt(),
                        TokenPattern.NOT.ValueOfIt()
                    };
                    if (!arr1.Contains(_tokensExpressedInEnums[index - 1]) ||
                        !arr2.Contains(_tokensExpressedInEnums[index + 1]))
                        throw new UnsuspectedTokenException(_tokenVals, index + 1);
                }
                else if (TokenPattern.NOT.ValueOfIt().Contains(pattern))
                {
                    List<string> arr2 = new List<string>
                    {
                        TokenPattern.VARIABLE.ValueOfIt(), TokenPattern.CONST_INT.ValueOfIt()
                    };
                    if (!operations.Contains(_tokensExpressedInEnums[index - 1]) ||
                        !arr2.Contains(_tokensExpressedInEnums[index + 1]))
                        throw new UnsuspectedTokenException(_tokenVals, index + 1);
                }
            }

            var result = new Dictionary<string, Dictionary<string, List<string>>>();

            for (var indexToken = 0; indexToken < _tokensExpressedInEnums.Count; indexToken++)
            {
                var inEnum = _tokensExpressedInEnums[indexToken];
                List<string> arr1 = new List<string> {TokenPattern.FUNCTION.ValueOfIt(), TokenPattern.MAIN.ValueOfIt()};
                List<string> arr2 = new List<string> {TokenPattern.FLOAT.ValueOfIt(), TokenPattern.INT.ValueOfIt()};

                if (arr1.Contains(inEnum) && arr2.Contains(_tokensExpressedInEnums[indexToken - 1]))
                {
                    if (result.ContainsKey((string) _tokenVals[indexToken]))
                    {
                        throw new InvalidStatementException("Invalid statement: func " + _tokenVals[indexToken] +
                                                            " already exists");
                    }

                    for (var indexTokenIn = 0; indexTokenIn < _tokensExpressedInEnums.Count; indexTokenIn++)
                    {
                        var tokensExpressedInEnum = _tokensExpressedInEnums[indexTokenIn];
                        if (indexTokenIn > indexToken)
                        {
                            if (tokensExpressedInEnum.Equals(TokenPattern.VARIABLE.ValueOfIt()) ||
                                tokensExpressedInEnum.Equals(TokenPattern.RETURN.ValueOfIt()))
                            {
                                List<string> checkList = new List<string>
                                {
                                    TokenPattern.FLOAT.ValueOfIt(), TokenPattern.INT.ValueOfIt()
                                };
                                if (_tokenVals[indexTokenIn].Equals(TokenPattern.RETURN) &&
                                    result[(string) _tokenVals[indexToken]].ContainsKey(TokenPattern.RETURN.Field))
                                {
                                    result.Add((string) _tokenVals[indexTokenIn],
                                        new Dictionary<string, List<string>>());
                                }
                                else if (checkList.Contains(_tokensExpressedInEnums[indexTokenIn - 1]))
                                {
                                    if (result[(string) _tokenVals[indexToken]]
                                        .ContainsKey((string) _tokenVals[indexTokenIn]))
                                        result[(string) _tokenVals[indexToken]].Add(
                                            (string) _tokenVals[indexTokenIn],
                                            new List<string>());
                                }
                                else
                                {
                                    throw new InvalidStatementException("Invalid statement: \nVariable " +
                                                                        _tokenVals[indexTokenIn] +
                                                                        " already exists in func " +
                                                                        _tokenVals[indexToken]);
                                }
                            }

                            if (_tokensExpressedInEnums[indexTokenIn + 1].Equals(TokenPattern.EQUAL))
                            {
                                if (result[(string) _tokenVals[indexToken]]
                                    .ContainsKey((string) _tokenVals[indexTokenIn]))
                                {
                                    List<string> val = new List<string>();
                                    result.Add((string) _tokenVals[indexToken],
                                        new Dictionary<string, List<string>>());
                                    for (var index = 0; index < _tokensExpressedInEnums.Count; index++)
                                    {
                                        var item = _tokensExpressedInEnums[index];
                                        if (index > indexTokenIn + 1)
                                        {
                                            if (item.Equals(TokenPattern.SEMICOLON.ValueOfIt())) break;
                                        }
                                        else if (item.Equals(TokenPattern.VARIABLE.ValueOfIt()))
                                        {
                                            if (result[(string) _tokenVals[indexToken]]
                                                .ContainsKey((string) _tokenVals[index]))
                                            {
                                                val.Add((string) _tokenVals[index]);
                                            }
                                            else
                                            {
                                                throw new InvalidStatementException("Invalid statement: variable " +
                                                    _tokenVals[index] +
                                                    " doesn`t exists in func " +
                                                    _tokenVals[indexToken]);
                                            }
                                        }
                                        else
                                        {
                                            val.Add((string) _tokenVals[index]);
                                        }

                                        result[(string) _tokenVals[indexToken]]
                                            .Add((string) _tokenVals[indexTokenIn], val);
                                    }
                                }
                                else
                                {
                                    throw new InvalidStatementException(
                                        "Invalid statement: variable " + _tokenVals[indexTokenIn] +
                                        " doesn`t exist in func " + _tokenVals[indexToken]);
                                }

                                if (_tokensExpressedInEnums[indexTokenIn]
                                    .Equals(TokenPattern.RETURN.ValueOfIt()))
                                {
                                    List<string> val = new List<string>();
                                    for (var index = 0; index < _tokensExpressedInEnums.Count; index++)
                                    {
                                        var expressedInEnum = _tokensExpressedInEnums[index];
                                        if (index > indexTokenIn)
                                        {
                                            if (expressedInEnum.Equals(TokenPattern.SEMICOLON.ValueOfIt()))
                                                break;
                                            if (expressedInEnum.Equals(TokenPattern.VARIABLE.ValueOfIt()))
                                            {
                                                if (result[(string) _tokenVals[indexToken]]
                                                    .ContainsKey((string) _tokenVals[index]))
                                                {
                                                    val.Add((string) _tokenVals[index]);
                                                }
                                                else
                                                {
                                                    throw new InvalidStatementException("Invalid code: variable " +
                                                        _tokenVals[index] +
                                                        " doesn`t exists in func " +
                                                        _tokenVals[indexToken]);
                                                }
                                            }
                                            else
                                            {
                                                val.Add((string) _tokenVals[index]);
                                            }

                                            result[(string) _tokenVals[indexToken]]
                                                .Add((string) _tokenVals[indexTokenIn], val);
                                        }
                                        else if (tokensExpressedInEnum.Equals(TokenPattern.CС_BRACE
                                            .ValueOfIt())) break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}