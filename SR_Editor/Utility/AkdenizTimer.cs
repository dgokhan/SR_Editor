using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace SR_Editor.Core.Utility
{
	public class EditorTimer : IDisposable
	{
		private System.Windows.Forms.Timer mainTimer;

		public EditorTimer(bool enabled, int interval)
		{
			this.mainTimer = new System.Windows.Forms.Timer()
			{
				Interval = interval,
				Enabled = enabled
			};
			this.mainTimer.Tick += new EventHandler((object sender, EventArgs e) => this.EditorTick(sender, e));
			this.mainTimer.Disposed += new EventHandler((object sender, EventArgs e) => this.EditorDispose(sender, e));
		}

		public EditorTimer() : this(true, 0)
		{
		}

		public EditorTimer(int interval) : this(true, interval)
		{
		}

		public void Dispose()
		{
			if (this.mainTimer != null)
			{
				this.mainTimer.Dispose();
			}
			GC.SuppressFinalize(this);
		}

		public void Start()
		{
			this.mainTimer.Start();
		}

		public void Stop()
		{
			this.mainTimer.Stop();
		}

		public event EditorTimer.EditorDisposeDelegate EditorDispose;

		public event EditorTimer.EditorTickDelegate EditorTick;

		public delegate void EditorDisposeDelegate(object sender, EventArgs e);

		public delegate void EditorTickDelegate(object sender, EventArgs e);
	}
}