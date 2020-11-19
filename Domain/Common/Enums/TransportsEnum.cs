using System;

namespace Domain.Common.Enums
{
    [Flags]
    public enum TransportsEnum
    {
        None = 0,
        Bus = 1 << 0,
        Train = 1 << 1,
        Tramvway = 1 << 2,
        Taxi = 1 << 3,
        Underground = 1 << 4
    }
}