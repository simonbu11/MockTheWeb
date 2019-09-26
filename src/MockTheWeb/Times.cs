namespace MockTheWeb
{
    public class Times
    {
        public int Min { get; }
        public int Max { get; }

        private Times(int min, int max)
        {
            Min = min;
            Max = max;
        }

        internal bool Verify(int times)
        {
            return times >= Min && times <= Max;
        }


        public static Times Never()
        {
            return new Times(0, 0);
        }

        public static Times Once()
        {
            return new Times(1, 1);
        }

        public static Times Twice()
        {
            return new Times(2, 2);
        }

        public static Times Exactly(int times)
        {
            return new Times(times, times);
        }

        public static Times Between(int inclusiveMin, int inclusiveMax)
        {
            return new Times(inclusiveMin, inclusiveMax);
        }

        public static Times AtLeastOnce()
        {
            return Times.Between(1, int.MaxValue);
        }

        public static Times NoMoreThanOnce()
        {
            return Times.Between(0, 1);
        }

        public override string ToString()
        {
            if (Min == Max)
            {
                return Min.ToString();
            }

            return $"between {Min} and {Max}";
        }
    }
}