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

        var tagD = new Tag("Dangerous");
        var tagB = new Tag("Boring");

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
    public void Create_New_Tag()
    {   
        var tg = new TagCreateDTO("Cool");
        var expect = new TagDTO(3,"Cool");

        var (status, created) = _repository.Create(tg);
        
        status.Should().Be(Created);
        _repository.Find(3).Should().BeEquivalentTo(expect);
        
    }

}
