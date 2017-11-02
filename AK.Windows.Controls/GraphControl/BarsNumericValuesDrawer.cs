using System.Windows;
using System.Windows.Media;

namespace AK.Windows.Controls.GraphControl
{
	public class BarsNumericValuesDrawer : INumericValuesDrawer
	{
		public Brush PositiveBrush { get; set; }
		public Brush NegativeBrush { get; set; }

		public BarsNumericValuesDrawer()
		{
			PositiveBrush = new SolidColorBrush(Colors.Green);
			NegativeBrush = new SolidColorBrush(Colors.Red);
		}

		public Drawing Draw(Transform transform, double[] values, int count)
		{
			var positiveGeometry = new StreamGeometry() { Transform = transform };
			var negativeGeometry = new StreamGeometry() { Transform = transform };
			
			using (var positiveCtx = positiveGeometry.Open())
			using (var negativeCtx = negativeGeometry.Open())
			{
				double x = 0.0;
				for (int i = 0; i < count; i++, x += 1.0)
				{
					double value = values[i];
					StreamGeometryContext ctx = value >= 0 ? positiveCtx : negativeCtx;
					
					ctx.BeginFigure(new Point(x, 0), true, true);
					ctx.LineTo(new Point(x, value), false, false);
					ctx.LineTo(new Point(x+1.0, value), false, false);
					ctx.LineTo(new Point(x+1.0, 0), false, false);
				}
			}

			var drawingGroup = new DrawingGroup();
			drawingGroup.Children.Add(new GeometryDrawing() { Brush = PositiveBrush, Geometry = positiveGeometry });
			drawingGroup.Children.Add(new GeometryDrawing() { Brush = NegativeBrush, Geometry = negativeGeometry });
			return drawingGroup;
		}
	}
}