using Data;

namespace Logic
{
    internal class LogicApi : LogicAbstractApi
    {
        private readonly DataAbstractApi _dataApi;
        public LogicApi(DataAbstractApi dataApi)
        {
            _dataApi = dataApi;
        }
        public override void GenerateBalls(int count, double maxX, double maxY)
        {
            _dataApi.CreateBalls(count, maxX, maxY);
        }
        public override List<Ball> GetBalls()
        {
            return _dataApi.GetBalls();
        }
        public override void StartSimulation()
        {
            // Tutaj można dodać logikę symulacji ruchu kul, np. aktualizację pozycji kul na podstawie ich prędkości
        }
    }
}
