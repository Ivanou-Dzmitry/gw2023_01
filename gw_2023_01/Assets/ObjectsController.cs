using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectsController : MonoBehaviour
{
    private Component[] ObjectFrames;
    private float ObjectTime;
    private int Digit, MyShift, MyShift2;

    List<GameObject> F1 = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        ObjectFrames = GetComponentsInChildren<SpriteRenderer>();

        //Debug.Log(ObjectFrames.Length);

    }

    // Update is called once per frame
    void Update()
    {

        ObjectTime = ObjectTime + Time.unscaledDeltaTime;



        if (ObjectTime > 1f)
        {
            Digit++;
            MyShift = Digit >> 1;
            MyShift2 = Digit << 1;


            //Debug.Log("Digit: " + Digit.ToString());

            //Debug.Log("Shift: " + MyShift.ToString() +"/" + MyShift2.ToString());

            ObjectTime = 0;
        }

            //LFSR_Fibonacci.Generate();

            
        //F1[0].gameObject.SetActive(true);
        //ObjectFrames[0].gameObject.SetActive(true);
    }

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
