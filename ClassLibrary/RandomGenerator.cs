namespace ClassLibrary
{
    public class RandomGenerator
    {
        private static Random _global = new();
        [ThreadStatic]
        private static Random _local;

        public static int Next(int i)
        {
            Random inst = _local;
            if (inst == null)
            {
                int seed;
                lock (_global) seed = _global.Next();
                _local = inst = new Random(seed);
            }
            return inst.Next(i);
        }
    }
}
