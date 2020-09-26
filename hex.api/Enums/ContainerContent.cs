using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hex.api.Enums
{
    public enum ContainerContent
    {
        Empty = 0,
        Reserved = 1,
        ReadyToFill = 2,
        Filled = 3,
        ReadyAndWeighted = 4
    }
}
