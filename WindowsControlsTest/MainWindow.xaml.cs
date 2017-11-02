using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AK.Essentials;
using AK.Essentials.Collections;
using AK.Windows.Controls.GraphControl;

namespace WindowsControlsTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			var viz = new MultiNumericGraphVisualiser()
			                   	{
			                   		ShowZeroLine = false,
			                   		TimeStep = 10,
			                   		Range = new Range<double>(-2.0, 2.0),
			                   		ShowYGuides = true,
			                   		YGuideStep = 0.5
			                   	};
			viz.Drawers.Add(new LinesNumericValuesDrawer() { LinePen = new Pen(Brushes.Red, 1)});
			viz.Drawers.Add(new LinesNumericValuesDrawer() { LinePen = new Pen(Brushes.Blue, 1) });

			graph.Visualiser = viz;

			graph.DataSource = Enumerables.MergeAll(Numeric.Randoms(-1.0, 1.0), Numeric.Randoms(-1.5, 1.5));
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			graph.Start();
		}

	}
}
