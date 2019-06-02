using System.IO;
using Newtonsoft.Json;

namespace ServerlessMicroservice.EmailSenderMicroservice.SDK
{
    internal static class Extensions
    {
        internal static T Deserialize<T>(this Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
            {
                using (var jsonTextReader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }
    }
}
