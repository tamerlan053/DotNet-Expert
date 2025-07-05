namespace Exercise1
{
    public class BalloonProgram
    {
        private readonly WriteDelegate _writer;

        public BalloonProgram(WriteDelegate writer)
        {
            _writer = writer;
        }

        public void Run()
        {
           Random random = Random.Shared;

            int balloonCount = 5;
            int maxSize = 100;

            Balloon[] balloons = new Balloon[balloonCount];

            for (int i = 0; i < balloonCount; i++)
            {
                balloons[i] = random.NextBalloon(maxSize);
                _writer($"A balloon of size ' {balloons[i].Size}' and color ' {balloons[i].Color}'");
            }

            Balloon  popped = random.NextBalloonFromArray(balloons);
            _writer($"Popped balloon of size '{popped.Size}' and color '{popped.Color}'");
        }
    }
}