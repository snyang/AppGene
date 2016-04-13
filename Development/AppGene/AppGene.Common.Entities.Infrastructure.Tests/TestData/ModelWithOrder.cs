using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppGene.Common.Entities.Infrastructure.Tests.TestData
{
    public class ModelWithOrder : EntityWithOrder
    {

        [Display(Order = 1)]
        public int ModelFieldB { get; set; }

        [Display(Order = 2)]
        public Int32 ModelFieldD { get; set; }
    }
}
