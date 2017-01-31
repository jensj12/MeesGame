namespace MeesGame
{
    static class Helpers
    {
        /// <summary>
        /// Returns an array from 0 to (n-1) in a random order.
        /// </summary>
        /// <param name="n"> highest number </param>
        /// <returns> int[n] with 0 to (n-1) in random order </returns>
        public static int[] getZeroToNInRandomOrder(int n)
        {
            int[] ints = new int[n];
            for (int i = 0; i < n; i++)
            {
                ints[i] = -1;
            }
            for (int i = 0; i < n; i++)
            {
                while (true)
                {
                    int pos = GameEnvironment.Random.Next(n);
                    if (ints[pos] != -1) continue;
                    ints[pos] = i;
                    break;
                }
            }
            return ints;
        }
    }
}
