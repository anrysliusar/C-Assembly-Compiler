using System;
using System.Collections.Generic;
using System.Text;

namespace KR
{
    public class UnsuspectedTokenException : Exception
    {
        public UnsuspectedTokenException(List<object> list, int index)
        {
            var whereItHappened = new StringBuilder();
            string signValue = null;
            
            for (var i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (i == index) {
                    signValue = (string) item;
                    if (signValue.Equals(TokenPattern.SEMICOLON.Field)) {
                        whereItHappened.Append(item).Append("\n");
                    } else {
                        whereItHappened.Append(item);
                    }
                } else if (item.Equals(TokenPattern.SEMICOLON.Field) || 
                           item.Equals(TokenPattern.CС_BRACE.Field)) {
                    whereItHappened.Append(item).Append("\n");
                } else if (item.Equals(TokenPattern.OС_BRACE.Field)) {
                    whereItHappened.Append("\n").Append(item).Append("\n");
                } else {
                    whereItHappened.Append(" ").Append(item);
                }
            }

            Console.WriteLine("Unsuspected symbol " + signValue);
            Console.WriteLine("Caused at " + whereItHappened);
            Console.ReadLine();
        }
    }

    public class InvalidStatementException : Exception
    {
        public InvalidStatementException(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }
    }
}
