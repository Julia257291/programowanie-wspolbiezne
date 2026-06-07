using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Logic
{
    internal class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi _dataApi;

        private double _width;
        private double _height;
        private readonly object _collisionLock = new object();

        private IList<IBall> _currentBalls = new List<IBall>();

        public LogicApi(DataAbstractApi dataApi)
        {
            _dataApi = dataApi;
        }

        public override void GenerateBalls(int count, double maxX, double maxY)
        {
            _width = maxX;
            _height = maxY;

            UnsubscribeFromBalls();

            _dataApi.CreateBalls(count, maxX, maxY);
            _currentBalls = _dataApi.GetBalls();

            foreach (var ball in _currentBalls)
            {
                ball.PropertyChanged += Ball_PositionedChanged;
            }
        }

        private void UnsubscribeFromBalls()
        {
            if (_currentBalls != null)
            {
                foreach (var ball in _currentBalls)
                {
                    ball.PropertyChanged -= Ball_PositionedChanged;
                }
            }
        }

        public override void UpdateBoardSize(double width, double height)
        {
            lock (_collisionLock)
            {
                _width = width;
                _height = height;
            }
        }

        private void Ball_PositionedChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IBall.X) || e.PropertyName == nameof(IBall.Y))
            {
                if (sender is not IBall ball) return;

                lock (_collisionLock)
                {
                    if (ball.X <= 0)
                    {
                        ball.VelX = Math.Abs(ball.VelX);
                    }
                    else if (ball.X + (2 * ball.Radius) >= _width)
                    {
                        ball.VelX = -Math.Abs(ball.VelX);
                    }

                    if (ball.Y <= 0)
                    {
                        ball.VelY = Math.Abs(ball.VelY);
                    }
                    else if (ball.Y + (2 * ball.Radius) >= _height)
                    {
                        ball.VelY = -Math.Abs(ball.VelY);
                    }

                    CheckBallCollision(ball);
                }
            }
        }

        private void CheckBallCollision(IBall ball)
        {
            foreach (var other in _currentBalls)
            {
                if (other == ball) continue;

                double BallCenterX = ball.X + ball.Radius;
                double BallCenterY = ball.Y + ball.Radius;
                double OtherCenterX = other.X + other.Radius;
                double OtherCenterY = other.Y + other.Radius;

                double dx = BallCenterX - OtherCenterX;
                double dy = BallCenterY - OtherCenterY;
                double distance = Math.Sqrt(dx * dx + dy * dy);

                if (distance <= (ball.Radius + other.Radius))
                {
                    double relativeVelX = ball.VelX - other.VelX;
                    double relativeVelY = ball.VelY - other.VelY;

                    if ((dx * relativeVelX + dy * relativeVelY) >= 0) continue;

                    double oldVelX = ball.VelX;
                    double oldVelY = ball.VelY;

                    ball.VelX = ((ball.Mass - other.Mass) * ball.VelX + 2 * other.Mass * other.VelX) / (ball.Mass + other.Mass);
                    ball.VelY = ((ball.Mass - other.Mass) * ball.VelY + 2 * other.Mass * other.VelY) / (ball.Mass + other.Mass);

                    other.VelX = ((other.Mass - ball.Mass) * other.VelX + 2 * ball.Mass * oldVelX) / (ball.Mass + other.Mass);
                    other.VelY = ((other.Mass - ball.Mass) * other.VelY + 2 * ball.Mass * oldVelY) / (ball.Mass + other.Mass);
                }
            }
        }

        public override void StartSimulation()
        {
            // Kulki same poruszają się w swoich metodach
        }

        public override void StopSimulation()
        {
            foreach (var ball in _currentBalls)
            {
                ball.StopMoving();
            }

            _dataApi.StopLogging();
        }

        public override IList<IBall> GetBalls()
        {
            return _currentBalls;
        }
    }
}