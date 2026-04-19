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
            _logicApi.GenerateBalls(count, 500, 500);
            _ballsModels.Clear();

            foreach (var ball in _logicApi.GetBalls())
            {
                var newBallModel = new BallModel
                {
                    X = ball.X,
                    Y = ball.Y,
                    Radius = ball.Radius
                };
                _ballsModels.Add(newBallModel);

                // Tutaj w przyszłości dodasz mechanizm (reaktywny), 
                // który będzie aktualizował BallModel, gdy zmieni się pozycja w Logice
            }
        }

        public override void StartSimulation() => _logicApi.StartSimulation();
        public override void StopSimulation() { /* implementacja zatrzymania */ }
    }
}
