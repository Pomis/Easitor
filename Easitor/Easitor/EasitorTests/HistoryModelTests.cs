using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Easitor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Easitor.Tests
{
    [TestClass()]
    public class HistoryModelTests
    {
        [TestMethod()]
        public void CheckIfTooLongTest()
        {
            // Arrange
            HistoryModel TestingHistory = HistoryModel.Instance;
            // Act
            for (int i = 0; i < 20; i++)
            {
                TestingHistory.CommandHistory.Add(new ToolCommandViewModel()
                {
                    CommandName = "test",
                    CommandIndex = i.ToString(),
                });
                TestingHistory.CheckIfTooLong();
            }
            
            // Assert
            Assert.IsTrue(TestingHistory.CommandHistory.Count(n => n == null) == 0
                       && TestingHistory.CommandHistory.Count(n => n != null) <= 15); 
        }
    }
}
