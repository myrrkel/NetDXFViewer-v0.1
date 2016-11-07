/*
 * Created by SharpDevelop.
 * User: michel
 * Date: 08/25/2016
 * Time: 11:00
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using MColor = System.Windows.Media.Color;
using DColor = System.Drawing.Color;
using netDxf.Entities;
using netDxf;
using System.Collections.Generic;
using System.Windows.Media;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using System.Globalization;


namespace NetDXFViewer
{
	/// <summary>
	/// Description of TypeConverter.
	/// </summary>
	public class TypeConverter
	{
		
		public static double defaultThickness = 0.1;
		
		public TypeConverter()
		{
			

			
		}
		
		public static MColor ToMediaColor(DColor color)
		{
			return MColor.FromArgb(color.A, color.R, color.G, color.B);
		}

		
		
		public static void Entity2Shape(netDxf.Entities.EntityObject xEntity, System.Windows.Shapes.Shape wShape)
		{
			double dThickness = xEntity.getLineweightValue();
			double dScale = xEntity.LinetypeScale;
			
			/* By Block */
			if (dThickness==-2)
			{
				dThickness=xEntity.Owner.Layer.getLineweightValue();
			}
			if (dScale==-2) dScale=1;
			
			
			/* By Default */
			if (dThickness==-3) dThickness= defaultThickness;
			if (dScale==-3) dScale=1;
			
			/*Debug.WriteLine("dThickness="+dThickness);*/
			/*Debug.WriteLine("dThickness="+dThickness+" dScale="+dScale);*/
			
			wShape.StrokeThickness = (dThickness > 0) ? dThickness*dScale*3.4 : 0.1;
			
			DoubleCollection myCollec = new DoubleCollection(xEntity.Linetype.Segments.ToArray());
			wShape.StrokeDashArray = myCollec;
			
			AciColor myColor = xEntity.getColor();
			wShape.Stroke = new SolidColorBrush(TypeConverter.ToMediaColor(myColor.ToColor()));
		}
		
		

		public static Vector3 Vertex2ToVertex3(Vector2 V2)
		{
			Vector3 V3 = new Vector3();
			V3.X=V2.X;
			V3.Y=V2.Y;
			return V3;
		}
		
		public static Vector2 Vertex3ToVertex2(Vector3 V3)
		{
			Vector2 V2 = new Vector2();
			V2.X=V3.X;
			V2.Y=V3.Y;
			return V2;
		}
		
		
		public static System.Windows.Point Vertex3ToPoint(Vector3 V3)
		{
			System.Windows.Point newPoint = new System.Windows.Point();
			newPoint.X=V3.X;
			newPoint.Y=V3.Y;
			return newPoint;
		}
		
		

		
		public static System.Windows.Point Vertex3ToPoint(Vector3 V3,double CanvasHeight)
		{
			DrawEntities.getMaxPt(V3);
			System.Windows.Point newPoint = new System.Windows.Point();
			newPoint.X=V3.X;
			newPoint.Y=CanvasHeight-V3.Y;
			return newPoint;
		}
		
		public static System.Windows.Point Vertex2ToPoint(Vector2 V2)
		{
			System.Windows.Point newPoint = new System.Windows.Point();
			newPoint.X=V2.X;
			newPoint.Y=V2.Y;
			return newPoint;
		}
		
		public static System.Windows.Point Vertex2ToPoint(Vector2 V2,double CanvasHeight)
		{
			DrawEntities.getMaxPt(V2);
			System.Windows.Point newPoint = new System.Windows.Point();
			newPoint.X=V2.X;
			newPoint.Y=CanvasHeight-V2.Y;
			return newPoint;
		}

		public static double PointsToPixels(double points)
		{
			return points*(96.0/72.0);
		}
		
		
		public static double PixelsToPoints(double px)
		{
			return px/(96.0/72.0);
		}
		
		public static Size MeasureString(TextBlock thisTextBlock, string candidate)
		{
			var formattedText = new FormattedText(
				candidate,
				CultureInfo.CurrentUICulture,
				FlowDirection.LeftToRight,
				new Typeface(thisTextBlock.FontFamily, thisTextBlock.FontStyle, thisTextBlock.FontWeight, thisTextBlock.FontStretch),
				thisTextBlock.FontSize,
				Brushes.Black);
			
			return new Size(formattedText.Width, formattedText.Height);
		}
		
		public static Size MeasureString(TextBlock thisTextBlock, string candidate,double MaxTextWidth)
		{
			var formattedText = new FormattedText(
				candidate,
				CultureInfo.CurrentUICulture,
				FlowDirection.LeftToRight,
				new Typeface(thisTextBlock.FontFamily, thisTextBlock.FontStyle, thisTextBlock.FontWeight, thisTextBlock.FontStretch),
				thisTextBlock.FontSize,
				Brushes.Black);
			
			formattedText.MaxTextWidth = MaxTextWidth;
			return new Size(formattedText.Width, formattedText.Height);
		}
		
		
		public static System.Windows.TextAlignment AttachmentPointToAlign(MTextAttachmentPoint Attach)
		{
			System.Windows.TextAlignment hAlign = new System.Windows.TextAlignment();
			
			List<int> list = new List<int> {2,5,8};
			if (list.Contains((int)Attach)) hAlign = System.Windows.TextAlignment.Center;
			
			list = new List<int> {1,4,7};
			if (list.Contains((int)Attach)) hAlign = System.Windows.TextAlignment.Left;
			
			list =  new List<int>{3,6,9};
			if (list.Contains((int)Attach)) hAlign = System.Windows.TextAlignment.Right;
			
			return hAlign;
		}
		
		
		public static Vector3 TextAttachmentToPosition(MTextAttachmentPoint Attach,Vector3 position,double width,double height)
		{
			Vector3 newPos = new Vector3();
			newPos = position;
			
			List<int> list = new List<int> {2,5,8}; /*Center*/
			if (list.Contains((int)Attach))
			{
				newPos.X = newPos.X-width/2;
			}
			
			list = new List<int> {1,4,7}; /*Left*/
			if (list.Contains((int)Attach))
			{
				newPos.X = newPos.X;
			}
			
			list =  new List<int>{3,6,9}; /*Right*/
			if (list.Contains((int)Attach))
			{
				newPos.X = newPos.X-width;
			}
			
			list = new List<int> {1,2,3}; /*Top*/
			if (list.Contains((int)Attach))
			{
				/*newPos.Y = newPos.Y+height;*/
			}
			
			list = new List<int> {4,5,6}; /*Middle*/
			if (list.Contains((int)Attach))
			{
				newPos.Y = newPos.Y+height/2;
				
			}
			
			list =  new List<int>{7,8,9}; /*Bottom*/
			if (list.Contains((int)Attach))
			{
				newPos.Y = newPos.Y+height;
			}
			
			return newPos;
		}
		
		
		
		public static Vector3 TextAlignmentToPosition(netDxf.Entities.TextAlignment Attach,Vector3 position,double width,double height)
		{
			Vector3 newPos = new Vector3();
			newPos = position;
			
			List<int> list = new List<int> {1,4,7,10}; /*Center*/
			if (list.Contains((int)Attach))
			{
				newPos.X = newPos.X-width/2;
			}
			
			list = new List<int> {0,3,6,9}; /*Left*/
			if (list.Contains((int)Attach))
			{
				/*Idem*/
			}
			
			list =  new List<int>{2,5,8,11}; /*Right*/
			if (list.Contains((int)Attach))
			{
				newPos.X = newPos.X-width;
			}
			
			list = new List<int> {0,1,2}; /*Top*/
			if (list.Contains((int)Attach))
			{
				/*newPos.Y = newPos.Y+height;*/
			}
			
			list = new List<int> {3,4,5}; /*Middle*/
			if (list.Contains((int)Attach))
			{
				newPos.Y = newPos.Y+height/2;
				
			}
			
			list =  new List<int>{6,7,8,9,10,11}; /*Bottom*/
			if (list.Contains((int)Attach))
			{
				newPos.Y = newPos.Y+height;
			}
			
			return newPos;
		}
		
		
		public static System.Windows.Media.SolidColorBrush AciColorToBrush(AciColor myColor)
		{
			
			return new SolidColorBrush(TypeConverter.ToMediaColor(myColor.ToColor()));
		}
		
		
		public static System.Windows.Media.Brush PatternToBrush(HatchPattern myPattern,AciColor myColor)
		{

			VisualBrush vBrush = new VisualBrush();
			System.Windows.Media.Brush resBrush = vBrush;
			
			if(myPattern.Fill==netDxf.Entities.HatchFillType.PatternFill)
			{
				System.Windows.Shapes.Path path1 = new System.Windows.Shapes.Path();
				path1.StrokeThickness=1;
				path1.Stroke = AciColorToBrush(myColor);
				
				System.Windows.Point p0 = TypeConverter.Vertex2ToPoint(myPattern.LineDefinitions[0].Origin);
				double deltaY = myPattern.LineDefinitions[0].Delta.Y;
				
				
				if(myPattern.Name=="LINE")
				{
					System.Windows.Point p1 = p0;
					System.Windows.Point p2 = new System.Windows.Point(p1.X,p1.Y+10);
					/*path1.Data=Geometry.Parse("M 5 0 L 5 10 Z");*/
					path1.Data=DrawUtils.GetStreamGeoLine(p1,p2);
				}
				
				vBrush.TileMode=TileMode.Tile;
				vBrush.Viewport = new Rect(p0.X,p0.Y,deltaY,deltaY);

				vBrush.ViewportUnits = BrushMappingMode.Absolute;
				vBrush.Viewbox = new Rect(p0.X,p0.Y,10,10);
				
				vBrush.ViewboxUnits = BrushMappingMode.Absolute;
				vBrush.Visual = path1;
				
				vBrush.RelativeTransform = new RotateTransform(myPattern.Angle,5,5);
				
				resBrush = vBrush;
				
			}
			else if (myPattern.Fill==netDxf.Entities.HatchFillType.SolidFill)
			{
				resBrush = PatternSolidFillToBrush((HatchGradientPattern)myPattern,myColor);
			}
			
			return resBrush;
			
			
		}
		
		
		public static System.Windows.Media.Brush PatternSolidFillToBrush(HatchGradientPattern myPattern,AciColor myColor)
		{
			GradientStopCollection stops = new GradientStopCollection();
			LinearGradientBrush lgBrush = new LinearGradientBrush(stops);
			RadialGradientBrush rgBrush = new RadialGradientBrush(stops);
			
			if(myPattern.GradientType==HatchGradientPatternType.InvSpherical)
			{
				rgBrush.Center = new System.Windows.Point(0.25,0.25);
				rgBrush.GradientOrigin = new System.Windows.Point(0.25,0.25);

				rgBrush.RadiusX = 1;
				rgBrush.RadiusY = 1;
				GradientStop stop1 = new GradientStop();
				stop1.Color = ToMediaColor(myPattern.Color1.ToColor());
				stop1.Offset=0.15;
				stops.Add(stop1);
				GradientStop stop2 = new GradientStop();
				stop2.Color = ToMediaColor(myPattern.Color2.ToColor());
				stop2.Offset=0.5;
				stops.Add(stop2);
				
				rgBrush.GradientStops = stops;
				
				return rgBrush;
			}
			else if(myPattern.GradientType==HatchGradientPatternType.Hemispherical)
			{
				rgBrush.Center = new System.Windows.Point(1,1);
				rgBrush.GradientOrigin = new System.Windows.Point(1,1);

				rgBrush.RadiusX = 1.5;
				rgBrush.RadiusY = 1.5;
				GradientStop stop1 = new GradientStop();
				stop1.Color = ToMediaColor(myPattern.Color2.ToColor());
				stop1.Offset=0.1;
				stops.Add(stop1);
				GradientStop stop2 = new GradientStop();
				stop2.Color = ToMediaColor(myPattern.Color1.ToColor());
				stop2.Offset=0.9;
				stops.Add(stop2);
				
				rgBrush.GradientStops = stops;
				
				return rgBrush;
			}
			else
			{
				return AciColorToBrush(myColor);
			}
			
			
			
			
			
			
			
			
		}
	}
	
}


