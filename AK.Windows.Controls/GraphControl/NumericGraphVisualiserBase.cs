using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using AK.Essentials;

namespace AK.Windows.Controls.GraphControl
{
	public abstract class NumericGraphVisualiserBase : IGraphVisualiser
	{
		public double TimeStep { get; set; }
		public Pen ZeroLinePen { get; set; }
		public bool ShowZeroLine { get; set; }
		
		public bool ShowYGuides { get; set; }
		public double YGuideStep { get; set; }
		public bool ShowYGuidesLabels { get; set; }
		
		public bool ShowHorizontalGuides { get; set; }
		public double HorizontalGuideStep { get; set; }

		public Pen GuidePen { get; set; }

		public Range<double> Range { get; set; }

		protected NumericGraphVisualiserBase()
		{
			Range = Range<double>.Empty;

			TimeStep = 5;
			ZeroLinePen = new Pen(Brushes.Black, 1);
			ShowZeroLine = true;

			GuidePen = new Pen(Brushes.LightPink, 1);
		}

		protected int GetVisibleValuesCount(double width)
		{
			return (int)Math.Ceiling(width / TimeStep)+1;
		}
		
		protected double CalculateScale(Range<double> range, Size size)
		{
			return size.Height / Math.Abs(range.Max - range.Min);
		}

		protected Transform PrepareTransform(double scale, Range<double> range)
		{
			double zeroLineY = scale * range.Max;
			return new MatrixTransform(TimeStep, 0, 0, -scale, 0, zeroLineY);
		}

		protected abstract void PrepareValues(IEnumerator enumerator, int maxCount, out double min, out double max);

		protected abstract void RenderValues(DrawingContext dc, Transform transform);

		#region Implementation of IGraphVisualiser
		public void Visualise(DrawingContext dc, IEnumerator valuesEnumerator, Size size)
		{
			int maxCount = GetVisibleValuesCount(size.Width);
			
			double min, max;
			PrepareValues(valuesEnumerator, maxCount, out min, out max);

			if (this.Range != Range<double>.Empty)
			{
				min = this.Range.Min;
				max = this.Range.Max;
			}
			var Range = new Range<double>(min, max);
			
			double scale = CalculateScale(Range, size);
			var transform = PrepareTransform(scale, Range);
			
			RenderValues(dc, transform);
			
			//if (ShowZeroLine)
			//    dc.DrawLine(ZeroLinePen,
			//                new Point(0, zeroLineY),
			//                new Point(size.Width, zeroLineY));

			if(ShowYGuides)
				DrawYGuides(dc, scale, Range, size);
		}
		#endregion
		
		protected virtual void DrawYGuides(DrawingContext dc, double scale, Range<double> range, Size size)
		{
			double value = Math.Truncate(range.Min / YGuideStep) * YGuideStep;
			while (value <= range.Max)
			{
				DrawValueLine(dc, value, scale, range, size);
				value += YGuideStep;
			}
		}

		protected void DrawValueLine(DrawingContext dc, double value, double scale, Range<double> range, Size size)
		{
			if (range.Contains(value))
			{
				var y = Math.Ceiling((range.Max - value) * scale) - GuidePen.Thickness / 2;

				//dc.PushGuidelineSet(new GuidelineSet(new double[0], new[] { y + GuidePen.Thickness / 2 }));
				dc.DrawLine(GuidePen, new Point(0, y), new Point(size.Width, y));
				//dc.Pop();

				dc.DrawText(
					new FormattedText(value.ToString("F"), CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface("Tahoma"),
									  9, Brushes.Black), new Point(0, y));

				
			}
		}

		
	}
}