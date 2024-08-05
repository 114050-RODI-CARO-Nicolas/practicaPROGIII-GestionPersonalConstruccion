using Parcial.Dtos;

namespace Parcial.Servicios.Interfaces
{
    public interface IGestionPersonalService
    {
        Task<BaseResponse<List<ObraDto>>> GetObrasAsync();
        Task<BaseResponse<AlbanilXObraDto>> PostAlbanilXObraAsync(AlbanilXObraDto albanilXObraDto);
        Task<BaseResponse<AlbanilDto>> PostAlbanilAsync(AlbanilDto albanilDto);
        Task<BaseResponse<List<AlbanilDto>>> GetAlbanilesNotInObraAsync(Guid obraId);
        


    }
}
