using CityInfoAPI.Entities;
using Entities;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();//Utilizzare un metodo asincrono fa sì che la memeoria venga subito liberata per far lavorare altre cose!!!
        Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest);// Mi serve per cercare una città
        Task<bool> CityExistsAsync(int cityId);//Controlla se esistono punti di interesse per una città, nel caso non esistano il risultato sarà false, in caso contrario sarà true!
        Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId);
        Task<PointOfInterest?> GetPointOfInterestsForCityAsync(
            int cityId, 
            int pointOfInterestId);
        Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        Task<bool> SaveChangesAsync();
        

    }
}
