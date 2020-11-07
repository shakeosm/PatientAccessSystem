using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Pas.Common.Extensions
{
    /// <summary>
    ///     Extension methods for enumerarions
    /// </summary>
    /// <remarks>
    ///     Provide access to attributes or store logic relevant to enumeration item display.
    /// </remarks>
    public static class EnumerationExtensions
    {
        /// <summary>
        ///     Used to provided a predictable value for initial data entries.
        /// </summary>
        /// <remarks>
        ///     should be a pre generated guid
        /// </remarks>
        public static string Description(this Enum enumeration)
        {
            //first try display attributes.
            //might be localised or not, we don't care here.
            var displayAttribute = GetDisplayAttribute(enumeration);

            var description = displayAttribute?.GetDescription();

            if (!string.IsNullOrWhiteSpace(description)) return description;

            //fallback to attribute directly on property.
            var attribute = GetText<DescriptionAttribute>(enumeration);

            description = attribute.Description;

            return description;
        }

        /// <summary>
        ///     Access the Name attribute of the enum.
        /// </summary>
        /// <param name="enumeration"></param>
        /// <returns>string</returns>
        public static string Name(this Enum enumeration)
        {
            //first try display attributes.
            //might be localised or not, we don't care here.
            var displayAttribute = GetDisplayAttribute(enumeration);

            var name = displayAttribute?.GetName();

            if (!string.IsNullOrWhiteSpace(name)) return name;

            //fallback to attribute directly on property.
            var attribute = GetText<DisplayNameAttribute>(enumeration);

            name = attribute.DisplayName;

            return name;
        }

        
        /// <summary>
        ///     Accesses the Text of Type <T> - utility method for internal use only.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        private static T GetText<T>(Enum enumeration) where T : Attribute
        {
            var type = enumeration.GetType();

            var memberInfo = type.GetMember(enumeration.ToString());

            if (!memberInfo.Any())
                throw new ArgumentException($"No public members for the argument '{enumeration}'.");

            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);

            if (attributes == null || attributes.Length != 1)
                if (typeof(T) == typeof(DisplayNameAttribute))
                    return enumeration.ToString() as T;
                else
                    throw new ArgumentException(
                        $"Can't find an attribute matching '{typeof(T).Name}' for the argument '{enumeration}'");

            return attributes.Single() as T;
        }

        /// <summary>
        ///     Returns the Display attribute of the enum.
        /// </summary>
        /// <param name="enumeration"></param>
        /// <returns>DisplayAttribute</returns>
        public static DisplayAttribute GetDisplayAttribute(Enum enumeration)
        {
            var type = enumeration.GetType();

            var memberInfo = type.GetMember(enumeration.ToString());

            if (!memberInfo.Any())
                return null;
            // throw new ArgumentException($"No public members for the argument '{enumeration}'.");

            var attributes = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

            if (attributes == null || attributes.Length != 1)
                return null;
            //throw new ArgumentException(
            //    $"Can't find an attribute matching '{typeof(DisplayAttribute).Name}' for the argument '{enumeration}'");

            return attributes.Single() as DisplayAttribute;
        }

        /// <summary>
        ///     Accesses a strongly typed attribute of type <T>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <returns>Int</returns>
        public static T GetNumber<T>(Enum enumeration) where T : Attribute
        {
            var type = enumeration.GetType();

            var memberInfo = type.GetMember(enumeration.ToString());

            if (!memberInfo.Any())
                throw new ArgumentException($"No public members for the argument '{enumeration}'.");

            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);

            if (attributes == null || attributes.Length != 1)
                throw new ArgumentException(
                    $"Can't find an attribute matching '{typeof(T).Name}' for the argument '{enumeration}'");

            return attributes.Single() as T;
        }

        /// <summary>
        ///     Converts an 'EnumValue' to 'Enum Value'
        /// </summary>
        /// <param name="enumeration"></param>
        /// <returns>string</returns>
        public static string ToNormalForm(this Enum enumeration)
        {
            return enumeration.ToString().ToNormalForm();
        }


        /// <summary>
        ///     Converts an 'EnumValue' to 'Enum_Value'
        /// </summary>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public static string ToUnderScoreLowercase(this Enum enumeration)
        {
            return enumeration.ToString().ToUnderScoreLowercase();
        }

        /// <summary>
        ///     Accesses the enum Description Attribute
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns>string</returns>
        public static T FromEnumDescription<T>(this string description) where T : struct
        {
            Debug.Assert(typeof(T).IsEnum);

            return (T) typeof(T)
                .GetFields()
                .First(f => f.GetCustomAttributes<DescriptionAttribute>()
                    .Any(a => a.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
                )
                .GetValue(null);
        }


        public static IDictionary<string, string> ToDictionary(this NameValueCollection col)
        {
            var dict = new Dictionary<string, string>();

            foreach (var key in col.Keys) dict.Add(key.ToString(), col[key.ToString()]);

            return dict;
        }


        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T? Parse<T>(string input) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("Generic Type 'T' must be an Enum");

            if (!string.IsNullOrEmpty(input))
                if (
                    Enum.GetNames(typeof(T))
                        .Any(e => string.Equals(e.Trim(), input.Trim(), StringComparison.InvariantCultureIgnoreCase)))
                    return (T) Enum.Parse(typeof(T), input, true);

            return null;
        }



        public static Dictionary<T, string> ToDictionary<T>(this T @enum) where T : struct
            => Enum.GetValues(typeof(T)).Cast<T>().ToDictionary(e => e, e => e.ToString().ToNormalForm());


        public static List<T> ToList<T>(this Enum @enum)
        {
            var type = @enum.GetType();
            return Enum.GetValues(type).Cast<T>().Select(e => e).ToList();
        }

        public static Dictionary<string, string> ToStringDictionary(this Enum @enum)
        {
            var type = @enum.GetType();
            return Enum.GetValues(type).Cast<int>().ToDictionary(Convert.ToString, e => Enum.GetName(type, e));
        }

        public static bool TryParseEnum<T>(this int enumValue, out T ret)
        {
            ret = default;

            var success = Enum.IsDefined(typeof(T), enumValue);

            if (success) ret = (T) Enum.ToObject(typeof(T), enumValue);

            return success;
        }
    }
}