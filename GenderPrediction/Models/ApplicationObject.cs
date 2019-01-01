using LibLinearDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GenderPrediction.Models
{
    public class ApplicationObject
    {
        public LibLinearDotNet.Model Model { get; set; }
        public string[] lines { get; set; }
    }
}