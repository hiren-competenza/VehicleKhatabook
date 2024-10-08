using System.Diagnostics.Contracts;
using VehicleKhatabook.Infrastructure;

namespace VehicleKhatabook.EndPoints
{
    public class UserEndPoint : IEndpointDefinition
    {
        public void DefineEndpoints(WebApplication app)
        {
            var userRoute = app.MapGroup("api/user").WithTags("User Endpoints");
            userRoute.MapPost("/v1/check", UserSignup);
        }
        public void DefineServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddScoped<>();
        }

        internal void UserSignup()
        {

        }
    }
}
