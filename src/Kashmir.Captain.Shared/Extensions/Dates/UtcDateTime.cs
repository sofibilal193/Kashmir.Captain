using System;

namespace Kashmir.Captain.Shared.Extensions
{
    public class UtcDateTime : IDateTime
    {
        public DateTime DateTimeNow => DateTime.UtcNow;

        public DateOnly DateNow => DateOnly.FromDateTime(DateTimeNow);

        public int Year => DateTimeNow.Year;

        public string ShortDateString => DateTimeNow.ToShortDateString();
    }
}
