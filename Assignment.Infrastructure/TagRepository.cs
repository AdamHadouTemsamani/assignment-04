namespace Assignment.Infrastructure;

public class TagRepository : ITagRepository
{
    private readonly KanbanContext _context;

    public TagRepository(KanbanContext context)
    {
        _context = context;
    }

    public (Response Response, int TagId) Create(TagCreateDTO tag)
    {   

       var tg = new Tag(tag.Name);
       
       _context.Tags.Add(tg);
       _context.SaveChanges();

       return (Created,tg.Id);
    }

    public TagDTO Find(int tagId){
       var tg = from c in _context.Tags
       where c.Id == tagId
       select new TagDTO(c.Id,c.Name);

       return tg.FirstOrDefault()!;
    }

    public IReadOnlyCollection<TagDTO> Read(){
        var characters = from c in _context.Tags
        select new TagDTO(c.Id,c.Name);

        return characters.ToList();
    }

    public Response Update(TagUpdateDTO tag){
        var entity = _context.Tags.Find(tag.Id);

        if (entity == null)
        {
            return NotFound;
        }

        entity.Id = tag.Id;
        entity.Name = tag.Name;
        

        return Updated;
    }


    public Response Delete(int tagId, bool force){

        var entity = _context.Tags.Find(tagId);

        if (entity == null)
        {
        return NotFound;
        }
        else if(!force && entity.WorkItems.Any()){
        return Conflict;
        }
        _context.Tags.Remove(entity);
        _context.SaveChanges();
        return Deleted;
        

    }
}
