using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Net5Template.Core
{
    public static class ExtensionMethods
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            string description = null;

            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(System.Globalization.CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptionAttributes.Length > 0)
                        {
                            // we're only getting the first description we find
                            // others will be ignored
                            description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                        }

                        break;
                    }
                }
            }

            return description;
        }
        public static string GetEnumDescription(this int integerEnum, Type enumType)
        {
            var o = Enum.ToObject(enumType, integerEnum);
            if (o is IConvertible)
                return (o as IConvertible).GetDescription();
            return string.Empty;
        }
        //public static string GetErrorMessage<T, TProp>(this T e, Expression<Func<T, TProp>> propertySelector)
        //{
        //    string errorMessage = string.Empty;

        //    MemberExpression body = (MemberExpression)propertySelector.Body;

        //    if (body != null)
        //    {
        //        var attrs = body.Member.GetCustomAttributes();
        //        foreach (var a in attrs)
        //        {
        //            if (a != null && a is RequiredAttribute)
        //            {
        //                errorMessage = (a as RequiredAttribute)?.ErrorMessage;
        //                break;
        //            }
        //        }
        //    }
        //    return errorMessage;
        //}
        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 100 ? str.Length : 100).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        //public static string GetEnumImageUrl(this Enum value, bool returnAlt = false)
        //{
        //    Type type = value.GetType();
        //    FieldInfo fieldInfo = type.GetField(value.ToString());

        //    ImageUrlAttribute attr = fieldInfo.GetCustomAttribute(typeof(ImageUrlAttribute), false) as ImageUrlAttribute;
        //    if (attr != null)
        //    {
        //        return returnAlt ? attr.ImageUrlAlt : attr.ImageUrl;
        //    }
        //    return string.Empty;//debería retornar error?
        //}
        public static object GetDefaultValue(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());

            DefaultValueAttribute attr = fieldInfo.GetCustomAttribute(typeof(DefaultValueAttribute), false) as DefaultValueAttribute;
            return attr.Value;
        }
        public static string ToStringUser(this Guid value)
        {
            return value.ToString("D");
        }
    }
}
