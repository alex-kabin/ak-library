using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace AK.Windows.Controls.GraphControl
{
	public class SimpleNumericGraphVisualiser : NumericGraphVisualiserBase
	{
		public INumericValuesDrawer Drawer { get; set; }

		private double[] _values;
		private int _count;

		protected override void PrepareValues(IEnumerator enumerator, int maxCount, out double min, out double max)
		{
			_values = new double[maxCount];
			
			min = Double.MaxValue;
			max = Double.MinValue;

			var index = 0;
			while (enumerator.MoveNext() && index < maxCount)
			{
				double value = Convert.ToDouble(enumerator.Current);
				min = Math.Min(min, value);
				max = Math.Max(max, value);
				_values[index++] = value;
			}

			_count = index;
		}

		protected override void RenderValues(DrawingContext dc, Transform transform)
		{
			var drawing = Drawer.Draw(transform, _values, _count);
			dc.DrawDrawing(drawing);
		}
	}
}