using System;


namespace SlimResolution.Core.ErrorHandling;

public readonly struct LinkToken : IEquatable<LinkToken>
{
    private readonly int _token;


    public LinkToken()
    {
        _token = new Random().Next();
    }
    private LinkToken(int token)
    {
        _token = token;
    }
    public static LinkToken Create<T1, T2>()
    {
        var seed = new Random().Next();
        var token = HashCode.Combine(typeof(T1), typeof(T2), seed);

        return new(token);
    }


    public override int GetHashCode() => HashCode.Combine(_token);


    public bool Equals(LinkToken other) => _token == other._token;
    public override bool Equals(object? obj) => obj is LinkToken token && Equals(token);
    
    
    public static bool operator ==(LinkToken left, LinkToken right) => left.Equals(right);
    public static bool operator !=(LinkToken left, LinkToken right) => !(left == right);   
}