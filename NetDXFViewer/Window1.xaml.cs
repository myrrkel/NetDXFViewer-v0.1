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
			
			

			myDXF.border.Reset();
			DrawDXF();
			
			Button resetBtn = new Button();
			resetBtn.Width=100;
			resetBtn.VerticalAlignment=VerticalAlignment.Top;
			resetBtn.HorizontalAlignment=HorizontalAlignment.Left;
			resetBtn.Content = "Reset";
			resetBtn.Click += ResetButton_Click;
			StackPanel stack = new StackPanel();
			stack.Children.Add(resetBtn);
			
			
			Button openBtn = new Button();
			openBtn.Width=100;
			openBtn.VerticalAlignment=VerticalAlignment.Top;
			openBtn.HorizontalAlignment=HorizontalAlignment.Left;
			openBtn.Content = "Open";
			openBtn.Click += btnOpenFile_Click;
			stack.Children.Add(openBtn);
			
			Button LineWeightBtn = new Button();
			LineWeightBtn.Width=100;
			LineWeightBtn.VerticalAlignment=VerticalAlignment.Top;
			LineWeightBtn.HorizontalAlignment=HorizontalAlignment.Left;
			LineWeightBtn.Content = "LineWeight";
			LineWeightBtn.Click += LineWeigh_Click;
			
			stack.Children.Add(LineWeightBtn);
			
			
			myDXF.mainGrid.Children.Add(stack);
			
			/*DrawUtils.SaveAsPng(DrawUtils.GetImage(myDXF.mainCanvas));*/
			
			
		}
		
		
		
		private void LineWeigh_Click(object sender, RoutedEventArgs e)
		{
			
			
			myDXF.border.Reset(10,10);
		}
		
		
		private void ResetButton_Click(object sender, RoutedEventArgs e)
		{
			IMGutil.SaveCanvas(this,myDXF.mainCanvas,96,@"c:\temp\logo.png");
			
			myDXF.border.Reset();
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
			
			netDxf.Entities.Line ligneTmp = new netDxf.Entities.Line();
			
			ligneTmp.StartPoint = new Vector3(0,0,0);
			ligneTmp.EndPoint = new Vector3(100,100,0);

			myDXF.DxfDoc = new DxfDocument();
			myDXF.DxfDoc.AddEntity(ligneTmp);
			
			if(fileDXF=="") fileDXF="tracery20.dxf";
			netDxf.Blocks.Block myBlock = netDxf.Blocks.Block.Load(fileDXF);
			netDxf.Entities.Insert myInsert = new netDxf.Entities.Insert(myBlock);
			
			myDXF.DxfDoc.AddEntity(myInsert);
			
			/*netDxf.Entities.Insert myInsert2 = new netDxf.Entities.Insert(myBlock);
			myInsert2.Rotation = 180;
			Vector3 pos= new Vector3(myInsert2.Position.X+40,myInsert2.Position.Y,0);
			myInsert2.Position = pos;
			myDXF.DxfDoc.AddEntity(myInsert2);*/
			
			
			
			this.Content = myDXF.GetMainGrid(myDXF.DxfDoc);

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