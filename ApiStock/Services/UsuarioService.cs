using ApiStock.Interfaces;
using ApiStock.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiStock.Services;

public class UsuarioService : IService<Usuario>
{
    private readonly StockContext _context;
    public UsuarioService(StockContext context)
    {
        _context = context;
    }
    public async Task<Usuario> CreateAsync(Usuario entidad)
    {
        var nuevoUsuario = await _context.Usuarios.AddAsync(entidad);
        await _context.SaveChangesAsync();
        return nuevoUsuario.Entity;
    }

    public async Task<Usuario?> DeleteAsync(int id)
    {
        var usuarioABorrar = await _context.Usuarios.FindAsync(id);
        if(usuarioABorrar == null) return null;
        _context.Usuarios.Remove(usuarioABorrar);
        await _context.SaveChangesAsync();
        return usuarioABorrar;
    }

    public async Task<Usuario[]> GetAllAsync()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return [..usuarios];
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id) ?? null;
        return usuario;
    }

    public Task<Usuario> UpdateAsync(int id, Usuario entidad)
    {
        throw new NotImplementedException();
    }
}