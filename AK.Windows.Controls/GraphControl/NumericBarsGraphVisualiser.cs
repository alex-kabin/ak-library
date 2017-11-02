using System;
using System.Collections;
using System.Windows;
using System.Windows.Media;

namespace AK.Windows.Controls.GraphControl
{
	public sealed class NumericBarsGraphVisualiser : NumericGraphVisualiserBase
	{
		public Brush PositiveBrush { get; set; }
		public Pen PositivePen { get; set; }
		public Brush NegativeBrush { get; set; }
		public Pen NegativePen { get; set; }

		public NumericBarsGraphVisualiser()
		{
			PositiveBrush = new SolidColorBrush(Colors.Green);
			PositivePen = new Pen(PositiveBrush, 1);
			NegativeBrush = new SolidColorBrush(Colors.Red);
			NegativePen = new Pen(NegativeBrush, 1);
		}

		protected override void RenderValues(DrawingContext dc, Matrix matrix, double[] values, int count)
		{
			double x = 0.0;
			int i = 0;
			for (; i < count; i++, x += 1.0)
			{
				double value = values[i];

				Rect bar = new Rect(matrix.Transform(new Point(x, 0)), matrix.Transform(new Point(x+1.0, value)));
				
				Brush brush;
				Pen pen;
				if (value >= 0.0)
				{
					brush = PositiveBrush;
					pen = PositivePen;
				}
				else
				{
					brush = NegativeBrush;
					pen = NegativePen;
				}
				dc.DrawRectangle(brush, pen, bar);
			}
		}
	}
}