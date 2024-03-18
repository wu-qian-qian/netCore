using Microsoft.Extensions.DependencyInjection;

public interface IModuleInitializer
  {
    void Initialize(IServiceCollection services);
  }