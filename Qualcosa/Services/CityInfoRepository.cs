using CityInfoAPI.DbContexts;
using CityInfoAPI.Entities;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if(includePointsOfInterest)//Se include i punti di interesse mada le città con i punti
            {
                return await _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId).FirstOrDefaultAsync();
            }

            return await _context.Cities //se non li include manda solo le città
                .Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }
        public async Task<PointOfInterest?> GetPointOfInterestsForCityAsync(
            int cityId, 
            int pointOfInterestId)
        {
            return await _context.PointOfInterests //prende solo un punto di interesse per la citta, se esiste 
               .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointOfInterestsForCityAsync(int cityId)
        {
            return await _context.PointOfInterests// Prende tutti i punti di interesse id una città
                .Where(p => p.CityId == cityId).ToListAsync();
        }

        
    }
}
