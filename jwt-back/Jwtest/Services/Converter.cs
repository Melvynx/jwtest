namespace Jwtest.Services;

public static class Converter
{
    public static int ObjectToInt(object? input)
    {
        if (input == null) return 0;

        var inputString = input.ToString();

        if (inputString?.Length == 0) return 0;

        return int.TryParse(inputString, out var output) ? output : 0;
    }
}