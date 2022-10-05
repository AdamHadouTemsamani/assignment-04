namespace Assignment.Infrastructure;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly KanbanContext _context;

    public WorkItemRepository(KanbanContext context)
    {
        _context = context;
    }

    (Response Response, int ItemId) Create(WorkItemCreateDTO item){
       
 
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

    WorkItemDetailsDTO Find(int itemId){

       var item1 = from c in _context.Items
       where c.Id == itemId
       select new WorkItemDetailsDTO(c.Id,c.Title,c.Description,c.Created, c.AssignedTo.ToString(),c.Tags.Select(x=>x.Name).ToList(),c.State, c.StateUpdated);

       return item1.FirstOrDefault()!;
    }
    IReadOnlyCollection<WorkItemDTO> Read(){
        var Items = from i in _context.Items
        select new WorkItemDTO(i.Id,i.Title, i.AssignedTo.Name, i.Tags.Select(x=>x.Name).ToList(), i.State);

        return Items.ToList();
    }
    IReadOnlyCollection<WorkItemDTO> ReadRemoved(){
        throw new NotImplementedException() ;
    }
    //Temp
    IReadOnlyCollection<WorkItemDTO> ReadByTag(string tag){
        var Items = from i in _context.Items
        select new WorkItemDTO(i.Id,i.Title, i.AssignedTo.Name, i.Tags.Where(i=>i.Equals(tag)).Select(x=>x.Name).ToList(), i.State);

        return Items.ToList();
    }
    IReadOnlyCollection<WorkItemDTO> ReadByUser(int userId){
        var Items = from i in _context.Items
        select new WorkItemDTO(i.Id,i.Title, i.AssignedTo.Name, i.Tags.Select(x=>x.Name).ToList(), i.State);

        return Items.ToList();
    }

    IReadOnlyCollection<WorkItemDTO> ReadByState(State state){
        throw new NotImplementedException() ;
    }
    Response Update(WorkItemUpdateDTO item){
        throw new NotImplementedException() ;
    }

    Response Delete(int itemId){
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
