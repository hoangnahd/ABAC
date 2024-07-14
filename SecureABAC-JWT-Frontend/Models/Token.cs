using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABAC_Fe.Models
{
    public class Token
    {
        public bool Success { get; set; }
        public string TokenValue { get; set; }
        public string Message { get; set; }
    }
}
