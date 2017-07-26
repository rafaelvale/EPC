using System;
using System.Runtime.Serialization;

namespace Rohr.EPC.Business
{
    public class MyException : Exception
    {
        public MyException(){}
        public MyException(string message)
            : base(message)
        {

        }
        public MyException(string message, Exception inner)
            : base(message, inner)
        {

        }
        public MyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
