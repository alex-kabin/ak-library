using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using AK.Essentials.Collections;

namespace AK.Windows.Controls.GraphControl
{
	public class Graph : FrameworkElement
	{
		protected IEnumerator _dataSourceEnumerator;

		protected DispatcherTimer _pollingTimer;
		protected CircularBuffer<object> _history;

		#region PollInterval property
		public static readonly DependencyProperty PollIntervalProperty =
			DependencyProperty.Register("PollInterval", typeof(int), typeof(Graph),
			                            new PropertyMetadata(1000, OnPollIntervalChanged));

		private static void OnPollIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((Graph)d)._pollingTimer.Interval = TimeSpan.FromMilliseconds((int)e.NewValue);
		}

		public int PollInterval
		{
			get { return (int)GetValue(PollIntervalProperty); }
			set { SetValue(PollIntervalProperty, value); }
		}
		#endregion


		#region DataSource property
		public static readonly DependencyProperty DataSourceProperty =
			DependencyProperty.Register("DataSource", typeof(IEnumerable), typeof(Graph),
			                            new PropertyMetadata(null, DataSourcePropertyChangedCallback));

		private static void DataSourcePropertyChangedCallback(DependencyObject dependencyObject,
		                                                      DependencyPropertyChangedEventArgs
		                                                      	dependencyPropertyChangedEventArgs)
		{
			var graph = (Graph)dependencyObject;
			graph._dataSourceEnumerator = dependencyPropertyChangedEventArgs.NewValue != null
			                              	? ((IEnumerable)dependencyPropertyChangedEventArgs.NewValue).GetEnumerator()
			                              	: null;
		}

		public IEnumerable DataSource
		{
			get { return (IEnumerable)GetValue(DataSourceProperty); }
			set { SetValue(DataSourceProperty, value); }
		}
		#endregion


		#region Visualiser property
		public static readonly DependencyProperty VisualiserProperty =
			DependencyProperty.Register("Visualiser", typeof(IGraphVisualiser), typeof(Graph),
			                            new FrameworkPropertyMetadata(default(IGraphVisualiser),
			                                                          FrameworkPropertyMetadataOptions.AffectsRender));


		public IGraphVisualiser Visualiser
		{
			get { return (IGraphVisualiser)GetValue(VisualiserProperty); }
			set { SetValue(VisualiserProperty, value); }
		}
		#endregion


		public Graph()
		{
			ClipToBounds = true;
			
			_pollingTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(PollInterval) };
			_pollingTimer.Tick += delegate { PollDataSourceForNextValue(); };

			_history = new CircularBuffer<object>(1000);
		}

		private void PollDataSourceForNextValue()
		{
			if (_dataSourceEnumerator != null)
			{
				if(_dataSourceEnumerator.MoveNext())
					_history.Add(_dataSourceEnumerator.Current);
				else
				{
					_pollingTimer.Stop();
				}
				InvalidateVisual();
			}
		}

		protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
		{
			if (Visualiser != null && _history.Count > 0)
			{
				var enumerator = _history.GetBackwardEnumerator();
				Visualiser.Visualise(drawingContext, enumerator, RenderSize);
			}
		}

		public void Start()
		{
			_pollingTimer.Start();
		}

		public void Stop()
		{
			_pollingTimer.Stop();
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			return availableSize;
		}
	}
}