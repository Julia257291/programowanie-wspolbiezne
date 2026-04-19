using Data;
using System.Timers;

namespace Logic
{
    internal class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi _dataApi;
        private System.Timers.Timer _timer;

        private double _width;
        private double _height;
        public LogicApi(DataAbstractApi dataApi)
        {
            _dataApi = dataApi;
        }
        public override void GenerateBalls(int count, double maxX, double maxY)
        {
            _width = maxX;
            _height = maxY;
            _dataApi.CreateBalls(count, maxX, maxY);
        }
        public override List<Ball> GetBalls()
        {
            return _dataApi.GetBalls();
        }
        public override void StartSimulation()
        {
            _timer = new System.Timers.Timer(16); // Ok. 60 odświeżeń na sekundę
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            var balls = _dataApi.GetBalls();

            foreach (var ball in balls)
            {
                ball.Move();

                // Odbicia od ścian
                if (ball.X <= 0 || ball.X + ball.Radius >= _width)
                    ball.VelX *= -1;

                if (ball.Y <= 0 || ball.Y + ball.Radius >= _height)
                    ball.VelY *= -1;

                Console.WriteLine(ball.X);
            }
            
        }

        public override void StopSimulation()
        {
            _timer?.Stop();
            _timer?.Dispose();
        }
    }
}
