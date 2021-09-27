using System;

namespace ChatAPI.Helper
{
    public static class StringUtils
    {
        public static byte[] TrimEnd(byte[] array)
        {
            int lastIndex = Array.FindLastIndex(array, b => b != 0);

            Array.Resize(ref array, lastIndex + 1);

            return array;
        }

        public static string BufferToString(byte[] buffer)
        {
            return System.Text.Encoding.Default.GetString(buffer)
                            .Replace("\n", "")
                            .Replace("\t", "")
                            .Replace("\r", "")
                            .Trim();
        }
    }
}