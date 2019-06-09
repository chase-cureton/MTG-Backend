using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MTGLambda.MTGLambda.Helpers.Enum
{
    public enum CardEnum
    {
        [Description("Land")]
        Land = 0,
        [Description("Creature")]
        Creature = 1,
        [Description("Enchantment")]
        Enchantment = 2,
        [Description("Artifact")]
        Artifact = 3,
        [Description("Planeswalker")]
        Planeswalker = 4,
        [Description("Sorcery")]
        Sorcery = 5,
        [Description("Instant")]
        Instant = 6
    }
}

namespace EnumExtensions
{
    public static class EnumExtension
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false)
                                                             .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return null; // could also return string.Empty
        }
    }
}