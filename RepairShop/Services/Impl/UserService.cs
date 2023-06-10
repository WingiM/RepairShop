namespace RepairShop.Services.Impl;

public class UserService : IUserService
{
    private readonly ApplicationContext _context;

    public UserService(ApplicationContext context)
    {
        _context = context;
    }

    public User? GetUser(int id)
    {
        return _context.Users.FirstOrDefault(x => x.Id == id);
    }
}
