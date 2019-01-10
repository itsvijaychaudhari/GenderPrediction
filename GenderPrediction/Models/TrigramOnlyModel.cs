using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LibLinearDotNet;

namespace GenderPrediction.Models
{
    public class TrigramOnlyModel
    {
        string name = "";
        ApplicationObject appObj = new ApplicationObject();
        string[] new_lines;

        public TrigramOnlyModel(string name)
        {
            this.name = name;
            appObj = HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject;
            new_lines = appObj.lines;
        }

        public string getTestData_trigram_only()
        {
            bool skip = false;
            List<int> indices = new List<int>();
            int a = 0;
            List<string> features = new List<string>();
            string feature = "";
            string suffix = "";
            string test_data = "0";
            var char_array = name.ToCharArray();
            Array.Reverse(char_array);
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                    suffix = "_1";
                else if (i == 1)
                    suffix = "_2";
                else if (i == 2)
                    suffix = "_3";

                for (int j = i; j < i + 3; j++)
                {
                    var result = feature.Split('_');
                    if (j == char_array.Length)
                        break;
                    feature = char_array[j].ToString() + result[0];
                }
                feature = feature + suffix;
                features.Add(feature);
                feature = "";
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
                    // write1.WriteLine(item.ToString() + "\tTrigram-Only");
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
            // write1.Close();
            return test_data;

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
                //if (!MainWindow.isLoading)
                //    ((MainWindow)System.Windows.Application.Current.MainWindow).outputListBox.Items.Add("Can't perdict");
                return "Can't predict";
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
                if (greater >= dMaleCoefficient * 100)
                {
                    return "M " + greater;
                }

                else
                {
                    //return "MF " + greater;
                    return "Both " + greater;
                }
            }

            else
            {
                if (greater >= dFemaleCoefficient * 100)
                {
                    return "F " + greater;
                }
                else
                {
                    //return "FM " + greater;
                    return "Both " + greater;
                }
            }
            //return "";
            // write1.Close();
        }
    }
}