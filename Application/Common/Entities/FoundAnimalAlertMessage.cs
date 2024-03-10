using Application.Common.Enums;

namespace Application.Common.Entities;

public class FoundAnimalAlertMessage
{
	public required Guid Id { get; set; }
	public required double FoundLocationLatitude { get; set; }
	public required double FoundLocationLongitude { get; set; }
	public required Gender Gender { get; set; }
	public required int SpeciesId { get; set; }
	public required int BreedId { get; set; }
	public required List<int> ColorIds { get; set; } = null!;
}