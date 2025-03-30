using Core.Entities;
using MediatR;

namespace Application.Features.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand(string Name) : IRequest<Category>;

}
