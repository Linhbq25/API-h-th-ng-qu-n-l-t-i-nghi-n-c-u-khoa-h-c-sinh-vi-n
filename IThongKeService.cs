using quanlyNCKH.API.DTOs;

namespace quanlyNCKH.API.Services;

public interface IThongKeService
{
    Task<ThongKeDto> GetAsync();
}