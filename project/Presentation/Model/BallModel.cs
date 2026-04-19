using Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Presentation.Model
{
    public class BallModel : INotifyPropertyChanged
    {
        private double _x;
        private double _y;
        private double _radius;

        public BallModel(Ball ball)
        {
            this._x = ball.X;
            this._y = ball.Y;
            this._radius = ball.Radius;

            ball.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(ball.X))
                {
                    this.X = ball.X;
                }
                else if (args.PropertyName == nameof(ball.Y))
                {
                    this.Y = ball.Y;
                }
            };
        }

        public double X
        {
            get => _x;
            set { _x = value; OnPropertyChanged(); }
        }

        public double Y
        {
            get => _y;
            set { _y = value; OnPropertyChanged(); }
        }

        public double Radius
        {
            get => _radius;
            set { _radius = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
