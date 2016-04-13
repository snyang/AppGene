using AppGene.Common.Entities.Infrastructure.Inferences;
using AppGene.Common.Entities.Infrastructure.Tests.TestData;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text;

namespace AppGene.Common.Entities.Infrastructure.Tests.Inferences
{
    [TestFixture]
    public class DisplayPropertiesGetterOrderTest
    {
        [Test, Sequential]
        public void TestEntityWithoutOrderConfigruation(
            [Values(
            typeof(EntityWithoutOrder), 
            typeof(ModelWithoutOrder),
            typeof(EntityWithOrder),
            typeof(ModelWithOrder),
            typeof(EntityWithOrderAttribute),
            typeof(ModelWithOrderAttribute)
            )] Type model,
            [Values(
            "FieldB,FieldC,FieldD", 
            "FieldB,FieldC,FieldModelA,FieldD",
            "FieldB,FieldD,FieldC",
            "ModelFieldB,FieldB,ModelFieldD,FieldD,FieldC",
            "FieldA,FieldB,FieldC",
            "ModelFieldA,FieldA,FieldB,FieldC")] String nameList)
        {
            var displayProperties = new DisplayPropertiesGetter().GetProperties(new EntityAnalysisContext
            {
                EntityType = model,
                Source = this.GetType().FullName,
            });

            var shownProperties = displayProperties.Where(p => p.IsHidden == false);
            var expectedCount = nameList.Split(',').Length;
            Assert.AreEqual(expectedCount, shownProperties.Count());

            var orderNameList = shownProperties.Aggregate(new StringBuilder(), (a, b) => {
                if (a.Length > 0)
                    a.Append(",");
                a.Append(b.PropertyName);
                return a;
            });
            Assert.AreEqual(nameList, orderNameList.ToString());
        }
     
    }
}
