namespace VehicleKhatabook.Infrastructure
{
    public interface IEndpointDefinition
    {
        void DefineEndpoints(WebApplication app);
        void DefineServices(IServiceCollection services, IConfiguration configuration);
    }
}
