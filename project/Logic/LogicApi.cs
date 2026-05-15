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

        private void Ball_PositionedChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Ball.X) || e.PropertyName == nameof(Ball.Y))
            {
                var ball = sender as Ball;
                if (ball == null) return;

                // Sekcja krytyczna - tylko jeden wątek naraz może liczyć kolizje
                lock (_collisionLock)
                {
                    // Odbicia od ścian
                    if (ball.X <= 0) { ball.VelX = Math.Abs(ball.VelX); }
                    else if (ball.X + ball.Radius >= _width) { ball.VelX = -Math.Abs(ball.VelX); }

                    if (ball.Y <= 0) { ball.VelY = Math.Abs(ball.VelY); }
                    else if (ball.Y + ball.Radius >= _height) { ball.VelY = -Math.Abs(ball.VelY); }

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

                // Obliczamy odległość między środkami kul (Twierdzenie Pitagorasa)
                double dx = (ball.X + ball.Radius / 2) - (other.X + other.Radius / 2);
                double dy = (ball.Y + ball.Radius / 2) - (other.Y + other.Radius / 2);
                double distance = Math.Sqrt(dx * dx + dy * dy);

                // Jeśli odległość jest mniejsza lub równa sumie promieni, to kulki się stykają
                if (distance <= (ball.Radius / 2 + other.Radius / 2))
                {
                    // m1v1 + m2v2 = m1v1' + m2v2' - zasada zachowania pędu, masa się nie zmienia
                    double oldVelX = ball.VelX;
                    double oldVelY = ball.VelY;
                    //Obliczamy nowe prędkości osobno dla każdej osi
                    ball.VelX = ((ball.Mass - other.Mass) * ball.VelX + 2 * other.Mass * other.VelX) / (ball.Mass + other.Mass);
                    ball.VelY = ((ball.Mass - other.Mass) * ball.VelY + 2 * other.Mass * other.VelY) / (ball.Mass + other.Mass);
                    other.VelX = ((other.Mass - ball.Mass) * other.VelX + 2 * ball.Mass * oldVelX) / (ball.Mass + other.Mass);
                    other.VelY = ((other.Mass - ball.Mass) * other.VelY + 2 * ball.Mass * oldVelY) / (ball.Mass + other.Mass);
                }
            }
        }
        public override List<Ball> GetBalls()
        {
            return _dataApi.GetBalls();
        }
        

    }
}
