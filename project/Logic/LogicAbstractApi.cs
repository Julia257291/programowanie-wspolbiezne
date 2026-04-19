using Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic
{
    public abstract class LogicAbstractApi
    {
        public abstract List<Ball> GetBalls(); //pobierania listy kul, która będzie wywoływać metodę z DataAbstractApi
        public abstract void StartSimulation(); //obsługa ruchu kul

        public abstract void GenerateBalls(int count, double maxX, double maxY); //metoda do generowania kul

        public static LogicAbstractApi CreateApi(DataAbstractApi dataApi = null)
        {
            return new LogicApi(dataApi ?? DataAbstractApi.CreateApi());
        }
    }
}
