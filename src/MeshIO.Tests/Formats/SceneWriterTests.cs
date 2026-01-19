using MeshIO.Entities.Primitives;
using MeshIO.Formats;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.Tests.Formats
{
	public class SceneWriterTests
	{
		public static TheoryData<FormatTestCase> OutputCases { get; } = new();

		private readonly ITestOutputHelper _output;

		static SceneWriterTests()
		{
			OutputCases.Add(new FormatTestCase("basic", FileFormatType.Stl, ContentType.ASCII));
			OutputCases.Add(new FormatTestCase("basic", FileFormatType.Stl, ContentType.Binary));
		}

		public SceneWriterTests(ITestOutputHelper output)
		{
			_output = output;
		}

		[Theory]
		[MemberData(nameof(OutputCases))]
		public void WriteTest(FormatTestCase test)
		{
			Scene scene = this.createScene();
			var options = FileFormat.GetWriterOptions(test.Format);
			options.ContentType = test.Content;
			using (ISceneWriter writer = FileFormat.GetWriter(test.Path, scene, options, onNotification))
			{
				writer.Write();
			}
		}

		private Scene createScene()
		{
			Scene scene = new();
			scene.RootNode.Entities.Add(new Box("my_box"));
			return scene;
		}

		private void onNotification(object sender, MeshIO.NotificationEventArgs e)
		{
			this._output.WriteLine(e.Message);
		}
	}
}