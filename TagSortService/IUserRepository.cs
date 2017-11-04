using Nancy.Security;

namespace TagSortService
{
    public interface IUserRepository
    {
        bool ValidateUser(string user, string password);

        User GetUserByUserName(string user);
    }
}