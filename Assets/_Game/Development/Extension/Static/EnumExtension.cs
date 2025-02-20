using System;

namespace _Game.Development.Extension.Static
{
    public static class EnumExtension
    {
        public static int ToInt<T>(this T value) where T : Enum
        {
            return Convert.ToInt32(value);
        }

        public static T[] ToArray<T>() where T : Enum
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}