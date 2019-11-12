using Epam.ImitationGames.Production.Common;
using Epam.ImitationGames.Production.Common.Activity;
using Epam.ImitationGames.Production.Common.Base;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace CalculationEngine
{
    public class Game
    {
        public List<Customer> Customers { get; set; }

        public GameTime Time { get; set; }

        public int TotalGameDays { get; set; }

        public List<ActivityLog> Activity { get; set; }

        public Game()
        {
            Customers = new List<Customer>();
            Time = new GameTime();
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

        protected string GetHashString(byte[] array)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < array.Length; i++)
            {
                sb.Append(array[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public void AddActivity(BaseChanging changing)
        {
            using (var md5 = MD5.Create())
            {
                var thisHashCode = GetHashString(md5.ComputeHash(ObjectToByteArray(changing)));
                var prevHashCode = Activity.LastOrDefault()?.HashCode ?? string.Empty;

                var hashCode = GetHashString(md5.ComputeHash(Encoding.ASCII.GetBytes(thisHashCode + prevHashCode)));

                Activity.Add(new ActivityLog
                {
                    Change = changing,
                    HashCode = hashCode
                });
            }
        }
    }
}