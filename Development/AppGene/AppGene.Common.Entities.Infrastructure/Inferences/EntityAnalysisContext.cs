using System;
using System.Reflection;

namespace AppGene.Common.Entities.Infrastructure.Inferences
{
    public class EntityAnalysisContext
    {
        public Type EntityType { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        public String Source { get; set; }
    }
}