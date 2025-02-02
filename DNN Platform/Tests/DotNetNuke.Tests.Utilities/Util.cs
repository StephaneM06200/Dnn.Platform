﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information

namespace DotNetNuke.Tests.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public static class Util
    {
        public static List<int> CreateIntegerList(int count)
        {
            var list = new List<int>();
            for (int i = 0; i < count; i++)
            {
                list.Add(i);
            }

            return list;
        }

        public static TMember GetPrivateMember<TInstance, TMember>(TInstance instance, string fieldName)
        {
            // retrieve private field from class
            var member = typeof(TInstance).GetMember(fieldName, BindingFlags.NonPublic | BindingFlags.Instance)[0];
            if (member is PropertyInfo property)
            {
                return (TMember)property.GetValue(instance, null);
            }

            if (member is FieldInfo field)
            {
                return (TMember)field.GetValue(instance);
            }

            return default(TMember);
        }

        public static void SetPrivateMember<TInstance, TField>(TInstance instance, string fieldName, TField value)
        {
            Type type = typeof(TInstance);

            BindingFlags privateBindings = BindingFlags.NonPublic | BindingFlags.Instance;

            // retrieve private field from classS
            FieldInfo field = type.GetField(fieldName, privateBindings);

            field.SetValue(instance, value);
        }

        public static string GetFileName(string testFilesFolder, string fileName)
        {
            string fullName = string.Format("{0}\\{1}", testFilesFolder, fileName);
            if (!fullName.ToLowerInvariant().EndsWith(".csv") && !fullName.ToLowerInvariant().EndsWith(".sql"))
            {
                fullName += ".csv";
            }

            return fullName;
        }

        public static Stream GetFileStream(string testFilesFolder, string fileName)
        {
            var filePath = GetFileName(testFilesFolder, fileName);
            FileStream stream = null;
            if (File.Exists(filePath))
            {
                stream = new FileStream(GetFileName(testFilesFolder, fileName), FileMode.Open, FileAccess.Read);
            }

            return stream;
        }

        public static string ReadStream(string testFilesFolder, string fileName)
        {
            string text = string.Empty;
            Stream stream = GetFileStream(testFilesFolder, fileName);
            if (stream != null)
            {
                using (var reader = new StreamReader(GetFileStream(testFilesFolder, fileName)))
                {
                    text = reader.ReadToEnd();
                }
            }

            return text;
        }

        public static void ReadStream(string testFilesFolder, string fileName, Action<string, string> onReadLine)
        {
            string text = string.Empty;
            var stream = GetFileStream(testFilesFolder, fileName);
            if (stream != null)
            {
                using (var reader = new StreamReader(GetFileStream(testFilesFolder, fileName)))
                {
                    string line;
                    string header = string.Empty;
                    int count = 0;
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Ignore first line
                        if (count > 0)
                        {
                            onReadLine(line, header);
                        }
                        else
                        {
                            header = line;
                        }

                        count++;
                    }
                }
            }
        }
    }
}
