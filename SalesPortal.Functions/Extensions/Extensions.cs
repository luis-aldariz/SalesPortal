using System;
namespace SalesPortal.Functions.Extensions
{
    public static class Extensions
    {
        public static double RoundNearestFiveCents(this double value) => Math.Ceiling(value / .05) * .05;
    }
}
