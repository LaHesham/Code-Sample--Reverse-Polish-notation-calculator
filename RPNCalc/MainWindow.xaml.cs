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
            try
            {
                txtAnswer.Text = rpn.CalculateExpression().ToString();
            }
            catch (System.Exception ex)
            {

                if (ex is System.InvalidCastException)
                {
                    txtAnswer.Text = ex.Message;
                }
                if (ex is System.InvalidOperationException)
                {
                    txtAnswer.Text = ex.Message;
                }
                if (ex is System.DivideByZeroException)
                {
                    txtAnswer.Text = ex.Message;
                }
            }
        }
    }
}
