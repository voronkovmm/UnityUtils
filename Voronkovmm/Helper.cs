using System;

namespace Voronkovmm
{
    public static class Helper
    {
        public static T With<T>(this T self, Action<T> set)
        {
            set.Invoke(self);
            return self;
        }
    } 
}