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

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ViewModel.MainViewModel();
        }
        private void ItemsControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
        {
            if (DataContext is ViewModel.MainViewModel viewModel)
            {
                viewModel.UpdateCanvasSize(e.NewSize.Width, e.NewSize.Height);
            }
        }
    }
}