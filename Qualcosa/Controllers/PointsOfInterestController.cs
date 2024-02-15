using AutoMapper;
using CityInfo.API.Services;
using CityInfoAPI.Models;
using CityInfoAPI.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace CityInfoAPI.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : Controller
    {
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
           IMailService mailService,
           ICityInfoRepository cityInfoRepository,
           IMapper mapper)
        {
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ?? 
                throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ?? 
                throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
       public async Task<ActionResult<IEnumerable<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                _logger.LogInformation(
                    $"City with Id {cityId} wasn't found when accessing points of interest.");
                return NotFound();
            }
            var pointsOfInterestForCity = await _cityInfoRepository
                .GetPointOfInterestsForCityAsync(cityId);

            return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));
            //try
            //{
            //    //throw new Exception("Exception sample.");

            //    var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);

            //    if (city == null)
            //    {
            //        _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest");
            //        return NotFound();
            //    }

            //    return Ok(city.PointsOfInterest);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogCritical(
            //        $"Exeption while getting points of interest for city with id {cityId}.",
            //        ex);
            //    return StatusCode(500, "A problem happened while handling your request.");
            //}
            
        }

        [HttpGet("{pointOfInterestId}", Name = "GetPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(
            int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            var pointsOfInterest = await _cityInfoRepository
                .GetPointOfInterestsForCityAsync(cityId, pointOfInterestId);

            if (pointsOfInterest == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<PointOfInterestDto>(pointsOfInterest));
            //var city = _citiesDataStore.Cities
            //    .FirstOrDefault(c => c.Id == cityId);

            //if (city == null)
            //{
            //    return NotFound();
            //}

            ////find point of interest
            //var pointOfInterest = city.PointsOfInterest
            //    .FirstOrDefault(c => c.Id == pointofinterestid);

            //if(pointOfInterest == null)
            //{
            //    return NotFound();
            //}

            //return Ok(pointOfInterest);
        }

        
        [HttpPost]
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(
            int cityId,
            PointOfInterestForCreationDto pointOfInterest)
        {

            ////if (!ModelState.IsValid)  POSSO NON SCRIVERLO PERCHè VIENE CONTROLLATO DALL ApiController
            ////{
            ////    return BadRequest();
            ////}

            //var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            ////demo purposes - to be improved//Posso eliminarlo perchè ora la chiave viene generata in automatico!!! FIGOOO
            //var maxPointOfInterstId = _citiesDataStore.Cities.SelectMany(
            //    c => c.PointsOfInterest).Max(p => p.Id);

            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            //Questo viene sostituito da quello sopra!!! 
            //var finalPointOfInterest = new PointOfInterestDto()
            //{
            //    Id = ++maxPointOfInterstId,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};

            await _cityInfoRepository.AddPointOfInterestForCityAsync(
                cityId, finalPointOfInterest);

            await _cityInfoRepository.SaveChangesAsync();

            //city.PointsOfInterest.Add(finalPointOfInterest);//Questo viene eliminato perche abbiamo sostituito l'acquisizione della città con CityExistsAsync!!!

            var createdPointOfInterestToReturn =
                _mapper.Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointOfInterestId = finalPointOfInterest.Id
                },
                createdPointOfInterestToReturn);
        }

        
        [HttpPut("{pointOfInterestId}")]

        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId,
            PointOfInterestForUpdateDto pointOfInterest)
        {
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }
            //Sostituito con quello sopra
            //var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //find point of interest
            var pointOfInterestEntity = await _cityInfoRepository
                .GetPointOfInterestsForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }
            //Sostituito con quello sopra!!!
            //var pointOfInterestFromStore = city.PointsOfInterest
            //    .FirstOrDefault(c => c.Id == pointOfInterestId);
            //if (pointOfInterestFromStore == null)
            //{
            //    return NotFound();
            //}

            _mapper.Map(pointOfInterest, pointOfInterestEntity);//Questo sovrascrive il secondo campo con il primo ex (asd, 123) in questo caso 123 diventerà asd

            await _cityInfoRepository.SaveChangesAsync();//Cosi vado a salvare le cose nel DB!!!

            //pointOfInterestFromStore.Name = pointOfInterest.Name;
            //pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();
        }
        
        [HttpPatch("{pointofinterestid}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(
            int cityId, int pointOfInterestId,
            JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            //Controllo se la città esiste
            if(!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            //Sostituito con il metodo sopra!
            //var city = _citiesDataStore.Cities.FirstOrDefault(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //controlla che il punto d'interesse esista
            var pointOfInterestEntity = await _cityInfoRepository
                .GetPointOfInterestsForCityAsync (cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound(); 
            }

            //Sostituito con il metodo sopra!
            //var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(c => c.Id == pointOfInterestId);
            //if(pointOfInterestFromStore == null)
            //{
            //    return NotFound();
            //}

            //vado a prendere il punto da patchare
            var pointOfInterestToPatch = _mapper.Map<PointOfInterestForUpdateDto>(
                pointOfInterestEntity);

            //sostituito con quello sopra!
            //var pointOfInterestToPatch =
            //    new PointOfInterestForUpdateDto()
            //    {
            //        Name = pointOfInterestFromStore.Name,
            //        Description = pointOfInterestFromStore.Description,
            //    };

            //Viene applicata la patch
            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            //Vado ad implementarlo sul DB
            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);
            await _cityInfoRepository .SaveChangesAsync();

            //Sostituito con quello sopra
            //pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            //pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        
        
        [HttpDelete("{pointOfInterestId}")]
        
        public async Task<ActionResult> DeletePointOfInterest(
            int cityId, int pointOfInterestId)
        {
            if (!await _cityInfoRepository.CityExistsAsync(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = await _cityInfoRepository
                .GetPointOfInterestsForCityAsync(cityId, pointOfInterestId);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            //Rimuovo il punto di interesse
            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            //Sostituito dal metodo sopra
            //city.PointsOfInterest.Remove(pointOfInterestFromStore);

            await _cityInfoRepository .SaveChangesAsync();

            _mailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted");
            return NoContent();
        }
        
    }
}
