using System.Threading.Tasks;
using BaseLibrary.DTOs;

namespace ServerLibrary.Repositories.Contracts
{
    public interface IWeatherService
    {
        Task<object> GetForecastData(WeatherDTO weather);
        Task<object> GetHistoricalData(WeatherDTO weather);
    }
}
