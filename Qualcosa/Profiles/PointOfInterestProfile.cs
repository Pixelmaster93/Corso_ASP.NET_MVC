using AutoMapper;
using CityInfoAPI;

namespace CityInfo.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile() 
        {
            CreateMap<CityInfoAPI.Entities.PointOfInterest, CityInfoAPI.Models.PointOfInterestDto>();
        }
    }
}
