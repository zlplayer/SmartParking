using AutoMapper;
using MediatR;
using SmartParking.Application.Dtos;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Queries.GetReservationByUserId;

public class GetReservationByUserIdQueryHandler: IRequestHandler<GetReservationByUserIdQuery, IEnumerable<GetReservationDto>>
{
    private readonly IMapper _mapper;
    private readonly IReservationRepositories _reservationRepositories;

    public GetReservationByUserIdQueryHandler(IMapper mapper, IReservationRepositories reservationRepositories)
    {
        _mapper = mapper;
        _reservationRepositories = reservationRepositories;
    }
    public async Task<IEnumerable<GetReservationDto>> Handle(GetReservationByUserIdQuery request, CancellationToken cancellationToken)
    {
        var reservations = await _reservationRepositories.GetReservationsByUserIdAsync(request.UserId);
        return _mapper.Map<IEnumerable<GetReservationDto>>(reservations);
    }
}