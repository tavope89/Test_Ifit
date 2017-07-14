using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestKarkov.Models
{
    public class RuleOrder
    {
        public int order { get; set; }
        public int rule { get; set; }
        public bool isTermination { get; set; }
    }
}