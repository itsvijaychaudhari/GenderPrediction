using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using GenderPrediction.Models;
using LibLinearDotNet;
using Model = LibLinearDotNet.Model;

namespace GenderPrediction
{
    public class MvcApplication : System.Web.HttpApplication
    {

        ApplicationObject appObj = new ApplicationObject();
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LoadModelSVM();
            LoadModelNN();
        }

        private void LoadModelSVM()
        {
            

            //string path = HttpContext.Current.Server.MapPath("~/PredictionModel/gram_features.txt");
            //StreamReader read = new StreamReader(path);
            ////StreamReader read = new StreamReader("gram_features.txt");
            //appObj.lines = new string[1945];
            //string line = read.ReadToEnd();
            //var lines = line.Split('\n');
            //for (int i = 0; i < lines.Count(); i++)
            //{
            //    appObj.lines[i] = lines[i].Trim();
            //}
            //read.Close();
            //string path2 = HttpContext.Current.Server.MapPath("~/PredictionModel/gender_detection_train.model");
            //appObj.Model = Model.Load(path2);


            string path = HttpContext.Current.Server.MapPath("~/PredictionModel/only_tri_grams.txt");
            StreamReader read = new StreamReader(path);
            //StreamReader read = new StreamReader("gram_features.txt");
            appObj.lines = new string[5679];
            string line = read.ReadToEnd();
            var lines = line.Split('\n');
            for (int i = 0; i < lines.Count(); i++)
            {
                appObj.lines[i] = lines[i].Trim();
            }
            read.Close();
            string path2 = HttpContext.Current.Server.MapPath("~/PredictionModel/LR_Only_trigram_143.model");
            appObj.Model = Model.Load(path2);

            //appObj.Model = Model.Load("gender_detection_train.model");
            HttpContext.Current.Application["GobleObjectSVM"] = appObj;
        }

        private void LoadModelNN()
        {
            string NeuralNetModel_path = HttpContext.Current.Server.MapPath("~/PredictionModel/NN_10_prob_143.net");
            appObj.NeuralNetModel = new FANNCSharp.Float.NeuralNet(NeuralNetModel_path);
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Probabilities");
            DirectoryInfo directory = new DirectoryInfo(path);
            appObj.files = directory.GetFiles();
            string line = "";
            foreach (var file in appObj.files)
            {
                Dictionary<string, string> file_dict = new Dictionary<string, string>();
                StreamReader read = new StreamReader(file.DirectoryName + "\\" + file.ToString());

                while ((line = read.ReadLine()) != null)
                {
                    string[] parts = line.Split('\t');
                    file_dict.Add(parts[0], parts[1]);
                }

                appObj.Set(file.ToString(), file_dict);
            }
            readParam();
        }

        public void readParam()
        {
           
            string line = "";
            string path = HttpContext.Current.Server.MapPath("~/PredictionModel/Corr_Train_x.data.parameter");
            StreamReader read = new StreamReader(path);//"Train_x.data.parameter");
            int count = 0;
            while ((line = read.ReadLine()) != null)
            {
                count++;
                if (count < 3)
                    continue;
                string[] parameters = line.Split(' ');
                ArrayList list = new ArrayList(2);
                list.Add(parameters[1]);
                list.Add(parameters[2]);
                appObj.paramDictionary.Add(Convert.ToInt32(parameters[0]), list);

            }

        }

    }
}
