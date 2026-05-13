using Auth.Application.Responses;
using CSharpFunctionalExtensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Features.User.GetAllUsers;

public record GetAllUsersQuery : IRequest<Result<IEnumerable<UserResponse>>>;
