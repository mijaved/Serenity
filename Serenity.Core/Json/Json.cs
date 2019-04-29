﻿using Newtonsoft.Json;
using System;
using System.IO;

namespace Serenity
{
    /// <summary>
    /// Contains shortcuts to Newtonsoft.Json serialization / deserialization methods, and default
    /// Serenity settings.
    /// </summary>
    public static class JSON
    {
        /// <summary>
        /// Deserializes a JSON string to an object
        /// </summary>
        /// <typeparam name="T">Type to deserialize</typeparam>
        /// <param name="input">JSON string</param>
        /// <returns>Deserialized object</returns>
        public static T Parse<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input, JsonSettings.Strict);
        }

        /// <summary>
        /// Deserializes a JSON string to an object
        /// </summary>
        /// <param name="targetType">Type to deserialize</param>
        /// <param name="input">JSON string</param>
        /// <returns>Deserialized object</returns>
        public static object Parse(string input, Type targetType)
        {
            return JsonConvert.DeserializeObject(input, targetType, JsonSettings.Strict);
        }

        /// <summary>
        /// Deserializes a JSON string to an object, using more tolerant settings.
        /// </summary>
        /// <typeparam name="T">Type to deserialize</typeparam>
        /// <param name="input">JSON strng</param>
        /// <returns>Deserialized object</returns>
        public static T ParseTolerant<T>(string input)
        {
            return JsonConvert.DeserializeObject<T>(input, JsonSettings.Tolerant);
        }

        /// <summary>
        /// Deserializes a JSON string to an object, using more tolerant settings
        /// </summary>
        /// <param name="targetType">Type to deserialize</param>
        /// <param name="input">JSON string</param>
        /// <returns>Deserialized object</returns>
        public static object ParseTolerant(string input, Type targetType)
        {
            return JsonConvert.DeserializeObject(input, targetType, JsonSettings.Tolerant);
        }

        /// <summary>
        /// Converts object to its JSON representation
        /// </summary>
        /// <param name="value">Value to convert to JSON</param>
        /// <returns>Serialized JSON string</returns>
        public static string Stringify(object value)
        {
            return JsonConvert.SerializeObject(value, JsonSettings.Strict);
        }

        /// <summary>
        /// Converts object to its JSON representation
        /// </summary>
        /// <param name="value">Value to convert to JSON</param>
        /// <param name="indentation">Indentation (default 4)</param>
        /// <returns>Serialized JSON string</returns>
        public static string StringifyIndented(object value, int indentation = 4)
        {
            using (var sw = new StringWriter())
            using (var jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                jw.IndentChar = ' ';
                jw.Indentation = indentation;

                var serializer = JsonSerializer.Create(JsonSettings.Strict);
                serializer.Serialize(jw, value);
                return sw.ToString();
            }
        }

        /// <summary>
        ///   Converts an object to its JSON representation (extension method for Stringify)</summary>
        /// <param name="value">
        ///   Object</param>
        /// <returns>
        ///   JSON representation string.</returns>
        /// <remarks>
        ///   null, Int32, Boolean, DateTime, Decimal, Double, Guid types handled automatically.
        ///   If object has a ToJson method it is used, otherwise value.ToString() is used as last fallback.</remarks>
        public static string ToJson(this object value)
        {
            return Stringify(value);
        }
    }
}