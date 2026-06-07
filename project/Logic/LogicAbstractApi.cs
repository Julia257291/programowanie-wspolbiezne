using Data;
using System.Collections.Generic;

namespace Logic
{
    public abstract class LogicAbstractApi
    {
        public abstract IList<IBall> GetBalls();
        public abstract void StartSimulation();
        public abstract void StopSimulation();
        public abstract void GenerateBalls(int count, double maxX, double maxY);
        public abstract void UpdateBoardSize(double width, double height);

        public static LogicAbstractApi CreateApi(DataAbstractApi dataApi = null)
        {
            return new LogicApi(dataApi ?? DataAbstractApi.CreateApi());
        }
    }
}