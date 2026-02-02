using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel;
using System.Reflection;

namespace SurPathWeb
{
    public static class Helper
    {
        ///<summary>
        /// Base 64 Encoding with URL and Filename Safe Alphabet using UTF-8 character set.
        ///</summary>
        ///<param name="str">The origianl string</param>
        ///<returns>The Base64 encoded string</returns>
        public static string Base64ForUrlEncode(string str)
        {
            byte[] encbuff = Encoding.UTF8.GetBytes(str);
            string encvalue = HttpServerUtility.UrlTokenEncode(encbuff);
            if (encvalue.Contains("/"))
            {
                encvalue = encvalue.Replace("/", "=");
            }
            return encvalue;
        }
        ///<summary>
        /// Decode Base64 encoded string with URL and Filename Safe Alphabet using UTF-8.
        ///</summary>
        ///<param name="str">Base64 code</param>
        ///<returns>The decoded string.</returns>
        public static string Base64ForUrlDecode(string str)
        {
            if (str.Contains("="))
            {
                str = str.Replace("=", "/");
            }
            byte[] decbuff = HttpServerUtility.UrlTokenDecode(str);
            return Encoding.UTF8.GetString(decbuff);
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string ToDescriptionString(this SurPath.Enum.DonorRegistrationStatus val)
        {
            System.ComponentModel.DescriptionAttribute[] attributes = (System.ComponentModel.DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }            

        // this class from internal TemplateHelpers class in System.Web.Mvc namespace
        private class ViewDataContainer : IViewDataContainer
        {
            public ViewDataContainer(ViewDataDictionary viewData)
            {
                ViewData = viewData;
            }

            public ViewDataDictionary ViewData { get; set; }
        }

        private static HtmlHelper htmlHelper;

        public static HtmlHelper CreateActionLink(Controller controller)
        {
            //if (htmlHelper == null)
            //{
                var vdd = new ViewDataDictionary();
                var tdd = new TempDataDictionary();
                var controllerContext = controller.ControllerContext;
                var view = new RazorView(controllerContext, "/", "/", false, null);
                htmlHelper = new HtmlHelper(new ViewContext(controllerContext, view, vdd, tdd, new StringWriter()),
                     new ViewDataContainer(vdd), RouteTable.Routes);
            //}
            return htmlHelper;
        }

        public static HtmlHelper CreateActionLink(Controller controller, object model)
        {
            if (htmlHelper == null || htmlHelper.ViewData.Model == null || !htmlHelper.ViewData.Model.Equals(model))
            {
                var vdd = new ViewDataDictionary();
                vdd.Model = model;
                var tdd = new TempDataDictionary();
                var controllerContext = controller.ControllerContext;
                var view = new RazorView(controllerContext, "/", "/", false, null);
                htmlHelper = new HtmlHelper(new ViewContext(controllerContext, view, vdd, tdd, new StringWriter()),
                     new ViewDataContainer(vdd), RouteTable.Routes);
            }
            return htmlHelper;
        }

        /// <summary>
        /// Get description form data model for a particular property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        {
            var attrType = typeof(T);
            var property = instance.GetType().GetProperty(propertyName);
            return (T)property.GetCustomAttributes(attrType, false).First();
        }
    }
}