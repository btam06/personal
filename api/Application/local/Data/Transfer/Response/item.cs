using System;

public record ItemResponseDto : IDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
}
