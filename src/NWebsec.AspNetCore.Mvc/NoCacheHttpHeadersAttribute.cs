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
    /// Specifies whether appropriate headers to prevent browser caching should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class NoCacheHttpHeadersAttribute : HttpHeaderAttributeBase
    {
        private readonly SimpleBooleanConfiguration _config;
        private readonly HeaderConfigurationOverrideHelper _configurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="NoCacheHttpHeadersAttribute"/> class
        /// </summary>
        public NoCacheHttpHeadersAttribute()
        {
            _config = new SimpleBooleanConfiguration { Enabled = true };
            _configurationOverrideHelper = new HeaderConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper(new CspReportHelper());
        }

        /// <summary>
        /// Gets of sets whether cache headers should be included in the response to prevent browser caching. The default is true.
        /// </summary>
        public bool Enabled { get => _config.Enabled; set => _config.Enabled = value; }

        public override void OnActionExecuting(FilterContext filterContext)
        {
            _configurationOverrideHelper.SetNoCacheHeadersOverride(new HttpContextWrapper(filterContext.HttpContext), _config);
            base.OnActionExecuting(filterContext);
        }

        public override void SetHttpHeadersOnActionExecuted(FilterContext filterContext)
        {
            _headerOverrideHelper.SetNoCacheHeaders(new HttpContextWrapper(filterContext.HttpContext));
        }
    }
}
