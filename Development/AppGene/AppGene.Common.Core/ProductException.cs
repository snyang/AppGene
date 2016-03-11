using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Common.Core
{
    public class ProductException
        : ApplicationException
    {
        public ProductException()
            : base()
        { }

        public ProductException(string message)
            : base(message)
        { }

        public ProductException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public ProductException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}
