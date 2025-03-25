using Core.Entities;
using MediatR;

namespace Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(int Id) : IRequest<User>;

}
