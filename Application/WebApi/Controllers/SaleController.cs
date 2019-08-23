using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Data.Infrastructure;
using Model.Models;
using Service;
using WebAPI.Infrastructure.Core;

namespace WebApi.Controllers
{
    [RoutePrefix("api/Sale")]
    public class SaleController : ApiControllerBase
    {
        private ISaleService _saleService { get; set; }
        public SaleController(IErrorService errorService, ISaleService saleService) : base(errorService)
        {
            this._saleService = saleService;
        }

        public IHttpActionResult Get()
        {
            try
            {
                var sale = this._saleService.GetAll();
                return this.Success<dynamic>(sale);
            }
            catch (Exception ex)
            {
                return this.Failed(ex);
            }
            
        }
    }
}