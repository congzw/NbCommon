using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common
{
    public static class TestExtensions
    {
        public static void ShouldThrows<T>(this Action action) where T : Exception
        {
            AssertHelper.ShouldThrows<T>(action);
        }

        public static object ShouldNull(this object value, string appendMessage = null)
        {
            AssertHelper.WriteLineForShouldBeNull(value, appendMessage);
            Assert.IsNull(value);
            return value;
        }

        public static object ShouldNotNull(this object value, string appendMessage = null)
        {
            AssertHelper.WriteLineForShouldBeNotNull(value, appendMessage);
            Assert.IsNotNull(value);
            return value;
        }

        public static object ShouldEqual(this object value, object expectedValue)
        {
            string message = string.Format("Should {0} equals {1}", value, expectedValue);
            Assert.AreEqual(expectedValue, value, message.WithKoPrefix());
            AssertHelper.WriteLineOk(message);
            return value;
        }

        public static object ShouldNotEqual(this object value, object expectedValue)
        {
            string message = string.Format("Should {0} not equals {1}", value, expectedValue);
            Assert.AreNotEqual(expectedValue, value, message.WithKoPrefix());
            AssertHelper.WriteLineOk(message);
            return value;
        }

        //public static void ShouldNbEqual(this string value, string str2)
        //{
        //    var nbEquals = value.NbEquals(str2);
        //    Debug.WriteLine("{0} nbqueal {1} ? {2}", value ?? "null", str2 ?? "null", nbEquals);
        //    Assert.IsTrue(nbEquals);
        //}

        //public static void ShouldNotNbEqual(this string value, string str2)
        //{
        //    var nbEquals = value.NbEquals(str2);
        //    Debug.WriteLine("{0} nbqueal {1} ? {2}", value ?? "null", str2 ?? "null", nbEquals);
        //    Assert.IsFalse(nbEquals);
        //}


        public static object ShouldSame(this object value, object expectedValue)
        {
            if (value == null || expectedValue == null)
            {
                Assert.AreNotSame(expectedValue, value);
                return value;
            }
            string message = string.Format("Should Same [{0}] => <{1}> : <{2}>", value.GetType().Name, value.GetHashCode(), expectedValue.GetHashCode());
            Assert.AreSame(expectedValue, value, message.WithKoPrefix());
            AssertHelper.WriteLine(message.WithOkPrefix());
            return value;
        }

        public static object ShouldNotSame(this object value, object expectedValue)
        {
            if (value == null || expectedValue == null)
            {
                Assert.AreNotSame(expectedValue, value);
                return value;
            }
            string message = string.Format("Should Not Same [{0}] => <{1}> : <{2}>", value.GetType().Name, value.GetHashCode(), expectedValue.GetHashCode());
            Assert.AreNotSame(expectedValue, value, message.WithKoPrefix());
            AssertHelper.WriteLine(message.WithOkPrefix());
            return value;
        }

        public static void ShouldTrue(this bool result, string appendMessage = null)
        {
            AssertHelper.WriteLineForShouldBeTrue(result, appendMessage);
            Assert.IsTrue(result);
        }

        public static void ShouldFalse(this bool result, string appendMessage = null)
        {
            AssertHelper.WriteLineForShouldBeFalse(result, appendMessage);
            Assert.IsFalse(result);
        }

        public static object LogHashCode(this object value)
        {
            string message = string.Format("{0} <{1}>", value.GetHashCode(), value.GetType().Name);
            AssertHelper.WriteLine(message);
            return value;
        }
        public static object LogHashCodeWiths(this object value, object value2)
        {
            string message = string.Format("{0} <{1}> {2} {3}<{4}>", value.GetHashCode(), value.GetType().Name, value == value2 ? "==" : "!=", value2.GetHashCode(), value2.GetType().Name);
            AssertHelper.WriteLine(message);
            return value;
        }

        public static object Log(this object value)
        {
            if (value == null)
            {
                Debug.WriteLine("null");
            }

            if (value is string)
            {
                Debug.WriteLine(value);
                return value;
            }

            var items = value as IEnumerable;
            if (items != null)
            {
                foreach (var item in items)
                {
                    Debug.WriteLine(item);
                }
                return value;
            }
            Debug.WriteLine(value);
            return value;
        }
        public static object LogProperties(this object value)
        {

            if (value == null)
            {
                Debug.WriteLine("null");
            }

            if (value is string)
            {
                Debug.WriteLine(value);
                return value;
            }

            var items = value as IEnumerable;
            if (items != null)
            {
                foreach (var item in items)
                {
                    LogProperties(item);
                }
                return value;
            }


            var props = GetPropertyNameValue(value);
            var stringBuilder = new StringBuilder();
            int loopIndex = 0;
            foreach (var prop in props)
            {
                loopIndex++;
                int? collCount = null;
                var o = prop.Value;
                if (o != null)
                {
                    var type = o.GetType();
                    //shouldAppend = type == typeof(string) || !(o is IEnumerable);
                    if (type != typeof(string))
                    {
                        var enumerable = o is IEnumerable;
                        if (enumerable)
                        {
                            var coll = o as ICollection;
                            if (coll != null)
                            {
                                collCount = coll.Count;
                            }
                        }
                    }
                }

                if (collCount.HasValue)
                {
                    stringBuilder.AppendFormat("{0}=[{1}]", prop.Key, collCount);
                }
                else
                {
                    stringBuilder.AppendFormat("{0}={1}", prop.Key, prop.Value);
                }

                if (loopIndex != props.Count)
                {
                    stringBuilder.Append(", ");
                }
            }
            Debug.WriteLine(stringBuilder.ToString());
            return value;
        }

        public static string WithOkPrefix(this string value)
        {
            return AssertHelper.PrefixOk(value);
        }
        public static string WithKoPrefix(this string value)
        {
            return AssertHelper.PrefixKo(value);
        }
        public static string WithPrefix(this string value, bool isOk = true)
        {
            return AssertHelper.PrefixKo(value);
        }
        public static string ObjectInfo(this object obj)
        {
            return string.Format("<{0},{1}>", obj.GetType().Name, obj.GetHashCode());
        }



        internal static IDictionary<string, object> GetPropertyNameValue<T>(T obj)
        {
            var result = new Dictionary<string, object>();
            if (obj != null)
            {
                //获取类型信息
                Type t = typeof(T);

                if (t == typeof(object))
                {
                    t = obj.GetType();
                }

                PropertyInfo[] propertyInfos = t.GetProperties();
                foreach (PropertyInfo var in propertyInfos)
                {
                    result.Add(var.Name, var.GetValue(obj, null));
                }
            }
            return result;
        }
    }
}