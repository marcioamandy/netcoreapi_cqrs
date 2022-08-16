using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities.Dashboard
{
    public class Dashboard
    {
        public string Key { get; set; }
        public string Label { get; set; }
        public dynamic Value { get; set; }
    }
}
