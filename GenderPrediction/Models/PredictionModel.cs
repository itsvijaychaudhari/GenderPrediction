using System.ComponentModel.DataAnnotations;

namespace GenderPrediction.Models
{
    public class PredictionModel
    {
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "Please enter valid Name.")]
        public string Name { get; set; }
        public string Suggestedgender { get; set; }
        public string Currectedgender { get; set; }
        //private string GenderType { get; set; }
/*
        private double[] _probability;
*/

        //public PredictionModel GetPredict(PredictionModel gender)
        //{
        //    //PredictionModel getGender = new PredictionModel();
        //    ApplicationObject appObj = HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject;

        //    string testData = "0";
        //    string lastThree = "";
        //    string lastTwo = "";
        //    string lastOne = "";
        //    string isVowel = "";
        //    string vowels = "aeiou";

        //    List<int> indices = new List<int>();
        //    int a = 0, b = 0, c = 0, d = -1;


        //    //getGender.Name = gender.Name;

        //    if (gender.Name.Length > 3)
        //    {
        //        lastThree = gender.Name.Substring(gender.Name.Length - 3);
        //    }

        //    if (gender.Name.Length > 2)
        //    {
        //        lastTwo = gender.Name.Substring(gender.Name.Length - 2);
        //    }

        //    if (gender.Name.Length > 1)
        //    {
        //        lastOne = gender.Name.Substring(gender.Name.Length - 1);
        //    }

        //    if (appObj != null && appObj.lines.Contains(lastThree))
        //    {
        //        a = Array.IndexOf(appObj.lines, lastThree);
        //        indices.Add(a + 1);
        //    }

        //    if (appObj.lines.Contains(lastTwo))
        //    {
        //        b = Array.IndexOf(appObj.lines, lastTwo);
        //        indices.Add(b + 1);
        //    }

        //    if (appObj.lines.Contains(lastOne))
        //    {
        //        c = Array.IndexOf(appObj.lines, lastOne);
        //        indices.Add(c + 1);
        //    }


        //    if (vowels.Contains(lastOne))
        //    {
        //        d = 1;
        //    }

        //    isVowel = "1942:" + d;

        //    indices.Sort();
        //    foreach (int i in indices)
        //    {
        //        if (i != 0)
        //        {
        //            testData = testData + " " + i + ":" + 1;
        //        }
        //    }

        //    testData = testData + " " + isVowel;

        //    int predict = GetPrediction(testData);

        //    double greater = 0.0;
        //    double dMaleCoefficient = 58;
        //    double dFemaleCoefficient = 63;
        //    if (_probability[0] > _probability[1])
        //    {
        //        greater = Convert.ToInt16(_probability[0] * 100);
        //    }
        //    else
        //    {
        //        greater = Convert.ToInt16(_probability[1] * 100);
        //    }

        //    if (predict == 2)
        //    {
        //        if (greater >= dMaleCoefficient)
        //        {
        //            gender.GenderType = "Male (" + greater + "%)";
        //            gender.Suggestedgender = "M";


        //        }
        //        else
        //        {
        //            gender.GenderType = "Male/Female (" + greater + "%)";
        //            gender.Suggestedgender = "M";


        //        }
        //    }
        //    else
        //    {
        //        if (greater >= dFemaleCoefficient)
        //        {
        //            gender.GenderType = "Female (" + greater + "%)";
        //            gender.Suggestedgender = "F";


        //        }
        //        else
        //        {
        //            gender.GenderType = "Female/Male (" + greater + "%)";
        //            gender.Suggestedgender = "F";
        //        }
        //    }
        //    indices.Clear();
        //    return gender;
        //}

        //public int GetPrediction(string testData)
        //{

        //    double bias = -1.0;
        //    int predict = 0;
        //    // StreamWriter write = new StreamWriter("Prediction.txt");

        //    Problem test = ReadTestData(testData, bias);
        //    // using (model)
        //    {
        //        FeatureNodeArrayCollecion x = test.X;
        //        for (int i = 0; i < test.Length; i++)
        //        {
        //            FeatureNodeArray array = x[i];

        //            predict = (int)LibLinear.Predict((HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject)?.Model, array, out _probability);
        //            // write.WriteLine(predict);
        //        }
        //    }

        //    //write.Close();
        //    return predict;
        //}

        //public Problem ReadTestData(string testData, double bias)
        //{
        //    List<FeatureNode[]> x = new List<FeatureNode[]>();
        //    List<double> y = new List<double>();
        //    //  var lines = File.ReadAllLines(path);

        //    string[] testDataParts = testData.Split(' ');
        //    y.Add(double.Parse(testDataParts[0]));

        //    List<FeatureNode> nodes = new List<FeatureNode>();
        //    for (int i = 1; i <= testDataParts.Length - 1; i++)
        //    {
        //        string[] token = testDataParts[i].Trim().Split(':');
        //        nodes.Add(new FeatureNode
        //        {
        //            Index = int.Parse(token[0]),
        //            Value = int.Parse(token[1])
        //        });
        //    }

        //    x.Add(nodes.ToArray());


        //    return new Problem(x.ToArray(), y.ToArray(), bias);
        //}


    }
}