using System;
using System.Linq;
using System.Text.Json;

namespace Refactorizando.Shared.Utilities{
    public static class ParseUtility
    {

        public static T GetUnparsedValue<T>(object value)
        {
            if (value is JsonElement)
            {
                var element = (JsonElement)value;
                var kind = element.ValueKind;
                if (kind == JsonValueKind.Object
                    || kind == JsonValueKind.Array)
                {
                    return JsonSerializer.Deserialize<T>(value.ToString());
                }
                else
                {
                    if (typeof(T) == typeof(Guid))
                    {
                        if (Guid.TryParse(element.GetString(), out Guid id))
                        {
                            return (T)Convert.ChangeType(id, typeof(T));
                        }
                        else
                        {
                            throw new InvalidCastException($"Can't cast '{element.GetString()}' to Guid");
                        }
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        return (T)Convert.ChangeType(element.GetString(), typeof(T));
                    }
                    else if (typeof(T) == typeof(int))
                    {
                        return (T)Convert.ChangeType(element.GetInt32(), typeof(T));
                    }
                    else if (typeof(T) == typeof(DateTime))
                    {
                        return (T)Convert.ChangeType(element.GetDateTime(), typeof(T));
                    }
                    else
                    {
                        Console.WriteLine(typeof(T).Name);
                        throw new NotImplementedException();
                    }
                }
            }
            else
            {
                if (value == null)
                {
                    return default(T);
                }
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }


        public static T ChangeType<T>(string data)
        {
            var returned = GenericStringCast<T>(data); ;
            if (returned == null)
            {
                return default(T);
            }
            return (T)returned;
        }

        private static object GenericStringCast<T>(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            var type = typeof(T);
            if (type == typeof(string))
            {
                return data;
            }
            else if (type == typeof(Guid))
            {
                if (Guid.TryParse(data, out Guid parsed))
                {
                    return parsed;
                }
                throw new InvalidCastException();
            }
            else if (type == typeof(bool))
            {
                if (new string[] { "true", "1", "yes", "si" }.ToList().IndexOf(data.ToLower()) > 0)
                {
                    return true;
                }
                else if (new string[] { "false", "0", "no" }.ToList().IndexOf(data.ToLower()) > 0)
                {
                    return false;
                }
                throw new InvalidCastException();
            }
            else if (type == typeof(int) || type == typeof(Int32))
            {
                if (int.TryParse(data, out int parsed))
                {
                    return parsed;
                }
                throw new InvalidCastException();
            }
            else if (type == typeof(decimal))
            {
                if (decimal.TryParse(data, out decimal parsed))
                {
                    return parsed;
                }
                throw new InvalidCastException();
            }
            else if (type == typeof(double))
            {
                if (double.TryParse(data, out double parsed))
                {
                    return parsed;
                }
                throw new InvalidCastException();
            }
            else if (type == typeof(DateTime))
            {
                if (DateTime.TryParse(data, out DateTime parsed))
                {
                    return parsed;
                }
                throw new InvalidCastException();
            }
            else
            {
                throw new NotImplementedException(type.Name);
            }
        }
    }
}