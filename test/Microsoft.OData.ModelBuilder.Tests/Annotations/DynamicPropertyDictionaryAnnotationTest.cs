﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using System.Reflection;
using Microsoft.OData.ModelBuilder.Tests.Commons;
using Moq;
using Xunit;

namespace Microsoft.OData.ModelBuilder.Tests.Annotations
{
    public class DynamicPropertyDictionaryAnnotationTest
    {
        [Fact]
        public void Ctor_ThrowsForNullPropertyInfo()
        {
            ExceptionAssert.ThrowsArgumentNull(
                () => new DynamicPropertyDictionaryAnnotation(propertyInfo: null),
                "propertyInfo");
        }

        [Fact]
        public void Ctor_ThrowsForNotIDictionaryPropretyInfo()
        {
            // Arrange
            Mock<PropertyInfo> propertyInfo = new Mock<PropertyInfo>();
            propertyInfo.Setup(p => p.PropertyType).Returns(typeof(int));

            // Act & Assert
            ExceptionAssert.Throws<ArgumentException>(() => new DynamicPropertyDictionaryAnnotation(
                propertyInfo: propertyInfo.Object),
                "Type 'Int32' is not supported as dynamic property annotation. " +
                "Referenced property must be of type 'IDictionary<string, object>'. (Parameter 'propertyInfo')");
        }
    }
}
