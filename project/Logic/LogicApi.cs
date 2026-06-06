using Data;
using System.Timers;

namespace Logic
{
    internal class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi _dataApi;

        private double _width;
        private double _height;
        private readonly object _collisionLock = new object();
        public LogicApi(DataAbstractApi dataApi)
        {
            _dataApi = dataApi;
        }
        public override void GenerateBalls(int count, double maxX, double maxY)
        {
            _width = maxX;
            _height = maxY;
            _dataApi.CreateBalls(count, maxX, maxY);

            foreach (var ball in _dataApi.GetBalls())
            {
                ball.PropertyChanged += Ball_PositionedChanged;
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

        private void Ball_PositionedChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Ball.X) || e.PropertyName == nameof(Ball.Y))
            {
                var ball = sender as Ball;
                if (ball == null) return;

                // Sekcja krytyczna - tylko jeden wątek naraz może liczyć kolizje
                lock (_collisionLock)
                {
                    // Odbicia od ścian (szerokość kulki to 2 * Radius)
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

                    // Odbicia od innych kul
                    CheckBallCollision(ball);
                }
            }
        }

        private void CheckBallCollision(Ball ball)
        {
            foreach (var other in _dataApi.GetBalls())
            {
                if (other == ball) continue; // Nie sprawdzamy kolizji samej ze sobą

                // Prawdziwy środek kuli to: lewa krawędź + promień
                double BallCenterX = ball.X + ball.Radius;
                double BallCenterY = ball.Y + ball.Radius;
                double OtherCenterX = other.X + other.Radius;
                double OtherCenterY = other.Y + other.Radius;

                // Obliczamy odległość między środkami kul (Twierdzenie Pitagorasa)
                double dx = BallCenterX - OtherCenterX;
                double dy = BallCenterY - OtherCenterY;
                double distance = Math.Sqrt(dx * dx + dy * dy);

                // Kulki stykają się, gdy odległość między środkami <= suma ich prawdziwych promieni!
                if (distance <= (ball.Radius + other.Radius))
                {
                    // Sprawdzamy prędkość względną - ochrona przed wrażeniem sklejania się kul
                    double relativeVelX = ball.VelX - other.VelX;
                    double relativeVelY = ball.VelY - other.VelY;

                    // Jeśli kule już się od siebie oddalają, nie licz kolizji ponownie
                    if ((dx * relativeVelX + dy * relativeVelY) >= 0) continue;

                    // Zasada zachowania pędu
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
            //Kulki same poruszają się w swoich metodach StartMoving, więc tutaj nie musimy nic robić
        }
        public override void StopSimulation()
        {
            foreach (var ball in _dataApi.GetBalls())
            {
                ball.StopMoving(); // zatrzyma pętle while w klasie Ball
            }

            _dataApi.StopLogging();
        }
        public override List<Ball> GetBalls()
        {
            return _dataApi.GetBalls();
        }
        

    }
}
