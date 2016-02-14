using System.Reflection;

namespace AppGene.Model.EntityPerception
{
    public class EditPropertyInfo
    {
        /// <summary>
        /// Gets or sets a value that is used to display a description in the UI.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value that is used to group fields in the UI.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets a value that is used for display in the UI.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the order weight of the column.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Gets or sets a value that will be used to set the watermark for prompts in the UI.
        /// </summary>
        public string Prompt { get; set; }

        /// <summary>
        /// Gets or sets the property name.
        /// </summary>
        public string PropertyName
        {
            get
            {
                if (PropertyInfo == null) return null;
                return PropertyInfo.Name;
            }
        }

        /// <summary>
        /// Gets or sets the property type.
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// Gets or sets if the column is readonly.
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets a value that is used for the grid column label.
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets a value that is used to set the length for the input in the UI.
        /// </summary>
        public int ContentLength { get; set; }

        /// <summary>
        ///  Gets or sets a value that is used to set the display format for the input in the UI.
        /// </summary>
        public string DisplayFormat { get; set; }
    }
}