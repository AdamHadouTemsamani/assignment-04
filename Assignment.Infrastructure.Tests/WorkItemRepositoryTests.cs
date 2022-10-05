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

        var item1 = new WorkItem("Flamethrower");
        var item2 = new WorkItem("Gun");
        var item3 = new WorkItem("Money");

        _context.Items.AddRange(item1, item2, item3);

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

        
    }

}
