using AutoMapper;
using MediatR;
using SmartParking.Application.Dtos;
using SmartParking.Domain.Interfaces;

namespace SmartParking.Application.SmartParking.Queries.GetReservationById;

public class GetReservationByIdQueryHandler: IRequestHandler<GetReservationByIdQuery, GetReservationDto>
{
    private readonly IMapper _mapper;
    private readonly IReservationRepositories _reservationRepositories;

    public GetReservationByIdQueryHandler(IMapper mapper, IReservationRepositories reservationRepositories)
    {
        _mapper = mapper;
        _reservationRepositories = reservationRepositories;
    }
    public async Task<GetReservationDto> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        var reservation= await _reservationRepositories.GetReservationByIdAsync(request.Id);
        return _mapper.Map<GetReservationDto>(reservation);
    }
}