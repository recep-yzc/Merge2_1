using System;

namespace _Game.Development.Static
{
    public static class EnumExtension
    {
        public static int ToInt<T>(this T value) where T : System.Enum
        {
            return Convert.ToInt32(value);
        }

        public static int ToAmount<T>() where T : System.Enum
        {
            return System.Enum.GetValues(typeof(T)).Length;
        }

        public static T[] ToArray<T>() where T : System.Enum
        {
            return (T[])System.Enum.GetValues(typeof(T));
        }
    }
}