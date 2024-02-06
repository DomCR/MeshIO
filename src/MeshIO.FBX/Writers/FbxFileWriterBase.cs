using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MeshIO.FBX.Writers.Connections;
using MeshIO.FBX.Writers.Objects;
using MeshIO.FBX.Writers.StreamWriter;

namespace MeshIO.FBX.Writers
{
	internal abstract class FbxFileWriterBase : IDisposable
	{
		public FbxWriterOptions Options { get; }

		public Scene Scene { get; }

		public Node RootNode { get { return this.Scene.RootNode; } }

		private readonly IFbxStreamWriter _writer;

		private readonly Dictionary<string, FbxPropertyTemplate> _tempaltes = new();

		private readonly Dictionary<string, List<IFbxObjectWriter>> _definedObjects = new();

		private readonly Dictionary<ulong, IFbxObjectWriter> _objectWriters = new();

		private readonly List<Connections.FbxConnection> _connections = new();

		protected FbxFileWriterBase(Scene scene, FbxWriterOptions options, Stream stream)
		{
			this.Scene = scene;
			this.Options = options;
			this._writer = FbxStreamWriterBase.Create(options, stream);
		}

		public static FbxFileWriterBase Create(Scene scene, FbxWriterOptions options, Stream stream)
		{
			FbxVersion version = options.Version;
			switch (version)
			{
				case FbxVersion.v2000:
				case FbxVersion.v2001:
				case FbxVersion.v3000:
				case FbxVersion.v3001:
				case FbxVersion.v4000:
				case FbxVersion.v4001:
				case FbxVersion.v4050:
				case FbxVersion.v5000:
				case FbxVersion.v5800:
				case FbxVersion.v6000:
				case FbxVersion.v6100:
					throw new NotSupportedException($"Fbx version {version} no supported for writer");
				case FbxVersion.v7000:
				case FbxVersion.v7100:
				case FbxVersion.v7200:
				case FbxVersion.v7300:
				case FbxVersion.v7400:
				case FbxVersion.v7500:
				case FbxVersion.v7600:
				case FbxVersion.v7700:
					return new FbxFileWriter7000(scene, options, stream);
				default:
					throw new NotSupportedException($"Unknown Fbx version {version} for writer");

			}
		}

		public void Write()
		{
			this.initializeRoot();

			this.writeHeaderComment();

			this.writeFBXHeaderExtension();

			if (this.Options.IsBinaryFormat)
			{
				//Seems to need some extra fields
				Random random = new Random();
				var fileId = new byte[16];
				random.NextBytes(fileId);

				_writer.WritePairNodeValue("FileId", fileId);
				_writer.WritePairNodeValue("CreationTime", "1970-01-01 10:00:00:000");
				_writer.WritePairNodeValue("Creator", "MeshIO.FBX");
			}

			this.writeGlobalSettings();

			this.writeDocuments();

			this.writeReferences();

			this.writeDefinitions();

			this.writeObjects();

			this.writeConnections();
		}

		public bool TryGetPropertyTemplate(string fbxName, out FbxPropertyTemplate template)
		{
			return this._tempaltes.TryGetValue(fbxName, out template);
		}

		public void WriteProperties(IEnumerable<Property> properties)
		{
			if (properties == null || !properties.Any())
			{
				return;
			}

			this._writer.WriteName(FbxFileToken.GetPropertiesName(this.Options.Version));
			this._writer.WriteOpenBracket();

			foreach (Property p in properties)
			{
				if (p is not FbxProperty fbxProp)
				{
					fbxProp = FbxProperty.CreateFrom(p);
				}

				if (fbxProp == null)
				{
					this._writer.WriteEmptyLine();
					continue;
				}

				this._writer.WriteName("P");
				this._writer.WriteValue(fbxProp.Name);
				this._writer.WriteValue(fbxProp.FbxType);
				this._writer.WriteValue(fbxProp.Label);
				this._writer.WriteValue(FbxProperty.MapPropertyFlags(fbxProp.Flags));

				if (fbxProp.Value is null)
				{
					this._writer.WriteEmptyLine();
					continue;
				}

				object value = fbxProp.GetFbxValue();
				if (value is System.Array arr)
				{
					foreach (var v in arr)
					{
						this._writer.WriteValue(v);
					}
				}
				else
				{
					this._writer.WriteValue(value);
				}

				this._writer.WriteEmptyLine();
			}

			this._writer.WriteCloseBracket();
			this._writer.WriteEmptyLine();
		}

		public void CreateConnection(Element3D child, IFbxObjectWriter parent)
		{
			IFbxObjectWriter objwriter = FbxObjectWriterFactory.Create(child);
			if (objwriter is null)
			{
				return;
			}

			FbxConnection conn = new FbxConnection(objwriter, parent);

			this._connections.Add(conn);

			objwriter.ProcessChildren(this);

			this._objectWriters.Add(child.Id.Value, objwriter);
			if (!this._definedObjects.TryGetValue(objwriter.FbxObjectName, out List<IFbxObjectWriter> lst))
			{
				this._definedObjects.Add(objwriter.FbxObjectName, lst = new List<IFbxObjectWriter>());
			}
			lst.Add(objwriter);
		}

		private void initializeRoot()
		{
			//Root node should be processed to create the connections but it is not writen in the file
			this.RootNode.Id = 0;

			IFbxObjectWriter objwriter = FbxObjectWriterFactory.Create(this.RootNode);

			objwriter.ProcessChildren(this);
		}

		private void writeHeaderComment()
		{
			int version = (int)this.Options.Version;

			int major = (version / 1000) % 10;
			int minor = (version / 100) % 10;

			this._writer.WriteComment($" FBX {major}.{minor}.0 project file");
		}

		private void writeFBXHeaderExtension()
		{
			_writer.WriteName(FbxFileToken.FBXHeaderExtension);
			_writer.WriteOpenBracket();

			_writer.WritePairNodeValue(FbxFileToken.Creator, "MeshIO.FBX");
			_writer.WritePairNodeValue(FbxFileToken.FBXVersion, (int)this.Options.Version);
			_writer.WritePairNodeValue(FbxFileToken.FBXHeaderVersion, 1003);

			if (this.Options.IsBinaryFormat)
			{
				_writer.WritePairNodeValue(FbxFileToken.EncryptionType, 0);
			}

			_writer.WriteName(FbxFileToken.CreationTimeStamp);
			_writer.WriteOpenBracket();
			System.DateTime now = System.DateTime.Now;
			_writer.WritePairNodeValue(FbxFileToken.Version, 1000);
			_writer.WritePairNodeValue(nameof(now.Year), now.Year);
			_writer.WritePairNodeValue(nameof(now.Month), now.Month);
			_writer.WritePairNodeValue(nameof(now.Day), now.Day);
			_writer.WritePairNodeValue(nameof(now.Hour), now.Hour);
			_writer.WritePairNodeValue(nameof(now.Minute), now.Minute);
			_writer.WritePairNodeValue(nameof(now.Second), now.Second);
			_writer.WritePairNodeValue(nameof(now.Millisecond), now.Millisecond);
			_writer.WriteCloseBracket();
			_writer.WriteEmptyLine();

			this.writeSceneInfo();

			_writer.WriteCloseBracket();
			_writer.WriteEmptyLine();
		}

		private void writeSceneInfo()
		{
			//TODO: writeSceneInfo
		}

		private void writeGlobalSettings()
		{
			FbxGlobalSettingsWriter settingsWriter = new FbxGlobalSettingsWriter();

			_writer.WriteName(FbxFileToken.GlobalSettings);
			_writer.WriteOpenBracket();
			_writer.WritePairNodeValue(FbxFileToken.Version, 100);

			this.WriteProperties(settingsWriter.FbxProperties);

			_writer.WriteCloseBracket();
			_writer.WriteEmptyLine();

			this._definedObjects.Add(FbxFileToken.GlobalSettings, new List<IFbxObjectWriter> { settingsWriter });
		}

		private void writeDocuments()
		{
			_writer.WriteName(FbxFileToken.Documents);

			_writer.WriteOpenBracket();
			_writer.WritePairNodeValue(FbxFileToken.Count, this.Scene.SubScenes.Count + 1);

			_writer.WriteName(FbxFileToken.Document);

			_writer.WriteValue(Scene.GetIdOrDefault());
			_writer.WriteValue(Scene.Name);
			_writer.WriteValue(FbxFileToken.Scene);

			_writer.WriteOpenBracket();

			this.WriteProperties(Scene.Properties);

			_writer.WritePairNodeValue(FbxFileToken.RootNode, 0L);

			_writer.WriteCloseBracket();
			_writer.WriteEmptyLine();

			foreach (Scene s in Scene.SubScenes)
			{
				//TODO: WriteSubScenes Verify this implementation
				_writer.WriteValue(s.GetIdOrDefault());
				_writer.WriteValue(s.Name);
				_writer.WriteValue(FbxFileToken.Scene);
				_writer.WriteOpenBracket();

				this.WriteProperties(s.Properties);

				_writer.WriteCloseBracket();
			}

			_writer.WriteCloseBracket();
			_writer.WriteEmptyLine();
		}

		private void writeReferences()
		{
			_writer.WriteName(FbxFileToken.References);
			_writer.WriteOpenBracket();

			//TODO: Write fbx references

			_writer.WriteCloseBracket();
			_writer.WriteEmptyLine();
		}

		private void writeDefinitions()
		{
			_writer.WriteName(FbxFileToken.Definitions);
			_writer.WriteOpenBracket();

			_writer.WritePairNodeValue(FbxFileToken.Version, 100);
			_writer.WritePairNodeValue(FbxFileToken.Count, this._definedObjects.Sum(o => o.Value.Count));

			foreach (var item in this._definedObjects)
			{
				_writer.WriteName(FbxFileToken.ObjectType);
				_writer.WriteValue(item.Key);
				_writer.WriteOpenBracket();
				_writer.WritePairNodeValue(FbxFileToken.Count, item.Value.Count);

				if (item.Key == FbxFileToken.GlobalSettings)
				{
					_writer.WriteCloseBracket();
					_writer.WriteEmptyLine();
					continue;
				}

				FbxPropertyTemplate template = FbxPropertyTemplate.Create(item.Key);

				this._tempaltes.Add(item.Key, template);

				_writer.WriteName("PropertyTemplate");
				_writer.WriteValue(template.Name);
				_writer.WriteOpenBracket();

				this.WriteProperties(template.Properties.Values);

				_writer.WriteCloseBracket();
				_writer.WriteEmptyLine();

				_writer.WriteCloseBracket();
				_writer.WriteEmptyLine();
			}

			_writer.WriteCloseBracket();
			_writer.WriteEmptyLine();
		}

		private void writeObjects()
		{
			_writer.WriteName(FbxFileToken.Objects);
			_writer.WriteOpenBracket();

			foreach (IFbxObjectWriter obj in this._objectWriters.Values)
			{
				if (!this._tempaltes.TryGetValue(obj.FbxObjectName, out FbxPropertyTemplate template))
				{
					template = new FbxPropertyTemplate();
				}

				obj.ApplyTemplate(template);

				obj.Write(this, _writer);
			}

			_writer.WriteCloseBracket();
			_writer.WriteEmptyLine();
		}

		private void writeConnections()
		{
			_writer.WriteName(FbxFileToken.Connections);
			_writer.WriteOpenBracket();

			foreach (FbxConnection c in this._connections)
			{
				this._writer.WriteComment(c.GetComment());

				_writer.WriteName("C");

				switch (c.ConnectionType)
				{
					case FbxConnectionType.ObjectObject:
						_writer.WriteValue("OO");
						break;
					default:
						throw new NotImplementedException();
				}

				_writer.WriteValue(c.Child.Id);
				_writer.WriteValue(c.Parent.Id);

				_writer.WriteEmptyLine();
			}

			_writer.WriteCloseBracket();
			_writer.WriteEmptyLine();
		}

		public void Dispose()
		{
			this._writer.Dispose();
		}
	}
}
