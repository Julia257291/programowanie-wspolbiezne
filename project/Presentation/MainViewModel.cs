using Data;
using Logic;
using Presentation.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace Presentation.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ModelAbstractApi _modelApi;
        private int _ballsCount;

        public ObservableCollection<BallModel> Balls => _modelApi.GetBallsModel();

        public int BallsCount
        {
            get => _ballsCount;
            set
            {
                _ballsCount = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public MainViewModel()
        {
            _modelApi = ModelAbstractApi.CreateApi();

            StartCommand = new RelayCommand(StartSimulation);
            StopCommand = new RelayCommand(StopSimulation);

            BallsCount = 5;
        }

        private void StartSimulation()
        {
            _modelApi.CreateBalls(BallsCount);
            _modelApi.StartSimulation();
        }

        private void StopSimulation()
        {
            _modelApi.StopSimulation();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
