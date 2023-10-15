namespace MediaEncoder.Domain;

public class MediaEncoderFactory
{
    private readonly IEnumerable<IMediaEncoder> _encoders;
    public MediaEncoderFactory(IEnumerable<IMediaEncoder> encoders)
    {
        _encoders = encoders;
    }

    public IMediaEncoder? Create(string outputFormat)
    {
        return _encoders.FirstOrDefault(e => e.Accept(outputFormat));
    }
}