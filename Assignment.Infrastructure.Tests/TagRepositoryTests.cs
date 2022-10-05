namespace Assignment.Infrastructure.Tests;

public sealed class TagRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly KanbanContext _context;
    private readonly TagRepository _repository;

    public TagRepositoryTests(){
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>().UseSqlite(_connection);
        _context = new KanbanContext(builder.Options);
        _context.Database.EnsureCreated();


        var tagD = new Tag("Dangerous"){ Id = 1, WorkItems = new List<WorkItem>{}};
        var tagB = new Tag("Boring"){ Id = 2, WorkItems = new List<WorkItem>{}};
        var ItemA = new WorkItem("Sand"){Id = 1, AssignedTo=null, State=State.New, Tags = new List<Tag>{tagB}};

        _context.Items.AddRange(ItemA);
        _context.Tags.AddRange(tagD, tagB);
        _context.SaveChanges();

        _repository = new TagRepository(_context);
    }


    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public void First_Tag_is_Dangerous()
    {
        var expect = new TagDTO(1,"Dangerous");

        _repository.Find(1).Should().BeEquivalentTo(expect);
        Console.WriteLine(_repository);
    }

    [Fact]
    public void Create_New_Tag_Third_Tag_should_be_Cool()

    {   
        var tg = new TagCreateDTO("Cool");
        var expect = new TagDTO(3,"Cool");

        var (status, created) = _repository.Create(tg);
        
        status.Should().Be(Created);
        _repository.Find(3).Should().BeEquivalentTo(expect);
        
    }


    [Fact]
    public void Delete_Tag_Not_in_use_Should_Delete()
    {   
        var response = _repository.Delete(1,false);
        response.Should().Be(Deleted);
        var search = _context.Tags.Find(1);
        search.Should().Be(null);
    }

    [Fact]
    public void Force_Delete_Tag_in_use_should_Delete()
    {   
        var response = _repository.Delete(2,true);
        response.Should().Be(Deleted);
        var search = _context.Tags.Find(2);
        search.Should().Be(null);
    }

    [Fact]
    public void Delete_Tag_in_use_should_return_conflict()
    {   
        var response = _repository.Delete(2,false);
        response.Should().Be(Conflict);
        var search = _context.Tags.Find(2);
        search.Should().NotBeNull();
    }
    
    [Fact]
    public void Create_Duplicate_Tag_should_return_Conflict()
    {   
        var tg = new TagCreateDTO("Boring");

        var (status, created) = _repository.Create(tg);
        
        status.Should().Be(Conflict);
        
    }
}



