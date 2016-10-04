/*
 * Created by SharpDevelop.
 * User: michel
 * Date: 02/09/2016
 * Time: 09:15
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Diagnostics;
/*using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using netDxf.Entities;
using netDxf.Blocks;
using netDxf;*/

namespace NetDXFViewer
{
	/// <summary>
	/// Description of TypeConverter.
	/// </summary>
	public class TextUtils
	{
		public TextUtils()
		{

		}
		
		public static string GetVulgarFraction(int numerator, int denominator)
		{
			if(numerator<0)
			{
				// Handle -1/2 as "-½"
				return string.Format("-{0}",
				                     GetVulgarFraction(-numerator, denominator));
			}
			if(numerator>denominator)
			{
				// Handle 7/4 as "1 ¾"
				return string.Format("{0} {1}",
				                     numerator/denominator,
				                     GetVulgarFraction(numerator%denominator, denominator));
			}
			// Handle 0/1 = "0"
			if(numerator==0) return "0";
			// Handle 10/1 = "10"
			if(denominator==1) return numerator.ToString();
			// Handle 1/2 = ½
			if(denominator==2)
			{
				if(numerator==1) return "½";
			}
			// Handle 1/4 = ¼
			if(denominator==4)
			{
				if(numerator==1) return "¼";
				if(numerator==3) return "¾";
			}
			// Handle 1/8 = ⅛
			if(denominator==8)
			{
				if(numerator==1) return "⅛";
				if(numerator==3) return "⅜";
				if(numerator==5) return "⅝";
				if(numerator==7) return "⅞";
			}
			// Catch all
			return string.Format("{0}/{1}", numerator, denominator);
		}
		
		
		public static InlineCollection CADTxtToInlineCollection(InlineCollection txtCollec, string cadTxt,double fontHeight)
		{
			string formatTxt="";
			string txt=getCADFranction(cadTxt);
			int prec=0;
			
			Regex rx = new Regex(@"\{(.*?)}",RegexOptions.Compiled | RegexOptions.IgnoreCase);
			MatchCollection matches = rx.Matches(txt);
			
			if(matches.Count > 0)
			{
				foreach(Match sCad in matches)
				{
					if(prec != sCad.Index) txtCollec.Add(txt.Substring(prec,sCad.Index-prec));
					formatTxt = sCad.Groups[1].ToString();
					Run tRun = new Run(getCADTxtValue(formatTxt));
					tRun.Typography.Variants=System.Windows.FontVariants.Superscript;
					tRun.FontSize = fontHeight*getCADTxtHeight(formatTxt);
					/*tRun.FontFamily = new System.Windows.Media.FontFamily("simplex");*/
					tRun.BaselineAlignment = System.Windows.BaselineAlignment.Superscript;
					txtCollec.Add(tRun);
					prec=sCad.Index+sCad.Length;
				}
				if(prec != txt.Length) txtCollec.Add(txt.Substring(prec,txt.Length-prec));
			}
			else
			{
				txtCollec.Add(txt);
			}

			return txtCollec;
		}
		
		
		public static double getCADTxtHeight(string cadTxt2)
		{
			double height=0.0;
			string sHeight ="";
			Regex rx2 = new Regex(@"\\H(.*?)x;",RegexOptions.Compiled | RegexOptions.IgnoreCase);
			MatchCollection matches2 = rx2.Matches(cadTxt2);
			
			if(matches2.Count > 0)
			{
				sHeight = matches2[0].Groups[1].ToString();
				height = Convert.ToDouble(sHeight.Replace(".",","));
			}
			
			return height;
		}
		
		public static string getCADTxtValue(string cadTxt)
		{
			string val="";
			Regex rx = new Regex(@"\\s(.*?);",RegexOptions.Compiled | RegexOptions.IgnoreCase);
			MatchCollection matches = rx.Matches(cadTxt);
			
			if(matches.Count > 0)
			{
				val = matches[0].Groups[1].ToString();
			}
			return val;
		}
		
		
		public static string getCADFranction(string cadTxt)
		{
			string val="";
			string res=cadTxt;
			Regex rx = new Regex(@"(\d*?)#($|\d*)",RegexOptions.Compiled | RegexOptions.IgnoreCase);
			MatchCollection matches = rx.Matches(res);
			
			if(matches.Count > 0)
			{
				val = GetVulgarFraction(Convert.ToInt32(matches[0].Groups[1].ToString()),Convert.ToInt32(matches[0].Groups[2].ToString()));
				res = res.Replace(matches[0].ToString(),val);
			}
			
			return res;
		}
		
	}
}
