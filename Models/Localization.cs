using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.HttpClientPerformer.Models
{
	public class Localization
	{
		public string Add { get; set; }
		public string Save { get; set; }
		public string SaveAs { get; set; }
		public string Open { get; set; }
		public string RecentFiles { get; set; }
		public string Language { get; set; }
		public string Help { get; set; }
		public string Tutorial { get; set; }
		public string About { get; set; }
		public string Theme { get; set; }

		public string Run { get; set; }
		public string Stop { get; set; }
		public string RunTask { get; set; }
		public string Status { get; set; }
		public string ExportCode { get; set; }

		public string Url { get; set; }
		public string UrlParameters { get; set; }

		public string RequestHeaders { get; set; }
		public string HeadersRaw { get; set; }
		public string HeadersEditor { get; set; }
		public string CookiesEditor { get; set; }

		public string RequestBody { get; set; }
		public string ContentType { get; set; }
		public string Editor { get; set; }

		public string Name { get; set; }
		public string Value { get; set; }
		public string OpenFileDialog { get; set; }
		public string Result { get; set; }
		public string Error { get; set; }

		public string GetCookiesFromResponse { get; set; }
		public string SaveResponseBody { get; set; }
		public string ResponseBody { get; set; }
		public string PlainText { get; set; }
		public string CodeView { get; set; }
		public string Image { get; set; }
		public string Binary { get; set; }
		public string MissedHeaders { get; set; }

		public string Confirm { get; set; }
		public string OK { get; set; }
		public string Cancel { get; set; }

		public System.Collections.Generic.Dictionary<TaskStatus, string> TaskStatus { get; set; }

		public System.Collections.Generic.Dictionary<ValidationError, string> ValidationError { get; set; }

		public static readonly string InstallationDirectory = System.AppDomain.CurrentDomain.BaseDirectory + @"Localization\";

	}
}