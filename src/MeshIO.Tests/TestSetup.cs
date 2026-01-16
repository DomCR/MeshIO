using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
[assembly: TestFramework("MeshIO.Tests.TestSetup", "MeshIO.Tests")]

namespace MeshIO.Tests;

public sealed class TestSetup : XunitTestFramework
{
	public TestSetup(IMessageSink messageSink)
	  : base(messageSink)
	{
		this.init();
	}

	private void init()
	{
		TestVariables.CreateOutputFolders();
	}
}