using Globo.PIC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Globo.PIC.Domain.Models
{
    public class Location
    {
        public List<items> items { get; set; }

        public object Where(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
