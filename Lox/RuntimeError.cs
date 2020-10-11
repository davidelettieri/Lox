using System;
using System.Collections.Generic;
using System.Text;

namespace Lox
{
    public class RuntimeError : Exception
    {
        public Token Token { get; }

        public RuntimeError(Token token, string message) : base(message)
        {
            Token = token;
        }
    }
}
