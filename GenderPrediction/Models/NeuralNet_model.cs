using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GenderPrediction.Models
{
    public class NeuralNet_model
    {
        string name;
        Dictionary<string, Dictionary<string, string>> dict = new Dictionary<string, Dictionary<string, string>>();
        FileInfo[] files;
        float[] input_features;
        double[] input_features_double;
        List<string> all_features = new List<string>();
        Dictionary<int, ArrayList> dictionary = new Dictionary<int, ArrayList>();
        int cnt = 1;


        public NeuralNet_model(string name)
        {
            this.name = name;
           
        }
       
        //calculate features using NN
        public void calculateFeaturesForNN()
        {
            //name = textBox.Text;
            List<string> features = new List<string>();
            string feature = "";
            var char_array = name.ToCharArray();
            Array.Reverse(char_array);
            float feature_value = 0.0f;
            int index = 0;
            Array.Sort((HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject).files, (f1, f2) => f2.Name.CompareTo(f1.Name));
            string filename = "";
            string loadfile = "";
            Dictionary<string, string> temp = null;
            input_features = new float[10];

            for (int i = 0; i < 2; i++)
            {
                #region try
                if (i == 0)
                {
                    for (int t = i; t < i + 3; t++)
                    {
                        if (t == 0)
                            filename = "Uni";
                        else if (t == 1)
                            filename = "Bi";
                        else if (t == 2)
                            filename = "Tri0";
                        for (int k = 0; k < 2; k++)
                        {
                            if (k == 0)
                                loadfile = filename + "_M.txt";
                            else if (k == 1)
                                loadfile = filename + "_F.txt";
                            for (int j = 0; j < t + 1; j++)
                            {

                                if (j == char_array.Length)
                                {
                                    if (feature.Length == 1)
                                        feature = "**" + feature;
                                    else if (feature.Length == 2)
                                        feature = "*" + feature;
                                    break;
                                }

                                loadfile = string.Join("", (HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject).files.Where(f => f.Name.Equals(loadfile)).ToList());
                                temp = (HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject).Get(loadfile);
                                feature = char_array[j] + feature;
                            }
                            all_features.Add(feature);
                            if (temp.ContainsKey(feature))
                            {
                                feature_value = float.Parse(temp[feature]);
                                scaleFeature(ref feature_value);
                                input_features[index] = feature_value;
                            }
                            else
                            {
                                feature_value = 0.0f;
                                scaleFeature(ref feature_value);
                                input_features[index] = feature_value;
                            }

                            feature = "";
                            index++;

                        }

                    }

                }
                else
                {
                    for (int t = i; t < i + 2; t++)
                    {
                        if (t == 1)
                            filename = "Tri1";
                        else if (t == 2)
                            filename = "Tri2";
                        for (int k = 0; k < 2; k++)
                        {
                            if (k == 0)
                                loadfile = filename + "_M.txt";
                            else if (k == 1)
                                loadfile = filename + "_F.txt";
                            for (int j = t; j < t + 3; j++)
                            {

                                if (j == char_array.Length)
                                {
                                    if (feature.Length == 1)
                                        feature = "**" + feature;
                                    else if (feature.Length == 2)
                                        feature = "*" + feature;
                                    break;
                                }

                                loadfile = string.Join("", (HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject).files.Where(f => f.Name.Equals(loadfile)).ToList());
                                temp = (HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject).Get(loadfile);
                                feature = char_array[j] + feature;

                            }
                            all_features.Add(feature);
                            if (temp.ContainsKey(feature))
                            {
                                feature_value = float.Parse(temp[feature]);
                                scaleFeature(ref feature_value);
                                input_features[index] = feature_value;
                            }
                            else
                            {
                                feature_value = 0.0f;
                                scaleFeature(ref feature_value);
                                input_features[index] = feature_value;
                            }

                            feature = "";
                            index++;
                        }
                    }
                }
                #endregion
            }
        }

        public void scaleFeature(ref float index)
        {
            int upper = 1;
            int lower = -1;
            ArrayList l = (HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject).paramDictionary[cnt];
            l.Sort();
            float min = (float)Convert.ToDouble((HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject).paramDictionary[cnt][0]);
            float max = (float)Convert.ToDouble((HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject).paramDictionary[cnt][1]);

            //index = (index - min) / (max - min);
            if (index == min)
            {
                min = lower;
                // return;

            }
            else if (index == max)
            {
                max = upper;
                //  return;
            }
            //if(index != upper && index !=lower)
            index = lower + (((upper - lower) * (index - min)) / (max - min));

            cnt++;

        }

        //perdict gender using  NN

        public string predictFusionSVM_NN(FANNCSharp.Float.NeuralNet net_csharp)
        {
            var calc_out_float = net_csharp.Run(input_features);
            //double dMaleCoefficient = 0.42;
            //double dFemaleCoefficient = 0.58;
            double dMaleCoefficient = 0.4625;
            double dFemaleCoefficient = 0.5375;
            calc_out_float[0] = calc_out_float[0] / (calc_out_float[0] + calc_out_float[1]);
            calc_out_float[1] = calc_out_float[1] / (calc_out_float[0] + calc_out_float[1]);
            if (calc_out_float[0] > calc_out_float[1])
            {
                if (calc_out_float[0] >= dMaleCoefficient)
                {
                    return "M " + calc_out_float[0];
                }

                else
                {
                    return "Both " + calc_out_float[0];
                }

            }

            else if (calc_out_float[1] > calc_out_float[0])
            {
                if (calc_out_float[1] >= dFemaleCoefficient)
                {
                    return "F " + calc_out_float[1];
                }
                else
                {
                    return "Both " + calc_out_float[1];
                }

            }
            return "";

        }


    }
}