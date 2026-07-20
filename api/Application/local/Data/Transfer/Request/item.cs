public record ItemRequestDto : IDto
{
    public required string Name { get; init; }
}
