using Newtonsoft.Json;
using SantaHelpers.Exceptions;
using System;
using System.IO;

namespace SantaHelpers
{
    public static class FileHelper
    {
        public static T Load<T>(string fileName)
        {
            try
            {
                var result = JsonConvert.DeserializeObject<T>(File.ReadAllText(fileName));
                return result;
            }
            catch (Exception e)
            {
                throw new SantaHelpersException(e);
            }
        }

        public static void Save(string fileName, object data)
        {
            try
            {
                var serializedData = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(fileName, serializedData);
            }
            catch (Exception e)
            {
                throw new SantaHelpersException(e);
            }
        }
    }
}
