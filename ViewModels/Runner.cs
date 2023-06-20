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
				this.NotifyOfPropertyChange(() => this.Response);
				this._TextDocument = null;
				this.NotifyOfPropertyChange(() => this.TextDocument);
			}
		}

		private ICSharpCode.AvalonEdit.Document.TextDocument _TextDocument;
		public ICSharpCode.AvalonEdit.Document.TextDocument TextDocument
		{
			get
			{
				if (this._TextDocument == null)
				{
					if (this.Response == null)
						return null;
					this._TextDocument = new ICSharpCode.AvalonEdit.Document.TextDocument(this.Response.FormattedContentCode);
				}
				return this._TextDocument;
			}
		}

		private ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition _Highlighting;
		public ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition Highlighting
		{
			get
			{
				if(this._Highlighting==null)
					this._Highlighting= ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition(this.Response.ContentType.ToString());
				return this._Highlighting;
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


		public void CopyCookiesToNewTask()
		{
			Models.Task task = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Task>(Newtonsoft.Json.JsonConvert.SerializeObject(this.Task));
			task.ReplaceSelectedOldItems(task.CookieItems, this.Task.CookieContainer.GetCookies(task.Uri).Cast<System.Net.Cookie>().Select(c => new Models.RequestItem(c.Name, c.Value)).ToList());
			this.WindowManager.ShowWindowAsync(new Main { Task = task });
		}

	}
}