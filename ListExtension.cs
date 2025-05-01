using System.Collections.Generic;

namespace Hashira
{
    public static class ListExtension
    {
        private static System.Random rng = new System.Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1); // 0부터 n까지
                (list[n], list[k]) = (list[k], list[n]); // 스왑
            }
        }
    }
}