using System;
using System.Collections.Generic;
using System.Collections;

namespace RunAsService.Utils
{
    public static class ObjectExtension
    {
        public static bool IsNull(this object obj)
        {
            return (obj == null || obj == DBNull.Value);
        }

        public static bool IsNotNull(this object obj)
        {
            return (obj != null && obj != DBNull.Value);
        }

        #region AsString

        public static string AsString(this object obj)
        {
            return !obj.IsNull() ? obj.ToString() : "";
        }

        public static string AsString(this object obj, string defValue)
        {
            return !obj.IsNull() ? obj.ToString() : defValue;
        }

        #endregion

        #region AsInt

        public static int AsInt(this object obj)
        {
            return obj.AsInt(0);
        }

        public static int AsInt(this object obj, int defValue)
        {
            var res = defValue;
            try
            {
                res = !obj.IsNull() ? Convert.ToInt32(obj) : defValue;
            }
            catch
            {
            }
            return res;
        }

        #endregion

        #region AsFloat = AsDouble

        public static double AsFloat(this object obj)
        {
            return obj.AsDouble(0);
        }

        public static double AsFloat(this object obj, double defValue)
        {
            return obj.AsDouble(defValue);
        }

        public static double AsDouble(this object obj)
        {
            return obj.AsDouble(0);
        }

        public static double AsDouble(this object obj, double defValue)
        {
            var res = defValue;
            try
            {
                if (!obj.IsNull() && Double.TryParse(obj.ToString(), out defValue))
                    res = defValue;
            }
            catch
            {
            }
            return res;
        }

        #endregion

        #region AsDecimal

        public static decimal AsDecimal(this object obj)
        {
            return obj.AsDecimal(0);
        }

        public static decimal AsDecimal(this object obj, decimal defValue)
        {
            var res = defValue;
            try
            {
                if (!obj.IsNull() && Decimal.TryParse(obj.ToString(), out defValue))
                    res = defValue;
            }
            catch
            {
            }
            return res;
        }

        #endregion

        #region AsBool

        public static bool AsBool(this object obj)
        {
            return obj.AsBool(false);
        }

        public static bool AsBool(this object obj, bool defValue)
        {
            var res = defValue;
            try
            {
                if (!obj.IsNull())
                    if (obj.AsString() == "0" || obj.AsString().SameText("false") || obj.AsString() == "")
                        res = false;
                    else
                        res = true;
            }
            catch
            {
            }
            return res;
        }

        #endregion

        #region AsDateTime

        public static DateTime AsDateTime(this object obj)
        {
            return obj.AsDateTime(DateTime.MinValue);
        }

        public static DateTime AsDateTime(this object obj, DateTime defValue)
        {
            var res = defValue;
            try
            {
                if (!obj.IsNull() && DateTime.TryParse(obj.ToString(), out defValue))
                    res = defValue;
            }
            catch
            {
            }
            return DateTime.SpecifyKind(res, DateTimeKind.Unspecified);
        }

        #endregion

        #region AsDate

        /// <summary>
        /// Если время меньше 12-00, то возвращаем указанную дату без времени.
        /// Если после или равно 12-00, то возвращаем указанную дату без времени + 1 день.
        /// </summary>
        public static DateTime RoundDate(this DateTime dt)
        {
            var resDate = dt.Hour < 12 ? dt.Date : dt.Date.AddDays(1);
            resDate = DateTime.SpecifyKind(resDate, DateTimeKind.Unspecified);
            return resDate;
        }

        public static DateTime AsDate(this object obj)
        {
            return obj.AsDate(DateTime.MinValue);
        }

        public static DateTime AsDate(this object obj, DateTime defValue)
        {
            var res = defValue.Date;
            try
            {
                if (!obj.IsNull() && DateTime.TryParse(obj.ToString(), out defValue))
                    res = defValue.Date;
            }
            catch
            {
            }
            return DateTime.SpecifyKind(res, DateTimeKind.Unspecified);
        }

        public static DateTime? AsDateOrNull(this object obj)
        {
            return obj.IsNotNull() ? obj.AsDate() : (DateTime?) null;
        }

        #endregion

        #region AsTime

        public static TimeSpan AsTime(this object obj)
        {
            return obj.AsTime(DateTime.MinValue);
        }

        public static TimeSpan AsTime(this object obj, DateTime defValue)
        {
            var res = defValue.TimeOfDay;
            try
            {
                if (!obj.IsNull() && DateTime.TryParse(obj.ToString(), out defValue))
                    res = defValue.TimeOfDay;
            }
            catch
            {
            }
            return res;
        }

        #endregion

        #region As

        public static T As<T>(this object obj)
        {
            return obj.As(default(T));
        }

        public static T As<T>(this object obj, T defValue)
        {
            var res = defValue;
            try
            {
                if (!obj.IsNull())
                {
                    if (typeof (T) == typeof (string)) res = (T) (object) obj.AsString();
                    else if (typeof (T) == typeof (int) || typeof (T) == typeof (int?)) res = (T) (object) obj.AsInt();
                    else if (typeof (T) == typeof (double) || typeof (T) == typeof (double?))
                        res = (T) (object) obj.AsFloat();
                    else if (typeof (T) == typeof (decimal) || typeof (T) == typeof (decimal?))
                        res = (T) (object) obj.AsDecimal();
                    else if (typeof (T) == typeof (bool) || typeof (T) == typeof (bool?))
                        res = (T) (object) obj.AsBool();
                    else if (typeof (T) == typeof (DateTime) || typeof (T) == typeof (DateTime?))
                        res = (T) (object) obj.AsDateTime();
                    else res = (T) obj;
                }
            }
            catch
            {
            }
            return res;
        }

        #endregion

        #region SameText

        /// <summary>
        /// Сравнивает два объекта как строки без учета регистра
        /// </summary>
        /// <param name="self"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <author>Lion</author>
        public static bool SameText(this object self, object obj)
        {
            return string.Compare(self.AsString(), obj.AsString(), StringComparison.OrdinalIgnoreCase) == 0;
        }

        #endregion
    }

    public static class StringExtension
    {

        #region RightPart

        /// <summary>
        /// Возвращает правую часть строки
        /// </summary>
        /// <param name="S"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RightPart(this string S, int length)
        {
            if (length < 0)
                return "";
            else if (length >= S.Length)
                return S;
            else
                return S.Substring(S.Length - length, length);
        }

        #endregion

    }

    public static class ListOrArrayExtension
    {
        #region ForEach

        /// <summary>
        /// Перечисление элементов IEnumerable
        /// </summary>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (var item in self) action(item);
            return self;
        }

        public static IEnumerable ForEach<T>(this IEnumerable self, Action<T> action)
        {
            foreach (T item in self) action(item);
            return self;
        }

        #endregion
    }

}
