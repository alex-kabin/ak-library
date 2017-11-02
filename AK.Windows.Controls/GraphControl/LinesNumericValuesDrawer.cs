using System.Windows;
using System.Windows.Media;

namespace AK.Windows.Controls.GraphControl
{
	public class LinesNumericValuesDrawer : INumericValuesDrawer
	{
		public Pen LinePen { get; set; }

		public LinesNumericValuesDrawer()
		{
			LinePen = new Pen(Brushes.Red, 2);
		}

		public void Render(DrawingContext dc, Matrix matrix, double[] values, int count)
		{
			double x = 0.0;
			int i = 0;
			for (; i < count; i++, x += 1.0)
			{
				if(i + 1 < count)
				{
					double value0 = values[i];
					double value1 = values[i+1];

					Point p0 = matrix.TransformPoint(x, value0);
					Point p1 = matrix.TransformPoint(x + 1.0, value1);
					dc.DrawLine(LinePen, p0, p1);
				}
			}
		}

		public Drawing Draw(Transform transform, double[] values, int count)
		{
			var geometry = new StreamGeometry() { Transform = transform };
			using(var ctx = geometry.Open())
			{
				ctx.BeginFigure(new Point(0, values[0]), false, false);
				
				double x = 1.0;
				for (int i = 1; i < count; i++, x += 1.0)
				{
					ctx.LineTo(new Point(x, values[i]), true, false);
				}
			}

			return new GeometryDrawing() { Pen = LinePen, Geometry = geometry };
		}
	}
}