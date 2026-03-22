using Data;
using Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Presentation
{
    public class MainViewModel
    {
        public ObservableCollection<Ball> Balls { get; set; }

        public MainViewModel()
        {
            LogicApi logic = new LogicApi(); // or injected
            Balls = new ObservableCollection<Ball>(logic.CreateBalls(10));
        }
    }
}
