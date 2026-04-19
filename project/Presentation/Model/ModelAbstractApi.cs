using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Presentation.Model
{
    public abstract class ModelAbstractApi
    {
        public abstract ObservableCollection<BallModel> GetBallsModel();
        public abstract void CreateBalls(int count);
        public abstract void StartSimulation();
        public abstract void StopSimulation();

        public static ModelAbstractApi CreateApi(LogicAbstractApi logicApi = null)
        {
            return new ModelApi(logicApi ?? LogicAbstractApi.CreateApi());
        }
    }
}
