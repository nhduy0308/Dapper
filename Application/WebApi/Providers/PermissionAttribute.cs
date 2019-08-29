using Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using System.Web.Http.Controllers;
using WebAPI.Infrastructure.Core;
using WebAPI.Models;

namespace WebAPI.Providers
{
    public class PermissionAttribute : AuthorizeAttribute
    {
        public RoleEnum Function;
        public ActionEnum Action;

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
            void NoAuthorize(Exception ex = null)
            {
                HttpResponseMessage responseMessage = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Unauthorized,
                    Content = new StringContent(JsonConvert.SerializeObject(new Result
                    {
                        Error = new ResultError
                        {
                            Code = ex == null ? (int)HttpStatusCode.Unauthorized : (int)HttpStatusCode.InternalServerError,
                            Message = ex?.Message ?? HttpStatusCode.Unauthorized.ToString()
                        },
                        Message = ex?.Message ?? HttpStatusCode.Unauthorized.ToString(),
                        IsSuccess = false
                    }))
                };
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                actionContext.Response = responseMessage;
            }
            try
            {
                var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;
                if (!principal.Identity.IsAuthenticated)
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
                else
                {
                    var roles = JsonConvert.DeserializeObject<List<string>>(principal.FindFirst("roles").Value);
                    if (roles.Count > 0)
                    {
                        if (!principal.Equals(RoleEnum.Admin.ToString()))
                        {
                            var permissions = JsonConvert.DeserializeObject<List<PermissionViewModel>>(principal.FindFirst("permissions").Value);
                            if (!permissions.Exists(x => x.FunctionId == Function.ToString() && x.CanCreate) && Action == ActionEnum.Create)
                            {
                                NoAuthorize();
                                return;

                            }
                            else if (!permissions.Exists(x => x.FunctionId == Function.ToString() && x.CanRead) && Action == ActionEnum.Read)
                            {
                                NoAuthorize();
                                return;
                            }
                            else if (!permissions.Exists(x => x.FunctionId == Function.ToString() && x.CanDelete) && Action == ActionEnum.Delete)
                            {
                                NoAuthorize();
                                return;
                            }
                            else if (!permissions.Exists(x => x.FunctionId == Function.ToString() && x.CanUpdate) && Action == ActionEnum.Update)
                            {
                                NoAuthorize();
                                return;
                            }
                        }
                    }
                    else
                    {
                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
                    }
                }
            }
            catch (Exception ex)
            {
                NoAuthorize(ex);
                return;
            }
            
        }
    }
}