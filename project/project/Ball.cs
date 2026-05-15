using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class Ball : INotifyPropertyChanged
    {
        private double _x; private double _y;
        private double _velX; private double _velY;
        private bool _isMoving;
        private readonly object _velocityLock = new object();

        public double Mass { get; set; }

        //Kiedy x zostaje zmieniony, wywołujemy OnPropertyChanged, aby powiadomić UI o zmianie
        public double X { get { return _x; }
            set { _x = value;  OnPropertyChanged(); } 
        } //oś X
        public double Y { get { return _y; }
            set { _y = value; OnPropertyChanged(); } 
        } //oś Y
        public double Radius { get; set; } //promień - wielkość
        public double VelX {
            get { lock (_velocityLock) { return _velX; } },
            set { lock (_velocityLock) { _velX = value; } }
        } // Prędkość pozioma
        public double VelY {
            get { lock (_velocityLock) { return _velY; } },
            set { lock (_velocityLock) { _velY = value; } }
        } // Prędkość pionowa
        //Lock tworzy sekcję krytyczną więc wątki nie będą się nawzajem blokować podczas odczytu i zapisu prędkości

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public async Task StartMoving()
        {
            _isMoving = true;
            while (_isMoving)
            {
                X += VelX;
                Y += VelY;
                await Task.Delay(16);
            }
        }

        public void StopMoving()
        {
            _isMoving = false;
        }
    }
}
