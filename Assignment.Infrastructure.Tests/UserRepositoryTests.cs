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

        var item1 = new WorkItem("Flamethrower"){ Id = 1, AssignedTo = null, State = State.New, Tags = new List<Tag> { } };
        var u1 = new User("Obama","ObamaPrism@gmail.com"){Id=1,Items= new HashSet<WorkItem>{}};
        var u2 = new User("Jeff Bezos","Bezos4Lyfe@gmail.com"){Id=2,Items= new HashSet<WorkItem>{item1}};
        
        _context.Users.AddRange(u1, u2);
        _context.Items.AddRange(item1);
        _context.SaveChanges();

        _repository = new UserRepository(_context);
    }
        public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public void Create_Duplicate_User_should_return_Conflict()
    {   
        var u = new UserCreateDTO("Obunga","ObamaPrism@gmail.com");

        var (status, created) = _repository.Create(u);
        
        status.Should().Be(Conflict);

    }

    [Fact]
    public void Delete_user_with_no_Item_Should_Delete()
    {   
        var response = _repository.Delete(1,false);
        response.Should().Be(Deleted);
        var search = _context.Users.Find(1);
        search.Should().Be(null);
    }

    [Fact]
    public void Force_Delete_user_with_item_should_Delete()
    {   
        var response = _repository.Delete(2,true);
        response.Should().Be(Deleted);
        var search = _context.Users.Find(2);
        search.Should().Be(null);
    }

    [Fact]
    public void Delete_user_with_item_should_not_Delete()
    {   
        var response = _repository.Delete(2,false);
        response.Should().Be(Conflict);
        var search = _context.Users.Find(2);
        search.Should().NotBeNull();
    }

}
