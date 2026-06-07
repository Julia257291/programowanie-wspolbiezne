using System.Collections.Generic;

namespace Data
{
    public abstract class DataAbstractApi
    {
        public abstract void CreateBalls(int count, double maxX, double maxY);

        public abstract IList<IBall> GetBalls();

        public abstract void StopLogging();

        public static DataAbstractApi CreateApi()
        {
            return new DataApi();
        }
    }
}