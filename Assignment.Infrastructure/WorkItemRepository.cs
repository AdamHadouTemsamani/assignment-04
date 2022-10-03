namespace Assignment.Infrastructure;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly KanbanContext _context;

    public WorkItemRepository(KanbanContext context)
    {
        _context = context;
    }

    (Response Response, int ItemId) Create(WorkItemCreateDTO item){
        throw new NotImplementedException() ;
    }
    WorkItemDetailsDTO Find(int itemId){
        throw new NotImplementedException() ;
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
        throw new NotImplementedException() ;
    }

}
