using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fingerprint
{
    public class MinHash : IDisposable
    {
        public MinHash(int universeSize, int numHashFunctions)
        {
            this.numHashFunctions = numHashFunctions;
            int u = BitsForUniverse(universeSize);
            GenerateHashFunctions(u);
        }

        public delegate uint Hash(int toHash);
        private int numHashFunctions = 0;
        public int NumHashFunctions
        {
            get { return numHashFunctions; }
        }

        private Hash[] hashFunctions;
        public Hash[] HashFunctions
        {
            get { return hashFunctions; }
        }

        private void GenerateHashFunctions(int u)
        {
            hashFunctions = new Hash[numHashFunctions];
            Random r = new Random(10);
            for (int i = 0; i < numHashFunctions; i++)
            {
                uint a = 0;
                while (a % 1 == 1 || a <= 0)
                    a = (uint)r.Next();
                uint b = 0;
                int maxb = 1 << u;
                while (b <= 0)
                    b = (uint)r.Next(maxb);
                hashFunctions[i] = x => QHash(x, a, b, u);
            }
        }

        private int BitsForUniverse(int universeSize)
        {
            return (int)Math.Truncate(Math.Log((double)universeSize, 2.0)) + 1;
        }

        private static uint QHash(int x, uint a, uint b, int u)
        {
            return (a * (uint)x + b) >> (32 - u);
        }

        public List<uint> GetMinHash(List<int> wordIds)
        {
            uint[] minHashes = new uint[numHashFunctions];
            for (int h = 0; h < numHashFunctions; h++)
            {
                minHashes[h] = int.MaxValue;
            }
            foreach (var id in wordIds)
            {
                for (int h = 0; h < numHashFunctions; h++)
                {
                    uint hash = hashFunctions[h](id);
                    minHashes[h] = Math.Min(minHashes[h], hash);
                }
            }
            return minHashes.ToList();
        }

        public double Similarity(List<uint> l1, List<uint> l2)
        {
            return Jaccard.Calc<uint>(l1, l2);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
            numHashFunctions = 0;
            hashFunctions = null;
        }
    }
}
