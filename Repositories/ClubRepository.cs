using BilliardManagement.Data.Entities;

namespace BilliardManagement.Repositories
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetAllClubsAsync();
        Task<IEnumerable<Club>> GetClubsPagedAsync(int pageNumber, int pageSize, string name);
        Task<Club?> GetClubByIdAsync(int id);
        Task CreateClubAsync(Club club);
        Task UpdateClubAsync(Club club);
        Task DeleteClubAsync(int id);
        Task<bool> ClubExistsAsync(int id);
    }

    public class ClubRepository : IClubRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public ClubRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Club>> GetAllClubsAsync()
        {
            return await _unitOfWork._clubRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Club>> GetClubsPagedAsync(int pageNumber, int pageSize, string name)
        {
            // Calculate skipping items based on pageNumber and pageSize
            int skipCount = (pageNumber - 1) * pageSize;

            // Query operation to apply pagination and filter by name
            Func<IQueryable<Club>, IQueryable<Club>> queryOperation = query =>
                query.Skip(skipCount).Take(pageSize).OrderBy(c => c.Id);

            // Call GetManyAsync with the predicate and query operation
            return await _unitOfWork._clubRepository.GetManyAsync(
                predicate: c => c.Name!.Contains(name), // Filter by name
                queryOperation: queryOperation
            );
        }

        public async Task<Club?> GetClubByIdAsync(int id)
        {
            return await _unitOfWork._clubRepository.GetByIdAsync(id);
        }

        public async Task CreateClubAsync(Club club)
        {
            _unitOfWork._clubRepository.Add(club);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateClubAsync(Club club)
        {
            _unitOfWork._clubRepository.Update(club);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteClubAsync(int id)
        {
            var club = await _unitOfWork._clubRepository.GetByIdAsync(id);
            if (club != null)
            {
                _unitOfWork._clubRepository.Remove(club);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> ClubExistsAsync(int id)
        {
            return await _unitOfWork._clubRepository.ExistsAsync(id);
        }
    }
}