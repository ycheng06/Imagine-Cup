/*
 * Team Name: EOS
 * Team Memebers: Jason Cheng, Gregory Wong, Xihan Zhang, Wenshiang Chung
 * E-mail: eos_imaginecup@hotmail.com
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;

namespace MediviseMVC.Models
{
    public class DateTimeConversionBinder: DefaultModelBinder
    {
        protected override void SetProperty(ControllerContext controllerContext,
      ModelBindingContext bindingContext,
      System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
        {
            //we probably will want to get the user's time zone from the database later
            TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            if (value != null)//make sure that if an invalid date (ie. 4/31 or 2/31) is submitted, it will be dealt with as such
            {
                if (propertyDescriptor.PropertyType == typeof(DateTime))
                {
                    var newTime = (DateTime)value;
                    value = TimeZoneInfo.ConvertTimeToUtc(newTime, userTimeZone);
                }
            }

            base.SetProperty(controllerContext, bindingContext,
                                propertyDescriptor, value);
        }
        
        //probably will not be used
        protected override object GetPropertyValue( ControllerContext controllerContext,
    ModelBindingContext bindingContext,
    PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            var submittedValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            //we probably will want to get the user's time zone from the database later
            TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            /*
            if (propertyDescriptor.PropertyType == typeof(DateTime))
            {
                var newTime = (DateTime)submittedValue.ConvertTo(typeof(DateTime));
                return TimeZoneInfo.ConvertTimeFromUtc(newTime, userTimeZone);
            }
            */

            return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }
    }
}