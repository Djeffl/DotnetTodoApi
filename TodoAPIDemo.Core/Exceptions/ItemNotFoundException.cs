using System;
using System.Collections.Generic;
using System.Text;

namespace TodoAPIDemo.Core.Exceptions
{
    class ItemNotFoundException: Exception
    {
        public ItemNotFoundException() : base("Item not found")
        {

        }
    }
}
