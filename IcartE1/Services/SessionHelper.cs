using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace IcartE1.Services
{
    public static class SessionHelper
    {
        public static  void SetObjectAsJson(this ISession session, string key, object value)
        {
           session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static string GetObjectAsJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value ?? default;
        }
    }
}
