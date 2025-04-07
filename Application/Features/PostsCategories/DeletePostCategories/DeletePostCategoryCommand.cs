using MediatR;

namespace Application.Features.PostsCategories.DeletePostCategories
{
    public record DeletePostCategoryCommand(int id, List<int> categoryIds): IRequest<bool>;
}
