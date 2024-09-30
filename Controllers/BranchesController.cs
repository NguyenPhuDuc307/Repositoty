using Microsoft.AspNetCore.Mvc;
using BilliardManagement.Repositories;
using BilliardManagement.Data.Entities;
using BilliardManagement.Utils;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BilliardManagement.Controllers
{
    public class BranchesController : Controller
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IClubRepository _clubRepository;
        private readonly ICloudStorageService _cloudStorageService;

        public BranchesController(IClubRepository clubRepository,
        IBranchRepository branchRepository,
        ICloudStorageService cloudStorageService)
        {
            _clubRepository = clubRepository;
            _branchRepository = branchRepository;
            _cloudStorageService = cloudStorageService;
        }

        private async Task GenerateSignedUrl(Club request)
        {
            // Get Signed URL only when Saved File Name is available.
            if (!string.IsNullOrWhiteSpace(request.SavedFileName))
            {
                request.SignedUrl = await _cloudStorageService.GetSignedUrlAsync(request.SavedFileName);
            }
        }

        // GET: Branch
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string searchName = "")
        {
            // Call GetBranchPagedAsync with pagination parameters and searchName
            var branches = await _branchRepository.GetBranchesPagedAsync(pageNumber, pageSize, searchName);
            // Pass branches to the view
            foreach (var branch in branches)
            {
                await GenerateSignedUrl(branch.Club!);
            }
            return View(branches);
        }

        // GET: Branch/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _branchRepository.GetBranchByIdAsync(id.Value);
            if (branch == null)
            {
                return NotFound();
            }
            return View(branch);
        }

        // GET: Branch/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ClubId"] = new SelectList(await _clubRepository.GetAllClubsAsync(), "Id", "Name");
            return View();
        }

        // POST: Branch/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Branch branch)
        {
            if (ModelState.IsValid)
            {
                await _branchRepository.CreateBranchAsync(branch);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(await _clubRepository.GetAllClubsAsync(), "Id", "Name");
            return View(branch);
        }

        // GET: Branch/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _branchRepository.GetBranchByIdAsync(id.Value);
            if (branch == null)
            {
                return NotFound();
            }
            ViewData["ClubId"] = new SelectList(await _clubRepository.GetAllClubsAsync(), "Id", "Name");
            return View(branch);
        }

        // POST: Branch/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Branch branch)
        {
            if (id != branch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _branchRepository.UpdateBranchAsync(branch);
                }
                catch (KeyNotFoundException)
                {
                    if (!await _branchRepository.BranchExistsAsync(branch.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(await _clubRepository.GetAllClubsAsync(), "Id", "Name");
            return View(branch);
        }

        // GET: Branch/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _branchRepository.GetBranchByIdAsync(id.Value);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // POST: Branch/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _branchRepository.DeleteBranchAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                // code error: 404
                return NotFound();
            }
        }

        private bool BranchExists(int id)
        {
            return _branchRepository.GetBranchByIdAsync(id).Result != null;
        }
    }
}
