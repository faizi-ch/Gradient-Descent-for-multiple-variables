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
using DevExpress.Xpf.Core;

namespace Gradient_Descent_for_multiple_variables
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DXWindow
    {
        private int n = 0, m = 0;
        private double[,] xFeatures;
        private double alpha = 0;
        private string f = "";
        private int x = 0, y = 0, t = 0, total = 0, noOfFeatures=0, features=0;
        public MainWindow()
        {
            InitializeComponent();
            XList.Items.Add("x(1)");
        }

        private void XTextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (XTextEdit.Text != "")
                {
                    XList.Items[m+1] += f+XTextEdit.Text;

                    xFeatures[n, m] = Convert.ToDouble(XTextEdit.Text);
                    m++;
                    if (total == m && noOfFeatures == n + 1)
                    {
                        XTextEdit.IsEnabled = false;
                        YTextEdit.Focus();
                    }
                    if (m==total)
                    {
                        m = 0;
                        n++;

                        if(n!=noOfFeatures)
                            XList.Items[0] += string.Format("\tx({0})", n + 1);

                        if (n == 1)
                            f += "\t";
                    }
                }
                XTextEdit.Text = "";
            }
        }

        private void YTextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (YTextEdit.Text != "")
                {
                    YList.Items.Add(YTextEdit.Text);
                }
                y++;
                YTextEdit.Text = "";
            }
            if (total == y)
            {
                YTextEdit.IsEnabled = false;
            }
        }

        private void ThetaTextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ThetaTextEdit.Text != "")
                {
                    ThetasList.Items.Add(ThetaTextEdit.Text);
                }
                t++;
                ThetaTextEdit.Text = "";
                ThetaLabel.Content = string.Format("θ({0}): ", t);
            }
            if (noOfFeatures == t)
            {
                ThetaTextEdit.IsEnabled = false;
                XTextEdit.Focus();
            }
        }

        private void AlphaTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (AlphaTextEdit.Text != "")
            {
                alpha = Convert.ToDouble(AlphaTextEdit.Text);
            }
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            AlphaTextEdit.Focus();
        }

        private void TotalTextEdit_EditValueChanging(object sender, DevExpress.Xpf.Editors.EditValueChangingEventArgs e)
        {
            if (TotalTextEdit.Text != "")
            {
                total = Convert.ToInt32(TotalTextEdit.Text);
            }
            if (total>0)
            {
                XTextEdit.IsEnabled = true;
                YTextEdit.IsEnabled = true;
                ThetaTextEdit.IsEnabled = true;
            }
        }

        private void TotalTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (TotalTextEdit.Text != "")
            {
                total = Convert.ToInt32(TotalTextEdit.Text);
                for (int i = 0; i < total; i++)
                {
                    XList.Items.Add("");
                }
                if (NTextEdit.Text!= "")
                {
                    xFeatures = new double[noOfFeatures, total];
                }
            }
            if (total > 0)
            {
                XTextEdit.IsEnabled = true;
                YTextEdit.IsEnabled = true;
                ThetaTextEdit.IsEnabled = true;
            }
            if (total == 0)
            {
                XTextEdit.IsEnabled = false;
                YTextEdit.IsEnabled = false;
                ThetaTextEdit.IsEnabled = false;
            }
        }

        private void NTextEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (NTextEdit.Text!="")
            {
                noOfFeatures = Convert.ToInt32(NTextEdit.Text);

                if (TotalTextEdit.Text != "")
                {
                    xFeatures = new double[noOfFeatures, total];
                }
            }
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            double[] oldThetas;
            double[] updatedThetas = new double[ThetasList.Items.Count];
            if (NTextEdit.Text != "" && TotalTextEdit.Text != "" && XList.Items.Count > 0 && YList.Items.Count > 0)
            {
                oldThetas = new double[ThetasList.Items.Count];
                

                //newThetas[0] = Convert.ToDouble(ThetasList.Items[0]) - (alpha * (CalculateCostFunctionForTheta0(i)));

                for (int i = 0; i < 10; i++)
                {
                    UpdatedThetasList.Items.Add("");

                    for (int j = 0; j < ThetasList.Items.Count; j++)
                    {
                        if (j == 0)
                            updatedThetas[j] = oldThetas[j] - (alpha * (CalculateCostFunctionForTheta0(oldThetas)));
                        else
                            updatedThetas[j] = oldThetas[j] - (alpha * (CalculateCostFunctionForTheta1(oldThetas, j)));

                        /*if (t0 < 0 || t1 < 0)
                        {
                            break;
                        }*/
                        if (j == 0)
                            UpdatedThetasList.Items[i] += string.Format("{0:F4}", updatedThetas[j]);
                        else
                            UpdatedThetasList.Items[i] += string.Format("\t{0:F4}", updatedThetas[j]);

                        oldThetas[j] = updatedThetas[j];
                    }
                    
                }
            }
        }
        private double CalculateCostFunctionForTheta0(double[] thetas)
        {
            double h = 0, j = 0;
            int i = 0;
            if (ThetasList.Items.Count > 0 && XList.Items.Count > 0 && YList.Items.Count > 0)
            {
                //h += thetas[0];
                for (int n = 0; n < noOfFeatures; n++)
                {
                    for (int m = 0; m < total; m++)
                    {
                        h += thetas[n] * xFeatures[n, m];
                    }
                }

                foreach (var y in YList.Items)
                {
                    j += h - Convert.ToDouble(y);
                    //MessageBox.Show(j.ToString());
                    CalculatedList.Items.Add(j.ToString());
                    i++;
                }
                
                
                //MessageBox.Show(j.ToString());
                //double m = (2 * total);
                double d = 1 / (double)total;
                j *= d;

                //CalculatedLabel.Content += j.ToString();
            }
   
            return j;
        }

        private double CalculateCostFunctionForTheta1(double[] thetas, int f)
        {
            double h = 0, j = 0;
            int i = 0;
            if (ThetasList.Items.Count > 0 && XList.Items.Count > 0 && YList.Items.Count > 0)
            {
                //h += thetas[0];
                for (int n = 0; n < noOfFeatures; n++)
                {
                    for (int m = 0; m < total; m++)
                    {
                        h += thetas[n] * xFeatures[n, m];
                    }
                }

                foreach (var y in YList.Items)
                {
                    j += h - Convert.ToDouble(y);
                    j *= xFeatures[f, i];
                    CalculatedList.Items.Add(j.ToString());
                    i++;
                }


                //MessageBox.Show(j.ToString());
                //double m = (2 * total);
                double d = 1 / (double)total;
                j *= d;

                //CalculatedLabel.Content += j.ToString();
            }

            return j;
        }
    }
}
