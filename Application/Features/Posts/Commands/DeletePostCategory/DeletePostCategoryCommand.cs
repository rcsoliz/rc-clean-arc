using MediatR;

namespace Application.Features.Posts.Commands.DeletePostCategory
{
    public record DeletePostCategoryCommand(int id, List<int> categoryIds): IRequest<bool>;
}
