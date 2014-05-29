using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public IList<String> TestMethod1()
        {
            var client = new Client.Client.GameClient();

            return client.GetAvailableGameTypes();
        }
    }
}
