using GenderPrediction.Models;
using System.Diagnostics;
using System.Web.Mvc;

namespace GenderPrediction.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            PredictionModel predictionModel = new PredictionModel();
            //{
            //    SexList = new List<Sex>
            //    {
            //        new Sex{ID="1" , Type = "Male"},
            //        new Sex{ID="2" , Type = "Female"},
            //        new Sex{ID="3",Type="Both"}
            //    }
            //};
            return View(predictionModel);
        }

        [HttpGet]
        public JsonResult Predict(string gender)
        {
            if (string.IsNullOrEmpty(gender))
            {
                ModelState.AddModelError("Name", "Please Enter Name");
            }
            if (ModelState.IsValid)
            {
                if (gender == string.Empty)
                {
                    return Json(gender, JsonRequestBehavior.AllowGet);
                }

                gender = gender.ToLower();
                PredictionModel result = GetPrediction_Fusion(gender);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void SaveCorrectedAns(PredictionModel gender)
        {
            string correctPrediction;
            if (gender.Currectedgender == "Male")
            {
                correctPrediction = "Male";
            }
            else
            {
                correctPrediction = "Female";
            }

            if (gender.Suggestedgender != correctPrediction)
            {

                string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(ip))
                {
                    ip = Request.ServerVariables["REMOTE_ADDR"];
                }
                Logger.LogMsg(LogLevel.Info, gender.Name + "\t" + gender.Suggestedgender + "\t" + correctPrediction + "\t" + ip + "\t");


            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        private PredictionModel GetPrediction_Fusion(string name)
        {
            PredictionModel gender = new PredictionModel();
            ApplicationObject appObj = System.Web.HttpContext.Current.Application["GobleObjectSVM"] as ApplicationObject;
            //I
            //Shifted_trigram_model shift = new Shifted_trigram_model(name);
            //test_data = shift.getTestData();
            //string prediction_shifted = shift.predictFusion(appObj.Model, test_data, 75, 78);

            TrigramOnlyModel trigramOnlyModel = new TrigramOnlyModel(name);
            string testData = trigramOnlyModel.getTestData_trigram_only();
            const double maleCoeff = 0.524;
            const double femaleCoeff = 0.522;
            Debug.Assert(appObj != null, nameof(appObj) + " != null");
            string predictionTrigram = trigramOnlyModel.PredictFusion(appObj.Model, testData, maleCoeff, femaleCoeff);


            //II
            NeuralNetModel net = new NeuralNetModel(name);
            net.CalculateFeaturesForNn();
            string predictionNn = net.predictFusionSVM_NN(appObj.NeuralNetModel);

            string predictionShiftedGender = "";
            double predictionShiftedScore = 0;
            string predictionNnGender = "";
            double predictionNnScore = 0;



            if (predictionTrigram != "Can't predict")
            {
                predictionShiftedGender = predictionTrigram.Split(' ')[0];
                predictionShiftedScore = double.Parse(predictionTrigram.Split(' ')[1]);

            }
            if (predictionNn != "")
            {
                predictionNnGender = predictionNn.Split(' ')[0];
                predictionNnScore = double.Parse(predictionNn.Split(' ')[1]) * 100;
            }



            if (predictionShiftedGender == "M" && predictionNnGender == "M")
            {
                gender.Suggestedgender = "Male";
            }
            else if (predictionShiftedGender == "M" && predictionNnGender == "Both")
            {
                gender.Suggestedgender = "Male";
            }
            else if (predictionShiftedGender == "Both" && predictionNnGender == "M")
            {
                gender.Suggestedgender = "Male";
            }
            else if (predictionShiftedGender == "Both" && predictionNnGender == "Both")
            {
                gender.Suggestedgender = "Both";
            }
            else if (predictionShiftedGender == "Both" && predictionNnGender == "F")
            {
                gender.Suggestedgender = "Female";
            }
            else if (predictionShiftedGender == "F" && predictionNnGender == "Both")
            {
                gender.Suggestedgender = "Female";
            }
            else if (predictionShiftedGender == "F" && predictionNnGender == "F")
            {
                gender.Suggestedgender = "Female";
            }
            else if (predictionShiftedGender == "F" && predictionNnGender == "M")
            {
                if (predictionNnScore > predictionShiftedScore)
                {
                    gender.Suggestedgender = "Male";
                }
                else if (predictionShiftedScore > predictionNnScore)
                {
                    gender.Suggestedgender = "Female";
                }
            }
            else if (predictionShiftedGender == "M" && predictionNnGender == "F")
            {
                if (predictionNnScore > predictionShiftedScore)
                {
                    gender.Suggestedgender = "Female";
                }
                else if (predictionShiftedScore > predictionNnScore)
                {
                    gender.Suggestedgender = "Male";
                }
            }
            else if (predictionTrigram == "Can't predict")
            {
                if (predictionNnGender == "M")
                {
                    gender.Suggestedgender = "Male";
                }
                else if (predictionNnGender == "F")
                {
                    gender.Suggestedgender = "Female";
                }
                else if (predictionNnGender == "Both")
                {
                    gender.Suggestedgender = "Both";
                }
            }
            return gender;
        }



    }
}