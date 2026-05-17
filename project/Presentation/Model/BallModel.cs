using Data;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
            set
            {
                _x = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RenderX)); 
            }
        }

        public double Y
        {
            get => _y;
            set
            {
                _y = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RenderY));
            }
        }

        public double Radius
        {
            get => _radius;
            set
            {
                _radius = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Diameter));
            }
        }

        // Dane do bindowania w XAML:
        public double Diameter => _radius * 2;
        public double RenderX => _x;
        public double RenderY => _y;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}