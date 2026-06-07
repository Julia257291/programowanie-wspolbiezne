using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data
{
    internal class DataApi : DataAbstractApi
    {
        private readonly List<Ball> _balls = new List<Ball>();
        private readonly Random _random = new Random();

        private readonly Logger _logger = new Logger();
        private bool _isSimulating = false;

        private readonly object _ballsLock = new object();

        public override void CreateBalls(int count, double maxX, double maxY)
        {
            lock (_ballsLock)
            {
                _balls.Clear();
                for (int i = 0; i < count; i++)
                {
                    double radius = 10.0;
                    double mass = _random.NextDouble() * 4 + 1; // Masa w zakresie [1, 5]
                    var ball = new Ball
                    {
                        X = _random.NextDouble() * (maxX - radius),
                        Y = _random.NextDouble() * (maxY - radius),
                        Radius = radius,
                        Mass = mass,
                        VelX = (_random.NextDouble() * 2 - 1) * 2,
                        VelY = (_random.NextDouble() * 2 - 1) * 2
                    };
                    _balls.Add(ball);
                    _ = ball.StartMoving();
                }
            }

            if (!_isSimulating)
            {
                _isSimulating = true;
                _ = Task.Run(LogDataPeriodically);
            }
        }

        public override IList<IBall> GetBalls()
        {
            lock (_ballsLock)
            {
                return _balls.Cast<IBall>().ToList();
            }
        }

        public override void StopLogging()
        {
            _isSimulating = false;
            _logger.StopLogging();
        }

        private async Task LogDataPeriodically()
        {
            while (_isSimulating)
            {
                List<Ball> ballsSnapshot;

                lock (_ballsLock)
                {
                    ballsSnapshot = new List<Ball>(_balls);
                }

                _logger.Log(ballsSnapshot);
                await Task.Delay(100);
            }
        }
    }
}