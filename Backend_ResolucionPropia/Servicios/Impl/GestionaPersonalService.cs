using AutoMapper;
using Parcial.Dtos;
using Parcial.Models;
using Parcial.Repositories.Interfaces;
using Parcial.Servicios.Interfaces;
using Parcial.Validators;

namespace Parcial.Servicios.Impl
{
    public class GestionaPersonalService : IGestionPersonalService
    {
        private readonly IAlbanilRepository _albanilRepository;
        private readonly IObraRepository _obraRepository;
        private readonly IMapper _mapper;
        private readonly AlbanilValidator _albanilValidator;
        private readonly AlbanileXObraValidator _albanilXObraValidator;

        public GestionaPersonalService(IAlbanilRepository albanilRepository, IObraRepository obraRepository, IMapper mapper, AlbanilValidator albanilValidator, AlbanileXObraValidator albanilXObraValidator)
        {
            _albanilRepository=albanilRepository;
            _obraRepository=obraRepository;
            _mapper=mapper;
            _albanilValidator=albanilValidator;
            _albanilXObraValidator=albanilXObraValidator;
        }

        public async Task<BaseResponse<List<AlbanilDto>>> GetAlbanilesNotInObraAsync(Guid obraId)
        {
            var response = new BaseResponse<List<AlbanilDto>>();
            try
            {
                var albaniles = await _obraRepository.GetAlbanilesNotInObra(obraId);
                var albanilesDto = _mapper.Map<List<AlbanilDto>>(albaniles);
                response.Data = albanilesDto;
                response.Success = true;
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "Error al obtener los albaniles";
                throw;
            }
            return response;
        }

        public async Task<BaseResponse<List<ObraDto>>> GetObrasAsync()
        {
            var response = new BaseResponse<List<ObraDto>>();
            try
            {
                var obras = await _obraRepository.GetActiveObras();
                var obrasDto = _mapper.Map<List<ObraDto>>(obras);
                response.Data = obrasDto;
                response.Success = true;
            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "Error al obtener las obras";
                throw;
            }
            return response;

        }

        public async Task<BaseResponse<AlbanilDto>> PostAlbanilAsync(AlbanilDto albanilDto)
        {
            var response = new BaseResponse<AlbanilDto>();
            var validacion = await _albanilValidator.ValidateAsync(albanilDto);
            if(!validacion.IsValid)
            {
                var errorMessage = string.Join
                    (", ", validacion.Errors.Select(x => x.ErrorMessage));
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }
            try
            {
                var existsAlbanil =  await _albanilRepository.AlbanilExists(albanilDto.Dni);
                if(existsAlbanil)
                {
                    response.Success = false;
                    response.Message = "El albañil ya está registrado en nuestro sistema";
                    return response;

                }
                var albanil = _mapper.Map<Albanile>(albanilDto);
                //albanil.Id = Guid.NewGuid();
                albanil.Activo = true;
                albanil.FechaAlta = DateTime.Now;
                await _albanilRepository.AddAlbanil(albanil);
                response.Success = true;
                response.Message = "El albanil se agregó con exito";

            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "Error al añadir el albañil";
                throw;
              
            }
            return response;
        }

        public async Task<BaseResponse<AlbanilXObraDto>> PostAlbanilXObraAsync(AlbanilXObraDto albanilXObraDto)
        {

            var response = new BaseResponse<AlbanilXObraDto>();
            var validacion = await _albanilXObraValidator.ValidateAsync(albanilXObraDto);

            if(!validacion.IsValid)
            {
                var errorMessage = string.Join(", ",
                    validacion.Errors.Select(x => x.ErrorMessage));
                response.Success = false;
                response.Message = errorMessage;
                return response;
            }

            try
            {
                var albanil = await _albanilRepository.GetAlbanilById(albanilXObraDto.IdAlbanil);
                if(albanil == null || !albanil.Activo)
                {
                    response.Success = false;
                    response.Message = "El albanil no se encontró o esta inactivo";
                    return response;
                }

                var albanilInObra = await _obraRepository.GetAlbanilInObra(albanilXObraDto.IdObra, albanilXObraDto.IdAlbanil);
                if(albanilInObra)
                {
                    response.Success = false;
                    response.Message = "El albanil ya forma parte de la obra";
                    return response;
                }

                var albanilXObra = _mapper.Map<AlbanilesXObra>(albanilXObraDto);
                // albanilXObra.Id = Guid.NewGuid();
                albanilXObra.FechaAlta = DateTime.Now;
                
                await _obraRepository.AddAlbanilToObra(albanilXObra);
                response.Success = true;
                response.Message = "El albañil fue agregado con exito a la obra";




            }
            catch (Exception)
            {
                response.Success = false;
                response.Message = "Error al añadir el albañil a la obra";
                throw;
            }
            return response;


      
        }
    }
}
