using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.PostEmulator.ViewModels
{
	public class Runner:Screen
	{
		//不知道为什么在xaml里面c:Binding使用models:TaskStatus会报错
		public object TaskStatus { get; } = new { Models.TaskStatus.Done };

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
				//this._TextDocument = null;
				//this.NotifyOfPropertyChange(() => this.TextDocument);
				this._DeaultSaveFileName = null;
				this.NotifyOfPropertyChange(() => this.DeaultSaveFileName);
			}
		}

		//private ICSharpCode.AvalonEdit.Document.TextDocument _TextDocument;
		//public ICSharpCode.AvalonEdit.Document.TextDocument TextDocument
		//{
		//	get
		//	{
		//		if (this._TextDocument == null)
		//		{
		//			if (this.Response == null)
		//				return null;
		//			this._TextDocument = new ICSharpCode.AvalonEdit.Document.TextDocument(this.Response.FormattedContentCode);
		//		}
		//		return this._TextDocument;
		//	}
		//}

		//private ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition _Highlighting;
		//public ICSharpCode.AvalonEdit.Highlighting.IHighlightingDefinition Highlighting
		//{
		//	get
		//	{
		//		if(this._Highlighting==null)
		//			this._Highlighting= ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition(this.Response.ContentType.ToString());
		//		return this._Highlighting;
		//	}
		//}

		private string _DeaultSaveFileName;
		public string DeaultSaveFileName
		{
			get
			{
				if (this._DeaultSaveFileName == null)
				{
					if (this.Response == null)
						return null;
					if (string.IsNullOrWhiteSpace(this.Response.FileName))
						switch (this.Response.ContentType)
						{
							case Models.ResponseContentType.PlainText:
								this._DeaultSaveFileName = ".text";
								break;
							case Models.ResponseContentType.HTML:
								this._DeaultSaveFileName = ".html";
								break;
							case Models.ResponseContentType.Json:
								this._DeaultSaveFileName = ".json";
								break;
							case Models.ResponseContentType.JavaScript:
								this._DeaultSaveFileName = ".js";
								break;
							case Models.ResponseContentType.CSS:
								this._DeaultSaveFileName = ".css";
								break;
							case Models.ResponseContentType.XML:
								this._DeaultSaveFileName = ".xml";
								break;
							case Models.ResponseContentType.Image:
								if (this.Response.MediaType != null)
									this._DeaultSaveFileName = "." + this.Response.MediaType.Split('/').Last();
								break;
							default:
								this._DeaultSaveFileName = "";
								break;
						}
					else this._DeaultSaveFileName = this.Response.FileName;
				}
				return this._DeaultSaveFileName;
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
			this.Response.NotifyOfPropertyChange(() => this.Response.FormattedContentCode);
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

		public void SaveFile(string path)
		{
			System.IO.File.WriteAllBytes(path, this.Response.Content);
		}
	}
}