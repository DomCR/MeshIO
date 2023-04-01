--https://help.autodesk.com/view/3DSMAX/2016/ENU/?guid=__files_GUID_54B1F140_B304_48A2_9829_C68B345E2044_htm

fn export_fbx_sample_files = 
(
	--fbx saves
	file_versions = #( "FBX200611", "FBX200900", "FBX201000", "FBX201100", "FBX201200", "FBX201300", "FBX201400", "FBX201600", "FBX201800", "FBX201900", "FBX202000" )
	
	--Configuration
	--Geometry------------------------------------------------------------------------
	FBXExporterSetParam "SmoothingGroups" true
	FBXExporterSetParam "NormalsPerPoly" false
	FBXExporterSetParam "TangentSpaceExport" true
	FBXExporterSetParam "SmoothMeshExport" false
	FBXExporterSetParam "Preserveinstances" false
	FBXExporterSetParam "SelectionSetExport" false
	FBXExporterSetParam "GeomAsBone" false
	FBXExporterSetParam "ColladaTriangulate" true
	FBXExporterSetParam "PreserveEdgeOrientation" true
	--Animation------------------------------------------------------------------------
	FBXExporterSetParam "Animation" false
	--Cameras------------------------------------------------------------------------
	FBXExporterSetParam "Cameras" false
	--Lights------------------------------------------------------------------------
	FBXExporterSetParam "Lights" false
	--Embed Media--------------------------------------------------------------------
	FBXExporterSetParam "EmbedTextures" false
	--Units----------------------------------------------------------------------------
	--Axis Conversion-----------------------------------------------------------------
	FBXExporterSetParam "AxisConversionMethod" "None"
	FBXExporterSetParam "UpAxis" "Y" 
	--UI----------------------------------------------------------------
	FBXExporterSetParam "ShowWarnings" true
	FBXExporterSetParam "GenerateLog" false
	--FBX File Format----------------------------------------------------------------
	FBXExporterSetParam "ASCII" true
	
	file_format = "ascii"

	for i = 1 to 2 do
	(
		for version in file_versions do
		(
			FBXExporterSetParam "FileVersion" version

			file_path = "./sample_" + version + "_" + file_format + ".fbx"
			
			exportFile file_path #noPrompt selectedOnly:false using:FBXEXP 
			Print ( "exported file: " + file_path )
		)

		file_format = "binary"
		FBXExporterSetParam "ASCII" false
	)
	
	-- file_path = "./sample.fbx"
)
