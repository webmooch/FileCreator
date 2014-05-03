using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileCreator.Models
{
    internal class JunkFile
    {
        public event EventHandler<ProgressChangedEventArgs> ProgressChangedEvent;

        public async Task<TimeSpan> Create(FileInfo file, long desiredTotalSizeBytes, int blockSizeBytes, bool fillWithJunk, CancellationTokenSource ct)
        {
            file.Delete();

            return await Task.Factory.StartNew<TimeSpan>(() =>
            {
                var sw = new Stopwatch();
                using (var destination = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
                {
                    long remainder;
                    long iterations = Math.DivRem(desiredTotalSizeBytes, (long)blockSizeBytes, out remainder);

                    double previousPercent = 0;
                    var emptyBuffer = new byte[blockSizeBytes];

                    sw.Start();

                    for (int i = 0; i < iterations; i++)
                    {
                        if (ct.IsCancellationRequested)
                            break;

                        destination.Write(fillWithJunk ? GenerateJunkByteArray(blockSizeBytes) : emptyBuffer, 0, blockSizeBytes);
                        destination.Flush();

                        var percent = Math.Round(((float)i / iterations) * 100, 0);
                        if (percent != previousPercent)
                        {
                            OnProgressChangedEvent(percent);
                            previousPercent = percent;
                        }
                    }

                    if (!ct.IsCancellationRequested && remainder > 0)
                    {
                        destination.Write(fillWithJunk ? GenerateJunkByteArray((int)remainder) : emptyBuffer, 0, (int)remainder);
                        destination.Flush();
                    }

                    sw.Stop();
                    destination.Close();
                }

                if (ct.IsCancellationRequested)
                    file.Delete();

                GC.Collect();
                GC.WaitForPendingFinalizers();

                return sw.Elapsed;
            }, ct.Token);
        }

        private void OnProgressChangedEvent(double percent)
        {
            if (ProgressChangedEvent != null)
                ProgressChangedEvent(this, new ProgressChangedEventArgs() { PercentComplete = (int)percent });
        }

        private static readonly Random random = new Random();
        private static byte[] GenerateJunkByteArray(int length)
        {
            var arr = new Byte[length];
            random.NextBytes(arr); // TODO: Make this non-blocking with larger sizes
            return arr;
        }
    }
}