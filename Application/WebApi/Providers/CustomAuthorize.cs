using AutoMapper;
using common.JWT;
using Common.Enums;
using Data.Infrastructure;
using Microsoft.AspNet.Identity;
using Model.Models;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Results;
using WebAPI.Infrastructure.Core;
using WebAPI.Models;

namespace WebApi.Providers
{
    public class CustomAuthorize : AuthorizeAttribute
    {
        private string accessToken
        {
            get
            {
                return "accessToken";
            }
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
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
                            Message = ex?.Message ?? "UnAuthorize"
                        },
                        Message = ex?.Message ?? "UnAuthorize",
                        IsSuccess = false
                    }))
                };
                responseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                actionContext.Response = responseMessage;
            }
            try
            {
                var token = actionContext.Request.Headers.FirstOrDefault(x => x.Key == this.accessToken);

                if (token.Value == null || token.Value.Count() == 0)
                {
                    NoAuthorize();
                    return;
                }
                else
                {
                    var UserIdentity = JwtManager.ValidToken(token.Value.FirstOrDefault());
                    if (UserIdentity == null)
                    {
                        NoAuthorize();
                        return;
                    }
                    var userAspNet = this.GetUser(UserIdentity.Name);
                    if (userAspNet == null)
                    {
                        NoAuthorize();
                        return;
                    }

                    string[] Roles = { "Recipient" }; //get roles in db
                    var permission = new List<PermissionViewModel>();
                    permission.Add(new PermissionViewModel
                    {
                        FunctionId = RoleEnum.Member.ToString(),
                        CanCreate = true,
                        CanDelete = true,
                        CanRead = true,
                        CanUpdate = true
                    });


                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.PrimarySid, userAspNet.Id));
                    claims.Add(new Claim(ClaimTypes.Name, userAspNet.UserName));
                    claims.Add(new Claim("roles", JsonConvert.SerializeObject(Roles)));
                    claims.Add(new Claim("permissions", JsonConvert.SerializeObject(permission)));
                    var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                    GenericPrincipal MyPrincipal = new GenericPrincipal(identity, Roles);
                    IPrincipal userIdentity = (IPrincipal)MyPrincipal;
                    ((ApiController)actionContext.ControllerContext.Controller).User = userIdentity;

                }
                base.OnAuthorization(actionContext);

            }
            catch (Exception ex)
            {
                NoAuthorize(ex);
                return;
            }

        }
        private AspNetUser GetUser(string userId)
        {
            //var userAspNetService = new AspNetUserService(new UnitOfWork(new DbFactory().Init()));
            //return userAspNetService.FindById(userId);
            return new AspNetUser();
        }
    }
}