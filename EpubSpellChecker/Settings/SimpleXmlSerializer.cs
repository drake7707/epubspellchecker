using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Runtime.Serialization;

namespace EpubSpellChecker
{
    /// <summary>
    /// This is a simple serializer/deserializer that doesn't care about namespaces and types
    /// It stores the type qualified name in the xml so it can deserialize without a problem.
    /// The standard XML serialization in .NET is too much boilerplate and BinaryFormatter isn't 
    /// text friendly to fiddle in the settings file manually
    /// </summary>
    public class SimpleXmlSerializer
    {
        public static string Serialize(object obj)
        {
            StringBuilder str = new StringBuilder();
            Serialize(str, 0, obj.GetType().Name, obj, true);
            return str.ToString();
        }

        private static void Serialize(StringBuilder str, int padding, string nodeName, object obj, bool inclFullType)
        {
            if (inclFullType)
                str.Append(new string(' ', padding) + "<" + nodeName + " QualifiedName='" + obj.GetType().AssemblyQualifiedName + "'");
            else
                str.Append(new string(' ', padding) + "<" + nodeName);

            if (obj == null)
            {
                str.Append(">null</" + nodeName + ">");
                str.AppendLine("");
                return;
            }

            Dictionary<PropertyInfo, object> arrays = new Dictionary<PropertyInfo, object>();
            Dictionary<PropertyInfo, object> objects = new Dictionary<PropertyInfo, object>();

            foreach (var prop in obj.GetType().GetProperties())
            {
                if (prop.CanRead && prop.CanWrite && prop.GetIndexParameters().Length == 0)
                {
                    var value = prop.GetValue(obj, null);
                    if (prop.PropertyType.IsArray && value != null)
                        arrays.Add(prop, value);
                    else if (IsTypeInline(prop.PropertyType))
                    {
                        str.Append(" " + prop.Name + "='" + System.Security.SecurityElement.Escape(GetXmlValue(value)) + "'");
                    }
                    else
                    {
                        objects.Add(prop, value);
                    }

                }
            }

            if (arrays.Count == 0 && objects.Count == 0)
                str.Append(" />");
            else
            {
                str.Append(">");
            }
            str.AppendLine("");

            foreach (var pair in arrays)
            {
                str.AppendLine(new string(' ', padding + 5) + "<" + pair.Key.Name + ">");

                foreach (var item in ((Array)pair.Value))
                {
                    var arrayType = pair.Key.PropertyType.GetElementType();
                    if (IsTypeInline(arrayType))
                    {
                        str.AppendLine(new string(' ', padding + 10) + "<Item>" + GetXmlValue(item) + "</Item>");
                    }
                    else
                        Serialize(str, padding + 10, arrayType.Name, item, false);
                }
                str.AppendLine(new string(' ', padding + 5) + "</" + pair.Key.Name + ">");
            }

            foreach (var pair in objects)
            {
                Serialize(str, padding + 5, pair.Key.Name, pair.Value, pair.Key.PropertyType == typeof(object));
            }
            if (arrays.Count > 0 || objects.Count > 0)
                str.AppendLine(new string(' ', padding) + "</" + nodeName + ">");
        }

        private static bool IsTypeInline(Type type)
        {
            return type.IsPrimitive || type == typeof(DateTime) || type == typeof(TimeSpan) || type == typeof(string) || type.IsEnum;
        }

        private static string GetXmlValue(object value)
        {
            if (value == null)
                return "NULL";

            if (value.GetType().IsEnum)
            {
                return value.ToString();
            }

            if (value is DateTime)
                return ((DateTime)value).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);
            else if (value is TimeSpan)
                return ((TimeSpan)value).ToString("g", System.Globalization.CultureInfo.InvariantCulture);
            else if (value is double || value is float)
                return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", value);
            else if (value is bool)
                return (bool)value ? "true" : "false";
            else
                return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", value);
        }

        private static object GetValueFromXml(Type t, string value)
        {
            if (value == "NULL")
                return null;

            if (t.IsEnum)
            {
                var matchingItems = Enum.GetValues(t).Cast<object>().Where(itm => Enum.GetName(t, itm) == value).ToArray();
                if (matchingItems.Length == 0)
                {
                    int val;
                    if (int.TryParse(value, out val))
                        return val;
                    else
                        return 0;
                }
                else
                    return matchingItems[0];
            }
            if (t == typeof(DateTime))
                return DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);
            if (t == typeof(TimeSpan))
                return TimeSpan.ParseExact(value, "g", System.Globalization.CultureInfo.InvariantCulture);
            else if (t == typeof(double))
                return double.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            else if (t == typeof(float))
                return float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            else if (t == typeof(int))
                return int.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            else if (t == typeof(long))
                return long.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
            else if (t == typeof(bool))
                return value == "true" ? true : false;
            else
                return value;
        }

        public static object Deserialize(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            var root = doc.ChildNodes[0];
            string type = root.Attributes["QualifiedName"].Value;

            Type t = Type.GetType(type);
            return Deserialize(root, t);
        }

        private static object Deserialize(XmlNode node, Type t)
        {
            var obj = FormatterServices.GetSafeUninitializedObject(t);

            foreach (var prop in t.GetProperties())
            {
                if (prop.CanRead && prop.CanWrite && prop.GetIndexParameters().Length == 0)
                {
                    if (prop.PropertyType.IsArray)
                    {
                        var arrayNode = node.SelectSingleNode(prop.Name);
                        if (arrayNode != null)
                        {
                            var arrayType = prop.PropertyType.GetElementType();
                            var array = Array.CreateInstance(arrayType, arrayNode.ChildNodes.Count);

                            int i = 0;
                            foreach (XmlNode ch in arrayNode.ChildNodes)
                            {
                                object arrayObj;
                                if (IsTypeInline(arrayType))
                                    arrayObj = GetValueFromXml(arrayType, ch.InnerText);
                                else
                                    arrayObj = Deserialize(ch, arrayType);

                                array.SetValue(arrayObj, i);
                                i++;
                            }
                            prop.SetValue(obj, array, null);
                        }
                    }
                    else if (IsTypeInline(prop.PropertyType))
                    {
                        var attr = node.Attributes[prop.Name];
                        if (attr != null)
                            prop.SetValue(obj, GetValueFromXml(prop.PropertyType, attr.Value), null);
                    }
                    else
                    {
                        var objNode = node.SelectSingleNode(prop.Name);
                        if (objNode != null)
                        {
                            if (objNode.InnerText == "null")
                                prop.SetValue(obj, null, null);
                            else
                            {
                                object subObj;
                                if (objNode.Attributes["QualifiedName"] != null)
                                {
                                    var type = Type.GetType(objNode.Attributes["QualifiedName"].Value);
                                    if(type != null)
                                        subObj = Deserialize(objNode, type);
                                    else
                                        subObj = Deserialize(objNode, prop.PropertyType);
                                }
                                else
                                    subObj = Deserialize(objNode, prop.PropertyType);

                                prop.SetValue(obj, subObj, null);
                            }
                        }
                    }
                }
            }
            return obj;
        }
    }
}
