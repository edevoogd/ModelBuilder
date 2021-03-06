﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.IO;
using Xunit;

namespace Microsoft.OData.ModelBuilder.Tests.PublicApi
{
    public class PublicApiTest
    {
        private const string AssemblyName = "Microsoft.OData.ModelBuilder.dll";
        private const string OutputFileName = "Microsoft.OData.ModelBuilder.PublicApi.out";
        private const string BaseLineFileName = "Microsoft.OData.ModelBuilder.PublicApi.bsl";
        private const string BaseLineFileFolder = @"test\Microsoft.OData.ModelBuilder.Tests\PublicApi\";

        [Fact]
        public void TestPublicApi()
        {
            // Arrange
            string outputPath = Environment.CurrentDirectory;
            string outputFile = outputPath + Path.DirectorySeparatorChar + OutputFileName;

            // Act
            using (FileStream fs = new FileStream(outputFile, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    string assemblyPath = outputPath + Path.DirectorySeparatorChar + AssemblyName;
                    Assert.True(File.Exists(assemblyPath), string.Format("{0} does not exist in current directory", assemblyPath));
                    PublicApiHelper.DumpPublicApi(sw, assemblyPath);
                }
            }
            string outputString = File.ReadAllText(outputFile);
            string baselineString = GetBaseLineString();

            // Assert
            Assert.True(String.Compare(baselineString, outputString, StringComparison.Ordinal) == 0,
                String.Format("Base line file {1} and output file {2} do not match, please check.{0}" +
                "To update the baseline, please run:{0}{0}" +
                "copy /y \"{2}\" \"{1}\"", Environment.NewLine,
                BaseLineFileFolder + BaseLineFileName,
                outputFile));
        }

        private string GetBaseLineString()
        {
            const string projectDefaultNamespace = "Microsoft.OData.ModelBuilder.Tests";
            const string resourcesFolderName = "PublicApi";
            const string pathSeparator = ".";
            string path = projectDefaultNamespace + pathSeparator + resourcesFolderName + pathSeparator + BaseLineFileName;

            using (Stream stream = typeof(PublicApiTest).Assembly.GetManifestResourceStream(path))
            {
                if (stream == null)
                {
                    string message = Error.Format("The embedded resource '{0}' was not found.", path);
                    throw new FileNotFoundException(message, path);
                }

                using (TextReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
