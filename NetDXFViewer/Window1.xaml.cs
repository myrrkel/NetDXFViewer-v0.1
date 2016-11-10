/*
 * Created by SharpDevelop.
 * User: michel
 * Date: 24/08/2016
 * Time: 14:49
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using netDxf;
using Microsoft.Win32;

namespace NetDXFViewer
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	/// 
	
	
	
	public partial class Window1 : Window
	{
		
		public DXF2WPF myDXF = new DXF2WPF();
		
		/*public netDxf.DxfDocument DxfDoc = netDxf.DxfDocument.Load("sample2.dxf");
		public static Color bgColor = Color.FromRgb(33,40,48);*/
		
		public Window1()
		{
			InitializeComponent();

			myDXF.GridHeight = 5000;
			myDXF.GridWidth = 5000;
			myDXF.ViewHeight = grid1.ActualHeight;
			myDXF.ViewWidth = grid1.ActualWidth;
			myDXF.WinHeight = this.Height;
			myDXF.WinWidth = this.Width;

			myDXF.border.Reset(myDXF.GridHeight,myDXF.GridWidth,true,this.Height,this.Width,this.Height,this.Width);
			DrawDXF();
			
			Button resetBtn = new Button();
			resetBtn.Width=100;
			resetBtn.VerticalAlignment=VerticalAlignment.Top;
			resetBtn.HorizontalAlignment=HorizontalAlignment.Left;
			resetBtn.Content = "Reset";
			resetBtn.Click += ZoomOut_Click;
			StackPanel stack = new StackPanel();
			stack.Children.Add(resetBtn);
			
			
			Button openBtn = new Button();
			openBtn.Width=100;
			openBtn.VerticalAlignment=VerticalAlignment.Top;
			openBtn.HorizontalAlignment=HorizontalAlignment.Left;
			openBtn.Content = "Open";
			openBtn.Click += btnOpenFile_Click;
			stack.Children.Add(openBtn);
			
			Button ZoomAutoBtn = new Button();
			ZoomAutoBtn.Width=100;
			ZoomAutoBtn.VerticalAlignment=VerticalAlignment.Top;
			ZoomAutoBtn.HorizontalAlignment=HorizontalAlignment.Left;
			ZoomAutoBtn.Content = "ZoomAuto";
			ZoomAutoBtn.Click += ZoomAuto_Click;
			
			stack.Children.Add(ZoomAutoBtn);
			
			
			myDXF.mainGrid.Children.Add(stack);
			
			//DrawUtils.SaveAsPng(DrawUtils.GetImage(myDXF.mainCanvas));
			
			
		}
		
		
		private void ZoomOut_Click(object sender, RoutedEventArgs e)
		{
			
			
			//Point current = myDXF.border.CurrentPosition(myDXF.GridHeight,myDXF.GridWidth,((Grid)Application.Current.MainWindow.Content).ActualHeight,((Grid)Application.Current.MainWindow.Content).ActualWidth);
			//DrawUtils.DrawPoint(current.X,current.Y,this.myDXF.mainCanvas,Colors.Red,25,1);
			
			myDXF.border.Zoom(myDXF.GridHeight,myDXF.GridWidth,((Grid)Application.Current.MainWindow.Content).ActualHeight,((Grid)Application.Current.MainWindow.Content).ActualWidth,0,0,1);
		}
		
		
		private void ZoomAuto_Click(object sender, RoutedEventArgs e)
		{
			
			
			myDXF.border.ZoomAuto(5000,5000,((Grid)Application.Current.MainWindow.Content).ActualHeight,((Grid)Application.Current.MainWindow.Content).ActualWidth);
			//myDXF.border.ZoomAuto(5000,5000,523,784);
			//myDXF.border.ZoomAuto(5000,5000,Application.Current.MainWindow.Height,Application.Current.MainWindow.Width);
		}
		
		
		private void ResetButton_Click(object sender, RoutedEventArgs e)
		{
			//IMGutil.SaveCanvas(this,myDXF.mainCanvas,96,@"c:\temp\logo.png");
			
			myDXF.border.Reset();
			//myDXF.border.ZoomOut(myDXF.GridHeight,myDXF.GridWidth,myDXF.WinHeight,myDXF.WinWidth);
			//myDXF.border.ZoomAuto(5000,5000,523,784);

		}
		
		private void DrawDXF_Click(object sender, RoutedEventArgs e)
		{
			DrawDXF();
		}
		
		
		private void btnOpenFile_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if(openFileDialog.ShowDialog() == true)
			{
				String fileDXF = openFileDialog.FileName;
				DrawDXF(fileDXF);
			}
			
			
			
			
		}
		
		private void DrawDXF(string fileDXF)
		{
			
			TypeConverter.defaultThickness = 0.01;
			
			
			myDXF.DxfDoc = new DxfDocument();

			
		
			
			netDxf.Blocks.Block myBlock = netDxf.Blocks.Block.Load("P4035PINM.dxf");
			netDxf.Entities.Insert myInsert = new netDxf.Entities.Insert(myBlock);
			myInsert.Lineweight= Lineweight.W100;
			myInsert.LinetypeScale= 100;
			myInsert.Position = new Vector3(0,100,0);
			myInsert.Scale = new Vector3(-2,-2,0);
			Vector3 pos0 = new Vector3(myInsert.Position.X,myInsert.Position.Y,0);
			myInsert.Position = pos0;
			myInsert.Rotation = 0;
			AciColor bgcolor = new AciColor();
			myDXF.DxfDoc.AddEntity(myInsert);
			
			
			
			
			/*netDxf.Blocks.Block myBlock2 = netDxf.Blocks.Block.Load(fileDXF);

			netDxf.Entities.Insert myInsert2 = new netDxf.Entities.Insert(myBlock2);
			myInsert2.Position = new Vector3(30,40,0);
			myInsert2.Scale = new Vector3(1,1,0);
			
			myInsert2.Color = AciColor.Blue;

			myInsert2.Rotation = 0;

			myDXF.DxfDoc.AddEntity(myInsert2);*/
			
			
			
			/*
			netDxf.Entities.Insert myInsert2 = new netDxf.Entities.Insert(myBlock);

			Vector3 scaleInsert = new Vector3(1,-1,1);
			myInsert2.Scale = scaleInsert;
			Vector3 pos= new Vector3(myInsert2.Position.X+5,myInsert2.Position.Y,0);
			myInsert2.Position = pos;
			myDXF.DxfDoc.AddEntity(myInsert2);
			 */
			
			//if(fileDXF=="") fileDXF="sample2.dxf";
			if(fileDXF!="")
			{
				this.Content = myDXF.GetMainGrid(fileDXF,true,true);
				DrawUtils.SaveAsPng(DrawUtils.GetImage(myDXF.mainCanvas));
					
			}
			else
			{
				this.Content = myDXF.GetMainGrid(true,true);
			}
			
			//myDXF.border.ZoomAuto(5000,5000,((Grid)Application.Current.MainWindow.Content).ActualHeight,((Grid)Application.Current.MainWindow.Content).ActualWidth);
			myDXF.border.ZoomAuto(5000,5000,win1.myDXF.WinHeight,win1.myDXF.WinWidth);
			DrawUtils.DrawPoint(100,0,myDXF.mainCanvas,Colors.Red,25,1);
			DrawUtils.DrawPoint(-225,0,myDXF.mainCanvas,Colors.Red,25,1);
			DrawUtils.SaveAsPng(DrawUtils.GetImage(myDXF.mainCanvas));

		}
		
		private void DrawDXFInsert(string fileDXF)
		{
			
			TypeConverter.defaultThickness = 0.01;
			DrawEntities.RazMaxDim();
			/*netDxf.Entities.Line ligneTmp = new netDxf.Entities.Line();
			
			ligneTmp.StartPoint = new Vector3(0,0,0);
			ligneTmp.EndPoint = new Vector3(100,100,0);*/

			myDXF.DxfDoc = new DxfDocument();
			//myDXF.DxfDoc.AddEntity(ligneTmp);
			
			if(fileDXF=="") fileDXF="raptor.dxf";
			netDxf.Blocks.Block myBlock = netDxf.Blocks.Block.Load(fileDXF);
			netDxf.Entities.Insert myInsert = new netDxf.Entities.Insert(myBlock);
			
			myDXF.DxfDoc.AddEntity(myInsert);
			
			netDxf.Entities.Insert myInsert2 = new netDxf.Entities.Insert(myBlock);
			//myInsert2.Rotation = 180;
			Vector3 scaleInsert = new Vector3(1,-1,1);
			myInsert2.Scale = scaleInsert;
			Vector3 pos= new Vector3(myInsert2.Position.X+5,myInsert2.Position.Y,0);
			myInsert2.Position = pos;
			myDXF.DxfDoc.AddEntity(myInsert2);
			
			
			
			this.Content = myDXF.GetMainGrid(myDXF.DxfDoc,true,true);

		}
		
		private void DrawDXF()
		{
			
			DrawDXF("");
		}
		
		private void DrawDXF2()
		{
			canvas1.Children.Clear();
			/*this.Content = DXF2WPF.GetMainGrid("sample2.dxf");*/
			//this.Content = myDXF.GetMainGrid("sample2.dxf",true,false);
			netDxf.Entities.Line ligneTmp = new netDxf.Entities.Line();
			
			ligneTmp.StartPoint = new Vector3(0,0,0);
			ligneTmp.EndPoint = new Vector3(100,100,0);
			/*ligneTmp.Thickness=20;
			ligneTmp.Lineweight = (Lineweight)15;
			ligneTmp.Color = new AciColor(8);*/
			myDXF.DxfDoc = new DxfDocument();
			myDXF.DxfDoc.AddEntity(ligneTmp);
			
			Grid mainGrid = new Grid();
			Canvas newMainCanvas = new Canvas();
			DXF2WPF.GetCanvas(myDXF.DxfDoc,myDXF.mainCanvas);
			myDXF.mainCanvas.Background = new SolidColorBrush(Colors.Blue);
			mainGrid.Children.Add(myDXF.mainCanvas);
			this.Content = mainGrid;
			//this.Content = myDXF.GetMainGrid("panther.dxf");
			/*CanvasCreator.GetCanvas(DxfDoc,canvas1);
			DrawUtils.DrawOrigin(canvas1);*/
		}
		
		
		
		
		
		
	}
}