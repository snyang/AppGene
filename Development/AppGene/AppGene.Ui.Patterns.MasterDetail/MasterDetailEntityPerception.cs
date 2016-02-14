using AppGene.Model.EntityPerception;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppGene.Ui.Patterns.MasterDetail
{
    public class MasterDetailEntityPerception
    {
        private IList<EditPropertyInfo> detailProperties;

        private IList<PropertyInfo> displayProperties;

        private IList<PropertyInfo> filterProperties;

        private IList<EditPropertyInfo> gridProperties;

        private IList<SortPropertyInfo> sortProperties;

        public MasterDetailEntityPerception(Type entityType)
        {
            EntityType = entityType;
        }

        public IList<EditPropertyInfo> DetailProperties
        {
            get
            {
                if (detailProperties == null)
                {
                    detailProperties = new EditPropertiesGetter().Get(new EntityAnalysisContext
                    {
                        EntityType = EntityType,
                        Source = this.GetType().FullName,
                    });
                }

                return detailProperties;
            }
        }

        public IList<PropertyInfo> DisplayProperties
        {
            get
            {
                if (displayProperties == null)
                {
                    displayProperties = new ReferencePropertyGetter().Get(new EntityAnalysisContext
                    {
                        EntityType = EntityType,
                        Source = this.GetType().FullName
                    });
                }

                return displayProperties;
            }
        }

        public Type EntityType { get; set; }

        public IList<PropertyInfo> FilterProperties
        {
            get
            {
                if (filterProperties == null)
                {
                    filterProperties = new FilterPropertyGetter().Get(new EntityAnalysisContext
                    {
                        EntityType = EntityType,
                        Source = this.GetType().FullName
                    });
                }

                return filterProperties;
            }
        }

        public IList<EditPropertyInfo> GridProperties
        {
            get
            {
                //TODO: distinct edit properties for grid and detail panel
                if (gridProperties == null)
                {
                    gridProperties = new EditPropertiesGetter().Get(new EntityAnalysisContext
                    {
                        EntityType = EntityType,
                        Source = this.GetType().FullName,
                    });
                }

                return gridProperties;
            }
        }

        public IList<SortPropertyInfo> SortProperties
        {
            get
            {
                if (sortProperties == null)
                {
                    sortProperties = new SortPropertyGetter().Get(new EntityAnalysisContext
                    {
                        EntityType = EntityType,
                        Source = this.GetType().FullName,
                    });
                }

                return sortProperties;
            }
        }
    }
}