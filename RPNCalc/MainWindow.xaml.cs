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
            rpn.Parse();
            txtAnswer.Text = rpn.Test();
        }
    }
}
