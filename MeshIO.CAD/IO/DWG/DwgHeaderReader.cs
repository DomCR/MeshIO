using CSUtilities.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MeshIO.CAD.IO.DWG
{
	/// <summary>
	/// Util class to read the cad header.
	/// </summary>
	internal static class DwgHeaderReader
	{
		/// <summary>
		/// R13-R14 Only
		/// </summary>
		private static bool R13_14Only;
		/// <summary>
		/// R13-R15 Only
		/// </summary>
		private static bool R13_15Only;
		/// <summary>
		/// R2000+ Only
		/// </summary>
		private static bool R2000Plus;
		/// <summary>
		/// Pre-2004 Only
		/// </summary>
		private static bool R2004Pre;
		/// <summary>
		/// R2004+
		/// </summary>
		private static bool R2004Plus;
		/// <summary>
		/// +R2007 Only
		/// </summary>
		private static bool R2007Plus;
		/// <summary>
		/// R2010+ Only
		/// </summary>
		private static bool R2010Plus;
		/// <summary>
		/// R2013+
		/// </summary>
		private static bool R2013Plus;
		/// <summary>
		/// R2018+
		/// </summary>
		private static bool R2018Plus;

		private static IDwgStreamHandler mianHandler;

		public static CadHeader Read(IDwgStreamHandler shandler, ACadVersion version,
			int acadMaintenanceVersion, out CadObjectPointerCollection objectPointers)
		{
			//Save the parameter handler in a local variable
			mianHandler = shandler;

			setVersionConditionals(version);

			CadHeader header = new CadHeader(version);
			objectPointers = new CadObjectPointerCollection();

			//0xCF,0x7B,0x1F,0x23,0xFD,0xDE,0x38,0xA9,0x5F,0x7C,0x68,0xB8,0x4E,0x6D,0x33,0x5F
			byte[] sn = shandler.ReadSentinel();

			//RL : Size of the section.
			long size = shandler.ReadRawLong();

			//R2010/R2013 (only present if the maintenance version is greater than 3!) or R2018+:
			if (R2010Plus && acadMaintenanceVersion > 3 || R2018Plus)
			{
				//Unknown (4 byte long), might be part of a 64-bit size.
				long unknown = shandler.ReadRawLong();
			}

			long initialPos = shandler.PositionInBits();

			//+R2007 Only:
			if (R2007Plus)
			{
				//RL : Size in bits
				long sizeInBits = shandler.ReadRawLong();

				long lastPositionInBits = initialPos + sizeInBits - 1L;

				//Setup the text handler for versions 2007 and above
				IDwgStreamHandler textHandler = DwgStreamHanlder.GetStreamHandler(version,
					//Create a copy of the stream
					new StreamIO(shandler.StreamToRead, true).Stream);
				//Set the position and use the flag
				textHandler.SetPositionByFlag(lastPositionInBits);

				//Setup the handler for the references for versions 2007 and above
				IDwgStreamHandler referenceHandler = DwgStreamHanlder.GetStreamHandler(version,
					//Create a copy of the stream
					new StreamIO(shandler.StreamToRead, true).Stream);
				//Set the position and jump the flag
				referenceHandler.SetPositionInBits(lastPositionInBits + 0b1);

				shandler = new DwgMultipleHandlerAC21(shandler, textHandler, referenceHandler);
			}

			//R2013+:
			if (R2013Plus)
				//BLL : Variabele REQUIREDVERSIONS, default value 0, read only.
				header.RequiredVersions = shandler.ReadBitLongLong();

			//Common:
			//BD : Unknown, default value 412148564080.0
			double unknownbd = shandler.ReadBitDouble();
			//BD: Unknown, default value 1.0
			unknownbd = shandler.ReadBitDouble();
			//BD: Unknown, default value 1.0
			unknownbd = shandler.ReadBitDouble();
			//BD: Unknown, default value 1.0
			unknownbd = shandler.ReadBitDouble();
			//TV: Unknown text string, default "m"
			var unknowntv = shandler.ReadVariableText();
			//TV: Unknown text string, default ""
			unknowntv = shandler.ReadVariableText();
			//TV: Unknown text string, default ""
			unknowntv = shandler.ReadVariableText();
			//TV: Unknown text string, default ""
			unknowntv = shandler.ReadVariableText();
			//BL : Unknown long, default value 24L
			var unknownbl = shandler.ReadBitLong();
			//BL: Unknown long, default value 0L;
			unknownbl = shandler.ReadBitLong();

			//R13-R14 Only:
			if (R13_14Only)
			{
				//BS : Unknown short, default value 0
				short unknowns = shandler.ReadBitShort();
			}

			//Pre-2004 Only:
			if (R2004Pre)
			{
				//H : Handle of the current viewport entity header (hard pointer)
				ulong pointerViewPort = shandler.HandleReference();
			}

			//Common:
			//B: DIMASO
			header.AssociatedDimensions = shandler.ReadBit();
			//B: DIMSHO
			header.UpdateDimensionsWhileDragging = shandler.ReadBit();

			//R13-R14 Only:
			if (R13_14Only)
				//B : DIMSAV Undocumented.
				header.DIMSAV = shandler.ReadBit();

			//Common:
			//B: PLINEGEN
			header.PolylineLineTypeGeneration = shandler.ReadBit();
			//B : ORTHOMODE
			header.OrthoMode = shandler.ReadBit();
			//B: REGENMODE
			header.RegenerationMode = shandler.ReadBit();
			//B : FILLMODE
			header.FillMode = shandler.ReadBit();
			//B : QTEXTMODE
			header.QuickTextMode = shandler.ReadBit();
			//B : PSLTSCALE
			header.PaperSpaceLineTypeScaling = shandler.ReadBit();
			//B : LIMCHECK
			header.LimitCheckingOn = shandler.ReadBit();

			//R13-R14 Only (stored in registry from R15 onwards):
			if (R13_14Only)
				//B : BLIPMODE
				header.BlipMode = shandler.ReadBit();
			//R2004+:
			if (R2004Plus)
				//B : Undocumented
				shandler.ReadBit();

			//Common:
			//B: USRTIMER(User timer on / off).
			header.UserTimer = shandler.ReadBit();
			//B : SKPOLY
			header.SketchPolylines = shandler.ReadBit();
			//B : ANGDIR
			header.AngularDirection = shandler.ReadBitAsShort();
			//B : SPLFRAME
			header.ShowSplineControlPoints = shandler.ReadBit();

			//R13-R14 Only (stored in registry from R15 onwards):
			if (R13_14Only)
			{
				//B : ATTREQ
				shandler.ReadBit();
				//B : ATTDIA
				shandler.ReadBit();
			}

			//Common:
			//B: MIRRTEXT
			header.MirrorText = shandler.ReadBit();
			//B : WORLDVIEW
			header.WorldView = shandler.ReadBit();

			//R13 - R14 Only:
			if (R13_14Only)
				//B: WIREFRAME Undocumented.
				shandler.ReadBit();

			//Common:
			//B: TILEMODE
			header.ShowModelSpace = shandler.ReadBit();
			//B : PLIMCHECK
			header.PaperSpaceLimitsChecking = shandler.ReadBit();
			//B : VISRETAIN
			header.RetainXRefDependentVisibilitySettings = shandler.ReadBit();

			//R13 - R14 Only(stored in registry from R15 onwards):
			if (R13_14Only)
				//B : DELOBJ
				shandler.ReadBit();

			//Common:
			//B: DISPSILH
			header.DisplaySilhouetteCurves = shandler.ReadBit();
			//B : PELLIPSE(not present in DXF)
			header.CreateEllipseAsPolyline = shandler.ReadBit();
			//BS: PROXYGRAPHICS
			header.ProxyGraphics = shandler.ReadBitShortAsBool();

			//R13-R14 Only (stored in registry from R15 onwards):
			if (R13_14Only)
			{
				//BS : DRAGMODE
				shandler.ReadBitShort();
			}

			//Common:
			//BS: TREEDEPTH
			header.SpatialIndexMaxTreeDepth = shandler.ReadBitShort();
			//BS : LUNITS
			header.LinearUnitFormat = shandler.ReadBitShort();
			//BS : LUPREC
			header.LinearUnitPrecision = shandler.ReadBitShort();
			//BS : AUNITS
			header.AngularUnit = shandler.ReadBitShort();
			//BS : AUPREC
			header.AngularUnitPrecision = shandler.ReadBitShort();

			//R13 - R14 Only Only(stored in registry from R15 onwards):
			if (R13_14Only)
				//BS: OSMODE
				header.ObjectSnapMode = shandler.ReadBitShort();

			//Common:
			//BS: ATTMODE
			header.AttributeVisibility = shandler.ReadBitShort();

			//R13 - R14 Only Only(stored in registry from R15 onwards):
			if (R13_14Only)
				//BS: COORDS
				shandler.ReadBitShort();

			//Common:
			//BS: PDMODE
			header.PointDisplayMode = shandler.ReadBitShort();

			//R13 - R14 Only Only(stored in registry from R15 onwards):
			if (R13_14Only)
				//BS: PICKSTYLE
				shandler.ReadBitShort();

			//R2004 +:
			if (R2004Plus)
			{
				//BL: Unknown
				shandler.ReadBitLong();
				//BL: Unknown
				shandler.ReadBitLong();
				//BL: Unknown
				shandler.ReadBitLong();
			}

			//Common:
			//BS : USERI1
			header.UserShort1 = shandler.ReadBitShort();
			//BS : USERI2
			header.UserShort2 = shandler.ReadBitShort();
			//BS : USERI3
			header.UserShort3 = shandler.ReadBitShort();
			//BS : USERI4
			header.UserShort4 = shandler.ReadBitShort();
			//BS : USERI5
			header.UserShort5 = shandler.ReadBitShort();

			//BS: SPLINESEGS
			header.NumberOfSplineSegments = shandler.ReadBitShort();
			//BS : SURFU
			header.SurfaceDensityU = shandler.ReadBitShort();
			//BS : SURFV
			header.SurfaceDensityV = shandler.ReadBitShort();
			//BS : SURFTYPE
			header.SurfaceType = shandler.ReadBitShort();
			//BS : SURFTAB1
			header.SurfaceMeshTabulationCount1 = shandler.ReadBitShort();
			//BS : SURFTAB2
			header.SurfaceMeshTabulationCount2 = shandler.ReadBitShort();
			//BS : SPLINETYPE
			header.SplineType = shandler.ReadBitShort();
			//BS : SHADEDGE
			header.ShadeEdge = shandler.ReadBitShort();
			//BS : SHADEDIF
			header.ShadeDiffuseToAmbientPercentage = shandler.ReadBitShort();
			//BS: UNITMODE
			header.UnitMode = shandler.ReadBitShort();
			//BS : MAXACTVP
			header.MaxViewportCount = shandler.ReadBitShort();
			//BS : ISOLINES
			header.SurfaceIsolineCount = shandler.ReadBitShort();
			//BS : CMLJUST
			header.CurrentMultilineJustification = shandler.ReadBitShort();
			//BS : TEXTQLTY
			header.TextQuality = shandler.ReadBitShort();
			//BD : LTSCALE
			header.LineTypeScale = shandler.ReadBitDouble();
			//BD : TEXTSIZE
			header.TextHeightDefault = shandler.ReadBitDouble();
			//BD : TRACEWID
			header.TraceWidthDefault = shandler.ReadBitDouble();
			//BD : SKETCHINC
			header.SketchIncrement = shandler.ReadBitDouble();
			//BD : FILLETRAD
			header.FilletRadius = shandler.ReadBitDouble();
			//BD : THICKNESS
			header.ThicknessDefault = shandler.ReadBitDouble();
			//BD : ANGBASE
			header.AngleBase = shandler.ReadBitDouble();
			//BD : PDSIZE
			header.PointDisplaySize = shandler.ReadBitDouble();
			//BD : PLINEWID
			header.PolylineWidthDefault = shandler.ReadBitDouble();
			//BD : USERR1
			header.UserDouble1 = shandler.ReadBitDouble();
			//BD : USERR2
			header.UserDouble2 = shandler.ReadBitDouble();
			//BD : USERR3
			header.UserDouble3 = shandler.ReadBitDouble();
			//BD : USERR4
			header.UserDouble4 = shandler.ReadBitDouble();
			//BD : USERR5
			header.UserDouble5 = shandler.ReadBitDouble();
			//BD : CHAMFERA
			header.ChamferDistance1 = shandler.ReadBitDouble();
			//BD : CHAMFERB
			header.ChamferDistance2 = shandler.ReadBitDouble();
			//BD : CHAMFERC
			header.ChamferLength = shandler.ReadBitDouble();
			//BD : CHAMFERD
			header.ChamferAngle = shandler.ReadBitDouble();
			//BD : FACETRES
			header.FacetResolution = shandler.ReadBitDouble();
			//BD : CMLSCALE
			header.CurrentMultilineScale = shandler.ReadBitDouble();
			//BD : CELTSCALE
			header.CurrentEntityLinetypeScale = shandler.ReadBitDouble();

			//TV: MENUNAME
			header.MenuFileName = shandler.ReadVariableText();

			//Common:
			//BL: TDCREATE(Julian day)
			//BL: TDCREATE(Milliseconds into the day)
			header.CreateDateTime = shandler.ReadDateTime();
			//BL: TDUPDATE(Julian day)
			//BL: TDUPDATE(Milliseconds into the day)
			header.UpdateDateTime = shandler.ReadDateTime();

			//R2004 +:
			if (R2004Plus)
			{
				//BL : Unknown
				shandler.ReadBitLong();
				//BL : Unknown
				shandler.ReadBitLong();
				//BL : Unknown
				shandler.ReadBitLong();
			}

			//Common:
			//BL: TDINDWG(Days)
			//BL: TDINDWG(Milliseconds into the day)
			header.TotalEditingTime = shandler.ReadTimeSpan();
			//BL: TDUSRTIMER(Days)
			//BL: TDUSRTIMER(Milliseconds into the day)
			header.UserElapsedTimeSpan = shandler.ReadTimeSpan();

			//CMC : CECOLOR
			header.CurrentEntityColor = shandler.ReadCmColor();

			//H : HANDSEED The next handle, with an 8-bit length specifier preceding the handle
			//bytes (standard hex handle form) (code 0). The HANDSEED is not part of the handle 
			//stream, but of the normal data stream (relevant for R21 and later).
			header.HandleSeed = mianHandler.HandleReference();

			//H : CLAYER (hard pointer)
			objectPointers.CLAYER = shandler.HandleReference();
			//H: TEXTSTYLE(hard pointer)
			objectPointers.TEXTSTYLE = shandler.HandleReference();
			//H: CELTYPE(hard pointer)
			objectPointers.CELTYPE = shandler.HandleReference();

			//R2007 + Only:
			if (R2007Plus)
			{
				//H: CMATERIAL(hard pointer)
				objectPointers.CMATERIAL = shandler.HandleReference();
			}

			//Common:
			//H: DIMSTYLE (hard pointer)
			objectPointers.DIMSTYLE = shandler.HandleReference();
			//H: CMLSTYLE (hard pointer)
			objectPointers.CMLSTYLE = shandler.HandleReference();

			//R2000+ Only:
			if (R2000Plus)
			{
				//BD: PSVPSCALE
				header.ViewportDefaultViewScaleFactor = shandler.ReadBitDouble();
			}

			//Common:
			//3BD: INSBASE(PSPACE)
			header.PaperSpaceUcs.PaperSpaceInsertionBase = shandler.Read3BitDouble();
			//3BD: EXTMIN(PSPACE)
			header.PaperSpaceUcs.PaperSpaceExtMin = shandler.Read3BitDouble();
			//3BD: EXTMAX(PSPACE)
			header.PaperSpaceUcs.PaperSpaceExtMax = shandler.Read3BitDouble();
			//2RD: LIMMIN(PSPACE)
			header.PaperSpaceUcs.PaperSpaceLimitsMin = shandler.Read2RawDouble();
			//2RD: LIMMAX(PSPACE)
			header.PaperSpaceUcs.PaperSpaceLimitsMax = shandler.Read2RawDouble();
			//BD: ELEVATION(PSPACE)
			header.PaperSpaceUcs.PaperSpaceElevation = shandler.ReadBitDouble();
			//3BD: UCSORG(PSPACE)
			header.PaperSpaceUcs.Origin = shandler.Read3BitDouble();
			//3BD: UCSXDIR(PSPACE)
			header.PaperSpaceUcs.XAxis = shandler.Read3BitDouble();
			//3BD: UCSYDIR(PSPACE)
			header.PaperSpaceUcs.YAxis = shandler.Read3BitDouble();

			//H: UCSNAME (PSPACE) (hard pointer)
			objectPointers.UCSNAME_PSPACE = shandler.HandleReference();

			//R2000+ Only:
			if (R2000Plus)
			{
				//H : PUCSORTHOREF (hard pointer)
				objectPointers.PUCSORTHOREF = shandler.HandleReference();

				//BS : PUCSORTHOVIEW	??
				int PUCSORTHOVIEW = shandler.ReadBitShort();

				//H: PUCSBASE(hard pointer)
				objectPointers.PUCSBASE = shandler.HandleReference();

				//3BD: PUCSORGTOP
				header.PaperSpaceUcs.OrthographicTopDOrigin = shandler.Read3BitDouble();
				//3BD: PUCSORGBOTTOM
				header.PaperSpaceUcs.OrthographicBottomDOrigin = shandler.Read3BitDouble();
				//3BD: PUCSORGLEFT
				header.PaperSpaceUcs.OrthographicLeftDOrigin = shandler.Read3BitDouble();
				//3BD: PUCSORGRIGHT
				header.PaperSpaceUcs.OrthographicRightDOrigin = shandler.Read3BitDouble();
				//3BD: PUCSORGFRONT
				header.PaperSpaceUcs.OrthographicFrontDOrigin = shandler.Read3BitDouble();
				//3BD: PUCSORGBACK
				header.PaperSpaceUcs.OrthographicBackDOrigin = shandler.Read3BitDouble();
			}

			//Common:
			//3BD: INSBASE(MSPACE)
			header.InsertionBase = shandler.Read3BitDouble();
			//3BD: EXTMIN(MSPACE)
			header.ExtMin = shandler.Read3BitDouble();
			//3BD: EXTMAX(MSPACE)
			header.ExtMax = shandler.Read3BitDouble();
			//2RD: LIMMIN(MSPACE)
			header.LimitsMin = shandler.Read2RawDouble();
			//2RD: LIMMAX(MSPACE)
			header.LimitsMax = shandler.Read2RawDouble();
			//BD: ELEVATION(MSPACE)
			header.Elevation = shandler.ReadBitDouble();
			//3BD: UCSORG(MSPACE)
			header.Ucs.Origin = shandler.Read3BitDouble();
			//3BD: UCSXDIR(MSPACE)
			header.Ucs.XAxis = shandler.Read3BitDouble();
			//3BD: UCSYDIR(MSPACE)
			header.Ucs.YAxis = shandler.Read3BitDouble();

			//H: UCSNAME(MSPACE)(hard pointer)
			objectPointers.UCSNAME_MSPACE = shandler.HandleReference();

			//R2000 + Only:
			if (R2000Plus)
			{
				//H: UCSORTHOREF(hard pointer)
				objectPointers.UCSORTHOREF = shandler.HandleReference();

				//BS: UCSORTHOVIEW	??
				short UCSORTHOVIEW = shandler.ReadBitShort();

				//H : UCSBASE(hard pointer)
				objectPointers.UCSBASE = shandler.HandleReference();

				//3BD: UCSORGTOP
				header.Ucs.OrthographicTopDOrigin = shandler.Read3BitDouble();
				//3BD: UCSORGBOTTOM
				header.Ucs.OrthographicBottomDOrigin = shandler.Read3BitDouble();
				//3BD: UCSORGLEFT
				header.Ucs.OrthographicLeftDOrigin = shandler.Read3BitDouble();
				//3BD: UCSORGRIGHT
				header.Ucs.OrthographicRightDOrigin = shandler.Read3BitDouble();
				//3BD: UCSORGFRONT
				header.Ucs.OrthographicFrontDOrigin = shandler.Read3BitDouble();
				//3BD: UCSORGBACK
				header.Ucs.OrthographicBackDOrigin = shandler.Read3BitDouble();

				//TV : DIMPOST
				header.DimensionStyleOverrides.PostFix = shandler.ReadVariableText();
				//TV : DIMAPOST
				header.DimensionStyleOverrides.AlternateDimensioningSuffix = shandler.ReadVariableText();
			}

			//R13-R14 Only:
			if (R13_14Only)
			{
				//B: DIMTOL
				header.DimensionStyleOverrides.GenerateTolerances = shandler.ReadBit();
				//B : DIMLIM
				header.DimensionStyleOverrides.LimitsGeneration = shandler.ReadBit();
				//B : DIMTIH
				header.DimensionStyleOverrides.TextInsideHorizontal = shandler.ReadBit();
				//B : DIMTOH
				header.DimensionStyleOverrides.TextOutsideHorizontal = shandler.ReadBit();
				//B : DIMSE1
				header.DimensionStyleOverrides.SuppressFirstExtensionLine = shandler.ReadBit();
				//B : DIMSE2
				header.DimensionStyleOverrides.SuppressSecondExtensionnLine = shandler.ReadBit();
				//B : DIMALT
				header.DimensionStyleOverrides.AlternateUnitDimensioning = shandler.ReadBit();
				//B : DIMTOFL
				header.DimensionStyleOverrides.TextOutsideExtensions = shandler.ReadBit();
				//B : DIMSAH
				header.DimensionStyleOverrides.SeparateArrowBlocks = shandler.ReadBit();
				//B : DIMTIX
				header.DimensionStyleOverrides.TextInsideExtensions = shandler.ReadBit();
				//B : DIMSOXD
				header.DimensionStyleOverrides.SuppressOutsideExtensions = shandler.ReadBit();
				//RC : DIMALTD
				header.DimensionStyleOverrides.AlternateUnitDecimalPlaces = shandler.ReadRawChar();
				//RC : DIMZIN
				header.DimensionStyleOverrides.ZeroHandling = shandler.ReadRawChar();
				//B : DIMSD1
				header.DimensionStyleOverrides.SuppressFirstDimensionLine = shandler.ReadBit();
				//B : DIMSD2
				header.DimensionStyleOverrides.SuppressSecondDimensionLine = shandler.ReadBit();
				//RC : DIMTOLJ
				header.DimensionStyleOverrides.ToleranceAlignment = shandler.ReadRawChar();
				//RC : DIMJUST
				header.DimensionStyleOverrides.TextHorizontalAlignment = shandler.ReadRawChar();
				//RC : DIMFIT
				header.DimensionStyleOverrides.DimensionFit = shandler.ReadRawChar();
				//B : DIMUPT
				header.DimensionStyleOverrides.CursorUpdate = shandler.ReadBit();
				//RC : DIMTZIN
				header.DimensionStyleOverrides.ToleranceZeroHandling = shandler.ReadRawChar();
				//RC: DIMALTZ
				header.DimensionStyleOverrides.AlternateUnitZeroHandling = shandler.ReadRawChar();
				//RC : DIMALTTZ
				header.DimensionStyleOverrides.AlternateUnitToleranceZeroHandling = shandler.ReadRawChar();
				//RC : DIMTAD
				header.DimensionStyleOverrides.TextVerticalAlignment = shandler.ReadRawChar();
				//BS : DIMUNIT
				header.DimensionStyleOverrides.DimensionUnit = shandler.ReadBitShort();
				//BS : DIMAUNIT
				header.DimensionStyleOverrides.AngularDimensionDecimalPlaces = shandler.ReadBitShort();
				//BS : DIMDEC
				header.DimensionStyleOverrides.DecimalPlaces = shandler.ReadBitShort();
				//BS : DIMTDEC
				header.DimensionStyleOverrides.ToleranceDecimalPlaces = shandler.ReadBitShort();
				//BS : DIMALTU
				header.DimensionStyleOverrides.AlternateUnitFormat = shandler.ReadBitShort();
				//BS : DIMALTTD
				header.DimensionStyleOverrides.AlternateUnitToleranceDecimalPlaces = shandler.ReadBitShort();
				//H : DIMTXSTY(hard pointer)
				objectPointers.DIMTXSTY = shandler.HandleReference();
			}

			//Common:
			//BD: DIMSCALE
			header.DimensionStyleOverrides.ScaleFactor = shandler.ReadBitDouble();
			//BD : DIMASZ
			header.DimensionStyleOverrides.ArrowSize = shandler.ReadBitDouble();
			//BD : DIMEXO
			header.DimensionStyleOverrides.ExtensionLineOffset = shandler.ReadBitDouble();
			//BD : DIMDLI
			header.DimensionStyleOverrides.DimensionLineIncrement = shandler.ReadBitDouble();
			//BD : DIMEXE
			header.DimensionStyleOverrides.ExtensionLineExtension = shandler.ReadBitDouble();
			//BD : DIMRND
			header.DimensionStyleOverrides.Rounding = shandler.ReadBitDouble();
			//BD : DIMDLE
			header.DimensionStyleOverrides.DimensionLineExtension = shandler.ReadBitDouble();
			//BD : DIMTP
			header.DimensionStyleOverrides.PlusTolerance = shandler.ReadBitDouble();
			//BD : DIMTM
			header.DimensionStyleOverrides.MinusTolerance = shandler.ReadBitDouble();

			//R2007 + Only:
			if (R2007Plus)
			{
				//BD: DIMFXL
				header.DimensionStyleOverrides.FixedExtensionLineLength = shandler.ReadBitDouble();
				//BD : DIMJOGANG
				header.DimensionStyleOverrides.JoggedRadiusDimensionTransverseSegmentAngle = shandler.ReadBitDouble();
				//BS : DIMTFILL
				header.DimensionStyleOverrides.TextBackgroundFillMode = shandler.ReadBitShort();
				//CMC : DIMTFILLCLR
				header.DimensionStyleOverrides.TextBackgroundColor = shandler.ReadCmColor();
			}

			//R2000 + Only:
			if (R2000Plus)
			{
				//B: DIMTOL
				header.DimensionStyleOverrides.GenerateTolerances = shandler.ReadBit();
				//B : DIMLIM
				header.DimensionStyleOverrides.LimitsGeneration = shandler.ReadBit();
				//B : DIMTIH
				header.DimensionStyleOverrides.TextInsideHorizontal = shandler.ReadBit();
				//B : DIMTOH
				header.DimensionStyleOverrides.TextOutsideHorizontal = shandler.ReadBit();
				//B : DIMSE1
				header.DimensionStyleOverrides.SuppressFirstExtensionLine = shandler.ReadBit();
				//B : DIMSE2
				header.DimensionStyleOverrides.SuppressSecondExtensionLine = shandler.ReadBit();
				//BS : DIMTAD
				header.DimensionStyleOverrides.TextVerticalAlignment = (char)shandler.ReadBitShort();
				//BS : DIMZIN
				header.DimensionStyleOverrides.ZeroHandling = (char)shandler.ReadBitShort();
				//BS : DIMAZIN
				header.DimensionStyleOverrides.AngularZeroHandling = shandler.ReadBitShort();
			}

			//R2007 + Only:
			if (R2007Plus)
			{
				//BS: DIMARCSYM
				header.DimensionStyleOverrides.ArcLengthSymbolPosition = shandler.ReadBitShort();
			}

			//Common:
			//BD: DIMTXT
			header.DimensionStyleOverrides.TextHeight = shandler.ReadBitDouble();
			//BD : DIMCEN
			header.DimensionStyleOverrides.CenterMarkSize = shandler.ReadBitDouble();
			//BD: DIMTSZ
			header.DimensionStyleOverrides.TickSize = shandler.ReadBitDouble();
			//BD : DIMALTF
			header.DimensionStyleOverrides.AlternateUnitScaleFactor = shandler.ReadBitDouble();
			//BD : DIMLFAC
			header.DimensionStyleOverrides.LinearScaleFactor = shandler.ReadBitDouble();
			//BD : DIMTVP
			header.DimensionStyleOverrides.TextVerticalPosition = shandler.ReadBitDouble();
			//BD : DIMTFAC
			header.DimensionStyleOverrides.ToleranceScaleFactor = shandler.ReadBitDouble();
			//BD : DIMGAP
			header.DimensionStyleOverrides.DimensionLineGap = shandler.ReadBitDouble();

			//R13 - R14 Only:
			if (R13_14Only)
			{
				//T: DIMPOST
				header.DimensionStyleOverrides.PostFix = shandler.ReadVariableText();
				//T : DIMAPOST
				header.DimensionStyleOverrides.AlternateDimensioningSuffix = shandler.ReadVariableText();
				//T : DIMBLK
				header.DimensionBlockName = shandler.ReadVariableText();
				//T : DIMBLK1
				header.DimensionBlockNameFirst = shandler.ReadVariableText();
				//T : DIMBLK2
				header.DimensionBlockNameSecond = shandler.ReadVariableText();
			}

			//R2000 + Only:
			if (R2000Plus)
			{
				//BD: DIMALTRND
				header.DimensionStyleOverrides.AlternateUnitRounding = shandler.ReadBitDouble();
				//B : DIMALT
				header.DimensionStyleOverrides.AlternateUnitDimensioning = shandler.ReadBit();
				//BS : DIMALTD
				header.DimensionStyleOverrides.AlternateUnitDecimalPlaces = (char)shandler.ReadBitShort();
				//B : DIMTOFL
				header.DimensionStyleOverrides.TextOutsideExtensions = shandler.ReadBit();
				//B : DIMSAH
				header.DimensionStyleOverrides.SeparateArrowBlocks = shandler.ReadBit();
				//B : DIMTIX
				header.DimensionStyleOverrides.TextInsideExtensions = shandler.ReadBit();
				//B : DIMSOXD
				header.DimensionStyleOverrides.SuppressOutsideExtensions = shandler.ReadBit();
			}

			//Common:
			//CMC: DIMCLRD
			header.DimensionStyleOverrides.DimensionLineColor = shandler.ReadCmColor();
			//CMC : DIMCLRE
			header.DimensionStyleOverrides.ExtensionLineColor = shandler.ReadCmColor();
			//CMC : DIMCLRT
			header.DimensionStyleOverrides.TextColor = shandler.ReadCmColor();

			//R2000 + Only:
			if (R2000Plus)
			{
				//BS: DIMADEC
				header.DimensionStyleOverrides.AngularDimensionDecimalPlaces = shandler.ReadBitShort();
				//BS : DIMDEC
				header.DimensionStyleOverrides.DecimalPlaces = shandler.ReadBitShort();
				//BS : DIMTDEC
				header.DimensionStyleOverrides.ToleranceDecimalPlaces = shandler.ReadBitShort();
				//BS : DIMALTU
				header.DimensionStyleOverrides.AlternateUnitFormat = shandler.ReadBitShort();
				//BS : DIMALTTD
				header.DimensionStyleOverrides.AlternateUnitToleranceDecimalPlaces = shandler.ReadBitShort();
				//BS : DIMAUNIT
				header.DimensionStyleOverrides.AngularDimensionDecimalPlaces = shandler.ReadBitShort();
				//BS : DIMFRAC
				header.DimensionStyleOverrides.FractionFormat = shandler.ReadBitShort();
				//BS : DIMLUNIT
				header.DimensionStyleOverrides.LinearUnitFormat = shandler.ReadBitShort();
				//BS : DIMDSEP
				header.DimensionStyleOverrides.DecimalSeparator = (char)shandler.ReadBitShort();
				//BS : DIMTMOVE
				header.DimensionStyleOverrides.TextMovement = shandler.ReadBitShort();
				//BS : DIMJUST
				header.DimensionStyleOverrides.TextHorizontalAlignment = (char)shandler.ReadBitShort();
				//B : DIMSD1
				header.DimensionStyleOverrides.SuppressFirstExtensionLine = shandler.ReadBit();
				//B : DIMSD2
				header.DimensionStyleOverrides.SuppressSecondExtensionnLine = shandler.ReadBit();
				//BS : DIMTOLJ
				header.DimensionStyleOverrides.ToleranceAlignment = (char)shandler.ReadBitShort();
				//BS : DIMTZIN
				header.DimensionStyleOverrides.ToleranceZeroHandling = (char)shandler.ReadBitShort();
				//BS: DIMALTZ
				header.DimensionStyleOverrides.AlternateUnitZeroHandling = (char)shandler.ReadBitShort();
				//BS : DIMALTTZ
				header.DimensionStyleOverrides.AlternateUnitToleranceZeroHandling = (char)shandler.ReadBitShort();
				//B : DIMUPT
				header.DimensionStyleOverrides.CursorUpdate = shandler.ReadBit();
				//BS : DIMATFIT
				header.DimensionStyleOverrides.DimensionTextArrowFit = shandler.ReadBitShort();
			}

			//R2007 + Only:
			if (R2007Plus)
			{
				//B: DIMFXLON
				header.DimensionStyleOverrides.IsExtensionLineLengthFixed = shandler.ReadBit();
			}

			//R2010 + Only:
			if (R2010Plus)
			{
				//B: DIMTXTDIRECTION
				header.DimensionStyleOverrides.TextDirection = shandler.ReadBit();
				//BD : DIMALTMZF
				header.DimensionStyleOverrides.AltMzf = shandler.ReadBitDouble();
				//T : DIMALTMZS
				header.DimensionStyleOverrides.AltMzs = shandler.ReadVariableText();
				//BD : DIMMZF
				header.DimensionStyleOverrides.Mzf = shandler.ReadBitDouble();
				//T : DIMMZS
				header.DimensionStyleOverrides.Mzs = shandler.ReadVariableText();
			}

			//R2000 + Only:
			if (R2000Plus)
			{
				//H: DIMTXSTY(hard pointer)
				objectPointers.DIMTXSTY = shandler.HandleReference();
				//H: DIMLDRBLK(hard pointer)
				objectPointers.DIMLDRBLK = shandler.HandleReference();
				//H: DIMBLK(hard pointer)
				objectPointers.DIMBLK = shandler.HandleReference();
				//H: DIMBLK1(hard pointer)
				objectPointers.DIMBLK1 = shandler.HandleReference();
				//H: DIMBLK2(hard pointer)
				objectPointers.DIMBLK2 = shandler.HandleReference();
			}

			//R2007+ Only:
			if (R2007Plus)
			{
				//H : DIMLTYPE (hard pointer)
				objectPointers.DIMLTYPE = shandler.HandleReference();
				//H: DIMLTEX1(hard pointer)
				objectPointers.DIMLTEX1 = shandler.HandleReference();
				//H: DIMLTEX2(hard pointer)
				objectPointers.DIMLTEX2 = shandler.HandleReference();
			}

			//R2000+ Only:
			if (R2000Plus)
			{
				//BS: DIMLWD
				header.DimensionStyleOverrides.DimensionLineWeight = shandler.ReadBitShort();
				//BS : DIMLWE
				header.DimensionStyleOverrides.ExtensionLineWeight = shandler.ReadBitShort();
			}

			//H: BLOCK CONTROL OBJECT(hard owner)
			objectPointers.BLOCK_CONTROL_OBJECT = shandler.HandleReference();
			//H: LAYER CONTROL OBJECT(hard owner)
			objectPointers.LAYER_CONTROL_OBJECT = shandler.HandleReference();
			//H: STYLE CONTROL OBJECT(hard owner)
			objectPointers.STYLE_CONTROL_OBJECT = shandler.HandleReference();
			//H: LINETYPE CONTROL OBJECT(hard owner)
			objectPointers.LINETYPE_CONTROL_OBJECT = shandler.HandleReference();
			//H: VIEW CONTROL OBJECT(hard owner)
			objectPointers.VIEW_CONTROL_OBJECT = shandler.HandleReference();
			//H: UCS CONTROL OBJECT(hard owner)
			objectPointers.UCS_CONTROL_OBJECT = shandler.HandleReference();
			//H: VPORT CONTROL OBJECT(hard owner)
			objectPointers.VPORT_CONTROL_OBJECT = shandler.HandleReference();
			//H: APPID CONTROL OBJECT(hard owner)
			objectPointers.APPID_CONTROL_OBJECT = shandler.HandleReference();
			//H: DIMSTYLE CONTROL OBJECT(hard owner)
			objectPointers.DIMSTYLE_CONTROL_OBJECT = shandler.HandleReference();

			//R13 - R15 Only:
			if (R13_15Only)
			{
				//H: VIEWPORT ENTITY HEADER CONTROL OBJECT(hard owner)
				objectPointers.VIEWPORT_ENTITY_HEADER_CONTROL_OBJECT = shandler.HandleReference();
			}

			//Common:
			//H: DICTIONARY(ACAD_GROUP)(hard pointer)
			objectPointers.DICTIONARY_ACAD_GROUP = shandler.HandleReference();
			//H: DICTIONARY(ACAD_MLINESTYLE)(hard pointer)
			objectPointers.DICTIONARY_ACAD_MLINESTYLE = shandler.HandleReference();

			//H : DICTIONARY (NAMED OBJECTS) (hard owner)
			objectPointers.DICTIONARY_NAMED_OBJECTS = shandler.HandleReference();

			//R2000+ Only:
			if (R2000Plus)
			{
				//BS: TSTACKALIGN, default = 1(not present in DXF)
				header.StackedTextAlignment = shandler.ReadBitShort();
				//BS: TSTACKSIZE, default = 70(not present in DXF)
				header.StackedTextSizePercentage = shandler.ReadBitShort();

				//TV: HYPERLINKBASE
				header.HyperLinkBase = shandler.ReadVariableText();
				//TV : STYLESHEET
				header.StyleSheetName = shandler.ReadVariableText();

				//H : DICTIONARY(LAYOUTS)(hard pointer)
				objectPointers.DICTIONARY_LAYOUTS = shandler.HandleReference();
				//H: DICTIONARY(PLOTSETTINGS)(hard pointer)
				objectPointers.DICTIONARY_PLOTSETTINGS = shandler.HandleReference();
				//H: DICTIONARY(PLOTSTYLES)(hard pointer)
				objectPointers.DICTIONARY_PLOTSTYLES = shandler.HandleReference();
			}

			//R2004 +:
			if (R2004Plus)
			{
				//H: DICTIONARY (MATERIALS) (hard pointer)
				objectPointers.DICTIONARY_MATERIALS = shandler.HandleReference();
				//H: DICTIONARY (COLORS) (hard pointer)
				objectPointers.DICTIONARY_COLORS = shandler.HandleReference();
			}

			//R2007 +:
			if (R2007Plus)
			{
				//H: DICTIONARY(VISUALSTYLE)(hard pointer)
				objectPointers.DICTIONARY_VISUALSTYLE = shandler.HandleReference();
			}

			//R2000 +:
			if (R2000Plus)
			{
				//BL: Flags:
				int flags = shandler.ReadBitLong();
				//CELWEIGHT Flags & 0x001F
				header.CurrentEntityLineWeight = (short)(flags & 0x1F);
				//ENDCAPS Flags & 0x0060
				header.EndCaps = (short)(flags & 0x60);
				//JOINSTYLE Flags & 0x0180
				header.JoinStyle = (short)(flags & 0x180);
				//LWDISPLAY!(Flags & 0x0200)
				header.DisplayLineWeight = (short)(flags & 0x200);
				//XEDIT!(Flags & 0x0400)
				header.XEdit = (short)(flags & 0x400);
				//EXTNAMES Flags & 0x0800
				header.ExtendedNames = (short)(flags & 0x800);
				//PSTYLEMODE Flags & 0x2000
				header.PlotStyleMode = (short)(flags & 0x2000);
				//OLESTARTUP Flags & 0x4000
				header.LoadOLEObject = (short)(flags & 0x4000);

				//BS: INSUNITS
				header.InsUnits = shandler.ReadBitShort();
				//BS : CEPSNTYPE
				header.CurrentEntityPlotStyleType = shandler.ReadBitShort();

				if (header.CurrentEntityPlotStyleType == 3)
				{
					//H: CPSNID(present only if CEPSNTYPE == 3) (hard pointer)
					objectPointers.CPSNID = shandler.HandleReference();
				}

				//TV: FINGERPRINTGUID
				header.FingerPrintGuid = shandler.ReadVariableText();
				//TV : VERSIONGUID
				header.VersionGuid = shandler.ReadVariableText();
			}

			//R2004 +:
			if (R2004Plus)
			{
				//RC: SORTENTS
				header.EntitySortingFlags = shandler.ReadByte();
				//RC : INDEXCTL
				header.IndexCreationFlags = shandler.ReadByte();
				//RC : HIDETEXT
				header.HideText = shandler.ReadByte();
				//RC : XCLIPFRAME, before R2010 the value can be 0 or 1 only.
				header.ExternalReferenceClippingBoundaryType = shandler.ReadByte();
				//RC : DIMASSOC
				header.DimensionAssociativity = shandler.ReadByte();
				//RC : HALOGAP
				header.HaloGapPercentage = shandler.ReadByte();
				//BS : OBSCUREDCOLOR
				header.ObscuredColor = CadUtils.CreateColorFromIndex(shandler.ReadBitShort());
				//BS : INTERSECTIONCOLOR
				header.InterfereColor = CadUtils.CreateColorFromIndex(shandler.ReadBitShort());
				//RC : OBSCUREDLTYPE
				header.ObscuredType = shandler.ReadByte();
				//RC: INTERSECTIONDISPLAY
				header.IntersectionDisplay = shandler.ReadByte();

				//TV : PROJECTNAME
				header.ProjectName = shandler.ReadVariableText();
			}

			//Common:
			//H: BLOCK_RECORD(*PAPER_SPACE)(hard pointer)
			objectPointers.PAPER_SPACE = shandler.HandleReference();
			//H: BLOCK_RECORD(*MODEL_SPACE)(hard pointer)
			objectPointers.MODEL_SPACE = shandler.HandleReference();
			//H: LTYPE(BYLAYER)(hard pointer)
			objectPointers.BYLAYER = shandler.HandleReference();
			//H: LTYPE(BYBLOCK)(hard pointer)
			objectPointers.BYBLOCK = shandler.HandleReference();
			//H: LTYPE(CONTINUOUS)(hard pointer)
			objectPointers.CONTINUOUS = shandler.HandleReference();

			//R2007 +:
			if (R2007Plus)
			{
				//B: CAMERADISPLAY
				header.CameraDisplayObjects = shandler.ReadBit();

				//BL : unknown
				shandler.ReadBitLong();
				//BL : unknown
				shandler.ReadBitLong();
				//BD : unknown
				shandler.ReadBitDouble();

				//BD : STEPSPERSEC
				header.StepsPerSecond = shandler.ReadBitDouble();
				//BD : STEPSIZE
				header.StepSize = shandler.ReadBitDouble();
				//BD : 3DDWFPREC
				header.Dw3DPrecision = shandler.ReadBitDouble();
				//BD : LENSLENGTH
				header.LensLength = shandler.ReadBitDouble();
				//BD : CAMERAHEIGHT
				header.CameraHeight = shandler.ReadBitDouble();
				//RC : SOLIDHIST
				header.SolidsRetainHistory = shandler.ReadRawChar();
				//RC : SHOWHIST
				header.ShowSolidsHistory = shandler.ReadRawChar();
				//BD : PSOLWIDTH
				header.SweptSolidWidth = shandler.ReadBitDouble();
				//BD : PSOLHEIGHT
				header.SweptSolidHeight = shandler.ReadBitDouble();
				//BD : LOFTANG1
				header.DraftAngleFirstCrossSection = shandler.ReadBitDouble();
				//BD : LOFTANG2
				header.DraftAngleSecondCrossSection = shandler.ReadBitDouble();
				//BD : LOFTMAG1
				header.DraftMagnitudeFirstCrossSection = shandler.ReadBitDouble();
				//BD : LOFTMAG2
				header.DraftMagnitudeSecondCrossSection = shandler.ReadBitDouble();
				//BS : LOFTPARAM
				header.SolidLoftedShape = shandler.ReadBitShort();
				//RC : LOFTNORMALS
				header.LoftedObjectNormals = shandler.ReadRawChar();
				//BD : LATITUDE
				header.Latitude = shandler.ReadBitDouble();
				//BD : LONGITUDE
				header.Longitude = shandler.ReadBitDouble();
				//BD : NORTHDIRECTION
				header.NorthDirection = shandler.ReadBitDouble();
				//BL : TIMEZONE
				header.TimeZone = shandler.ReadBitLong();
				//RC : LIGHTGLYPHDISPLAY
				header.DisplayLightGlyphs = shandler.ReadRawChar();
				//RC : TILEMODELIGHTSYNCH	??
				shandler.ReadRawChar();
				//RC : DWFFRAME
				header.DwgUnderlayFramesVisibility = shandler.ReadRawChar();
				//RC : DGNFRAME
				header.DgnUnderlayFramesVisibility = shandler.ReadRawChar();

				//B : unknown
				shandler.ReadBit();

				//CMC : INTERFERECOLOR
				header.InterfereColor = shandler.ReadCmColor();

				//H : INTERFEREOBJVS(hard pointer)
				objectPointers.INTERFEREOBJVS = shandler.HandleReference();
				//H: INTERFEREVPVS(hard pointer)
				objectPointers.INTERFEREVPVS = shandler.HandleReference();
				//H: DRAGVS(hard pointer)
				objectPointers.DRAGVS = shandler.HandleReference();

				//RC: CSHADOW
				header.ShadowMode = shandler.ReadByte();
				//BD : unknown
				header.ShadowPlaneLocation = shandler.ReadBitDouble();
			}

			//Set the position at the end of the section
			mianHandler.SetPositionInBits(initialPos + size * 8);
			mianHandler.ResetShift();

			//Ending sentinel: 0x30,0x84,0xE0,0xDC,0x02,0x21,0xC7,0x56,0xA0,0x83,0x97,0x47,0xB1,0x92,0xCC,0xA0
			var endsn = mianHandler.ReadSentinel();

			return header;
		}
		private static void setVersionConditionals(ACadVersion dxfVersion)
		{
			R13_14Only = dxfVersion == ACadVersion.AC1014 || dxfVersion == ACadVersion.AC1012;
			R13_15Only = dxfVersion >= ACadVersion.AC1012 && dxfVersion <= ACadVersion.AC1015;
			R2000Plus = dxfVersion >= ACadVersion.AC1015;
			R2004Pre = dxfVersion < ACadVersion.AC1018;
			R2004Plus = dxfVersion >= ACadVersion.AC1018;
			R2007Plus = dxfVersion >= ACadVersion.AC1021;
			R2010Plus = dxfVersion >= ACadVersion.AC1024;
			R2013Plus = dxfVersion >= ACadVersion.AC1027;
			R2018Plus = dxfVersion >= ACadVersion.AC1032;
		}
	}
}
