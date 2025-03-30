using UnityEngine;

namespace Hashira.Utils
{
    public static class StringUtility
    {
        public static string Replace(this string text, string target, string value, Color color)
        {
            return text.Replace(target, value);
        }
    }
}
