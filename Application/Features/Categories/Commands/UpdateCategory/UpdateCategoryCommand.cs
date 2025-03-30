using Core.Entities;
using MediatR;

namespace Application.Features.Categories.Commands.UpdateCategory
{
    public record UpdateCategoryCommand(int Id, string Name) : IRequest<Category>;

}
