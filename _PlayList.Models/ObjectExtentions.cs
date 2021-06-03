using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PlayList.Models
{

    /// <summary>
    /// 对象的扩展方法
    /// 2021年4月24日18:16:04
    /// </summary>
    public static class ObjectExtention
    {
        #region 对象判空
        /// <summary>
        /// 判断对象是否为空
        /// </summary>
        /// <param name="thisObj"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object thisObj)
        {
            if (thisObj is string)
            {
                return string.IsNullOrWhiteSpace(Convert.ToString(thisObj));
            }
            if (thisObj is int)
            {
                return Convert.ToInt32(thisObj) == 0;
            }
            if (thisObj is double)
            {
                return Convert.ToDouble(thisObj) == 0;
            }
            if (thisObj is Boolean)
            {
                return !Convert.ToBoolean(thisObj);
            }
            if (thisObj is DBNull)
            {
                return true;
            }
            return thisObj == null;
        }
        /// <summary>
        /// 判断列表是否为空
        /// </summary>
        /// <param name="thisList"></param>
        /// <returns></returns>
        public static bool IsListNull<T>(this List<T> thisList) where T : class, new()
        {
            return thisList == null || thisList.Count == 0;
        }
        /// <summary>
        /// 判断对象不为空
        /// </summary>
        /// <param name="thisObj"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this object thisObj)
        {
            return !thisObj.IsNullOrEmpty();
        }
        /// <summary>
        /// 判断列表不为空
        /// </summary>
        /// <param name="thisList"></param>
        /// <returns></returns>
        public static bool IsListNotNull<T>(this List<T> thisList) where T : class, new()
        {
            return !thisList.IsListNull();
        }
        #endregion

        #region 类实例
        /// <summary>
        /// 类实例的ToString方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        [Obsolete("请使用对象转Json的扩展方法")]
        public static string ToString<T>(this T instance) where T : class, new()
        {
            //这里利用反射
            StringBuilder result = new StringBuilder();
            if (instance == null) { return ""; }
            PropertyInfo[] properties = instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0) { return null; }
            foreach (PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(instance, null);
                result.Append($" {name}:{value}, ");

            }
            return result.ToString();
        }
        #endregion
    }
}