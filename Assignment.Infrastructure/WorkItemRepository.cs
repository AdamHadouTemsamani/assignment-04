namespace Assignment.Infrastructure;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly KanbanContext _context;

    public WorkItemRepository(KanbanContext context)
    {
        _context = context;
    }

    (Response Response, int ItemId) Create(WorkItemCreateDTO item){
       
       var Witem = new WorkItem{
        Id = item.Id,
        Title = item.Title,

       }
       Witem.State=State.New;
       //created time
       _context.Items.Add(Witem);
       _context.SaveChanges();

       return (Created,Witem.Id);
    }

    WorkItemDetailsDTO Find(int itemId){
       var item = from c in _context.Items
       where c.Id == itemId
       select new WorkItemDetailsDTO(c.Id,c.Title, "",c.D);

       return WorkItemDetailsDTO();
    }
    IReadOnlyCollection<WorkItemDTO> Read(){
        throw new NotImplementedException() ;
    }
    IReadOnlyCollection<WorkItemDTO> ReadRemoved(){
        throw new NotImplementedException() ;
    }
    IReadOnlyCollection<WorkItemDTO> ReadByTag(string tag){
        throw new NotImplementedException() ;
    }
    IReadOnlyCollection<WorkItemDTO> ReadByUser(int userId){
        throw new NotImplementedException() ;
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
