using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KR
{
    public class Generator
    {
        private Dictionary<String, Dictionary<String, List<String>>> _varsForGeneration;

        public Generator(Dictionary<String, Dictionary<String, List<String>>> varsForGeneration)
        {
            this._varsForGeneration = varsForGeneration;
        }

        public string Generate()
        {
            var vars = new StringBuilder();
            var outcomeMasm = new StringBuilder();
            var valTokensForGeneration = new Dictionary<string, Dictionary<string, List<string>>>();

            var keysVar = _varsForGeneration.Keys;
            foreach (var key in keysVar)
            {
                valTokensForGeneration.Add(key, new Dictionary<string, List<string>>());
                var keysSetIn = _varsForGeneration[key].Keys;
                foreach (var keySetIn in keysSetIn)
                {
                    List<string> value = _varsForGeneration[key][keySetIn];
                    var arrayList = value
                        .Select(item => new TokenProcessor().MatchItemWithTokenPattern(item).ValueOfIt()).ToList();
                    valTokensForGeneration[key].Add(keySetIn, arrayList);
                }
            }

            foreach (var keyValuePair in valTokensForGeneration)
            {
                Dictionary<string, List<string>> value = keyValuePair.Value;
                foreach (var valuePair in value)
                {
                    for (var index = 0; index < valuePair.Value.Count; index++)
                    {
                        var item = valuePair.Value[index];
                        if (item.Equals(TokenPattern.VARIABLE.Field) && valuePair.Value.Count > 1)
                        {
                            if (valuePair.Value[index + 1].Equals(TokenPattern.OR_BRACKET.ValueOfIt()))
                            {
                                valuePair.Value[index] = TokenPattern.FUNCTION.ValueOfIt();
                            }
                        }
                    }
                }
            }

            int amount = 0;
            foreach (var keyValuePair in valTokensForGeneration)
            {
                var value = keyValuePair.Value;
                foreach (var valuePair in value)
                {
                    for (var a = 0; a < valuePair.Value.Count; a++)
                    {
                        var s = valuePair.Value[a];
                        if (s.Equals(TokenPattern.VARIABLE.ValueOfIt()))
                        {
                            List<string> list = _varsForGeneration[keyValuePair.Key][valuePair.Key];
                            _varsForGeneration[keyValuePair.Key][valuePair.Key][a] = list[a] + " " + keyValuePair.Key;
                        }
                    }

                    if (valuePair.Key.Equals(TokenPattern.RETURN.ValueOfIt()))
                    {
                        vars.Append(" solution_").Append(keyValuePair.Key).Append(" dd 0\n");
                    }
                    else
                    {
                        vars.Append(keyValuePair.Key).Append("_").Append(valuePair.Key).Append(" dd 0\n");
                    }
                }
            }

            var n = 0;
            var outcomeBlock = new StringBuilder();
            foreach (var keyValuePair in valTokensForGeneration)
            {
                var key = "";
                var appendix = 0;
                if (keyValuePair.Key.Equals(TokenPattern.MAIN.Field))
                {
                    outcomeBlock.Append(keyValuePair.Key).Append(" proc\n");
                    var value = keyValuePair.Value;
                    foreach (var stringListEntry in value)
                    {
                        if (stringListEntry.Value.Count != 0)
                        {
                            var flag = false;
                            if (stringListEntry.Value[0].Equals(TokenPattern.VARIABLE.ValueOfIt()))
                            {
                                outcomeBlock.Append("\tmov eax,")
                                    .Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [0])
                                    .Append("\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.CONST_INT.ValueOfIt()))

                            {
                                outcomeBlock.Append("\tmov eax,")
                                    .Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [0])
                                    .Append("\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                break;
                            }

                            else if (stringListEntry.Value[0].Equals(TokenPattern.EQUAL_L.ValueOfIt()))
                            {
                                n++;
                                outcomeBlock.Append("\tmov ebx,")
                                    .Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1])
                                    .Append("\n\tcmp eax,ebx\n\tje @start").Append(n)
                                    .Append("\n\tmov eax,0\n\tjmp @end").Append(n)
                                    .Append("\n\t@start").Append(n).Append(":\n\tmov eax,1\n\t@end").Append(n)
                                    .Append(":\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.QUEST_MARK.ValueOfIt()))
                            {
                                appendix++;
                                outcomeBlock.Append("\tcmp eax,1\n\tjne @false").Append(appendix).Append("\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.COLON.ValueOfIt()))
                            {
                                outcomeBlock.Append("\tjmp @exit\n\t@false").Append(appendix).Append(":\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                flag = true;
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.NOT_EQUAL.ValueOfIt()))
                            {
                                n++;
                                outcomeBlock.Append("\tmov ebx, {")
                                    .Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1])
                                    .Append("\n\tcmp eax,ebx\n\tje @true").Append(n)
                                    .Append("\n\tcmp eax,ebx\n\tje @true").Append(n)
                                    .Append("\n\t@true").Append(n).Append(":\n\tmov eax,0\n\t@exit").Append(n)
                                    .Append(":\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.ADDITION.ValueOfIt()))
                            {
                                outcomeBlock.Append("\tmov ebx,").Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1])
                                    .Append("\n\tadd eax,ebx\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.SUBTRACTION.ValueOfIt()))
                            {
                                outcomeBlock.Append("\tmov ebx,").Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1])
                                    .Append("\n\tsub eax,ebx\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.MULTIPLY.ValueOfIt()))
                            {
                                outcomeBlock.Append("\tmov ebx, ").Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1])
                                    .Append("\n\tmul ebx\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.DIVISION.ValueOfIt()))
                            {
                                outcomeBlock.Append("\tmov ecx,").Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1])
                                    .Append("\n\tcdq\n\tidiv ecx\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.GT.ValueOfIt()))
                            {
                                n++;
                                outcomeBlock.Append("\tmov ebx,").Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1])
                                    .Append("\n\tcmp eax,ebx\n\tjg @bigger").Append(n).Append(n)
                                    .Append("\n\tmov eax,0\n\tjmp @exit_bigger")
                                    .Append(n).Append("\n\t@bigger").Append(n)
                                    .Append(":\n\tmov eax,1\n\t@exit_bigger").Append(n).Append(":\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.LT.ValueOfIt()))
                            {
                                n++;
                                outcomeBlock.Append("\tmov ebx,").Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1])
                                    .Append("\n\tcmp eax,ebx\n\tjl @lesser").Append(n)
                                    .Append("\n\tmov eax,0\n\tjmp @exit_lesser").Append(n)
                                    .Append("\n\t@lesser").Append(n).Append(":\n\tmov eax,1\n\t@exit_lesser")
                                    .Append(n).Append(":\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.LEFT_SHIFT.ValueOfIt()))
                            {
                                outcomeBlock.Append("\tmov ebx,").Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1])
                                    .Append("\n\tmov cl,ebx\n\tshl eax,cl\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.RIGHT_SHIFT.ValueOfIt()))
                            {
                                outcomeBlock.Append("\tmov ebx,").Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1])
                                    .Append("\n\tmov cl,ebx\n\tshr eax,cl\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);

                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.BITWISE_XOR.ValueOfIt()))
                            {
                                outcomeBlock.Append("\tmov ebx,").Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1])
                                    .Append("}\n\txor eax,ebx\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.BITWISE_OR.ValueOfIt()))
                            {
                                outcomeBlock.Append("\tmov ebx,").Append(_varsForGeneration
                                        [keyValuePair.Key]
                                    [stringListEntry.Key]
                                    [1]).Append("\n\tor eax,ebx\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);

                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);

                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.BITWISE_AND.ValueOfIt()))
                            {
                                outcomeBlock.Append("\tmov ebx,").Append(_varsForGeneration
                                        [keyValuePair.Key]
                                    [stringListEntry.Key]
                                    [1]).Append("\n\tand eax,ebx\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);

                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);

                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.GTE.ValueOfIt()))
                            {
                                n++;
                                outcomeBlock.Append("\tmov ebx,").Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1]).Append("\n\tcmp eax,ebx\n\tjge @big_eq").Append(n)
                                    .Append("\n\tmov eax,0\n\tjmp @exit_big_eq").Append(n)
                                    .Append("\n\t@big_eq").Append(n).Append(":\n\tmov eax,1\n\t@exit_big_eq")
                                    .Append(n).Append(":\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);

                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.LTE.ValueOfIt()))
                            {
                                n++;
                                outcomeBlock.Append("\tmov ebx,").Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1]).Append("\n\tcmp eax,ebx\n\tjle @les_eq").Append(n)
                                    .Append("\n\tmov eax,0\n\tjmp @exit_les_eq")
                                    .Append(n).Append(":\n\tmov eax,1\n\t@exit_les_eq").Append(n).Append(":\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);

                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.MODULO.ValueOfIt()))
                            {
                                outcomeBlock.Append("\tmov ecx,").Append(_varsForGeneration
                                        [keyValuePair.Key]
                                    [stringListEntry.Key]
                                    [1]).Append("\n\tcdq\n\tidiv ecx\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);

                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);

                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.OR.ValueOfIt()))
                            {
                                n++;
                                outcomeBlock.Append("\tmov ebx,").Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1]).Append("\n\tcmp eax,0\n\tje @start")
                                    .Append(n).Append("\n\tjmp @end").Append(n)
                                    .Append("\n\t@start").Append(n).Append(":\n\tmov eax,ebx\n\t@end").Append(n)
                                    .Append(":\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);

                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.AND.ValueOfIt()))
                            {
                                n++;
                                outcomeBlock.Append("\tmov ebx,").Append(_varsForGeneration
                                            [keyValuePair.Key]
                                        [stringListEntry.Key]
                                        [1]).Append("\n\tcmp eax,0\n\tje @start").Append(n)
                                    .Append("\n\tcmp ebx,0\n\tje @start").Append(n).Append("_").Append(n)
                                    .Append("\n\tmov eax,ebx\n\tjmp @end").Append(n).Append("\n\t@start")
                                    .Append(n).Append(":\n\t@start").Append(n).Append("_").Append(n)
                                    .Append(":\n\tmov eax,0\n\t@end").Append(n).Append(":\n");
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);

                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);

                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.FUNCTION.ValueOfIt()))
                            {
                                amount = 0;
                                key = _varsForGeneration
                                        [keyValuePair.Key]
                                    [stringListEntry.Key]
                                    [0];
                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);

                                stringListEntry.Value.RemoveAt(0);
                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.COMMA.ValueOfIt()))
                            {
                                foreach (var listEntry in _varsForGeneration[key])
                                {
                                    for (var f = 0; f < listEntry.Value.Count; f++)
                                    {
                                        var l = listEntry.Value[f];
                                        if (amount == f)
                                        {
                                            outcomeBlock.Append("\tmov ").Append(l).Append("_").Append(key)
                                                .Append(", eax\n");
                                        }
                                    }
                                }

                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                stringListEntry.Value.RemoveAt(0);
                                amount++;
                                break;
                            }
                            else if (stringListEntry.Value[0].Equals(TokenPattern.CR_BRACKET.ValueOfIt()))
                            {
                                foreach (var listEntry in _varsForGeneration[key])
                                {
                                    for (var f = 0; f < listEntry.Value.Count; f++)
                                    {
                                        var l = listEntry.Value[f];
                                        if (f == 2)
                                        {
                                            outcomeBlock.Append("\tmov ").Append(l).Append("_").Append(key)
                                                .Append(",eax \n");
                                        }
                                    }
                                }

                                _varsForGeneration[keyValuePair.Key][stringListEntry.Key].RemoveAt(0);
                                stringListEntry.Value.RemoveAt(0);
                                outcomeBlock.Append("\tcall ").Append(key).Append("\n\tmov eax, solution_").Append(key)
                                    .Append("\n");
                                break;
                            }

                            if (flag) outcomeBlock.Append("@exit:\n");
                            if (!keyValuePair.Key.Equals("return"))
                                outcomeBlock.Append("\tmov ")
                                    .Append(keyValuePair.Key)
                                    .Append("_")
                                    .Append(stringListEntry.Key)
                                    .Append(",eax\n");
                            else outcomeBlock.Append("\tmov  solution_").Append(stringListEntry.Key).Append(",eax\n");
                        }

                        appendix++;
                    }

                    appendix++;
                    outcomeBlock.Append("\tfn MessageBox,0,").Append("str$(solution_main)," + "Sliusarenko")
                        .Append(", MB_OK\n\tret\n").Append(keyValuePair.Key).Append(" endp\n\n");
                }
                else
                {
                    outcomeMasm.Append(keyValuePair.Key).Append(" proc\n");
                    foreach (var keyValPair in valTokensForGeneration)
                    {
                        foreach (var stringListEntry in keyValPair.Value)
                        {
                            if (stringListEntry.Value.Count != 0)
                            {
                                var flag = false;
                                if (stringListEntry.Value[0].Equals(TokenPattern.CONST_INT.ValueOfIt()))
                                {
                                    outcomeMasm.Append("\tmov eax,")
                                        .Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [0])
                                        .Append("\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.VARIABLE.ValueOfIt()))
                                {
                                    outcomeBlock.Append("\tmov eax,")
                                        .Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [0])
                                        .Append("\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.QUEST_MARK.ValueOfIt()))
                                {
                                    appendix++;
                                    outcomeMasm.Append("\tcmp eax,1\n\tjne @false").Append(appendix).Append("\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.EQUAL_L.ValueOfIt()))
                                {
                                    n++;
                                    outcomeMasm.Append("\tmov ebx,")
                                        .Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1])
                                        .Append("\n\tcmp eax,ebx\n\tje @start").Append(n)
                                        .Append("\n\tmov eax,0\n\tjmp @end").Append(n)
                                        .Append("\n\t@start").Append(n).Append(":\n\tmov eax,1\n\t@end").Append(n)
                                        .Append(":\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.NOT_EQUAL.ValueOfIt()))
                                {
                                    n++;
                                    outcomeMasm.Append("\tmov ebx, {")
                                        .Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1])
                                        .Append("\n\tcmp eax,ebx\n\tje @true").Append(n)
                                        .Append("\n\tcmp eax,ebx\n\tje @true").Append(n)
                                        .Append("\n\t@true").Append(n).Append(":\n\tmov eax,0\n\t@exit").Append(n)
                                        .Append(":\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }

                                else if (stringListEntry.Value[0].Equals(TokenPattern.COLON.ValueOfIt()))
                                {
                                    outcomeMasm.Append("\tjmp @exit\n\t@false").Append(appendix).Append(":\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                    flag = true;
                                }

                                else if (stringListEntry.Value[0].Equals(TokenPattern.ADDITION.ValueOfIt()))
                                {
                                    outcomeMasm.Append("\tmov ebx,").Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1])
                                        .Append("\n\tadd eax,ebx\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.SUBTRACTION.ValueOfIt()))
                                {
                                    outcomeMasm.Append("\tmov ebx,").Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1])
                                        .Append("\n\tsub eax,ebx\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.MULTIPLY.ValueOfIt()))
                                {
                                    outcomeMasm.Append("\tmov ebx, ").Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1])
                                        .Append("\n\tmul ebx\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.DIVISION.ValueOfIt()))
                                {
                                    outcomeMasm.Append("\tmov ecx,").Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1])
                                        .Append("\n\tcdq\n\tidiv ecx\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.GT.ValueOfIt()))
                                {
                                    n++;
                                    outcomeMasm.Append("\tmov ebx,").Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1])
                                        .Append("\n\tcmp eax,ebx\n\tjg @bigger").Append(n).Append(n)
                                        .Append("\n\tmov eax,0\n\tjmp @exit_bigger")
                                        .Append(n).Append("\n\t@bigger").Append(n)
                                        .Append(":\n\tmov eax,1\n\t@exit_bigger").Append(n).Append(":\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.LT.ValueOfIt()))
                                {
                                    n++;
                                    outcomeMasm.Append("\tmov ebx,").Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1])
                                        .Append("\n\tcmp eax,ebx\n\tjl @lesser").Append(n)
                                        .Append("\n\tmov eax,0\n\tjmp @exit_lesser").Append(n)
                                        .Append("\n\t@lesser").Append(n).Append(":\n\tmov eax,1\n\t@exit_lesser")
                                        .Append(n).Append(":\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.RIGHT_SHIFT.ValueOfIt()))
                                {
                                    outcomeMasm.Append("\tmov ebx,").Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1])
                                        .Append("\n\tmov cl,ebx\n\tshr eax,cl\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.BITWISE_XOR.ValueOfIt()))
                                {
                                    outcomeMasm.Append("\tmov ebx,").Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1])
                                        .Append("}\n\txor eax,ebx\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.MODULO.ValueOfIt()))
                                {
                                    outcomeMasm.Append("\tmov ecx,").Append(_varsForGeneration
                                            [keyValPair.Key]
                                        [stringListEntry.Key]
                                        [1]).Append("\n\tcdq\n\tidiv ecx\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.BITWISE_OR.ValueOfIt()))
                                {
                                    outcomeMasm.Append("\tmov ebx,").Append(_varsForGeneration
                                            [keyValPair.Key]
                                        [stringListEntry.Key]
                                        [1]).Append("\n\tor eax,ebx\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.BITWISE_AND.ValueOfIt()))
                                {
                                    outcomeMasm.Append("\tmov ebx,").Append(_varsForGeneration
                                            [keyValPair.Key]
                                        [stringListEntry.Key]
                                        [1]).Append("\n\tand eax,ebx\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.GTE.ValueOfIt()))
                                {
                                    n++;
                                    outcomeMasm.Append("\tmov ebx,").Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1]).Append("\n\tcmp eax,ebx\n\tjge @big_eq").Append(n)
                                        .Append("\n\tmov eax,0\n\tjmp @exit_big_eq").Append(n)
                                        .Append("\n\t@big_eq").Append(n).Append(":\n\tmov eax,1\n\t@exit_big_eq")
                                        .Append(n).Append(":\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.OR.ValueOfIt()))
                                {
                                    n++;
                                    outcomeMasm.Append("\tmov ebx,").Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1]).Append("\n\tcmp eax,0\n\tje @start")
                                        .Append(n).Append("\n\tjmp @end").Append(n)
                                        .Append("\n\t@start").Append(n).Append(":\n\tmov eax,ebx\n\t@end").Append(n)
                                        .Append(":\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.LTE.ValueOfIt()))
                                {
                                    n++;
                                    outcomeMasm.Append("\tmov ebx,").Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1]).Append("\n\tcmp eax,ebx\n\tjle @les_eq").Append(n)
                                        .Append("\n\tmov eax,0\n\tjmp @exit_les_eq")
                                        .Append(n).Append(":\n\tmov eax,1\n\t@exit_les_eq").Append(n).Append(":\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.LEFT_SHIFT.ValueOfIt()))
                                {
                                    outcomeMasm.Append("\tmov ebx,").Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1])
                                        .Append("\n\tmov cl,ebx\n\tshl eax,cl\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }

                                else if (stringListEntry.Value[0].Equals(TokenPattern.AND.ValueOfIt()))
                                {
                                    n++;
                                    outcomeMasm.Append("\tmov ebx,").Append(_varsForGeneration
                                                [keyValPair.Key]
                                            [stringListEntry.Key]
                                            [1]).Append("\n\tcmp eax,0\n\tje @start").Append(n)
                                        .Append("\n\tcmp ebx,0\n\tje @start").Append(n).Append("_").Append(n)
                                        .Append("\n\tmov eax,ebx\n\tjmp @end").Append(n).Append("\n\t@start")
                                        .Append(n).Append(":\n\t@start").Append(n).Append("_").Append(n)
                                        .Append(":\n\tmov eax,0\n\t@end").Append(n).Append(":\n");
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.FUNCTION.ValueOfIt()))
                                {
                                    amount = 0;
                                    key = _varsForGeneration
                                            [keyValPair.Key]
                                        [stringListEntry.Key]
                                        [0];
                                    stringListEntry.Value.RemoveAt(0);
                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.COMMA.ValueOfIt()))
                                {
                                    foreach (var listEntry in _varsForGeneration[key])
                                    {
                                        for (var f = 0; f < listEntry.Value.Count; f++)
                                        {
                                            var l = listEntry.Value[f];
                                            if (amount == f)
                                            {
                                                outcomeMasm.Append("\tmov ").Append(l).Append("_").Append(key)
                                                    .Append(", eax\n");
                                            }
                                        }
                                    }

                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                    stringListEntry.Value.RemoveAt(0);
                                    amount++;
                                }
                                else if (stringListEntry.Value[0].Equals(TokenPattern.CR_BRACKET))
                                {
                                    foreach (var listEntry in _varsForGeneration[key])
                                    {
                                        for (var index = 0; index < listEntry.Value.Count; index++)
                                        {
                                            var value = listEntry.Value[index];
                                            if (index == 2)
                                            {
                                                outcomeMasm.Append("\tmov ").Append(value).Append("_").Append(key)
                                                    .Append(",eax \n");
                                            }
                                        }
                                    }

                                    _varsForGeneration[keyValPair.Key][stringListEntry.Key].RemoveAt(0);
                                    stringListEntry.Value.RemoveAt(0);
                                    outcomeMasm.Append("\tcall ").Append(key).Append("\n\tmov eax, solution_")
                                        .Append(key)
                                        .Append("\n");
                                }

                                if (flag) outcomeMasm.Append("@exit:\n");
                                if (!keyValPair.Key.Equals("return"))
                                    outcomeBlock.Append("\tmov ")
                                        .Append(keyValPair.Key)
                                        .Append("_")
                                        .Append(stringListEntry.Key)
                                        .Append(",eax\n");
                                else
                                    outcomeMasm.Append("\tmov  solution_").Append(stringListEntry.Key).Append(",eax\n");
                            }

                            appendix++;
                        }

                        appendix++;
                        outcomeMasm.Append("\tret\n").Append(keyValPair.Key).Append(" endp\n\n");
                    }
                }
            }

            return GenerateUsingTemplate(vars, outcomeBlock, outcomeMasm);
        }

        private static string GenerateUsingTemplate(StringBuilder vars, StringBuilder outcomeBlock,
            StringBuilder outcomeMasm)
        {
            var template = new StringBuilder();
            template.Append(".386\n")
                .Append(".model flat, stdcall\n")
                .Append("option Dictionary:none\n")
                .Append("include \\masm32\\include\\masm32rt.inc\n")
                .Append(".data\n")
                .Append(vars).Append("\n")
                .Append(".code\n")
                .Append("start:\n")
                .Append(outcomeBlock).Append("\n")
                .Append(outcomeMasm).Append("\n")
                .Append("invoke main\n")
                .Append("invoke ExitProcess, 0\n")
                .Append("END start");
            return template.ToString();
        }
    }
}