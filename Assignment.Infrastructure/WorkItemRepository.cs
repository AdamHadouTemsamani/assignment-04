namespace Assignment.Infrastructure;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly KanbanContext _context;

    public WorkItemRepository(KanbanContext context)
    {
        _context = context;
    }

    public (Response Response, int ItemId) Create(WorkItemCreateDTO item){
       
 
        var WItem = new WorkItem(item.Title)
        {
            AssignedTo = _context.Users.Find(item.AssignedToId),
            Description = item.Description,
            Created = DateTime.Now,
            State = State.New,
            Tags = _context.Tags.Where(x => x.Name.Equals(item.Tags)).ToList(),
            StateUpdated = DateTime.Now
        };

       _context.Items.Add(WItem);
       _context.SaveChanges();

       return (Created,WItem.Id);
    }

    public WorkItemDetailsDTO Find(int itemId){

       var item1 = from c in _context.Items
       where c.Id == itemId
       select new WorkItemDetailsDTO(c.Id,c.Title,c.Description,c.Created, c.AssignedTo.ToString(),c.Tags.Select(x=>x.Name).ToList(),c.State, c.StateUpdated);

       return item1.FirstOrDefault()!;
    }
    public IReadOnlyCollection<WorkItemDTO> Read(){
        var Items = from i in _context.Items
        select new WorkItemDTO(i.Id,i.Title, i.AssignedTo.Name, i.Tags.Select(x=>x.Name).ToList(), i.State);

        return Items.ToList();
    }
    /////
    public IReadOnlyCollection<WorkItemDTO> ReadRemoved(){
        var Items = from i in _context.Items
        select new WorkItemDTO(i.Id,i.Title, i.AssignedTo.Name, i.Tags.Select(x=>x.Name).ToList(), State.Removed);

        return Items.ToList();
    }
    //Temp
    public IReadOnlyCollection<WorkItemDTO> ReadByTag(string tag){
        var Items = from i in _context.Items
        select new WorkItemDTO(i.Id,i.Title, i.AssignedTo.Name, i.Tags.Where(i=>i.Equals(tag)).Select(x=>x.Name).ToList(), i.State);

        return Items.ToList();
    }
    //temp
    public IReadOnlyCollection<WorkItemDTO> ReadByUser(int userId){
        var Items = from i in _context.Items
        select new WorkItemDTO(userId,i.Title, i.AssignedTo.Name, i.Tags.Select(x=>x.Name).ToList(), i.State);

        return Items.ToList();
    }
    //doubt this should work
    public IReadOnlyCollection<WorkItemDTO> ReadByState(State state){
        var Items = from i in _context.Items
        select new WorkItemDTO(i.Id,i.Title, i.AssignedTo.Name, i.Tags.Select(x=>x.Name).ToList(), state);

        return Items.ToList();
    }
    public Response Update(WorkItemUpdateDTO item){

        var selected = _context.Items.Find(item);

        if(selected == null){
            return NotFound;
        }

        selected.Title = item.Title;
        selected.Id = item.Id;
        selected.StateUpdated = DateTime.Now;
        selected.State = item.State;
        selected.Description = item.Description;
        selected.AssignedToId = item.AssignedToId;
        
        _context.SaveChanges();


        return Updated;
    }

    public Response Delete(int itemId){
        var entity = _context.Items.Find(itemId);

        if (entity.State == State.New)
        {
        _context.Items.Remove(entity);
        _context.SaveChanges();

        return Deleted;
        }else if(entity.State == State.Active){
        entity.State = State.Removed;
         _context.SaveChanges();
         return Deleted;
        }else if(entity.State == State.Resolved || entity.State == State.Removed || entity.State == State.Closed){
         entity.State = State.Removed;
         _context.SaveChanges();
         return Conflict;
        }

        return NotFound;
    }

}
