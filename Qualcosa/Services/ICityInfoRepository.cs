using CityInfoAPI.Entities;
using Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();//Utilizzare un metodo asincrono fa sì che la memeoria venga subito liberata per far lavorare altre cose!!!
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);// Mi serve per cercare una città
        Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestsForCityAsync(
            int cityId, 
            int pointOfInterestId);

    }
}
