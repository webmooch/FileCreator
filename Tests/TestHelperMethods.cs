using System;

namespace Tests
{
    internal class TestHelperMethods
    {
        private static System.Random random = new System.Random((int)DateTime.Now.Ticks);

        public static int GetRandomIntFromRange(int smallest, int largest)
        {
            if (smallest > largest)
                throw new ArgumentOutOfRangeException("Smallest is greater than largest");

            if (smallest == largest)
                throw new ArgumentOutOfRangeException("Smallest is same as largest");

            return random.Next(smallest, largest + 1);
        }

        public static string RandomCharString(int length)
        {
            char[] c = new char[length];
            for (int i = 0; i < length; i++)
                c[i] = (char)random.Next(97, 122);
            return new string(c);
        }

        public static bool GetRandomBool()
        {
            return random.NextDouble() > 0.5;
        }

        public static bool TimeSpansAreSimilar(TimeSpan timeSpan1, TimeSpan timeSpan2, TimeSpan maximumAllowableVariance)
        {
            var msDifference = timeSpan1.Subtract(timeSpan2).TotalMilliseconds;
            return maximumAllowableVariance.TotalMilliseconds >= Math.Abs(msDifference);
        }
    }
}
