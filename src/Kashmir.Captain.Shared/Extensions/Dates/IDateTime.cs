using System;

namespace Kashmir.Captain.Shared.Extensions
{
    public interface IDateTime
    {
        DateTime DateTimeNow { get; }

        DateOnly DateNow { get; }

        int Year { get; }

        string ShortDateString { get; }
    }
}
