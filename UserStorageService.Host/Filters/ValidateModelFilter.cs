using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace UserStorageService.Host.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                actionContext.Response = BadRequest();
            }
        }

        private HttpResponseMessage BadRequest()
        {
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}
