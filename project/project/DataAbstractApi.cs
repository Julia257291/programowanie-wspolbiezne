using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public abstract class DataAbstractApi
    {
        //musi umieć stworzyć kule, maxX i maxY to ograniczenia dla pozycji kul, żeby nie wychodziły poza obszar
        public abstract void CreateBalls(int count, double maxX, double maxY);
        public abstract List<Ball> GetBalls(); //metoda do pobierania kul w logice, żeby można było je potem wyświetlić

        public static DataAbstractApi CreateApi()
        {
            return new DataApi();
        }
    }
}
