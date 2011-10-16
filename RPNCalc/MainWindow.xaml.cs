using System.Windows;
using RPNCalc.rpn;

namespace RPNCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void txtlInput_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtlInput.Text == "Expression") txtlInput.Text = "";
        }

        private void txtAnswer_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtAnswer.Text == "Answer") txtAnswer.Text = "";
        }

        private void btnEvaluate_Click(object sender, RoutedEventArgs e)
        {
            RPN rpn = new RPN(txtlInput.Text);
            txtAnswer.Text = rpn.CalculateExpression().ToString();
        }
    }
}
