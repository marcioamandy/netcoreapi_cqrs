using System;
namespace Globo.PIC.Infra.Helpers
{
    public static class EnumHelpers
    {
        public static string GetEnumDescription(this Enum @enum)
        {

            if (@enum != null)
                return @enum.ToString();

            return String.Empty;
        }
    }
}
