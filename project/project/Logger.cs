using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Text.Json;

namespace Data
{
    internal class Logger
    {
        private readonly ConcurrentQueue<string> _logQueue = new ConcurrentQueue<string>();
        private readonly Task _loggingTask;
        private bool _isLogging;

        public Logger()
        {
            _isLogging = true;
            File.WriteAllText("data_diagnostic.json", string.Empty);
            _loggingTask = Task.Run(WriteLogs);
        }

        public void Log(IEnumerable<Ball> balls)
        {
            var data = new List<object>();

            if (!_isLogging) {
                return;
            }

            foreach (var ball in balls)
            {
                data.Add(new { 
                    ball.X,
                    ball.Y,
                    ball.VelX,
                    ball.VelY,
                    ball.Mass,
                    ball.Radius 
                });
            }

            var logEntry = new
            {
                Time = DateTime.UtcNow,
                Balls = data
            };

            var json = JsonSerializer.Serialize(logEntry);
            _logQueue.Enqueue(json); 
        }

        private async Task WriteLogs()
        {
            while (_isLogging || !_logQueue.IsEmpty)
            {
                if (_logQueue.TryDequeue(out string logEntry))
                {
                    byte[] encodedText = Encoding.ASCII.GetBytes(logEntry + Environment.NewLine);

                    using (FileStream sourceStream = new FileStream("data_diagnostic.json",
                                            FileMode.Append, FileAccess.Write, FileShare.None,
                                            bufferSize: 4096, useAsync: true))
                    {
                        await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
                    }
                }
                else
                {
                    await Task.Delay(10);
                }
            }
        }
        public void StopLogging()
        {
            _isLogging = false;
            _loggingTask.Wait();
        }
    }
}
