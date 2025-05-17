using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public record CreateCompanyRequestDto([Required] string Name, [Required] string Description, [Required] string ManagerId)
    {
    }
}
