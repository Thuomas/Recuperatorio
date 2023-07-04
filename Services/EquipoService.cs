using SegundoParcial.Data;
using SegundoParcial.Models;
using Microsoft.EntityFrameworkCore;

namespace SegundoParcial.Services;

public class EquipoService : IEquipoService
{
    private readonly EquipoContext _context;

    public EquipoService(EquipoContext context)
    {
        _context = context;
    }
    public void Create(Equipo obj)
    {
        _context.Add(obj);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var borrar = GetById(id);
        if (borrar != null)
        {
            _context.Remove(borrar);
            _context.SaveChanges();
        }
    }

    public List<Equipo> GetAll(string nameFilter)
    {
        var query = GetQuery();
        if (!string.IsNullOrEmpty(nameFilter))
        {
            query = query.Where(x => x.NumSerie.ToLower().Contains(nameFilter.ToLower()) ||
                                x.Comentario.ToLower().Contains(nameFilter.ToLower()) ||
                                x.Deposito.Nombre.ToLower().Contains(nameFilter.ToLower()));
        }
        return query.ToList();
    }
    public List<Equipo> GetAll()
    {
        var query = GetQuery();
        return query.Include(x=>x.Deposito).ToList();
    }


    public Equipo? GetById(int id)
    {
        var equipo = GetQuery()
            .Include(x => x.Deposito)
            .FirstOrDefault(m => m.Id == id);

        return equipo;
    }

    public void Update(Equipo obj)
    {
        _context.Update(obj);
        _context.SaveChanges();
    }

    private IQueryable<Equipo> GetQuery()
    {
        return from equipo in _context.Equipo select equipo;
    }
}