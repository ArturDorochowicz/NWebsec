﻿// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.Filters;
using NWebsec.AspNetCore.Core.Helpers;
using NWebsec.AspNetCore.Core.Web;
using NWebsec.Core.Common.HttpHeaders;
using NWebsec.Core.Common.HttpHeaders.Configuration;
using NWebsec.AspNetCore.Mvc.Internals;
using NWebsec.Mvc.Common.Helpers;

namespace NWebsec.AspNetCore.Mvc
{
    /// <summary>
    /// Specifies whether the X-Frame-Options security header should be set in the HTTP response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class XFrameOptionsAttribute : HttpHeaderAttributeBase
    {
        private readonly IXFrameOptionsConfiguration _config;
        private readonly HeaderConfigurationOverrideHelper _configurationOverrideHelper;
        private readonly HeaderOverrideHelper _headerOverrideHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="XFrameOptionsAttribute"/> class
        /// </summary>
        public XFrameOptionsAttribute()
        {
            _config = new XFrameOptionsConfiguration { Policy = XfoPolicy.Deny };
            _configurationOverrideHelper = new HeaderConfigurationOverrideHelper();
            _headerOverrideHelper = new HeaderOverrideHelper(new CspReportHelper());
        }

        /// <summary>
        /// Gets or sets whether the X-Frame-Options security header should be set in the HTTP response.
        /// Possible values are: Disabled, Deny, SameOrigin. The default is Deny.
        /// </summary>
        public XFrameOptionsPolicy Policy
        {
            get => throw new NotSupportedException();
            set
            {
                switch (value)
                {
                    case XFrameOptionsPolicy.Deny:
                        _config.Policy = XfoPolicy.Deny;
                        break;

                    case XFrameOptionsPolicy.Disabled:
                        _config.Policy = XfoPolicy.Disabled;
                        break;

                    case XFrameOptionsPolicy.SameOrigin:
                        _config.Policy = XfoPolicy.SameOrigin;
                        break;
                    default:
                        throw new ArgumentException("Unknown value: " + value);
                }
            }
        }

        public override void OnActionExecuting(FilterContext filterContext)
        {
            _configurationOverrideHelper.SetXFrameoptionsOverride(new HttpContextWrapper(filterContext.HttpContext), _config);
            base.OnActionExecuting(filterContext);
        }

        public override void SetHttpHeadersOnActionExecuted(FilterContext filterContext)
        {
            _headerOverrideHelper.SetXFrameoptionsHeader(new HttpContextWrapper(filterContext.HttpContext));
        }
    }
}
