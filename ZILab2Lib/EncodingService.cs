using System;
using System.Linq;

namespace ZILab2Lib
{
    public class EncodingService
    {
        

        public bool ValidateKey(string key)
        {
            return key.All(e => ValidateKeyChar(Convert.ToInt32(e.ToString())));
        }

        private byte CyclicBitShift(byte a, byte s) => (byte)((a >> s) | ((a << (8 - s)) & 255));

        private byte ShiftByte(byte b, int i, byte[] key) 
        {
            const byte evenMask = 0b01010101;
            const byte oddMask = 0b10101010;
            var bEven = b & evenMask;
            var bOdd = b & oddMask;
            return CyclicBitShift((byte)((bEven << 1) | (bOdd >> 1)), key[i % key.Length]);
        }

        private byte UnshiftByte(byte b, int i, byte[] key)
        {
            var unshiftedB = CyclicBitShift(b, (byte)(8 - key[i % key.Length]));

            const byte evenMask = 0b01010101;
            const byte oddMask = 0b10101010;

            var bEven = unshiftedB & evenMask;
            var bOdd = unshiftedB & oddMask;

            return (byte)((bEven << 1) | (bOdd >> 1));
        }

        public byte[] Encode(byte[] data, string key)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var intKey = GetIntKey(key);
            return data.Select((b, i) => ShiftByte(b, i, intKey)).ToArray();
        }

        public byte[] Decode(byte[] encodedData, string key)
        {
            if (encodedData == null)
            {
                throw new ArgumentNullException(nameof(encodedData));
            }
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var intKey = GetIntKey(key);
            return encodedData.Select((b, i) => UnshiftByte(b, i, intKey)).ToArray();
        }

        private bool ValidateKeyChar(int shift)
        {
            if (shift < 0 || shift > 7)
            {
                return false;
            }

            return true;
        }

        private byte[] GetIntKey(string key) { 
            return key.Select(c => Convert.ToByte(c.ToString())).ToArray();
        }
    }
}
