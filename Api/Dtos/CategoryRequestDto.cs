using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{
    public record CategoryRequestDto([Required] string Name, [Required] string Description)
    {

    }
}
