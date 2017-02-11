using System;
using System.IO;

namespace YouTown
{
    /// <summary>
    /// Extend Bond with int64 ⟷ DateTime conversion
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/bond/manual/bond_cs.html#converter"/>
    public class BondTypeAliasConverter
    {
        #region DateTime
        /// <see cref="https://github.com/Microsoft/bond/blob/master/examples/cs/core/date_time/program.cs"/>
        public static long Convert(DateTime value, long unused)
        {
            return value.Ticks;
        }

        public static DateTime Convert(long value, DateTime unused)
        {
            return new DateTime(value);
        }
        #endregion

        #region Guid
        /// <see cref="https://github.com/Microsoft/bond/blob/master/examples/cs/core/guid/program.cs"/>
        public static Guid Convert(ArraySegment<byte> value, Guid unused)
        {
            if (value.Count != 16)
            {
                throw new InvalidDataException("value must be of length 16");
            }

            byte[] array = value.Array;
            int offset = value.Offset;

            int a =
                  ((int)array[offset + 3] << 24)
                | ((int)array[offset + 2] << 16)
                | ((int)array[offset + 1] << 8)
                | array[offset];
            short b = (short)(((int)array[offset + 5] << 8) | array[offset + 4]);
            short c = (short)(((int)array[offset + 7] << 8) | array[offset + 6]);

            return new Guid(a, b, c,
                array[offset + 8],
                array[offset + 9],
                array[offset + 10],
                array[offset + 11],
                array[offset + 12],
                array[offset + 13],
                array[offset + 14],
                array[offset + 15]);
        }

        public static ArraySegment<byte> Convert(Guid value, ArraySegment<byte> unused)
        {
            return new ArraySegment<byte>(value.ToByteArray());
        }
        #endregion
    }
}
