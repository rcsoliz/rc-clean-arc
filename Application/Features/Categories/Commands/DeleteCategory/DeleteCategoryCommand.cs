using MediatR;

namespace Application.Features.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(int id) : IRequest<bool>;

}
