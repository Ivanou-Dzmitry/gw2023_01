using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectsController : MonoBehaviour
{




   

    public class LFSR_Fibonacci
    {
        private static uint S = 0x00000001;

        public static uint Generate()
        {
            S = ((((S >> 31) ^ (S >> 30) ^ (S >> 29) ^ (S >> 27) ^ (S >> 25) ^ S) & 0x00000001) << 31) | (S >> 1);
            return S & 0x00000001;
        }
    }

    public class LFSR
    {
        public static uint LfsrFib()
        {
            ushort startState = 0xACE1;  // Any nonzero start state will work.
            ushort lfsr = startState;
            ushort bit;                    // Must be 16-bit to allow bit<<15 later in the code
            uint period = 0;
            do
            {
                // taps: 16 14 13 11; feedback polynomial: x^16 + x^14 + x^13 + x^11 + 1
                bit = (ushort)(((lfsr >> 0) ^ (lfsr >> 2) ^ (lfsr >> 3) ^ (lfsr >> 5)) & 1u);
                lfsr = (ushort)((lfsr >> 1) | (bit << 15));
                period++;
            }
            while (lfsr != startState);
            return period;
        }
    }

    public class LFSR_Galois
    {
        private static uint S = 0x00000001;

        public static int Generate()
        {
            if ((S & 0x00000001) != 0)
            {
                S = ((S ^ 0x80000057) >> 1) | 0x80000000;
                return 1;
            }
            else
            {
                S >>= 1;
                return 0;
            }
        }
    }


}
