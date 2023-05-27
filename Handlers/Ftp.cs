using ColdShineSoft.CustomFileCopier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColdShineSoft.CustomFileCopier.Handlers
{
	public class Ftp : Models.ResultHandler
	{
		public override string Name { get; }= "FTP";

		public override bool Remote => true;

		public override string CheckTargetDirectoryValid(Job job)
		{
			FluentFTP.FtpClient ftpClient = new FluentFTP.FtpClient(job.TargetServer, job.TargetPort, job.TargetUserName, job.TargetPassword);
			//ftpClient.EncryptionMode = FluentFTP.FtpEncryptionMode.Auto;
			try
			{
				ftpClient.Connect();
				if (ftpClient.DirectoryExists(job.TargetDirectoryPath))
					return null;
				ftpClient.CreateDirectory(job.TargetDirectoryPath);
				return null;
			}
			catch (System.Exception exception)
			{
				return exception.Message;
			}
			finally
			{
				ftpClient.Disconnect();
			}
		}

		public override bool TargetDirectoryEmpty(Job job)
		{
			FluentFTP.FtpClient ftpClient = new FluentFTP.FtpClient(job.TargetServer, job.TargetPort, job.TargetUserName, job.TargetPassword);
			try
			{
				ftpClient.Connect();
				return ftpClient.GetNameListing(job.TargetDirectoryPath).Length == 0;
			}
			finally
			{
				ftpClient.Disconnect();
			}
		}

		public override void Execute(Models.Job job)
		{
			FluentFTP.FtpClient ftpClient = new FluentFTP.FtpClient(job.TargetServer, job.TargetPort, job.TargetUserName, job.TargetPassword);
			try
			{
				ftpClient.Connect();
				foreach (Models.File sourceFile in job.SourceFiles)
				{
					sourceFile.Result = Models.CopyResult.Copying;
					string targetFilePath = job.GetTargetAbsoluteFilePath(sourceFile.Path);
					string targetDirectory = System.IO.Path.GetDirectoryName(targetFilePath).Replace('\\', '/');
					//if (!ftpClient.FileExists(targetDirectory))
					//	try
					//	{
					//		ftpClient.CreateDirectory(targetDirectory);
					//	}
					//	catch (System.Exception exception)
					//	{
					//		sourceFile.Result = Models.CopyResult.Failure;
					//		sourceFile.Error = exception.Message;
					//		continue;
					//	}
					try
					{
						ftpClient.UploadFile(sourceFile.Path, targetFilePath, createRemoteDir: true);
					}
					catch (System.Exception exception)
					{
						sourceFile.Result = Models.CopyResult.Failure;
						sourceFile.Error = exception.Message;
						continue;
					}
					sourceFile.Result = Models.CopyResult.Success;
					job.Task.CopiedFileCount++;
					job.Task.CopiedFileSize += sourceFile.FileInfo.Length;
				}
			}
			finally
			{
				ftpClient.DisconnectAsync();
			}
		}
	}
}
