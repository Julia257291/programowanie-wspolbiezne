using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Presentation.Model
{
    internal class ModelApi : ModelAbstractApi
    {
        private readonly LogicAbstractApi _logicApi;
        private readonly ObservableCollection<BallModel> _ballsModels;

        public ModelApi(LogicAbstractApi logicApi)
        {
            _logicApi = logicApi;
            _ballsModels = new ObservableCollection<BallModel>();
        }

        public override ObservableCollection<BallModel> GetBallsModel() => _ballsModels;

        public override void CreateBalls(int count)
        {
            _logicApi.GenerateBalls(count, 760, 510);
            _ballsModels.Clear();

            foreach (var ball in _logicApi.GetBalls())
            {
                var newBallModel = new BallModel(ball);
                _ballsModels.Add(newBallModel);
            }
        }

        public override void StartSimulation() => _logicApi.StartSimulation();
        public override void StopSimulation() => _logicApi.StopSimulation();
    }
}
