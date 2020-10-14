using System;

namespace Lox
{
    public class Return : Exception
    {
        public object Value;

        public Return(object value) : base()
        {
            Value = value;
        }
    }
}
