/*
 * Created by SharpDevelop.
 * User: michel
 * Date: 30/08/2016
 * Time: 10:24
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using netDxf.Entities;
using netDxf;

namespace NetDXFViewer
{
	/// <summary>
	/// Description of TypeConverter.
	/// </summary>
	public class DrawUtils
	{
		public DrawUtils()
		{
			
		}
		
		
		public static void DrawPoint(Vector3 position,Canvas mainCanvas,Color couleur, double radius, double thickness)
		{
			Canvas canvas1 = GetPoint(couleur,radius,thickness);
			Canvas.SetLeft(canvas1,position.X-radius);
			Canvas.SetTop(canvas1,mainCanvas.Height-(position.Y+radius));
			mainCanvas.Children.Add(canvas1);
		}
		
		public static void DrawPoint(double x, double y,Canvas mainCanvas,Color couleur, double radius, double thickness)
		{
			Vector3 position = new Vector3(x,y,0);
			DrawPoint(position,mainCanvas,couleur,radius,thickness);
		}
		
		
		public static void DrawPoint(Vector2 V2,Canvas mainCanvas,Color couleur, double radius, double thickness)
		{
			Vector3 position = TypeConverter.Vertex2ToVertex3(V2);
			DrawPoint(position,mainCanvas,couleur,radius,thickness);
		}
		
		
		public static void DrawPoint(Vector2 V2,string txtInfo,Canvas mainCanvas,Color couleur, double radius, double thickness)
		{
			DrawText(txtInfo,radius/1.5,couleur,V2.X+radius/2,V2.Y+radius,mainCanvas);
			DrawPoint(V2,mainCanvas,couleur,radius,thickness);
		}
		
		public static void DrawPoint(Vector3 V3,string txtInfo,Canvas mainCanvas,Color couleur, double radius, double thickness)
		{
			DrawText(txtInfo,radius/1.5,couleur,V3.X+radius/2,V3.Y+radius,mainCanvas);
			DrawPoint(V3,mainCanvas,couleur,radius,thickness);
		}
		
		public static Canvas GetPoint(Color couleur, double radius, double thickness)
		{
			SolidColorBrush myBrush = new SolidColorBrush(couleur);
			double myThickness = 1*(radius/40);
			if (thickness >0) myThickness = thickness;
			
			
			Canvas canvas1 = new Canvas();
			
			System.Windows.Shapes.Rectangle wRect = new System.Windows.Shapes.Rectangle();
			wRect.Width = radius;
			wRect.Height = radius;
			wRect.Stroke = myBrush;
			wRect.StrokeThickness=myThickness;
			Canvas.SetLeft(wRect,radius/2);
			Canvas.SetTop(wRect,radius/2);
			canvas1.Children.Add(wRect);
			
			System.Windows.Shapes.Line wLine = GetLine(0,radius,radius*2,radius);
			wLine.Stroke = myBrush;
			wLine.StrokeThickness=myThickness;
			wLine.SnapsToDevicePixels = false;
			canvas1.Children.Add(wLine);
			
			wLine = GetLine(radius,0,radius,radius*2);
			wLine.Stroke = myBrush;
			wLine.StrokeThickness=myThickness;
			canvas1.Children.Add(wLine);
			
			Canvas.SetZIndex(canvas1,9999);
			return canvas1;
		}
		
		
		public static System.Windows.Shapes.Line GetLine(Vector3 StartPoint,Vector3 EndPoint,Canvas mainCanvas)
		{
			return GetLine(StartPoint.X,StartPoint.Y,EndPoint.X,EndPoint.Y,mainCanvas);
		}
		
		public static System.Windows.Shapes.Line GetLine(double X1,double Y1,double X2,double Y2,Canvas mainCanvas)
		{
			System.Windows.Shapes.Line wLine = new System.Windows.Shapes.Line();
			wLine.X1 = X1;
			wLine.X2 = X2;
			wLine.Y1 = mainCanvas.Height-Y1;
			wLine.Y2 = mainCanvas.Height-Y2;
			return wLine;
		}
		
		public static System.Windows.Shapes.Line GetLine(double X1,double Y1,double X2,double Y2)
		{

			
			System.Windows.Shapes.Line wLine = new System.Windows.Shapes.Line();
			wLine.X1 = X1;
			wLine.X2 = X2;
			wLine.Y1 = Y1;
			wLine.Y2 = Y2;
			return wLine;
		}

		
		
		public static SolidColorBrush GetFillBrush(AciColor myColor,short transparency)
		{
			SolidColorBrush fillBrush = new SolidColorBrush(TypeConverter.ToMediaColor(myColor.ToColor()));
			
			if(transparency != 100 && transparency !=-1)
			{
				double dOpacity = ((double)transparency)/90;
				/*if(dOpacity < 0) dOpacity = 0.0;*/
				fillBrush.Opacity = 1-dOpacity;
			}
			return fillBrush;
		}
		
		
		public static void DrawText(string sTxt,double height,Color color,double X,double Y,Canvas mainCanvas)
		{
			System.Windows.Controls.TextBlock  wTxt = new System.Windows.Controls.TextBlock();
			
			wTxt.FontSize = TypeConverter.PointsToPixels(height);
			wTxt.FontFamily = new FontFamily("simplex");
			wTxt.Foreground = new SolidColorBrush(color);
			
			wTxt.LineHeight = height*1.66;
			wTxt.Padding = new Thickness(0, 0, 0, 0);
			wTxt.Margin = new Thickness(0,0, 0, 0);
			wTxt.LineStackingStrategy=LineStackingStrategy.BlockLineHeight;
			wTxt.TextAlignment = System.Windows.TextAlignment.Left;
			wTxt.FontStretch = FontStretches.UltraExpanded;
			wTxt.TextWrapping = TextWrapping.Wrap;
			
			TextUtils.CADTxtToInlineCollection(wTxt.Inlines,sTxt,wTxt.FontSize);
			

			
			Size txtSize = TypeConverter.MeasureString(wTxt,sTxt);
			wTxt.Width = txtSize.Width;
			/*
			DrawUtils.DrawPoint(X,Y,mainCanvas,Colors.Green,5,0.1);
			DrawUtils.DrawPoint(X+wTxt.Width,Y,mainCanvas,Colors.Blue,5,0.1);
			 */
			
			Canvas.SetLeft(wTxt,X);
			Canvas.SetTop(wTxt,mainCanvas.Height-Y);
			mainCanvas.Children.Add(wTxt);
			
		}
		
		public static void DrawText(string sTxt,double height,Color color,Vector3 V3,Canvas mainCanvas)
		{
			System.Windows.Point P1 = TypeConverter.Vertex3ToPoint(V3);
			DrawText(sTxt,height,color,P1.X,P1.Y,mainCanvas);
		}
		
		public static System.Windows.Shapes.Polygon GetArrowhead(Vector2 p1,Vector2 p2,Canvas mainCanvas)
		{
			System.Windows.Shapes.Polygon wPoly = new System.Windows.Shapes.Polygon();
			Vector2 A1 = p2-p1;
			Vector2 A2 = new Vector2(0,10);
			System.Windows.Point p=new System.Windows.Point();
			
			/*DrawUtils.DrawPoint(p1,mainCanvas,Colors.Green,10,0.1);
			/*DrawUtils.DrawPoint(p2,mainCanvas,Colors.Red,10,0);
			DrawUtils.DrawPoint(A1,mainCanvas,Colors.Yellow,10,0);*/
			
			wPoly.Points.Add(TypeConverter.Vertex2ToPoint(p1,mainCanvas.Height));
			
			p.X= p1.X+0.5;
			p.Y= mainCanvas.Height-(p1.Y+3);
			wPoly.Points.Add(p);
			
			p.X= p1.X-0.5;
			p.Y= mainCanvas.Height-(p1.Y+3);
			wPoly.Points.Add(p);
			
			wPoly.Points.Add(TypeConverter.Vertex2ToPoint(p1,mainCanvas.Height));
			double a1 = 0;
			/*Vector2.Angle(p1)*(180/Math.PI)*/
			if (p1.X > p2.X)
			{
				a1 = -Vector2.AngleBetween(A1,A2)*(180/Math.PI);
			}
			else
			{
				a1 = Vector2.AngleBetween(A2,A1)*(180/Math.PI);
			}
			
			
			RotateTransform  rotat = new RotateTransform(a1);
			rotat.CenterX = p1.X;
			rotat.CenterY = mainCanvas.Height-p1.Y;
			wPoly.RenderTransform = rotat;
			
			return wPoly;
		}
		
		
		public static VisualBrush GetVisualBrush()
		{
			VisualBrush vBrush = new VisualBrush();
			System.Windows.Point p1 = new System.Windows.Point(0,0);
			System.Windows.Point p2 = new System.Windows.Point(10,10);
			System.Windows.Shapes.Line geoLigne = new System.Windows.Shapes.Line();
			geoLigne.X1 = p1.X;
			geoLigne.X2 = p2.X;
			geoLigne.Y1 = p1.Y;
			geoLigne.Y2 = p2.Y;
			geoLigne.StrokeThickness=0.001;
			geoLigne.Stroke = new SolidColorBrush(Colors.Red);

			
			System.Windows.Shapes.Path path1 = new System.Windows.Shapes.Path();
			path1.StrokeThickness=1;
			path1.Stroke = new SolidColorBrush(Colors.Red);
			
			StreamGeometry geoStream = new StreamGeometry();
			geoStream.FillRule = FillRule.EvenOdd;
			using (StreamGeometryContext ctx = geoStream.Open())
			{

				// Begin the triangle at the point specified. Notice that the shape is set to
				// be closed so only two lines need to be specified below to make the triangle.
				ctx.BeginFigure(p1, true /* is filled */, true /* is closed */);

				// Draw a line to the next specified point.
				ctx.LineTo(p2, true /* is stroked */, false /* is smooth join */);

			}

			// Freeze the geometry (make it unmodifiable)
			// for additional performance benefits.
			geoStream.Freeze();
			
			path1.Data=geoStream;
			
			vBrush.TileMode=TileMode.Tile;
			vBrush.Viewport = new Rect(0,0,10,10);
			vBrush.ViewportUnits = BrushMappingMode.Absolute;
			vBrush.Viewbox = new Rect(0,0,10,10);
			vBrush.ViewboxUnits = BrushMappingMode.Absolute;
			vBrush.Visual = path1;
			
			return vBrush;
		}
		
		public static DrawingBrush GetGridBrush()
		{
			
			
			DrawingBrush dBrush = new DrawingBrush();
			dBrush.Viewport = new Rect(0,0,10,10);
			dBrush.ViewportUnits = BrushMappingMode.Absolute;
			/*dBrush.Viewbox = new Rect(0,0,10,10);
			dBrush.ViewboxUnits = BrushMappingMode.Absolute;*/
			dBrush.TileMode=TileMode.Tile;
			
			Color couleur = (Color)ColorConverter.ConvertFromString("#3A4053");
			SolidColorBrush brush = new SolidColorBrush(couleur);
			Pen pen = new Pen(brush, 0.01);
			
			
			DrawingGroup dGroup = new DrawingGroup();
			GeometryDrawing GeoD1 = new GeometryDrawing(
				brush,
				pen,
				Geometry.Parse("M0,0 L1,0 1,0.01, 0,0.01Z")
			);
			dGroup.Children.Add(GeoD1);

			GeometryDrawing GeoD2 = new GeometryDrawing(
				brush,
				pen,
				Geometry.Parse("M0,0 L0,1 0.01,1, 0.01,0Z")
			);
			dGroup.Children.Add(GeoD2);
			
			
			dBrush.Drawing = dGroup;
			
			
			
			return dBrush;
		}
		
		
		
		public static StreamGeometry GetStreamGeoLine(System.Windows.Point p1,System.Windows.Point p2)
		{
			StreamGeometry geoStream = new StreamGeometry();
			geoStream.FillRule = FillRule.EvenOdd;
			using (StreamGeometryContext ctx = geoStream.Open())
			{

				// Begin the triangle at the point specified. Notice that the shape is set to
				// be closed so only two lines need to be specified below to make the triangle.
				ctx.BeginFigure(p1, true /* is filled */, true /* is closed */);

				// Draw a line to the next specified point.
				ctx.LineTo(p2, true /* is stroked */, false /* is smooth join */);

			}

			// Freeze the geometry (make it unmodifiable)
			// for additional performance benefits.
			geoStream.Freeze();

			return geoStream;
		}
		
		
		public static void DrawOrigin(Canvas mainCanvas)
		{
			Vector3 V0 = new Vector3(0,0,0);
			Vector3 Vx = new Vector3(20,0,0);
			Vector3 Vy = new Vector3(0,20,0);
			
			System.Windows.Shapes.Line lx = GetLine(V0,Vx,mainCanvas);
			lx.Stroke = new SolidColorBrush(Colors.Red);
			lx.StrokeThickness = 0.5;
			lx.X2 = lx.X2-3;
			mainCanvas.Children.Add(lx);
			System.Windows.Shapes.Polygon arrowX = DrawUtils.GetArrowhead(TypeConverter.Vertex3ToVertex2(Vx), TypeConverter.Vertex3ToVertex2(V0), mainCanvas);
			arrowX.Stroke = new SolidColorBrush(Colors.Red);
			arrowX.StrokeThickness = 0.1;
			arrowX.Fill = arrowX.Stroke;
			mainCanvas.Children.Add(arrowX);
			
			System.Windows.Shapes.Line ly = GetLine(V0,Vy,mainCanvas);
			ly.Stroke = new SolidColorBrush(Colors.Green);
			ly.StrokeThickness = 0.5;
			ly.Y2 = ly.Y2+3;
			mainCanvas.Children.Add(ly);
			System.Windows.Shapes.Polygon arrowY = DrawUtils.GetArrowhead(TypeConverter.Vertex3ToVertex2(Vy), TypeConverter.Vertex3ToVertex2(V0), mainCanvas);
			arrowY.Stroke = new SolidColorBrush(Colors.Green);
			arrowY.StrokeThickness = 0.1;
			arrowY.Fill = arrowY.Stroke;
			mainCanvas.Children.Add(arrowY);
		}
		
		public static RenderTargetBitmap GetImage(Canvas canvas,double scale=1.0,bool avecFond=false)
		{
			DrawEntities.CalcMaxDimDoc();
			DimMax dim = DrawEntities.dimDoc;
			
			
			
			Size size = new Size(dim.Width(), dim.Height());
			//Size size = new Size(1000, 1000);
			//System.Windows.Point p0 = new System.Windows.Point(0,-200);
			System.Windows.Point p0 = new System.Windows.Point(dim.minX,dim.minY);
			if (size.IsEmpty)
				return null;

			//RenderTargetBitmap result = new RenderTargetBitmap((int)size.Width*2, (int)size.Height*2, 96*2, 96*2, PixelFormats.Pbgra32);
			RenderTargetBitmap result = new RenderTargetBitmap((int)(size.Width*scale), (int)(size.Height*scale), 96*scale, 96*scale, PixelFormats.Default);

			DrawingVisual drawingvisual = new DrawingVisual();
			using (DrawingContext context = drawingvisual.RenderOpen())
			{
				if(avecFond) context.DrawRectangle(Brushes.White, null, new Rect(p0, size));
				context.DrawRectangle(new VisualBrush(canvas), null, new Rect(p0, size));
				context.Close();
			}

			result.Render(drawingvisual);
			return result;
		}
		
		
		public static void SaveAsPng(RenderTargetBitmap src)
		{
			PngBitmapEncoder encoder = new PngBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(src));
			
			FileStream stream = new FileStream(@"c:\temp\logo.png", FileMode.Create);
			encoder.Save(stream);
			stream.Close();
		}
		
		
		public static void CreateBitmapFromVisual(FrameworkElement target, string filename)
		{
			if (target == null)
				return;

			

			RenderTargetBitmap rtb = new RenderTargetBitmap(6000, 6000, 96, 96, PixelFormats.Pbgra32);
			
			DrawingVisual dv = new DrawingVisual();
			
			using (DrawingContext dc = dv.RenderOpen())
			{
				VisualBrush vb = new VisualBrush(target);
				Size size = new Size(5000, 5000);
				System.Windows.Point p0 = new System.Windows.Point(0,-2000);
				dc.DrawRectangle(vb, null, new Rect(p0, size));
			}

			rtb.Render(dv);

			PngBitmapEncoder png = new PngBitmapEncoder();

			png.Frames.Add(BitmapFrame.Create(rtb));

			using (Stream stm = File.Create(filename))
			{
				png.Save(stm);
			}
		}
		
	}
	
	public static class IMGutil
	{
		public static void SaveWindow(Window window, int dpi, string filename)
		{

			var rtb = new RenderTargetBitmap(
				(int)window.Width, //width
				(int)window.Width, //height
				dpi, //dpi x
				dpi, //dpi y
				PixelFormats.Pbgra32 // pixelformat
			);
			rtb.Render(window);

			SaveRTBAsPNG(rtb, filename);

		}
		
		public static void SaveCanvas(Window window, Canvas canvas, int dpi, string filename)
		{
			Size size = new Size(window.Width , window.Height );
			canvas.Measure(size);
			//canvas.Arrange(new Rect(size));

			var rtb = new RenderTargetBitmap(
				5000, 5000,     // (int)window.Width, //width
				//(int)window.Height, //height
				dpi, //dpi x
				dpi, //dpi y
				PixelFormats.Pbgra32 // pixelformat
			);
			rtb.Render(canvas);

			SaveRTBAsPNG(rtb, filename);
		}
		

		public static void SaveCanvas(int hauteur, int largeur, Canvas canvas, int dpi, string filename)
		{
			Size size = new Size(largeur, hauteur);
			canvas.Measure(size);
			//canvas.Arrange(new Rect(size));

			var rtb = new RenderTargetBitmap(
				hauteur, largeur,     // (int)window.Width, //width
				//(int)window.Height, //height
				dpi, //dpi x
				dpi, //dpi y
				PixelFormats.Pbgra32 // pixelformat
			);
			rtb.Render(canvas);

			SaveRTBAsPNG(rtb, filename);
		}

		private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
		{
			var enc = new System.Windows.Media.Imaging.PngBitmapEncoder();
			enc.Frames.Add(System.Windows.Media.Imaging.BitmapFrame.Create(bmp));

			using (var stm = System.IO.File.Create(filename))
			{
				enc.Save(stm);
			}
		}
	}

}
