using Nancy.Security;

namespace TagSortService
{
    public interface ITokenizer
    {
        string Tokenize(IUserIdentity user); 
    }
}