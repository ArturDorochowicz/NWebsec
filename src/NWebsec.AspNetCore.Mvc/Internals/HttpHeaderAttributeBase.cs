// Copyright (c) André N. Klingsheim. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NWebsec.AspNetCore.Mvc.Internals
{
    public abstract class HttpHeaderAttributeBase : Attribute, IOrderedFilter, IActionFilter, IPageFilter
    {
        private static readonly Object MarkerObject = new Object();
        private string _contextKey;

        public int Order { get; set; }

        private string ContextKey => _contextKey ?? (_contextKey = "NWebsecHeaderSet" + ContextKeyIdentifier);

        internal virtual string ContextKeyIdentifier => GetType().Name;

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
            OnActionExecuting(context);
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            var httpContext = context.HttpContext;
            if (!httpContext.Items.ContainsKey(ContextKey))
            {
                httpContext.Items[ContextKey] = MarkerObject;
                SetHttpHeadersOnActionExecuted(context);
            }
        }

        void IPageFilter.OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
        }

        void IPageFilter.OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            OnActionExecuting(context);
        }

        void IPageFilter.OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            var httpContext = context.HttpContext;
            if (!httpContext.Items.ContainsKey(ContextKey))
            {
                context.HttpContext.Items[ContextKey] = MarkerObject;
                SetHttpHeadersOnActionExecuted(context);
            }
        }

        public virtual void OnActionExecuting(FilterContext filterContext)
        {
        }

        public abstract void SetHttpHeadersOnActionExecuted(FilterContext filterContext);

        /// <summary>
        /// Creates an exception with message prefixed with the current type name, to give a hint about the current attribute.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected Exception CreateAttributeException(string message, Exception e = null)
        {
            var errorMessage = $"{GetType().Name}: {message}";
            if (e != null)
            {
                errorMessage += "\nDetails: " + e.Message;
            }
            return new ArgumentException(errorMessage);
        }
    }
}
