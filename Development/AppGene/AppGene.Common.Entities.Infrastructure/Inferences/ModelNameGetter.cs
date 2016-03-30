using AppGene.Common.Entities.Infrastructure.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace AppGene.Common.Entities.Infrastructure.Inferences
{
    public class ModelNameGetter
    {

        public EntityAnalysisContext Context { get; set; }

        /// <summary>
        /// Gets the entity name.
        /// </summary>
        /// <param name="context">The entity analysis context.</param>
        /// <returns>The display entity name.</returns>
        public virtual String GetName(EntityAnalysisContext context)
        {
            Context = context;

            var modelAttribute = context.EntityType.GetCustomAttribute<ModelAttribute>();
            var name = GetEntityName(context.EntityType, modelAttribute);

            return name;
        }

        private string GetEntityName(Type entityType, ModelAttribute modelAttribute)
        {
            if (modelAttribute == null)
            {
                return EntityAnalysisHelper.GetPropertyDisplayName(entityType.Name);
            }
            else if (modelAttribute.ResourceType == null)
            {
                return modelAttribute.Name;
            }

            return EntityAnalysisHelper.GetResourceString(modelAttribute.ResourceType, modelAttribute.Name);
        }
    }
}