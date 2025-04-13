using MediatR;

namespace Application.Features.PostsCategories.Commands.DeletePostCategories
{
    public record DeletePostCategoryCommand(int id, List<int> categoryIds): IRequest<bool>;
}
