// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

namespace Microsoft.WindowsAzure.Management.Sync
{
    using System;
    using System.Diagnostics;
    using System.Timers;

    public class ProgressTracker : IDisposable
    {
        private readonly ProgressStatus progressStatus;
        private readonly Action<ProgressRecord> progress;
        private readonly Action<TimeSpan> complete;
        private readonly TimeSpan progressInterval;
        private Timer progressTimer;
        private object thisLock = new object();
        private ElapsedEventHandler progressTimerOnElapsed;
        private Stopwatch stopWatch;
        private bool isDisposed;

        public ProgressTracker(ProgressStatus progressStatus) : 
            this(progressStatus, Program.SyncOutput.ProgressUploadStatus, Program.SyncOutput.ProgressUploadComplete, TimeSpan.FromSeconds(1))
        {
        }

        public ProgressTracker(ProgressStatus progressStatus, Action<ProgressRecord> progress, Action<TimeSpan> complete, TimeSpan progressInterval)
        {
            this.progressStatus = progressStatus;
            this.progress = progress;
            this.complete = complete;
            this.progressInterval = progressInterval;
            this.stopWatch = new Stopwatch();
            InitilizeProgressTimer();
        }

        private void InitilizeProgressTimer()
        {
            stopWatch.Start();
            progressTimer = new Timer
            {
                AutoReset = false,
                Interval = progressInterval.TotalMilliseconds
            };
            progressTimerOnElapsed = (sender, args) =>
            {
                ProgressRecord pr;
                if (progressStatus.TryGetProgressRecord(out pr))
                {
                    this.progress(pr);
                }
                progressTimer.Enabled = true;
            };
            progressTimer.Elapsed += progressTimerOnElapsed;
            progressTimer.Enabled = true;
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            progressTimer.Elapsed -= progressTimerOnElapsed;
            progressTimer.Enabled = false;
            stopWatch.Stop();
            if(stopWatch.Elapsed != TimeSpan.Zero)
            {
                this.complete(stopWatch.Elapsed);
            }
            this.isDisposed = true;
        }
    }
}