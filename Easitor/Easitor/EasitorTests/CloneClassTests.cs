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
    public class CloneClassTests
    {
        [TestMethod()]
        public void CloneObjectTest()
        {
            // Arrange
            EditorModel Model = new EditorModel();
            // Act
            EditorModel Cloned = CloneClass.CloneObject<EditorModel>(Model);
            // Assert
            Assert.AreNotEqual(Model,Cloned);
        }
    }
}
