﻿using System;
using System.Collections;
using System.Collections.Generic;
using EuNet.Core;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SerializerTests
    {
        private void UsedOnlyForAOTCodeGeneration()
        {
            new TupleFormatter<int, string>();
            new ValueTupleFormatter<int, string>();
            new DictionaryFormatter<int, string>();
            throw new InvalidOperationException("This method is used for AOT code generation only. Do not call it at runtime.");
        }

        [Test]
        public void TestByteList()
        {
            List<byte> data = new List<byte>()
            {
                1,2,3,4,5,6,7,8,9,10
            };

            var deserializeObject = TestBase<List<byte>>(data);
            Assert.AreEqual(data.Count, deserializeObject.Count);

            for (int i = 0; i < data.Count; i++)
            {
                Assert.AreEqual(data[i], deserializeObject[i]);
            }
        }

        [Test]
        public void TestStringList()
        {
            List<string> data = new List<string>()
            {
                "1","2","3","4","5","6","7","8","9","10"
            };

            var deserializeObject = TestBase<List<string>>(data);
            Assert.AreEqual(data.Count, deserializeObject.Count);

            for (int i = 0; i < data.Count; i++)
            {
                Assert.AreEqual(data[i], deserializeObject[i]);
            }
        }

        [Test]
        public void TestDictionary()
        {
            Dictionary<int, string> data = new Dictionary<int, string>()
            {
                { 1, "_1" },
                { 2, "_2" },
                { 3, "_3" },
                { 4, "_4" },
                { 5, "_5" },
                { 6, "_6" },
            };

            var deserializeObject = TestBase<Dictionary<int, string>>(data);
            Assert.AreEqual(data.Count, deserializeObject.Count);

            foreach (var kvp in data)
            {
                Assert.AreEqual(kvp.Value, deserializeObject[kvp.Key]);
            }
        }

        [Test]
        public void TestTuple()
        {
            Tuple<int, string> data = new Tuple<int, string>(123, "123");

            var deserializeObject = TestBase<Tuple<int, string>>(data);

            Assert.AreEqual(data.Item1, deserializeObject.Item1);
            Assert.AreEqual(data.Item2, deserializeObject.Item2);
        }

        [Test]
        public void TestValueTuple()
        {
            ValueTuple<int, string> data = new ValueTuple<int, string>(123, "123");

            var deserializeObject = TestBase<ValueTuple<int, string>>(data);

            Assert.AreEqual(data.Item1, deserializeObject.Item1);
            Assert.AreEqual(data.Item2, deserializeObject.Item2);
        }

        private T TestBase<T>(T data)
        {
            var writer = NetPool.DataWriterPool.Alloc();
            try
            {
                NetDataSerializer.Serialize<T>(writer, data);
            }
            finally
            {
                NetPool.DataWriterPool.Free(writer);
            }

            byte[] buffer = writer.CopyData();

            var reader = new NetDataReader(buffer);
            return NetDataSerializer.Deserialize<T>(reader);
        }
    }
}
