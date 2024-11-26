using Microsoft.EntityFrameworkCore;
using PcbDispatchService.Domain.Logic.States;
using PcbDispatchService.Domain.Models;

namespace PcbDispatchService.Dal;

/// <inheritdoc />
public class PcbRepository : IPcbRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Создает экземпляр репозитория.
    /// </summary>
    /// <param name="context">Контекст БД.</param>
    public PcbRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<PrintedCircuitBoard?> GetPcbById(int id)
    {
        var pcb = await _context.PrintedCircuitBoards
            .Where(i => i.Id == id)
            .Include(i => i.Components)
            .FirstOrDefaultAsync();
        if(pcb is not null)
        {
            return pcb;
        }
        else
        {
            throw new ApplicationException($"Плата {id} не найдена.");
        }
    }

    /// <inheritdoc />
    public async Task<List<PrintedCircuitBoard>> GetAllPcbs()
    {
        var allPcbs = await _context.PrintedCircuitBoards
            .Include(i => i.Components)
            .ToListAsync();
        if (allPcbs.Count > 0)
        {
            return allPcbs;
        }
        else
        {
            throw new ApplicationException("В системе нет зарегистрированных плат.");
        }
    }

    /// <inheritdoc />
    public async Task AddNewBoard(PrintedCircuitBoard pcb)
    {
        await _context.AddAsync(pcb);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task DeletePcbById(int id)
    {
        var pcb = await _context.PrintedCircuitBoards
            .Where(i => i.Id == id)
            .Include(i => i.Components)
            .FirstOrDefaultAsync();
        if (pcb != null)
        {
            if (pcb.Components.Count > 0)
            {
                var componentsToDelete = pcb.Components;
                _context.BoardComponents.RemoveRange(componentsToDelete);
            }
            _context.PrintedCircuitBoards.Remove(pcb);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ApplicationException($"Не удалось удалить плату {id}, т.к. она не найдена.");
        }
    }

    /// <inheritdoc />
    public async Task RenameBoard(int id, string newName)
    {
        var pcb = await _context.PrintedCircuitBoards
            .Where(i => i.Id == id)
            .FirstOrDefaultAsync();
        if (pcb != null)
        {
            pcb.RenamePcb(newName);
            _context.PrintedCircuitBoards.Update(pcb);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ApplicationException($"Не удалось переименовать, плата {id} не найдена.");
        }
    }

    /// <inheritdoc />
    public async Task RemoveComponentsFromBoard(int id)
    {
        var pcb = await _context.PrintedCircuitBoards.Where(i => i != null && i.Id == id).FirstOrDefaultAsync();
        if (pcb != null)
        {
            pcb.RemoveAllComponentsFromBoard();
            _context.PrintedCircuitBoards.Update(pcb);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ApplicationException($"Не удалось удалить компоненты, плата {id} не найдена.");
        }
    }

    /// <inheritdoc />
    public async Task UpdateBoardState(PrintedCircuitBoard newStatePcb)
    {
        var pcb = await _context.PrintedCircuitBoards
            .Where(i => i.Id == newStatePcb.Id)
            .FirstOrDefaultAsync();
        if (pcb != null)
        {
            pcb.SetBusinessEnum(newStatePcb.BusinessProcessStatus);
            _context.PrintedCircuitBoards.Update(pcb);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ApplicationException($"Не удалось изменить статус, плата {newStatePcb.Id} не найдена.");
        }
    }

    /// <inheritdoc />
    public async Task AddComponentToBoard(int boardId, BoardComponent boardComponent)
    {
        var pcb = await _context.PrintedCircuitBoards
            .Where(i => i.Id == boardId)
            .Include(i => i.Components)
            .FirstOrDefaultAsync();
        if(pcb is not null)
        {
            if (pcb.Components.Count > 0)
            {
                var existingComponent = pcb.Components
                    .FirstOrDefault(i => i.ComponentType == boardComponent.ComponentType);
                if (existingComponent is not null)
                {
                    existingComponent.Quantity += boardComponent.Quantity;
                    pcb.AddComponentToPcb(existingComponent);
                }
                else
                {
                    pcb.AddComponentToPcb(boardComponent);
                }
            }
            else
            {
                pcb.AddComponentToPcb(boardComponent);
            }
            _context.PrintedCircuitBoards.Update(pcb);
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ApplicationException($"Плата {boardId} не найдена.");
        }
    }
}