namespace CommonService.Exceptions
{
    public class NotFoundException(string name) : Exception($"Not found {name}!");

    public class BadRequestException(string? error = null) : Exception(error ?? "Bad request!");
}
