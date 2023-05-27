using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.HttpClientPerformer.ViewModels
{
	public class Runner:Screen
	{
		public Models.Task Task { get; protected set; }

		public string Title { get; protected set; }

		//protected readonly System.Threading.Thread Thread;

		public Runner(Models.Task task, string fileName)
		{
			this.Task = task;
			this.Title = $"{this.Localization.RunTask} - {fileName}";

			//this.Thread = new System.Threading.Thread(this.Task.Run);
			//this.Thread.IsBackground = true;
		}

		public void Run()
		{
			System.Threading.Tasks.Task.Run(this.Task.Run, this.CancellationTokenSource.Token);
			//this.Thread.Start();
		}

		public void Stop()
		{
			//this.Thread.Abort();

			this.CancellationTokenSource.Cancel();
			//this.Task.Status = Models.TaskStatus.Standby;
			//this.TryClose();
		}
	}
}