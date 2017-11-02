using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace AK.Windows.Controls.GraphControl
{
	public class MultiNumericGraphVisualiser : NumericGraphVisualiserBase
	{
		public List<INumericValuesDrawer> Drawers { get; private set; }

		public MultiNumericGraphVisualiser()
		{
			Drawers = new List<INumericValuesDrawer>();
		}

		private double[][] _values;
		private int _count;

		#region Overrides of NumericGraphVisualiserBase
		protected override void PrepareValues(IEnumerator enumerator, int maxCount, out double min, out double max)
		{
			_values = null;

			min = Double.MaxValue;
			max = Double.MinValue;

			var index = 0;
			while (enumerator.MoveNext() && index < maxCount)
			{
				var currentArray = enumerator.Current as double[];
				if (currentArray != null)
				{
					if (_values == null)
					{
						_values = new double[currentArray.Length][];
						for (int i = 0; i < currentArray.Length; i++)
							_values[i] = new double[maxCount];
					}
					
					for (int i = 0; i < currentArray.Length; i++)
					{
						var currentValue = currentArray[i];
						min = Math.Min(min, currentValue);
						max = Math.Max(max, currentValue);

						_values[i][index] = currentValue;
					}
					index++;
				}
			}

			_count = index;
		}

		protected override void RenderValues(DrawingContext dc, Transform transform)
		{
			for (int i = 0; i < Math.Min(_values.Length, Drawers.Count); i++)
			{
				var values = _values[i];
				var drawer = Drawers[i];
				
				var drawing = drawer.Draw(transform, values, _count);
				dc.DrawDrawing(drawing);
			}
		}
		#endregion
	}
}