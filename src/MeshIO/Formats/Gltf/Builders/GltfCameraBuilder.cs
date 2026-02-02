using CSMath;
using MeshIO.Entities;
using MeshIO.Formats.Gltf.Readers;
using MeshIO.Formats.Gltf.Schema.V2;

namespace MeshIO.Formats.Gltf.Builders;

internal class GltfCameraBuilder : GltfObjectBuilder<GltfCamera>
{
	public Camera Camera { get; private set; }

	public override void Build(GlbFileBuilder builder)
	{
		base.Build(builder);

		this.Camera = new Camera(this.GltfObject.Name);

		switch (this.GltfObject.Type)
		{
			case GltfCamera.TypeEnum.perspective when this.GltfObject.Orthographic != null:
				this.mapOrthographicCamera(this.GltfObject.Orthographic);
				break;
			case GltfCamera.TypeEnum.orthographic when this.GltfObject.Perspective != null:
				this.mapPerspectiveCamera(this.GltfObject.Perspective);
				break;
			default:
				builder.Notify($"[Camera] Unkown camera type {this.GltfObject.Type}", NotificationType.Warning);
				break;
		}
	}

	private void mapPerspectiveCamera(GltfCameraPerspective gltfCamera)
	{
		this.Camera.ProjectionType = ProjectionType.Perspective;
		this.Camera.FieldOfView = gltfCamera.Yfov;
		this.Camera.NearPlane = gltfCamera.Znear;

		if (gltfCamera.AspectRatio.HasValue)
		{
			this.Camera.AspectRatio = gltfCamera.AspectRatio.Value;
		}

		if (gltfCamera.Zfar.HasValue)
		{
			this.Camera.FarPlane = gltfCamera.Zfar.Value;
		}
	}

	private void mapOrthographicCamera(GltfCameraOrthographic gltfCamera)
	{
		this.Camera.ProjectionType = ProjectionType.Orthographic;
		this.Camera.NearPlane = gltfCamera.Znear;
		this.Camera.FarPlane = gltfCamera.Zfar;
		this.Camera.OrtographicZoom = new XY(gltfCamera.Xmag, gltfCamera.Ymag);
	}
}