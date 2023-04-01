using MeshIO.Core;
using Xunit.Abstractions;

namespace MeshIO.Tests.Shared
{
	public class ReaderTestsBase
	{
		protected readonly ITestOutputHelper _output;

		public ReaderTestsBase(ITestOutputHelper output)
		{
			_output = output;
		}

		protected void onNotification(object sender, NotificationEventArgs e)
		{
			_output.WriteLine(e.Message);
		}
	}
}
