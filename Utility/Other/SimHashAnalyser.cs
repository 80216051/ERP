using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Other
{
    /// <summary>
    /// SimHash相似度比较
    /// </summary>
    public class SimHashAnalyser : IAnalyser
    {
        #region Constants and Fields

        private const int HashSize = 32;

        #endregion

        #region Public Methods and Operators

        public float GetLikenessValue(string needle, string haystack)
        {
            var needleSimHash = this.DoCalculateSimHash(needle);
            var hayStackSimHash = this.DoCalculateSimHash(haystack);
            return (HashSize - GetHammingDistance(needleSimHash, hayStackSimHash)) / (float)HashSize;
        }

        #endregion

        #region Methods

        private static IEnumerable<int> DoHashTokens(IEnumerable<string> tokens)
        {
            var hashedTokens = new List<int>();
            foreach (string token in tokens)
            {
                hashedTokens.Add(token.GetHashCode());
            }
            return hashedTokens;
        }

        private static int GetHammingDistance(int firstValue, int secondValue)
        {
            var hammingBits = firstValue ^ secondValue;
            var hammingValue = 0;
            for (int i = 0; i < 32; i++)
            {
                if (IsBitSet(hammingBits, i))
                {
                    hammingValue += 1;
                }
            }
            return hammingValue;
        }

        private static bool IsBitSet(int b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }

        private int DoCalculateSimHash(string input)
        {
            ITokeniser tokeniser = new OverlappingStringTokeniser(4, 3);
            var hashedtokens = DoHashTokens(tokeniser.Tokenise(input));
            var vector = new int[HashSize];
            for (var i = 0; i < HashSize; i++)
            {
                vector[i] = 0;
            }

            foreach (var value in hashedtokens)
            {
                for (var j = 0; j < HashSize; j++)
                {
                    if (IsBitSet(value, j))
                    {
                        vector[j] += 1;
                    }
                    else
                    {
                        vector[j] -= 1;
                    }
                }
            }

            var fingerprint = 0;
            for (var i = 0; i < HashSize; i++)
            {
                if (vector[i] > 0)
                {
                    fingerprint += 1 << i;
                }
            }
            return fingerprint;
        }

        #endregion
    }



    public interface IAnalyser
    {
        float GetLikenessValue(string needle, string haystack);
    }

    public interface ITokeniser
    {
        IEnumerable<string> Tokenise(string input);
    }

    public class FixedSizeStringTokeniser : ITokeniser
    {
        private readonly ushort tokensize = 5;
        public FixedSizeStringTokeniser(ushort tokenSize)
        {
            if (tokenSize < 2 || tokenSize > 127)
            {
                throw new ArgumentException("Token 不能超出范围");
            }
            this.tokensize = tokenSize;
        }

        public IEnumerable<string> Tokenise(string input)
        {
            var chunks = new List<string>();
            int offset = 0;
            while (offset < input.Length)
            {
                chunks.Add(new string(input.Skip(offset).Take(this.tokensize).ToArray()));
                offset += this.tokensize;
            }
            return chunks;
        }

    }


    public class OverlappingStringTokeniser : ITokeniser
    {

        private readonly ushort chunkSize = 4;
        private readonly ushort overlapSize = 3;

        public OverlappingStringTokeniser(ushort chunkSize, ushort overlapSize)
        {
            if (chunkSize <= overlapSize)
            {
                throw new ArgumentException("Chunck 必须大于 overlap");
            }
            this.overlapSize = overlapSize;
            this.chunkSize = chunkSize;
        }

        public IEnumerable<string> Tokenise(string input)
        {
            var result = new List<string>();
            int position = 0;
            while (position < input.Length - this.chunkSize)
            {
                result.Add(input.Substring(position, this.chunkSize));
                position += this.chunkSize - this.overlapSize;
            }
            return result;
        }


    }
}
