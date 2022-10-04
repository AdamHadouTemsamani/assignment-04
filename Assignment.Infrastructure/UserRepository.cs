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
       if(Find(u.Id).Email != user.Email){
       _context.Users.Add(u);
       _context.SaveChanges();

       return (Created,u.Id);
       }
       return (Conflict,u.Id);
    }

    UserDTO Find(int userId){
       var u = from c in _context.Users
       where c.Id == userId
       select new UserDTO(c.Id,c.Name,c.Email);

       return u.FirstOrDefault()!;
    }
    IReadOnlyCollection<UserDTO> Read(){
        var users = from c in _context.Users
        select new UserDTO(c.Id,c.Name,c.Email);

        return users.ToList();
    }
    Response Update(UserUpdateDTO user){
        //notImplemented
    return Updated;
    }

    Response Delete(int userId, bool force = false){
        var entity = _context.Tags.Find(userId);

        if (entity == null)
        {
            return NotFound;
        }

        _context.Tags.Remove(entity);
        _context.SaveChanges();

        return Deleted;
    }
 }

