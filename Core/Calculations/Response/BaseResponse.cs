using System;
using System.Reflection;
using StockTracker.Core.Interfaces.Calculations;

namespace StockTracker.Core.Calculations.Response
{
    public abstract class BaseResponse:IResponse
    {
        protected PropertyInfo[] properties;

        public BaseResponse()
        {
            //Load the property list so we can all for access by name
            Type type = this.GetType();
            properties = type.GetProperties();
        }

        public DateTime ActivityDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets the value of a numeric property based on the property's name
        /// </summary>
        /// <param name="PropertyName">Name of the property</param>
        /// <returns>An object repersenting the value of the property.  0 if the property is not found or cast fails.</returns>
        public float GetFloatValue(string PropertyName)
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

        /// <summary>
        /// Gets the value of a property based on the properties name
        /// </summary>
        /// <param name="PropertyName">Name of the property</param>
        /// <returns>An object repersenting the value of the property or null if the property is not found</returns>
        public virtual object GetValue(string PropertyName)
        {
            if (PropertyName.Trim() == String.Empty) return null;

            return FindValue(0, properties.Length -1 , PropertyName);
        }

        /// <summary>
        /// Sets the value of a Float value property
        /// </summary>
        /// <param name="propertyName">The name of the propety to update</param>
        /// <param name="value">The value to assign to the property</param>
        public void SetFloatValue(string propertyName, float value)
        {
            SetValue(0, properties.Length - 1, propertyName, value);
        }

        private void SetValue(int start, int end, string propertyName, object value)
        {
            //We have searched all of the properties and nothing was found;
            if (start > end) return;

            //We found it stop looking
            if (properties[start].Name.ToLower().Equals(propertyName.ToLower()))
            {
                properties[start].SetValue(this, value);
                return;
            }

            //keep looking
            SetValue(start + 1, end, propertyName, value);
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
