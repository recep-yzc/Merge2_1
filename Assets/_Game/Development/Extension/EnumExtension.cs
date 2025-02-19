using _Game.Development.Item;

namespace _Game.Development.Extension
{
    public static class EnumExtension
    {
        public static int ToInt(this ItemType value)
        {
            return (int)value;
        }

        public static int ToInt(this ProductType value)
        {
            return (int)value;
        }

        public static int ToInt(this GeneratorType value)
        {
            return (int)value;
        }
    }
}