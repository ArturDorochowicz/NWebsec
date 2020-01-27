﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.Filters;
using NWebsec.AspNetCore.Core.Helpers;
using NWebsec.AspNetCore.Core.Web;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.AspNetCore.Mvc.Internals;
using NWebsec.Mvc.Common.Helpers;

namespace NWebsec.AspNetCore.Mvc
{
    /// <summary>
    /// Specifies whether the X-Content-Type-Options security header should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class XContentTypeOptionsAttribute : HttpHeaderAttributeBase
    {
        private readonly SimpleBooleanConfiguration _config;
        private readonly HeaderConfigurationOverrideHelper _headerConfigurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="XContentTypeOptionsAttribute"/> class
        /// </summary>
        public XContentTypeOptionsAttribute()
        {
            _config = new SimpleBooleanConfiguration { Enabled = true };
            _headerConfigurationOverrideHelper = new HeaderConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper(new CspReportHelper());
        }
        /// <summary>
        /// Gets or sets whether the X-Content-Type-Options security header should be set in the HTTP response. The default is true.
        /// </summary>
        public bool Enabled { get => _config.Enabled; set => _config.Enabled = value; }

        public override void OnActionExecuting(FilterContext filterContext)
        {
            _headerConfigurationOverrideHelper.SetXContentTypeOptionsOverride(new HttpContextWrapper(filterContext.HttpContext), _config);
            base.OnActionExecuting(filterContext);
        }

        public override void SetHttpHeadersOnActionExecuted(FilterContext filterContext)
        {
            _headerOverrideHelper.SetXContentTypeOptionsHeader(new HttpContextWrapper(filterContext.HttpContext));
        }
    }
}
