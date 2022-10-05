namespace Assignment.Infrastructure;

public class UserRepository : IUserRepository
{   
    private readonly KanbanContext _context;

    public UserRepository(KanbanContext context)
    {
        _context = context;
    }

    public (Response Response, int UserId) Create(UserCreateDTO user){
       var u = new User(user.Name,user.Email);
       var search = _context.Users.Where(x=>x.Email.Equals(u.Email)).FirstOrDefault();
       if(search is not null){
         return (Conflict,u.Id);
       }
       _context.Users.Add(u);
       _context.SaveChanges();

       return (Created,u.Id);
    }

    public UserDTO Find(int userId){
       var u = from c in _context.Users
       where c.Id == userId
       select new UserDTO(c.Id,c.Name,c.Email);

       return u.FirstOrDefault()!;
    }
    public IReadOnlyCollection<UserDTO> Read(){
        var users = from c in _context.Users
        select new UserDTO(c.Id,c.Name,c.Email);

        return users.ToList();
    }
    public Response Update(UserUpdateDTO user){
        //notImplemented
    return Updated;
    }

    public Response Delete(int userId, bool force = false){
        var entity = _context.Users.Find(userId);

        if (entity == null)
        {
        return NotFound;
        }
        else if(!force && entity.Items.Any()){
        return Conflict;
        }
        _context.Users.Remove(entity);
        _context.SaveChanges();
        return Deleted;
    }
 }

