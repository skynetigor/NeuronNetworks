using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRecognizer.Implementation.ImageFilters
{
    class OrderAttribute: Attribute
    {
        public OrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; }
    }
}
