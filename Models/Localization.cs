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
		public string AutoRunWhenFilesFiltered { get; set; }
		public string Language { get; set; }
		public string FileList { get; set; }
		public string Help { get; set; }
		public string Tutorial { get; set; }
		public string About { get; set; }
		public string Theme { get; set; }

		public string Job { get; set; }
		public string AddJob { get; set; }
		public string NewJob { get; set; }
		public string Run { get; set; }
		public string Stop { get; set; }
		public string RunTask { get; set; }
		public string Status { get; set; }
		public string CompressFilePath { get; set; }
		public string CompressToZipFile { get; set; }
		public string AddNowToCompressFileName { get; set; }
		public string NowFormatString { get; set; }

		public string Name { get; set; }
		public string SourceDirectory { get; set; }
		public string ResultHandler { get; set; }
		public string TargetDirectory { get; set; }
		public string Server { get; set; }
		public string Port { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }

		public string Condition { get; set; }
		public string ConditionDesignMode { get; set; }
		public string AddCondition { get; set; }
		public string Property { get; set; }
		public string Operator { get; set; }
		public string LeftBracket { get; set; }
		public string RightBracket { get; set; }
		public string Connective { get; set; }
		public string Value { get; set; }
		public string Expression { get; set; }

		public string Path { get; set; }
		public string Directory { get; set; }
		public string Result { get; set; }
		public string Error { get; set; }

		public string Files { get; set; }
		public string Bytes { get; set; }

		public string OpenFileDialog { get; set; }
		public string TargetDirectoryIsNotEmpty { get; set; }

		public string Confirm { get; set; }
		public string OK { get; set; }
		public string Cancel { get; set; }

		public string FilePaths { get; set; }
		public string DirectoryPaths { get; set; }

		public System.Collections.Generic.Dictionary<string,string> Properties { get; set; }

		public System.Collections.Generic.Dictionary<string,string> Operators { get; set; }

		public System.Collections.Generic.Dictionary<TaskStatus, string> TaskStatus { get; set; }

		public System.Collections.Generic.Dictionary<ValidationError, string> ValidationError { get; set; }

		public static readonly string InstallationDirectory = System.AppDomain.CurrentDomain.BaseDirectory + @"Localization\";

	}
}