using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NbPilot.Common
{
    public class ObjectIntanceTestResult
    {
        public static ObjectIntanceTestResult Create(Object object1, object object2)
        {
            var resolveDemoTestResult = new ObjectIntanceTestResult
            {
                CreateInThreadId = Thread.CurrentThread.ManagedThreadId,
                Object1 = object1,
                Object2 = object2
            };
            return resolveDemoTestResult;
        }
        public static ObjectIntanceTestResult CreateWithFunc(Func<Object> createFunc1, Func<Object> createFunc2)
        {
            var resolveDemoTestResult = new ObjectIntanceTestResult
            {
                CreateInThreadId = Thread.CurrentThread.ManagedThreadId,
                Object1 = createFunc1.Invoke(),
                Object2 = createFunc2.Invoke()
            };
            return resolveDemoTestResult;
        }

        public Object Object1 { get; set; }
        public Object Object2 { get; set; }
        public int CreateInThreadId { get; set; }

        public void ShouldSame()
        {
            var desc1 = Object1 == null ? "Null" : Object1.GetHashCode().ToString();
            var desc2 = Object2 == null ? "Null" : Object2.GetHashCode().ToString();
            var message = string.Format("(Thread:{0}): {1} == {2}", CreateInThreadId.ToString("000"), desc1, desc2);
            var isOkMessage = Object1 == Object2 ? AssertHelper.PrefixOk(message) : AssertHelper.PrefixKo(message);
            AssertHelper.WriteLine(isOkMessage);
            Assert.AreSame(Object1, Object2);
        }

        public void ShouldNotSame()
        {
            var message = string.Format("(Thread:{0}): {1} != {2}", CreateInThreadId.ToString("000"), Object1.GetHashCode(), Object2.GetHashCode());
            var isOkMessage = Object1 != Object2 ? AssertHelper.PrefixOk(message) : AssertHelper.PrefixKo(message);
            AssertHelper.WriteLine(isOkMessage);
            Assert.AreNotSame(Object1, Object2);
        }

        public static void RunTestInMultiTasks(int multiThreadTaskCount, Func<Object> createFunc1, Func<Object> createFunc2, bool shouldSame)
        {
            List<Task> tasks = new List<Task>();
            ConcurrentBag<ObjectIntanceTestResult> testResults = new ConcurrentBag<ObjectIntanceTestResult>();
            for (int i = 0; i < multiThreadTaskCount; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    var resolveDemoTestResult = CreateWithFunc(createFunc1, createFunc2);
                    testResults.Add(resolveDemoTestResult);
                }));
            }
            Task.WaitAll(tasks.ToArray());

            foreach (var testResult in testResults)
            {
                if (shouldSame)
                {
                    testResult.ShouldSame();
                }
                else
                {
                    testResult.ShouldNotSame();
                }
            }
        }
    }
}
