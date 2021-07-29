using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fleximand.Core
{
#nullable enable
    public class FmdExecEventArgs : EventArgs
    {
        public string CommandName { get; set; } = "";

        public string Message { get; set; } = "";

        public object? CommandParameter { get; set; }
    }
#nullable restore
}
