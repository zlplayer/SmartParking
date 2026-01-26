using AutoMapper;
using MediatR;
using SmartParking.Application.Dtos;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Queries.GetAllReservations;

public class GetAllReservationQueryHandler: IRequestHandler<GetAllReservationQuery, IEnumerable<GetReservationDto>>
{
    private readonly IMapper _mapper;
    private readonly IReservationRepositories _reservationRepositories;

    public GetAllReservationQueryHandler(IMapper mapper, IReservationRepositories reservationRepositories)
    {
        _mapper = mapper;
        _reservationRepositories = reservationRepositories;
    }
    public async Task<IEnumerable<GetReservationDto>> Handle(GetAllReservationQuery request, CancellationToken cancellationToken)
    {
        var reservations = await _reservationRepositories.GetAllReservationsAsync();
        return _mapper.Map<IEnumerable<GetReservationDto>>(reservations);
    }
}