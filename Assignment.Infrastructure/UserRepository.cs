namespace Assignment.Infrastructure;

public class UserRepository
{   
    private readonly KanbanContext _context;

    public UserRepository(KanbanContext context)
    {
        _context = context;
    }

    (Response Response, int UserId) Create(UserCreateDTO user){
       var u = new User(user.Name,user.Email);
       
       _context.Users.Add(u);
       _context.SaveChanges();

       return (Created,u.Id);
    }
    UserDTO Find(int userId){
       var u = from c in _context.Users
       where c.Id == userId
       select new UserDTO(c.Id,c.Name,c.Email);

       return u.FirstOrDefault()!;
    }
    IReadOnlyCollection<UserDTO> Read(){

    }
    Response Update(UserUpdateDTO user){

    }
    Response Delete(int userId, bool force = false){

    }
}
