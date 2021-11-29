using System;
using System.Reflection;
using StockTracker.Core.Domain.Interfaces;

namespace StockTracker.Core.Domain
{
    public abstract class BaseObject:ITradingStructure
    {
        protected PropertyInfo[] properties;
        protected DateTime _activityDate;

        public BaseObject()
        {
            //Load the property list so we can all for access by name
            Type type = this.GetType();
            properties = type.GetProperties();
        }

        //Properties
        public DateTime ActivityDate { get => _activityDate; set => _activityDate = value; }  
        public int Id { get; set; }

        /// <summary>
        /// Gets the value of a property based on the properties name
        /// </summary>
        /// <param name="PropertyName">Name of the property</param>
        /// <returns>An object repersenting the value of the property or null if the property is not found</returns>
        public virtual object GetValue(string PropertyName)
        {
            if (PropertyName.Trim() == String.Empty) return null;

            return FindValue(0, properties.Length, PropertyName);
        }

        /// <summary>
        /// Gets the value of a numeric property based on the property's name
        /// </summary>
        /// <param name="PropertyName">Name of the property</param>
        /// <returns>An object repersenting the value of the property or null if the property is not found</returns>
        public float GetFloatValue(String PropertyName)
        {
            try
            {
                return (float)GetValue(PropertyName);
            }
            catch
            {
                return 0;
            }
        }

        private object FindValue(int start, int end, string PropertyName)
        {
            //We have searched all of the properties and nothing was found;
            if (start > end) return null;

            //We found it stop looking
            if (properties[start].Name.ToLower().Equals(PropertyName.ToLower())) return properties[start].GetValue(this);

            //keep looking
            return FindValue(start + 1, end, PropertyName);
        }

      
    }
}
