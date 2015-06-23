using MahApps.Metro.Controls;
using MI.Terminal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mission_Impossible {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : MetroWindow {
		public MainWindow() {
			InitializeComponent();
			this.DataContext = views;
			loadTerminal();
		}

		private ObservableCollection<FrameworkElement> views = new ObservableCollection<FrameworkElement>();

		private void loadTerminal() {
			UserControl1 u = new UserControl1();
			views.Add(u);
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e) {
			Terminal t = new Terminal();
			t.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
			t.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
			views.Add(t);
		}
	}
}
