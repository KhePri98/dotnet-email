namespace Alanyang.DotNetEmail.ApplicationCore.Extensions;

public static class StreamExtensions
{
    public static byte[] ReadAllBytes(this Stream inStream)
    {
        if (inStream is MemoryStream)
        {
            return ((MemoryStream)inStream).ToArray();
        }

        using var memoryStream = new MemoryStream();
        inStream.CopyTo(memoryStream);

        return memoryStream.ToArray();
    }
}