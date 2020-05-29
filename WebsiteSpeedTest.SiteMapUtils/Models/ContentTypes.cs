using System.Linq;
using System.Reflection;

namespace RequestSpeedTest.SiteMapUtils.Models
{
    public class ContentTypes
    {
        public const string TextXml = "text/xml";
        public const string ApplicationXml = "application/xml";
        public const string TextPlain = "text/plain";

        public static bool IsAvailable(string type)
        {
            var availableTypes = typeof(ContentTypes)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(info => info.IsLiteral && !info.IsInitOnly);

            var isAvailableType = availableTypes.Any(field => field.GetValue(null)?.ToString() == type);

            return isAvailableType;
        }
    }
}
