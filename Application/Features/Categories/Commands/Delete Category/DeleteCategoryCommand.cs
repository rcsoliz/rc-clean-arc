using MediatR;

namespace Application.Features.Categories.Commands.Delete_Category
{
    public record DeleteCategoryCommand(int id) : IRequest<bool>;

}
