using System;
using System.Linq;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Globo.PIC.Domain.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(this Enum value)
        {
            try {
                var field = value.GetType().GetField(value.ToString());

                var attributes =
                    (DescriptionAttribute[])field.GetCustomAttributes(
                    typeof(DescriptionAttribute),
                    false);

                if (attributes != null &&
                    attributes.Length > 0)
                    return attributes[0].Description;
                else
                    return value.ToString();

            }
            catch {
                return null;
            }
        }

		public static string GetEnumValue(this Enum @enum)
		{
			var attr =
				@enum.GetType().GetMember(@enum.ToString()).FirstOrDefault()?.
					GetCustomAttributes(false).OfType<EnumMemberAttribute>().
					FirstOrDefault();
			if (attr == null)
				return @enum.ToString();
			return attr.Value;
		}
	}
}
