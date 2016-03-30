using AppGene.Common.Entities.Infrastructure.Inferences;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppGene.Common.Entities.Infrastructure.Tests.Inferences
{
    [TestFixture]
    public class EntityAnalysisHelperTest
    {
        [Test]
        public void TestGetPropertyDisplayName()
        {
            Assert.AreEqual("ID", EntityAnalysisHelper.GetPropertyDisplayName("ID"));
            Assert.AreEqual("Employee Name", EntityAnalysisHelper.GetPropertyDisplayName("EmployeeName"));
            Assert.AreEqual("Employee XML Name", EntityAnalysisHelper.GetPropertyDisplayName("EmployeeXMLName"));
            Assert.AreEqual("Employee XML", EntityAnalysisHelper.GetPropertyDisplayName("EmployeeXML"));
            Assert.AreEqual("Employee XML", EntityAnalysisHelper.GetPropertyDisplayName("Employee_XML"));
            Assert.AreEqual("Data Type Group A", EntityAnalysisHelper.GetPropertyDisplayName("DataTypeGroupA"));
            Assert.AreEqual("Data Type Group A", EntityAnalysisHelper.GetPropertyDisplayName("__DataTypeGroupA"));
            Assert.AreEqual("Data Type Group A", EntityAnalysisHelper.GetPropertyDisplayName("__DataTypeGroupA_"));
        }
    }
}
