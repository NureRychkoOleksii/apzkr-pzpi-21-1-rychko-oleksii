using Backend.Abstraction.Services;
using Backend.Core;
using Backend.Core.DTOs;
using Backend.Core.DTOs.Newborn;
using Backend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Services;

public class ParentService : IParentService
{
    private readonly StarOfLifeContext _context;
    
    public ParentService(StarOfLifeContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Parent>> GetParents()
    {
        return await _context.Parents.ToListAsync();
    }

    public async Task<Parent> GetParentAsync(int id)
    {
        return await _context.Parents.FindAsync(id);
    }

    public async Task CreateParentAsync(CreateParentDTO parent)
    {
        if (parent == null)
        {
            throw new ArgumentNullException(nameof(parent));
        }

        _context.Parents.Add(new Parent
        {
            UserId = parent.UserId,
            Name = parent.Name,
            ContractInfo = parent.ContractInfo
        });

        await _context.SaveChangesAsync();
    }

    public async Task CreateParentWithNewbornAsync(CreateParentDTO parentDto, CreateNewbornDTO newbornDto)
    {
        if (parentDto == null)
        {
            throw new ArgumentNullException(nameof(parentDto));
        }

        var parent = _context.Parents.Add(new Parent
        {
            UserId = parentDto.UserId,
            Name = parentDto.Name,
            ContractInfo = parentDto.ContractInfo
        });

        var newborn = _context.Newborns.Add(new Newborn
        {
            UserId = newbornDto.UserId,
            Name = newbornDto.Name,
            DateOfBirth = newbornDto.DateOfBirth,
            Gender = newbornDto.Gender,
        });

        await _context.SaveChangesAsync();
        
        _context.UserParents.Add(new UserParent
        {
            NewbornId = newborn.Entity.Id,
            ParentId = parent.Entity.Id
        });

        await _context.SaveChangesAsync();
    }

    public async Task UpdateParentAsync(int id, UpdateParentDTO parent)
    {
        if (parent == null)
        {
            throw new ArgumentNullException(nameof(parent));
        }

        var parentDb = await _context.Parents.FindAsync(id);

        if (parentDb == null)
        {
            throw new ArgumentNullException(nameof(parent));
        }

        parentDb.Name = parent.Name;
        parentDb.ContractInfo = parent.ContractInfo;

        _context.Parents.Update(parentDb);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteParentAsync(int id)
    {
        var parent = await _context.Parents.FindAsync(id);
        
        if (parent != null)
        {
            _context.Parents.Remove(parent);
            
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Newborn>> GetParentNewbornsAsync(int id)
    {
        var parent = await _context.Parents.Include(n => n.UserParents).FirstOrDefaultAsync(n => n.Id == id);
        
        if (parent != null)
        {
            var newbornIds = parent.UserParents.Select(up => up.NewbornId).ToList();
            
            return await _context.Newborns
                .Where(n => newbornIds.Contains(n.Id))
                .ToListAsync();
        }

        return null;
    }
}