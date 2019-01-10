using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using LibLinearDotNet;

namespace GenderPrediction.Models
{
    public class Shifted_trigram_model
    {
        
        string name;
        private string[] new_lines;
        ApplicationObject appObj = new ApplicationObject();
       

        public Shifted_trigram_model(string name)
        {
            this.name = name;
            appObj = HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject;
            new_lines= appObj.lines;
        }

        

        public string getTestData()
        {
            int a = 0;
            bool skip = false;
            string feature_four = "";
            string feature_five = "";
            string test_data = "0";

            List<int> indices = new List<int>();


            //string[] features=new string[5];
            List<string> features = new List<string>();
            string feature = "";
            var char_array = name.ToCharArray();
            Array.Reverse(char_array);
            for (int i = 0; i < 3; i++)
            {
                string suffix = "";
                if (i == 0)
                {
                    suffix = "_1";
                    for (int j = i; j < i + 3; j++)
                    {
                        var result = feature.Split('_');
                        if (j == char_array.Length)
                            break;
                        feature = char_array[j].ToString() + result[0] + suffix;
                        // Console.WriteLine(feature);
                        features.Add(feature);
                    }

                    feature = "";
                }
                else
                {
                    if (i == 1)
                        suffix = "_2";
                    else if (i == 1)
                        suffix = "_1";
                    else if (i == 2)
                        suffix = "_3";
                    for (int j = i; j < i + 3; j++)
                    {
                        var result = feature.Split('_');
                        if (j == char_array.Length)
                            break;
                        feature = char_array[j].ToString() + result[0] + suffix;
                        // Console.WriteLine(feature);

                    }
                    features.Add(feature);
                    feature = "";
                }

            }

            foreach (var item in features)
            {
                if (new_lines.Contains(item.ToString()))
                {
                    a = Array.IndexOf(new_lines, item.ToString());
                    indices.Add(a + 1);
                }
                else
                {
                    // write1.WriteLine(item.ToString() + "\tShifted");
                    skip = true;
                }
            }
            //if (vowels.Contains(last_one))
            //    d = 1;

            //   isVowel = "1942:"+d;

            indices.Sort();
            //if(skip==false)
            {
                foreach (int i in indices)
                {
                    if (i != 0)
                        test_data = test_data + " " + i + ":" + 1;
                }
            }
           
            return test_data;
        }

        /*private Problem ReadTestData(string test_data, double bias)
        {
            var x = new List<FeatureNode[]>();
            var y = new List<double>();
            //  var lines = File.ReadAllLines(path);

            var test_dataParts = test_data.Split(' ');
            y.Add(double.Parse(test_dataParts[0]));

            var nodes = new List<FeatureNode>();
            for (var i = 1; i <= test_dataParts.Length - 1; i++)
            {
                var token = test_dataParts[i].Trim().Split(':');
                nodes.Add(new FeatureNode
                {
                    Index = int.Parse(token[0]),
                    Value = int.Parse(token[1])
                });
            }

            x.Add(nodes.ToArray());


            return new Problem(x.ToArray(), y.ToArray(), bias);
        } */



        /*public void predict(Model model, string test_data)
        {
            Problem p = null;
            var test = p;
            double bias = -1.0;
            var predict = 0;
            double[] prob = null;
            if (test_data != "0")
                test = ReadTestData(test_data, bias);
            else
            {
                if (!MainWindow.isLoading)
                    ((MainWindow)System.Windows.Application.Current.MainWindow).outputListBox.Items.Add("Can't perdict");
                else
                    MainWindow.write.Write("\tCP");
                return;
            }


            var x = test.X;
            for (var i = 0; i < test.Length; i++)
            {
                var array = x[i];
                predict = (int)LibLinear.Predict(model, array, out prob);

            }

            double greater = 0.0;
            double dMaleCoefficient = 58;
            double dFemaleCoefficient = 63;

            if (prob[0] > prob[1])
                greater = Convert.ToInt16(prob[0] * 100);
            else
                greater = Convert.ToInt16(prob[1] * 100);

            if (predict == 2)
            {
                if (greater >= dMaleCoefficient)
                {
                    if (!MainWindow.isLoading)
                        ((MainWindow)System.Windows.Application.Current.MainWindow).outputListBox.Items.Add("Male (" + greater + "%)");
                    else
                        MainWindow.write.Write("\tM");

                }

                else
                {
                    if (!MainWindow.isLoading)
                        ((MainWindow)System.Windows.Application.Current.MainWindow).outputListBox.Items.Add("Male/Female (" + greater + "%)");
                    else
                        MainWindow.write.Write("\tMF");
                }
            }

            else
            {
                if (greater >= dFemaleCoefficient)
                {
                    if (!MainWindow.isLoading)
                        ((MainWindow)System.Windows.Application.Current.MainWindow).outputListBox.Items.Add("Female (" + greater + "%)");
                    else
                        MainWindow.write.Write("\tF");
                }
                else
                {
                    if (!MainWindow.isLoading)
                        ((MainWindow)System.Windows.Application.Current.MainWindow).outputListBox.Items.Add("Female/Male (" + greater + "%)");
                    else
                        MainWindow.write.Write("\tFM");

                }
            }

            // write1.Close();
        }*/


        public string predictFusion(Model model, string test_data, double dMaleCoefficient, double dFemaleCoefficient)
        {
            Problem p = null;
            var test = p;
            double bias = -1.0;
            var predict = 0;
            double[] prob = null;
            if (test_data != "0")
                test = ReadTestData(test_data, bias);
            else
            {
                //if (!isLoading)
                //    ((MainWindow)System.Windows.Application.Current.MainWindow).outputListBox.Items.Add("Can't perdict");
                return "";
            }



            var x = test.X;
            for (var i = 0; i < test.Length; i++)
            {
                var array = x[i];
                predict = (int)LibLinear.Predict(model, array, out prob);
            }

            double greater = 0.0;
            //double dMaleCoefficient = 58;
            //double dFemaleCoefficient = 63;
            if (prob[0] > prob[1])
                greater = Convert.ToInt16(prob[0] * 100);
            else
                greater = Convert.ToInt16(prob[1] * 100);

            if (predict == 2)
            {
                if (greater >= dMaleCoefficient)
                {
                    return "M " + greater;
                }

                else
                {
                    return "MF " + greater;
                }
            }

            else
            {
                if (greater >= dFemaleCoefficient)
                {
                    return "F " + greater;
                }
                else
                {
                    return "FM " + greater;
                }
            }
            //return "";
            // write1.Close();
        }

        private Problem ReadTestData(string test_data, double bias)
        {
            var x = new List<FeatureNode[]>();
            var y = new List<double>();
            //  var lines = File.ReadAllLines(path);

            var test_dataParts = test_data.Split(' ');
            y.Add(double.Parse(test_dataParts[0]));

            var nodes = new List<FeatureNode>();
            for (var i = 1; i <= test_dataParts.Length - 1; i++)
            {
                var token = test_dataParts[i].Trim().Split(':');
                nodes.Add(new FeatureNode
                {
                    Index = int.Parse(token[0]),
                    Value = int.Parse(token[1])
                });
            }

            x.Add(nodes.ToArray());


            return new Problem(x.ToArray(), y.ToArray(), bias);
        }

    }
}