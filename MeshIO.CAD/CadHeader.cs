using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace MeshIO.CAD
{
	public class CadDimensionStyle
	{
		public string PostFix { get; internal set; }
		public string AlternateDimensioningSuffix { get; internal set; }
		public bool GenerateTolerances { get; internal set; }
		public bool LimitsGeneration { get; internal set; }
		public bool TextInsideHorizontal { get; internal set; }
		public bool TextOutsideHorizontal { get; internal set; }
		public bool SuppressFirstExtensionLine { get; internal set; }
		public bool SuppressSecondExtensionnLine { get; internal set; }
		public bool AlternateUnitDimensioning { get; internal set; }
		public bool TextOutsideExtensions { get; internal set; }
		public bool SeparateArrowBlocks { get; internal set; }
		public bool TextInsideExtensions { get; internal set; }
		public bool SuppressOutsideExtensions { get; internal set; }
		public char AlternateUnitDecimalPlaces { get; internal set; }
		public char ZeroHandling { get; internal set; }
		public bool SuppressFirstDimensionLine { get; internal set; }
		public bool SuppressSecondDimensionLine { get; internal set; }
		public char ToleranceAlignment { get; internal set; }
		public char TextHorizontalAlignment { get; internal set; }
		public char DimensionFit { get; internal set; }
		public bool CursorUpdate { get; internal set; }
		public char ToleranceZeroHandling { get; internal set; }
		public char AlternateUnitZeroHandling { get; internal set; }
		public short AngularDimensionDecimalPlaces { get; internal set; }
		public char AlternateUnitToleranceZeroHandling { get; internal set; }
		public char TextVerticalAlignment { get; internal set; }
		public short DimensionUnit { get; internal set; }
		public short DecimalPlaces { get; internal set; }
		public short ToleranceDecimalPlaces { get; internal set; }
		public short AlternateUnitFormat { get; internal set; }
		public short AlternateUnitToleranceDecimalPlaces { get; internal set; }
		public double ScaleFactor { get; internal set; }
		public double ArrowSize { get; internal set; }
		public double ExtensionLineOffset { get; internal set; }
		public double DimensionLineIncrement { get; internal set; }
		public double ExtensionLineExtension { get; internal set; }
		public double Rounding { get; internal set; }
		public double DimensionLineExtension { get; internal set; }
		public double PlusTolerance { get; internal set; }
		public double MinusTolerance { get; internal set; }
		public double FixedExtensionLineLength { get; internal set; }
		public double JoggedRadiusDimensionTransverseSegmentAngle { get; internal set; }
		public short TextBackgroundFillMode { get; internal set; }
		public Color TextBackgroundColor { get; internal set; }
		public bool SuppressSecondExtensionLine { get; internal set; }
		public short AngularZeroHandling { get; internal set; }
		public short ArcLengthSymbolPosition { get; internal set; }
		public double TextHeight { get; internal set; }
		public double CenterMarkSize { get; internal set; }
		public double TickSize { get; internal set; }
		public double AlternateUnitScaleFactor { get; internal set; }
		public double LinearScaleFactor { get; internal set; }
		public double TextVerticalPosition { get; internal set; }
		public double ToleranceScaleFactor { get; internal set; }
		public double DimensionLineGap { get; internal set; }
		public double AlternateUnitRounding { get; internal set; }
		public Color DimensionLineColor { get; internal set; }
		public Color ExtensionLineColor { get; internal set; }
		public Color TextColor { get; internal set; }
		public short FractionFormat { get; internal set; }
		public short LinearUnitFormat { get; internal set; }
		public char DecimalSeparator { get; internal set; }
		public short TextMovement { get; internal set; }
		public bool IsExtensionLineLengthFixed { get; internal set; }
		public bool TextDirection { get; internal set; }
		public double AltMzf { get; internal set; }
		public double Mzf { get; internal set; }
		public short DimensionLineWeight { get; internal set; }
		public short ExtensionLineWeight { get; internal set; }
		public string AltMzs { get; internal set; }
		public string Mzs { get; internal set; }
		public short DimensionTextArrowFit { get; internal set; }
	}

	public class CadUcs
	{
		public XYZ PaperSpaceInsertionBase { get; internal set; }
		public XYZ PaperSpaceExtMin { get; internal set; }
		public XYZ PaperSpaceExtMax { get; internal set; }
		public XYZ PaperSpaceLimitsMin { get; internal set; }
		public XYZ PaperSpaceLimitsMax { get; internal set; }
		public double PaperSpaceElevation { get; internal set; }
		public XYZ Origin { get; internal set; }
		public XYZ XAxis { get; internal set; }
		public XYZ YAxis { get; internal set; }
		public XYZ OrthographicTopDOrigin { get; internal set; }
		public XYZ OrthographicBottomDOrigin { get; internal set; }
		public XYZ OrthographicLeftDOrigin { get; internal set; }
		public XYZ OrthographicRightDOrigin { get; internal set; }
		public XYZ OrthographicFrontDOrigin { get; internal set; }
		public XYZ OrthographicBackDOrigin { get; internal set; }
	}

	internal class CadObjectPointerCollection
	{
		public ulong CMATERIAL { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong CLAYER { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong TEXTSTYLE { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong CELTYPE { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DIMSTYLE { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong CMLSTYLE { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong UCSNAME_PSPACE { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong UCSNAME_MSPACE { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong PUCSORTHOREF { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong PUCSBASE { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong UCSORTHOREF { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DIMTXSTY { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DIMLDRBLK { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DIMBLK { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DIMBLK1 { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DIMBLK2 { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DICTIONARY_LAYOUTS { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DICTIONARY_PLOTSETTINGS { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DICTIONARY_PLOTSTYLES { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong CPSNID { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong PAPER_SPACE { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong MODEL_SPACE { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong BYLAYER { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong BYBLOCK { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong CONTINUOUS { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DIMLTYPE { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DIMLTEX1 { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DIMLTEX2 { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong VIEWPORT_ENTITY_HEADER_CONTROL_OBJECT { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DICTIONARY_ACAD_GROUP { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DICTIONARY_ACAD_MLINESTYLE { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DICTIONARY_NAMED_OBJECTS { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong BLOCK_CONTROL_OBJECT { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong LAYER_CONTROL_OBJECT { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong STYLE_CONTROL_OBJECT { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong LINETYPE_CONTROL_OBJECT { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong VIEW_CONTROL_OBJECT { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong UCS_CONTROL_OBJECT { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong VPORT_CONTROL_OBJECT { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong APPID_CONTROL_OBJECT { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DIMSTYLE_CONTROL_OBJECT { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DICTIONARY_MATERIALS { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DICTIONARY_COLORS { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DICTIONARY_VISUALSTYLE { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong INTERFEREOBJVS { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong INTERFEREVPVS { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong DRAGVS { get { return getPointer(); } set { setPointer(value: value); } }
		public ulong UCSBASE { get { return getPointer(); } set { setPointer(value: value); } }

		private Dictionary<string, ulong> m_pointers = new Dictionary<string, ulong>();

		public ulong GetPointer(string name)
		{
			return m_pointers[name];
		}
		public void SetPointer(string name, ulong value)
		{
			m_pointers[name] = value;
		}
		public Queue<ulong> GetPointers()
		{

			throw new NotImplementedException();
		}

		public ulong getPointer([CallerMemberName] string name = null)
		{
			return GetPointer(name);
		}
		public void setPointer([CallerMemberName] string name = null, ulong value = 0)
		{
			SetPointer(name, value);
		}
	}

	public class CadHeader
	{
		public ACadVersion Version { get; private set; }

		#region Header System Variables
		/// <summary>
		/// System variable REQUIREDVERSIONS.
		/// The default value is 0.
		/// Read only.
		/// </summary>
		/// <remarks>Only in <see cref="ACadVersion.AC1024"/> or above.</remarks>
		public long RequiredVersions { get; internal set; }
		/// <summary>
		/// System variable DIMASO.
		/// </summary>
		public bool AssociatedDimensions { get; set; } = true;
		/// <summary>
		/// System variable DIMSHO.
		/// </summary>
		public bool UpdateDimensionsWhileDragging { get; set; } = true;
		/// <summary>
		/// Undocumented
		/// </summary>
		public bool DIMSAV { get; set; }
		/// <summary>
		/// System variable PLINEGEN.
		/// </summary>
		public bool PolylineLineTypeGeneration { get; set; }
		/// <summary>
		/// System variable ORTHOMODE.
		/// </summary>
		public bool OrthoMode { get; set; }
		/// <summary>
		/// System variable REGENMODE.
		/// </summary>
		public bool RegenerationMode { get; set; }
		/// <summary>
		/// System variable FILLMODE.
		/// </summary>
		public bool FillMode { get; set; }
		/// <summary>
		/// System variable QTEXTMODE.
		/// </summary>
		public bool QuickTextMode { get; set; }
		/// <summary>
		/// System variable PSLTSCALE.
		/// </summary>
		public bool PaperSpaceLineTypeScaling { get; set; }
		/// <summary>
		/// System variable LIMCHECK.
		/// </summary>
		public bool LimitCheckingOn { get; set; }
		/// <summary>
		/// System variable BLIPMODE.
		/// </summary>
		public bool BlipMode { get; set; }
		public bool UserTimer { get; set; }
		public bool SketchPolylines { get; set; }
		public short AngularDirection { get; set; }
		public bool ShowSplineControlPoints { get; set; }
		public bool MirrorText { get; set; }
		public bool WorldView { get; set; }
		public bool ShowModelSpace { get; set; }
		public bool PaperSpaceLimitsChecking { get; set; }
		public bool RetainXRefDependentVisibilitySettings { get; set; }
		public bool DisplaySilhouetteCurves { get; set; }
		public bool CreateEllipseAsPolyline { get; set; }
		public bool ProxyGraphics { get; set; }
		public short SpatialIndexMaxTreeDepth { get; set; }
		public short LinearUnitFormat { get; set; }
		public short LinearUnitPrecision { get; set; }
		public short AngularUnit { get; set; }
		public short AngularUnitPrecision { get; set; }
		public short ObjectSnapMode { get; set; }
		public short AttributeVisibility { get; set; }
		public short PointDisplayMode { get; set; }
		public short UserShort1 { get; set; }
		public short UserShort2 { get; set; }
		public short UserShort3 { get; set; }
		public short UserShort4 { get; set; }
		public short UserShort5 { get; set; }
		public short NumberOfSplineSegments { get; set; }
		public short SurfaceDensityU { get; set; }
		public short SurfaceDensityV { get; set; }
		public short SurfaceType { get; set; }
		public short SurfaceMeshTabulationCount1 { get; set; }
		public short SurfaceMeshTabulationCount2 { get; set; }
		public short SplineType { get; set; }
		public short ShadeEdge { get; set; }
		public short ShadeDiffuseToAmbientPercentage { get; set; }
		public short UnitMode { get; set; }
		public short MaxViewportCount { get; set; }
		public short SurfaceIsolineCount { get; set; }
		public short CurrentMultilineJustification { get; set; }
		public short TextQuality { get; set; }
		public double LineTypeScale { get; set; }
		public double TextHeightDefault { get; set; }
		public double TraceWidthDefault { get; set; }
		public double SketchIncrement { get; set; }
		public double FilletRadius { get; set; }
		public double ThicknessDefault { get; set; }
		public double AngleBase { get; set; }
		public double PointDisplaySize { get; set; }
		public double PolylineWidthDefault { get; set; }
		public double UserDouble1 { get; set; }
		public double UserDouble2 { get; set; }
		public double UserDouble3 { get; set; }
		public double UserDouble4 { get; set; }
		public double UserDouble5 { get; set; }
		public double ChamferDistance1 { get; set; }
		public double ChamferDistance2 { get; set; }
		public double ChamferLength { get; set; }
		public double ChamferAngle { get; set; }
		public double FacetResolution { get; set; }
		public double CurrentMultilineScale { get; set; }
		public double CurrentEntityLinetypeScale { get; set; }
		public string MenuFileName { get; set; }
		public DateTime CreateDateTime { get; set; }
		public DateTime UpdateDateTime { get; set; }
		public TimeSpan TotalEditingTime { get; set; }
		public TimeSpan UserElapsedTimeSpan { get; set; }
		public Color CurrentEntityColor { get; set; }
		public double ViewportDefaultViewScaleFactor { get; set; }
		/// <summary>
		/// PSPACE
		/// </summary>
		public CadUcs PaperSpaceUcs { get; set; } = new CadUcs();
		public XYZ InsertionBase { get; set; }
		public XYZ ExtMin { get; set; }
		public XYZ ExtMax { get; set; }
		public XYZ LimitsMin { get; set; }
		public XYZ LimitsMax { get; set; }
		public double Elevation { get; set; }
		public CadUcs Ucs { get; set; } = new CadUcs();
		public CadDimensionStyle DimensionStyleOverrides { get; set; } = new CadDimensionStyle();
		public string DimensionBlockName { get; set; }
		public string DimensionBlockNameFirst { get; set; }
		public string DimensionBlockNameSecond { get; set; }
		public short StackedTextAlignment { get; set; }
		public short StackedTextSizePercentage { get; set; }
		public string HyperLinkBase { get; set; }
		public short CurrentEntityLineWeight { get; set; }
		public short EndCaps { get; set; }
		public short JoinStyle { get; set; }
		public short DisplayLineWeight { get; set; }
		public short XEdit { get; set; }
		public short ExtendedNames { get; set; }
		public short PlotStyleMode { get; set; }
		public short LoadOLEObject { get; set; }
		public short InsUnits { get; set; }
		public short CurrentEntityPlotStyleType { get; set; }
		public string FingerPrintGuid { get; set; }
		public string VersionGuid { get; set; }
		public byte EntitySortingFlags { get; set; }
		public byte IndexCreationFlags { get; set; }
		public byte HideText { get; set; }
		public byte ExternalReferenceClippingBoundaryType { get; set; }
		public byte DimensionAssociativity { get; set; }
		public byte HaloGapPercentage { get; set; }
		public Color ObscuredColor { get; set; }
		public Color InterfereColor { get; set; }
		public byte ObscuredType { get; set; }
		public byte IntersectionDisplay { get; set; }
		public string ProjectName { get; set; }
		public bool CameraDisplayObjects { get; set; }
		public double StepsPerSecond { get; set; }
		public double StepSize { get; set; }
		public double Dw3DPrecision { get; set; }
		public double LensLength { get; set; }
		public double CameraHeight { get; set; }
		public char SolidsRetainHistory { get; set; }
		public char ShowSolidsHistory { get; set; }
		public double SweptSolidWidth { get; set; }
		public double SweptSolidHeight { get; set; }
		public double DraftAngleFirstCrossSection { get; set; }
		public double DraftAngleSecondCrossSection { get; set; }
		public double DraftMagnitudeFirstCrossSection { get; set; }
		public double DraftMagnitudeSecondCrossSection { get; set; }
		public short SolidLoftedShape { get; set; }
		public char LoftedObjectNormals { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public double NorthDirection { get; set; }
		public int TimeZone { get; set; }
		public char DisplayLightGlyphs { get; set; }
		public char DwgUnderlayFramesVisibility { get; set; }
		public char DgnUnderlayFramesVisibility { get; set; }
		public byte ShadowMode { get; set; }
		public double ShadowPlaneLocation { get; set; }
		public string StyleSheetName { get; set; }
		#endregion

		internal ulong HandleSeed { get; set; }

		public CadHeader(ACadVersion version)
		{
			Version = version;
		}
	}
}
