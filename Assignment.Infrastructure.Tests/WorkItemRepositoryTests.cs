namespace Assignment.Infrastructure.Tests;

public class WorkItemRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly KanbanContext _context;
    private readonly WorkItemRepository _repository;

        public WorkItemRepositoryTests(){
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>().UseSqlite(_connection);
        _context = new KanbanContext(builder.Options);
        _context.Database.EnsureCreated();

        var item1 = new WorkItem("Arson");
        var item2 = new WorkItem("Tax evasion");

        _context.Items.AddRange(item1, item2);
        _context.SaveChanges();

        _repository = new WorkItemRepository(_context);
    }
        public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public void Create_New_Item()
    {   
        var tags = new List<string>();
        tags.Add("Cool");

       
        var item = new WorkItemCreateDTO("Arson",null,null, tags);
        var expect = new WorkItemDetailsDTO(3,"Arson",null,DateTime.Today,"Joe Mama",tags,State.Active, DateTime.Now);

        var (status, created) = _repository.Create(item);
        
        status.Should().Be(Created);
        _repository.Find(3).Should().BeEquivalentTo(expect);
        
    }

}
