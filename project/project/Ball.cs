using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Data
{
    public class Ball : IBall
    {
        private double _x;
        private double _y;
        private double _velX;
        private double _velY;
        private bool _isMoving;

        // Jeden lock dla całego stanu kuli zapewniający atomowość operacji
        private readonly object _stateLock = new object();

        public double Mass { get; set; }
        public double Radius { get; set; }

        public double X
        {
            get { lock (_stateLock) return _x; }
            set
            {
                lock (_stateLock)
                {
                    if (_x == value) return;
                    _x = value;
                }
                OnPropertyChanged();
            }
        }

        public double Y
        {
            get { lock (_stateLock) return _y; }
            set
            {
                lock (_stateLock)
                {
                    if (_y == value) return; 
                    _y = value;
                }
                OnPropertyChanged();
            }
        }

        public double VelX
        {
            get { lock (_stateLock) return _velX; }
            set { lock (_stateLock) _velX = value; }
        }

        public double VelY
        {
            get { lock (_stateLock) return _velY; }
            set { lock (_stateLock) _velY = value; }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async Task StartMoving()
        {
            _isMoving = true;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (_isMoving)
            {
                stopwatch.Stop();
                double deltaTime = stopwatch.ElapsedMilliseconds / 1000.0;
                stopwatch.Restart();

                // Aktualizacja pozycji musi być operacją atomową wewnątrz locka
                lock (_stateLock)
                {
                    _x += _velX * deltaTime * 100;
                    _y += _velY * deltaTime * 100; //mnożnik 100, aby prędkość była bardziej zauważalna
                }

                // Wywołanie zdarzenia POZA lockiem - kluczowe, aby uniknąć zakleszczeń (Deadlocks)
                OnPropertyChanged(nameof(X));
                OnPropertyChanged(nameof(Y));

                await Task.Delay(8);
            }
        }

        public void StopMoving()
        {
            _isMoving = false;
        }
    }
}