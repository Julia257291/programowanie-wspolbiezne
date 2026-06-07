using System.ComponentModel;

namespace Data
{
    public interface IBall : INotifyPropertyChanged
    {
        double X { get; }
        double Y { get; }
        double VelX { get; set; }
        double VelY { get; set; }
        double Radius { get; }
        double Mass { get; }

        void StopMoving();
    }
}