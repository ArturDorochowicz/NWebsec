﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using NWebsec.AspNetCore.Mvc.Internals;
using Xunit;

namespace NWebsec.AspNetCore.Mvc.Tests.Internals
{
    public class HttpHeaderAttributeBaseTests
    {
        private readonly Mock<HttpHeaderAttributeBase> _httpHeaderAttributeBaseMock;
        private readonly ActionExecutedContext _actionExecutedContext;

        public HttpHeaderAttributeBaseTests()
        {
            _httpHeaderAttributeBaseMock = new Mock<HttpHeaderAttributeBase> { CallBase = true };
            var mockContext = new Mock<HttpContext>();
            mockContext.Setup(c => c.Items).Returns(new Dictionary<object, object>());
            _actionExecutedContext = new ActionExecutedContext(new ActionContext
            {
                HttpContext = mockContext.Object,
                RouteData = new RouteData(),
                ActionDescriptor = new ActionDescriptor()
            },
                new List<IFilterMetadata>(), null);
        }

        [Fact]
        public void OnActionExecuted_CallsSetHttpHeadersOnActionExecuted()
        {

            ((IActionFilter)_httpHeaderAttributeBaseMock.Object).OnActionExecuted(_actionExecutedContext);

            _httpHeaderAttributeBaseMock.Verify(m => m.SetHttpHeadersOnActionExecuted(_actionExecutedContext), Times.Once);
        }

        [Fact]
        public void OnActionExecuted_CallsSetHttpHeadersOnceOnMultipleOnActionExecuted()
        {

            ((IActionFilter)_httpHeaderAttributeBaseMock.Object).OnActionExecuted(_actionExecutedContext);
            ((IActionFilter)_httpHeaderAttributeBaseMock.Object).OnActionExecuted(_actionExecutedContext);

            _httpHeaderAttributeBaseMock.Verify(m => m.SetHttpHeadersOnActionExecuted(_actionExecutedContext), Times.Once);
        }

        [Fact]
        public void ContextKeyIdentifier_ReturnsClassName()
        {
            var testAttribute = new TestHeaderAttribute();

            Assert.Equal("TestHeaderAttribute", testAttribute.ContextKeyIdentifier);
        }

        protected class TestHeaderAttribute : HttpHeaderAttributeBase
        {
            public override void SetHttpHeadersOnActionExecuted(FilterContext filterContext)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}