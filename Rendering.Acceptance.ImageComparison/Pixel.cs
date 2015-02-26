using System;

namespace Rendering.Acceptance.ImageComparison
{
    public struct Pixel : IEquatable<Pixel>
    {
        public struct Difference
        {
            private readonly byte _bDifference;
            private readonly byte _gDifference;
            private readonly byte _rDifference;

            internal Difference(byte rDifference, byte gDifference, byte bDifference)
                : this()
            {
                _rDifference = rDifference;
                _gDifference = gDifference;
                _bDifference = bDifference;
            }

            public byte RDifference
            {
                get { return _rDifference; }
            }

            public byte GDifference
            {
                get { return _gDifference; }
            }

            public byte BDifference
            {
                get { return _bDifference; }
            }
        }

        private readonly byte _b;
        private readonly byte _g;
        private readonly byte _r;

        internal Pixel(byte r, byte g, byte b)
        {
            _r = r;
            _g = g;
            _b = b;
        }

        public bool Equals(Pixel other)
        {
            return _b == other._b && _g == other._g && _r == other._r;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Pixel && Equals((Pixel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = _b.GetHashCode();
                hashCode = (hashCode * 397) ^ _g.GetHashCode();
                hashCode = (hashCode * 397) ^ _r.GetHashCode();
                return hashCode;
            }
        }

        public static Difference operator -(Pixel lhs, Pixel rhs)
        {
            return new Difference((byte)Math.Abs(lhs._r - rhs._r), (byte)Math.Abs(lhs._g - rhs._g), (byte)Math.Abs(lhs._b - rhs._b));
        }
    }
}
