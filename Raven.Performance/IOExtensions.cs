using System;
using System.IO;
using System.Threading;

namespace Raven.Performance
{
	public static class IOExtensions
	{
		public static void DeleteDirectory(string directory)
		{
			const int retries = 10;
			for (int i = 0; i < retries; i++)
			{
				try
				{
					if (Directory.Exists(directory) == false)
						return;

					try
					{
						File.SetAttributes(directory, FileAttributes.Normal);
					}
					catch (IOException)
					{
					}
					catch (UnauthorizedAccessException)
					{
					}
					Directory.Delete(directory, true);
					return;
				}
				catch (IOException)
				{
					foreach (var childDir in Directory.GetDirectories(directory))
					{
						try
						{
							File.SetAttributes(childDir, FileAttributes.Normal);
						}
						catch (IOException)
						{
						}
						catch (UnauthorizedAccessException)
						{
						}
					}
					if (i == retries - 1)// last try also failed
						throw;
					Thread.Sleep(100);
				}
			}
		}
	}
}