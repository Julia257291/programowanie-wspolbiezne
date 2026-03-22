using Data;

namespace Logic
{
    public class LogicApi
    {
        public List<Ball> CreateBalls(int count)
        {
            List<Ball> balls = new List<Ball>();
            for (int i = 0; i < count; i++)
            {
                balls.Add(new Ball { X = i * 15, Y = i * 15, Radius = 10 });
            }
            return balls;
        }
    }
}
