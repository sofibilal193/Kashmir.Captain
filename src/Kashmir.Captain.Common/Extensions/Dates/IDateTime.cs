using System;

namespace Kashmir.Captain.Common.Extensions
{
    public interface IDateTime
    {
        DateTime Now { get; }

        DateOnly DateNow { get; }

        int Year { get; }

        string ShortDateString { get; }
    }
}
