using System;

namespace Hashira.Core
{
    [Serializable]
    public class Pair<T, U>
    {
        public T first;
        public U second;

        public Pair(T firstValue = default, U secondValue = default)
        {
            first = firstValue;
            second = secondValue;
        }

        public U FindPair(T key)
        {
            if (key.Equals(first))
                return second;
            else
                return default;
        }
        public T FindPair(U key)
        {
            if (key.Equals(second))
                return first;
            else
                return default;
        }
        public bool TryFindPair(T key, out U pairValue)
        {
            if (key.Equals(first))
            {
                pairValue = second;
                return true;
            }
            else
            {
                pairValue = default;
                return false;
            }
        }
        public bool TryFindPair(U key, out T pairValue)
        {
            if (key.Equals(second))
            {
                pairValue = first;
                return true;
            }
            else
            {
                pairValue = default;
                return false;
            }
        }
    }
}