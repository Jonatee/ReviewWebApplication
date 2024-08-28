using Review_Web_App.Context;
using Review_Web_App.Repositories.Interfaces;
using System;

namespace Review_Web_App.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ReviewAppContext _context;

        public UnitOfWork(ReviewAppContext context)
        {
            _context = context;
        }

        public async Task<int> SaveWork()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
