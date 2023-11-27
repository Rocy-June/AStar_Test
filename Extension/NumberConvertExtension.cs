namespace Extension
{
    public static class NumberConvertExtension
    {
        public static int ToInt32(this double value)
        { 
            return Convert.ToInt32(value);
        }
    }
}