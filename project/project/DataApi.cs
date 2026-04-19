using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    internal class DataApi : DataAbstractApi
    {
        private readonly List<Ball> _balls = new List<Ball>();
        private readonly Random _random = new Random();

        public override void CreateBalls(int count, double maxX, double maxY)
        {
            _balls.Clear();
            for (int i = 0; i < count; i++)
            {
                double radius = 10.0;
                var ball = new Ball
                {
                    X = _random.NextDouble() * (maxX - radius), //NextDouble() zwraca wartość z zakresu [0.0, 1.0)
                    Y = _random.NextDouble() * (maxY-radius ),
                    Radius = radius
                };
                _balls.Add(ball);
            }
        }

        public override List<Ball> GetBalls()
        {
            return _balls;
        }
    }
}