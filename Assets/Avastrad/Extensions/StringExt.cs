namespace Avastrad.Extensions
{
    public static class StringExt
    {
        public static bool IsNullOrEmpty(this string str) 
            => string.IsNullOrEmpty(str);
        
        public static bool Contains(this string[] array, string str)
        {
            if (array == null)
                return false;
            
            for (var i = 0; i < array.Length; i++)
                if (array[i] == str)
                    return true;

            return false;
        }
    }
}