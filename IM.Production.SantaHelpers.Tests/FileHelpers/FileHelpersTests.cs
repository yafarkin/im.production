using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SantaHelpers.Tests
{
    [TestClass]
    public class FileHelpersTests
    {
        private class User
        {
            public int ID { get; set; }
            public string Name { get; set; }
        };

        [TestMethod]
        public void Should_SaveAndLoadCollection()
        {
            const int USER_ID_1 = 1;
            const string USER_NAME_1 = "Alice";

            const int USER_ID_2 = 2;
            const string USER_NAME_2 = "Bob";

            var users = new List<User>
            {
                new User
                {
                    ID = USER_ID_1,
                    Name = USER_NAME_1
                },
                new User
                {
                    ID = USER_ID_2,
                    Name = USER_NAME_2
                }
            };

            var fileName = Path.GetTempFileName();

            FileHelper.Save(fileName, users);

            var loadedUsers = FileHelper.Load<List<User>>(fileName);

            Assert.AreEqual(USER_ID_1, loadedUsers.First().ID);
            Assert.AreEqual(USER_NAME_1, loadedUsers.First().Name);
            Assert.AreEqual(USER_ID_2, loadedUsers.Skip(1).First().ID);
            Assert.AreEqual(USER_NAME_2, loadedUsers.Skip(1).First().Name);
        }
    }
}
