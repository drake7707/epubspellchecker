using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace EpubSpellChecker
{
    public class Loader : IDisposable
    {
        private Timer tmr = new Timer();
        private ToolStripLabel statusLabel;
        private ToolStripProgressBar progressBar;

        private List<LoadState> states = new List<LoadState>();


        public Loader(ToolStripLabel statusLabel, ToolStripProgressBar progressBar)
        {
            this.statusLabel = statusLabel;
            this.progressBar = progressBar;

            tmr.Interval = 500;
            tmr.Tick += new EventHandler(tmr_Tick);
            tmr.Enabled = true;
        }

        void tmr_Tick(object sender, EventArgs e)
        {
            lock (states)
            {
                if (states.Count > 0)
                {
                    var state = states.First();
                    statusLabel.Text = state.Text;
                    if (state.Progress == -1)
                    {
                        if (progressBar.Style != ProgressBarStyle.Marquee)
                            progressBar.Style = ProgressBarStyle.Marquee;
                    }
                    else
                    {
                        if (progressBar.Style != ProgressBarStyle.Continuous)
                            progressBar.Style = ProgressBarStyle.Continuous;

                        progressBar.Value = (int)(state.Progress * 100);
                    }

                    if (!statusLabel.Visible)
                        statusLabel.Visible = true;

                    if (!progressBar.Visible)
                        progressBar.Visible = true;
                }
                else
                {
                    if (statusLabel.Visible)
                        statusLabel.Visible = false;

                    if (progressBar.Visible)
                        progressBar.Visible = false;
                }
            }
        }

        public class LoadState
        {
            public float Progress { get; set; }
            public string Text { get; set; }

            public bool Cancel { get; set; }
        }

        public void LoadAsync<T>(Func<LoadState, T> func, Action<T> gui)
        {
            LoadState state = new LoadState() { Progress = 0, Text = "" };

            lock (states)
                states.Add(state);

            var ui = TaskScheduler.FromCurrentSynchronizationContext();
            var t = Task.Factory.StartNew<T>(() =>
            {
                var val = func(state);
                return val;

            }, System.Threading.CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).ContinueWith(tsk =>
            {
                lock (states)
                    states.Remove(state);

                gui(tsk.Result);
            }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, ui);
        }

        public void CancelAll()
        {
            lock (states)
            {
                foreach (var state in states)
                    state.Cancel = true;
            }
        }

        public void Dispose()
        {
            tmr.Stop();
            tmr.Dispose();
        }
    }
}
