using AppGene.Common.EntityPerception;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppGene.Ui.Patterns.MasterDetail
{
    public class MasterDetailEntityPerception
    {
        private IList<DisplayPropertyInfo> displayProperties;

        private IList<PropertyInfo> referenceProperties;

        private IList<PropertyInfo> filterProperties;

        private IList<DisplayPropertyInfo> gridDisplayProperties;

        private IList<SortPropertyInfo> sortProperties;

        public MasterDetailEntityPerception(Type entityType)
        {
            EntityType = entityType;
        }

        public IList<DisplayPropertyInfo> DisplayProperties
        {
            get
            {
                if (displayProperties == null)
                {
                    displayProperties = new DisplayPropertiesGetter().GetProperties(new EntityAnalysisContext
                    {
                        EntityType = EntityType,
                        Source = this.GetType().FullName,
                    });
                }

                return displayProperties;
            }
        }

        public IList<PropertyInfo> ReferenceProperties
        {
            get
            {
                if (referenceProperties == null)
                {
                    referenceProperties = new ReferencePropertyGetter().GetProperties(new EntityAnalysisContext
                    {
                        EntityType = EntityType,
                        Source = this.GetType().FullName
                    });
                }

                return referenceProperties;
            }
        }

        public Type EntityType { get; set; }

        public IList<PropertyInfo> FilterProperties
        {
            get
            {
                if (filterProperties == null)
                {
                    filterProperties = new FilterPropertyGetter().GetProperties(new EntityAnalysisContext
                    {
                        EntityType = EntityType,
                        Source = this.GetType().FullName
                    });
                }

                return filterProperties;
            }
        }

        public IList<DisplayPropertyInfo> GridDisplayProperties
        {
            get
            {
                //TODO: distinct edit properties for grid and detail panel
                if (gridDisplayProperties == null)
                {
                    gridDisplayProperties = new DisplayPropertiesGetter().GetProperties(new EntityAnalysisContext
                    {
                        EntityType = EntityType,
                        Source = this.GetType().FullName,
                    });
                }

                return gridDisplayProperties;
            }
        }

        public IList<SortPropertyInfo> SortProperties
        {
            get
            {
                if (sortProperties == null)
                {
                    sortProperties = new SortPropertyGetter().GetProperties(new EntityAnalysisContext
                    {
                        EntityType = EntityType,
                        Source = this.GetType().FullName,
                    });
                }

                return sortProperties;
            }
        }

        public object GetPropertyDefaultValue(PropertyInfo property)
        {
            return new DefaultValueGetter().GetDefaultValue(new EntityAnalysisContext
            {
                EntityType = EntityType,
                PropertyInfo = property,
                Source = this.GetType().FullName
            });
        }

        public string GetPropertyFormatString(PropertyInfo property)
        {
            return new DisplayFormatGetter().GetFormatString(new EntityAnalysisContext
            {
                EntityType = EntityType,
                PropertyInfo = property,
                Source = this.GetType().FullName,
            });
        }
    }
}