using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Data
{
    public class Ball : INotifyPropertyChanged
    {
        private double _x; private double _y;

        //Kiedy x zostaje zmieniony, wywołujemy OnPropertyChanged, aby powiadomić UI o zmianie
        public double X { get { return _x; }
            set { _x = value;  OnPropertyChanged(); } 
        } //oś X
        public double Y { get { return _y; }
            set { _y = value; OnPropertyChanged(); } 
        } //oś Y
        public double Radius { get; set; } //promień - wielkość
        public double VelX { get; set; } // Prędkość pozioma
        public double VelY { get; set; } // Prędkość pionowa

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }   
        public void Move()
        {
            X += VelX;
            Y += VelY;
        }
    }
}
