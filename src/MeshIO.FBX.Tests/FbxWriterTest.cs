using MeshIO.Entities.Geometries;
using MeshIO.Entities.Primitives;
using MeshIO.FBX.Readers.Parsers;
using MeshIO.Tests.Shared;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace MeshIO.FBX.Tests
{
    public class FbxWriterTest : IOTestsBase
    {
        public static readonly TheoryData<FbxVersion> Versions = FbxTestCasesData.Versions;

        public FbxWriterTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public void IOTest()
        {
            string inPath = Path.Combine(FolderPath.InFilesFbx, "sample_basic_box_binary.fbx");
            inPath = Path.Combine(FolderPath.InFilesFbx, "sample_basic_box_ascii_clean.fbx");

            FbxRootNode root = null;
            using (FbxBinaryParser parser = new FbxBinaryParser(File.OpenRead(inPath)))
            {
                root = parser.Parse();
            }

            using (FbxAsciiWriter writer = new FbxAsciiWriter(root, File.Create(Path.Combine(FolderPath.OutFilesFbx, $"io_ascii.fbx"))))
            {
                writer.Write();
            }
        }

        [Theory]
        [MemberData(nameof(Versions))]
        public void WriteEmptyAsciiStream(FbxVersion version)
        {
            FbxWriterOptions options = new FbxWriterOptions
            {
                IsBinaryFormat = false,
                Version = version,
            };

            using (FbxWriter writer = new FbxWriter(new MemoryStream(), new Scene(), options))
            {
                writer.OnNotification += this.onNotification;
                writer.Write(new FbxWriterOptions() { IsBinaryFormat = false });
            }
        }

        [Theory]
        [MemberData(nameof(Versions))]
        public void WriteEmptyBinaryStream(FbxVersion version)
        {
            FbxWriterOptions options = new FbxWriterOptions
            {
                IsBinaryFormat = true,
                Version = version,
            };

            using (FbxWriter writer = new FbxWriter(new MemoryStream(), new Scene()))
            {
                writer.OnNotification += this.onNotification;
                writer.Write(new FbxWriterOptions() { IsBinaryFormat = true });
            }
        }

        [Theory]
        [MemberData(nameof(Versions))]
        public void WriteAsciiFbxWithMesh(FbxVersion version)
        {
            FbxWriterOptions options = new FbxWriterOptions
            {
                IsBinaryFormat = false,
                Version = version,
            };

            string path = Path.Combine(FolderPath.OutFilesFbx, $"box_{version}_ascii.fbx");

            Scene scene = new Scene();

            Node box = new Node("my_node");
            Mesh mesh = new Box("my_box").CreateMesh();
            box.Entities.Add(mesh);

            scene.RootNode.Nodes.Add(box);

            using (FbxWriter writer = new FbxWriter(path, scene, options))
            {
                writer.OnNotification += this.onNotification;
                writer.Write(new FbxWriterOptions() { IsBinaryFormat = false });
            }
        }

        [Theory]
        [MemberData(nameof(Versions))]
        public void WriteBinaryFbxWithMesh(FbxVersion version)
        {
            FbxWriterOptions options = new FbxWriterOptions
            {
                IsBinaryFormat = true,
                Version = version,
            };

            string path = Path.Combine(FolderPath.OutFilesFbx, $"box_{version}_bin.fbx");

            Scene scene = new Scene();

            Node box = new Node("my_node");
            Mesh mesh = new Box("my_box").CreateMesh();
            box.Entities.Add(mesh);

            scene.RootNode.Nodes.Add(box);

            using (FbxWriter writer = new FbxWriter(path, scene, options))
            {
                writer.OnNotification += this.onNotification;
                writer.Write(new FbxWriterOptions() { IsBinaryFormat = false });
            }

            FbxRootNode root = null;
            using (FbxBinaryParser parser = new FbxBinaryParser(File.OpenRead(path)))
            {
                root = parser.Parse();
            }

            string t = Path.Combine(FolderPath.OutFilesFbx, $"io_self_ascii.fbx");
            string tb = Path.Combine(FolderPath.OutFilesFbx, $"io_self_bin.fbx");
            using (FbxAsciiWriter writer = new FbxAsciiWriter(root, File.Create(t)))
            {
                writer.Write();
            }

            using (FbxAsciiParser ascii = new FbxAsciiParser(File.OpenRead(t)))
            {
                root = ascii.Parse();
            }

            using (FbxBinaryWriter writer = new FbxBinaryWriter(root, File.Create(tb)))
            {
                writer.Write();
            }
        }
    }
}
