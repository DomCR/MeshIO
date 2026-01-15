using MeshIO.Core;
using Xunit.Abstractions;

namespace MeshIO.Tests.Shared
{
	public abstract class IOTestsBase
	{
		protected readonly ITestOutputHelper _output;

		public IOTestsBase(ITestOutputHelper output)
		{
			_output = output;
		}

		protected void onNotification(object sender, NotificationEventArgs e)
		{
			_output.WriteLine(e.Message);
		}
	}
}
