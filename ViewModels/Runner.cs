﻿using System;
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

		private Models.Response _Response;
		public Models.Response Response
		{
			get
			{
				return this._Response;
			}
			set
			{
				this._Response = value;
			}
		}

		public Runner(Models.Task task, string fileName)
		{
			this.Task = task;
			this.Title = $"{this.Localization.RunTask} - {fileName}";

			//this.Thread = new System.Threading.Thread(this.Task.Run);
			//this.Thread.IsBackground = true;
		}

		public async Task Run()
		{
			this.Response = await this.Task.Run();
			//this.Thread.Start();
		}

		public void Stop()
		{
			//this.Thread.Abort();
			this.Task.CancelPendingRequests();
			//this.CancellationTokenSource.Cancel();
			//this.Task.Status = Models.TaskStatus.Standby;
			this.TryCloseAsync();
		}
	}
}