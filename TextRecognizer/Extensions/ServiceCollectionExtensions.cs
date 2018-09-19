using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using TextRecognizer.Implementation.ImageFilters;
using TextRecognizer.Interfaces;

namespace TextRecognizer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfig<T>(this IServiceCollection serviceCollection, string configPath) where T: class
        {
            return serviceCollection.AddSingleton(sp =>
            {
                using (var reader = new StreamReader(configPath))
                {
                    return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                }
            });
        }

        public static IServiceCollection AddImageFilters(this IServiceCollection serviceCollection)
        {
            var imageFilterInterfaceType = typeof(IImageFilter);
            var filterTypes = typeof(ServiceCollectionExtensions)
                .Assembly
                .GetTypes()
                .Where(t => imageFilterInterfaceType.IsAssignableFrom(t) && !t.IsAbstract)
                .OrderBy(t =>
                {
                    var orderAttribute = (OrderAttribute) t.GetCustomAttributes(typeof(OrderAttribute), false).FirstOrDefault();

                    if(orderAttribute != null)
                    {
                        return orderAttribute.Order;
                    }

                    return 0;
                })
                .ToArray();

            if(filterTypes.Any())
            {
                foreach(var filterType in filterTypes)
                {
                    serviceCollection.AddSingleton(imageFilterInterfaceType, filterType);
                }
            }

            return serviceCollection;
        }
    }
}
