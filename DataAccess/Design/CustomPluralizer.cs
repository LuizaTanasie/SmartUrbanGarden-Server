using Microsoft.EntityFrameworkCore.Design;
using System.Globalization;

namespace DataAccess.Design
{
    public class CustomPluralizer : IPluralizer
    {
        private readonly Inflector.Inflector inflector;
        public CustomPluralizer()
        {
            inflector = new Inflector.Inflector(CultureInfo.GetCultureInfo("en-US"));
        }

        public string Pluralize(string identifier)
        {
            return inflector.Pluralize(identifier) ?? identifier;
        }

        public string Singularize(string identifier)
        {
            return inflector.Singularize(identifier) ?? identifier;
        }
    }
}
