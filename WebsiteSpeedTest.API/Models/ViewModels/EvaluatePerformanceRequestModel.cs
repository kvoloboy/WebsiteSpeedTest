using System.ComponentModel.DataAnnotations;

namespace RequestSpeedTest.API.Models.ViewModels
{
    public class EvaluatePerformanceRequestModel
    {
        [RegularExpression(@"^https?://.*", ErrorMessage = "Uri should starts with http")]
        public string Uri { get; set; }
    }
}
