using System;
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
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            LoadModel();
        }

        private void LoadModel()
        {
            ApplicationObject appObj = new ApplicationObject();

            string path = HttpContext.Current.Server.MapPath("~/PredictionModel/gram_features.txt");
            StreamReader read = new StreamReader(path);
            //StreamReader read = new StreamReader("gram_features.txt");
            appObj.lines = new string[1945];
            string line = read.ReadToEnd();
            var lines = line.Split('\n');
            for (int i = 0; i < lines.Count(); i++)
            {
                appObj.lines[i] = lines[i].Trim();
            }
            read.Close();
            string path2 = HttpContext.Current.Server.MapPath("~/PredictionModel/gender_detection_train.model");
            appObj.Model = Model.Load(path2);
            //appObj.Model = Model.Load("gender_detection_train.model");
            HttpContext.Current.Application["GobleObject"] = appObj;
        }

    }
}
