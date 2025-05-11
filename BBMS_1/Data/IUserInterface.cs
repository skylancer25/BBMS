using BBMS_1.Models;
namespace BBMS_1.Data

{
    public interface IUserInterface
    {
        int ValidateUser(string username, string password);
      //  bool RegisterUser(string username, string password);
    }
}
