using System;
using System.Collections.Generic;

namespace ChatAPI.Helper
{

    public class DelegatedEqualityComparer<T> : EqualityComparer<T>
    {
        private Func<T, int> getHashCode;
        private Func<T, T, bool> equals;
        public DelegatedEqualityComparer(Func<T, int> getHashCode, Func<T, T, bool> equals)
        {
            if (getHashCode == null) throw new ArgumentNullException(nameof(getHashCode));
            if (equals == null) throw new ArgumentNullException(nameof(equals));
            this.getHashCode = getHashCode;
            this.equals = equals;
        }
        public override int GetHashCode(T x) => getHashCode(x);
        public override bool Equals(T x, T y) => equals(x, y);
    }
}
