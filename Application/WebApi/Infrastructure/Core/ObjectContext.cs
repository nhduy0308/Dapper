using System;
using System.Data.SqlClient;
using System.Web.Http;

namespace WebAPI.Infrastructure.Core
{
    public class ObjectContext
    {
        public readonly Guid RequestId = Guid.NewGuid();
        
        private ApiController Context { get { return _controller; } }
        public bool IsAuthorized
        {
            get
            {
                return this.Context.User != null && this.Context.User.Identity.IsAuthenticated;
            }
        }

        public static ObjectContext CreateContext(ApiController controller)
        {
            return new ObjectContext(controller);
        }

        private ApiController _controller;

        private ObjectContext(ApiController controller)
        {
            _controller = controller;
            //dependency injection
        }

    }
}
