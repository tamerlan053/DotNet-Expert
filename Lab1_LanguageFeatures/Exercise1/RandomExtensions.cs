using System.Drawing;

namespace Exercise1
{
    public static class RandomExtensions
    {
        public static Balloon NextBalloon(this Random random, int maxSize)
        {
            if (random == null)
            {
                throw new ArgumentNullException(nameof(random));
            }

            if (maxSize < 1)
            {
                throw new ArgumentException("maxSize must be at least 1", nameof(maxSize));
            }

            Color randomColor = Color.FromArgb(
                random.Next(256),
                random.Next(256),
                random.Next(256)
            );

            int randomSize = random.Next(1, maxSize + 1);

            return new Balloon(randomColor, randomSize);
        }

        public static Balloon NextBalloonFromArray(this Random random, Balloon[] balloons) { 
            int index = random.Next(balloons.Length);

            return balloons[index];
        }
    }
}
