namespace Assignment.Infrastructure.Tests;

public class UserRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly KanbanContext _context;
    private readonly UserRepository _repository;

        public UserRepositoryTests(){
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>().UseSqlite(_connection);
        _context = new KanbanContext(builder.Options);
        _context.Database.EnsureCreated();

        var u1 = new User("Obama","ObamaPrism@gmail.com");
        var u2 = new User("Jeff Bezos","Bezos4Lyfe@gmail.com");

        _context.Users.AddRange(u1, u2);
        _context.SaveChanges();

        _repository = new UserRepository(_context);
    }
        public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public void Create_New_User()
    {   
        
    }

}
