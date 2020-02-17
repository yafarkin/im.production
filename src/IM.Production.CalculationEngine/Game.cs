using System;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Activity;
using Epam.ImitationGames.Production.Domain.Base;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using Epam.ImitationGames.Production.Domain.Production;
using Epam.ImitationGames.Production.Domain.Static;

namespace CalculationEngine
{
    public class Game
    {
        public List<Customer> Customers { get; set; }

        public int TotalGameDays { get; set; }

        public List<ActivityLog> Activity { get; set; }

        public Game()
        {
            Reset();
        }

        public void Reset()
        {
            Customers = new List<Customer>();
            TotalGameDays = 0;
            Activity = new List<ActivityLog>();
        }

        public void Save(string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            using (var stream = File.Create(filename))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
            }
        }

        public static Game Load(string filename)
        {
            using (var stream = File.OpenRead(filename))
            {
                var formatter = new BinaryFormatter();
                var game = (Game)formatter.Deserialize(stream);
                return game;
            }
        }

        protected byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        protected static string GetHashString(byte[] array)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < array.Length; i++)
            {
                sb.Append(array[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public static string GetMD5Hash(string str)
        {
            return GetMD5Hash(Encoding.ASCII.GetBytes(str));
        }

        public static string GetMD5Hash(byte[] array)
        {
            using (var md5 = MD5.Create())
            {
                var hashCode = GetHashString(md5.ComputeHash(array));
                return hashCode;
            }
        }

        public void AddActivity(BaseChanging changing)
        {
            var thisHashCode = GetMD5Hash(ObjectToByteArray(changing));
            var prevHashCode = Activity.LastOrDefault()?.HashCode ?? string.Empty;

            var hashCode = GetMD5Hash(thisHashCode + prevHashCode);

            changing.DoAction();

            Activity.Add(new ActivityLog {Change = changing, HashCode = hashCode});
        }

        public IList<BaseChanging> FilterActivity(int? gameDay = null, Customer customer = null, Factory factory = null,
            IList<Type> activityTypes = null)
        {
            var result = Activity.Select(x => x.Change);

            if (gameDay.HasValue)
            {
                result = result.Where(x => x.Time.Day == (0 == gameDay ? CurrentGameProps.GameDay : gameDay.Value));
            }

            if (customer != null)
            {
                result = result.Where(x => x.Customer.Id == customer.Id);
            }

            if (factory != null)
            {
                result = result.Where(x => x is FactoryChange f && f.Factory.Id == factory.Id);
            }

            if (activityTypes != null && activityTypes.Any())
            {
                result = result.Where(x => activityTypes.Contains(x.GetType()));
            }

            return result.ToList();
        }
    }
}