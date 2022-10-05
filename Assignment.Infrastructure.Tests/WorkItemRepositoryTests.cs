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

        var item1 = new WorkItem("Flamethrower"){ Id = 1, AssignedTo = null, State = State.New, Tags = new List<Tag> { } };
        var item2 = new WorkItem("Gun"){ Id = 2, AssignedTo = null, State = State.Active, Tags = new List<Tag> { } };
        var item3 = new WorkItem("Money"){ Id = 3, AssignedTo = null, State = State.Removed, Tags = new List<Tag> { } };
        var item4 = new WorkItem("Drugs"){ Id = 4, AssignedTo = null, State = State.Resolved, Tags = new List<Tag> { } };
        var item5 = new WorkItem("Drip"){ Id = 5, AssignedTo = null, State = State.Closed, Tags = new List<Tag> { } };
        
        _context.Items.AddRange(item1, item2, item3, item4, item5);

        var tagD = new Tag("Dangerous"){ Id = 1, WorkItems = new List<WorkItem>{}};
        var tagB = new Tag("Boring"){ Id = 2, WorkItems = new List<WorkItem>{}};

         _context.Tags.AddRange(tagD,tagB);

        _context.SaveChanges();

        _repository = new WorkItemRepository(_context);
    }
        public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
    }

    [Fact]
    public void Delete_New_Item_Should_Delete()
    {   
        var response = _repository.Delete(1);
        response.Should().Be(Deleted);
        var search = _context.Items.Find(1);
        search.Should().BeNull();
    }
    [Fact]
    public void Delete_Active_Item_Should_update_state_to_Removed()
    {   
        var response = _repository.Delete(2);
        response.Should().Be(Updated);
        var search = _context.Items.Find(2);
        search.Should().NotBeNull();
        search!.State.Should().Be(State.Removed);
    }

    [Fact]
    public void Delete_Removed_resolved_closed_Item_Should_return_conflict()
    {   
        var response1 = _repository.Delete(3);
        var response2 = _repository.Delete(4);
        var response3 = _repository.Delete(5);

        response1.Should().Be(Conflict);
        response2.Should().Be(Conflict);
        response3.Should().Be(Conflict);

        var search1 = _context.Items.Find(3);
        var search2 = _context.Items.Find(4);
        var search3 = _context.Items.Find(5);

        search1.Should().NotBeNull();
        search2.Should().NotBeNull();
        search3.Should().NotBeNull();
    }

    [Fact]
    public void Creating_new_items_sets_state_to_new_and_Created_and_updated_time_should_be_now()
    {   
        
        _repository.Create(new WorkItemCreateDTO("Meth Cooking", null, null, new List<string>()));

        _context.Items.Find(6)!.State.Should().Be(State.New);
        var expectedTime = DateTime.UtcNow;
        _context.Items.Find(6)!.Created.Should().BeCloseTo(expectedTime, precision: TimeSpan.FromSeconds(5));
        _context.Items.Find(6)!.StateUpdated.Should().BeCloseTo(expectedTime, precision: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Update_item_updates_tags()
    {   
    
        _repository.Update(new WorkItemUpdateDTO(1, "Laundry", 1, "Do the laundry", new List<string>(){"Kekw"}, State.Resolved));
       
        _repository.Find(1).Tags.Should().BeEquivalentTo(new List<string>(){"Kekw"});
    }


}
