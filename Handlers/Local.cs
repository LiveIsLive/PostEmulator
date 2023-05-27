using ColdShineSoft.CustomFileCopier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Handlers
{
	public class Local : Models.ResultHandler
	{
		public override string Name
		{
			get
			{
				if (System.Threading.Thread.CurrentThread.CurrentUICulture.LCID == ChineseCulture?.LCID)
					return "本地目录";
				else return "Local Directory";
			}
		}

		public override bool Remote => false;

		public override string CheckTargetDirectoryValid(Job job)
		{
			try
			{
				if (System.IO.Directory.Exists(job.TargetDirectoryPath))
					return null;
				System.IO.Directory.CreateDirectory(job.TargetDirectoryPath);
				return null;
			}
			catch(System.Exception exception)
			{
				return exception.Message;
			}
		}

		public override bool TargetDirectoryEmpty(Job job)
		{
			return System.IO.Directory.EnumerateFileSystemEntries(job.TargetDirectoryPath).FirstOrDefault() == null;
		}

		public override void Execute(Models.Job job)
		{
			foreach (Models.File sourceFile in job.SourceFiles)
			{
				//if (sourceFile.Result != Models.CopyResult.Success)
				//{
					sourceFile.Result = Models.CopyResult.Copying;
					string targetFilePath = job.GetTargetAbsoluteFilePath(sourceFile.Path);
					string targetDirectory = System.IO.Path.GetDirectoryName(targetFilePath);
					if (!System.IO.Directory.Exists(targetDirectory))
						try
						{
							System.IO.Directory.CreateDirectory(targetDirectory);
						}
						catch (System.Exception exception)
						{
							sourceFile.Result = Models.CopyResult.Failure;
							sourceFile.Error = exception.Message;
							//return exception.Message;
							continue;
						}
					try
					{
						System.IO.File.Copy(sourceFile.Path, targetFilePath, true);
					}
					catch (System.Exception exception)
					{
						sourceFile.Result = Models.CopyResult.Failure;
						sourceFile.Error = exception.Message;
						//return exception.Message;
						continue;
					}
				//}
				sourceFile.Result = Models.CopyResult.Success;
				job.Task.CopiedFileCount++;
				job.Task.CopiedFileSize += sourceFile.FileInfo.Length;
			}
		}
	}
}