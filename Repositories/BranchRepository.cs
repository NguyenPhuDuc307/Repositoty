using System.Linq.Expressions;
using BilliardManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BilliardManagement.Repositories
{
    public interface IBranchRepository
    {
        Task<IEnumerable<Branch>> GetAllBranchesAsync();
        Task<IEnumerable<Branch>> GetBranchesPagedAsync(int pageNumber, int pageSize, string name);
        Task<Branch?> GetBranchByIdAsync(int id);
        Task CreateBranchAsync(Branch branch);
        Task UpdateBranchAsync(Branch branch);
        Task DeleteBranchAsync(int id);
        Task<bool> BranchExistsAsync(int id);
    }

    public class BranchRepository : IBranchRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public BranchRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Branch>> GetAllBranchesAsync()
        {
            return await _unitOfWork._branchRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Branch>> GetBranchesPagedAsync(int pageNumber, int pageSize, string name)
        {
            // Calculate skipping items based on pageNumber and pageSize
            int skipCount = (pageNumber - 1) * pageSize;

            // Query operation to apply pagination and filter by name
            Func<IQueryable<Branch>, IQueryable<Branch>> queryOperation = query =>
                query.Include(q => q.Club).Skip(skipCount).Take(pageSize).OrderBy(c => c.Id);

            // Call GetManyWithIncludesAsync with the predicate, include properties, and query operation
            return await _unitOfWork._branchRepository.GetManyAsync(
                predicate: c => c.Name!.Contains(name), // Filter by name
                queryOperation: queryOperation
            );
        }

        public async Task<Branch?> GetBranchByIdAsync(int id)
        {
            Expression<Func<Branch, object>>[] queryOperation = { c => c.Club! };
            var branch = await _unitOfWork._branchRepository.FindAsync(
                predicate: c => c.Id == id,
                queryOperation: queryOperation
            );
            return branch as Branch;
        }

        public async Task CreateBranchAsync(Branch branch)
        {
            _unitOfWork._branchRepository.Add(branch);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateBranchAsync(Branch branch)
        {
            _unitOfWork._branchRepository.Update(branch);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteBranchAsync(int id)
        {
            var branch = await _unitOfWork._branchRepository.GetByIdAsync(id);
            if (branch != null)
            {
                _unitOfWork._branchRepository.Remove(branch);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> BranchExistsAsync(int id)
        {
            return await _unitOfWork._branchRepository.ExistsAsync(id);
        }
    }
}