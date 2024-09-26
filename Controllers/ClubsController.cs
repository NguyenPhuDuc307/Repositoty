using Microsoft.AspNetCore.Mvc;
using BilliardManagement.Repositories;
using BilliardManagement.Data.Entities;
using BilliardManagement.Utils;

namespace BilliardManagement.Controllers
{
    public class ClubsController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly ICloudStorageService _cloudStorageService;

        public ClubsController(IClubRepository clubRepository, ICloudStorageService cloudStorageService)
        {
            _clubRepository = clubRepository;
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

        private async Task ReplacePhoto(Club request)
        {
            if (request.Photo != null)
            {
                //replace the file by deleting request.SavedFileName file and then uploading new request.Photo
                if (!string.IsNullOrEmpty(request.SavedFileName))
                {
                    await _cloudStorageService.DeleteFileAsync(request.SavedFileName);
                }
                request.SavedFileName = UtilityHelper.GenerateFileNameToSave(request.Photo.FileName);
                request.SavedUrl = await _cloudStorageService.UploadFileAsync(request.Photo, request.SavedFileName!);
            }
        }

        // GET: Clubs
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string searchName = "")
        {
            // Call GetClubsPagedAsync with pagination parameters and searchName
            var clubs = await _clubRepository.GetClubsPagedAsync(pageNumber, pageSize, searchName);
            foreach (var club in clubs)
            {
                await GenerateSignedUrl(club);
            }
            // Pass clubs to the view
            return View(clubs);
        }

        // GET: Clubs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _clubRepository.GetClubByIdAsync(id.Value);
            if (club == null)
            {
                return NotFound();
            }
            await GenerateSignedUrl(club);
            return View(club);
        }

        // GET: Clubs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clubs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Club club)
        {
            if (ModelState.IsValid)
            {
                if (club.Photo != null)
                {
                    club.SavedFileName = UtilityHelper.GenerateFileNameToSave(club.Photo.FileName);
                    club.SavedUrl = await _cloudStorageService.UploadFileAsync(club.Photo, club.SavedFileName!);
                }
                await _clubRepository.CreateClubAsync(club);
                return RedirectToAction(nameof(Index));
            }
            return View(club);
        }

        // GET: Clubs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _clubRepository.GetClubByIdAsync(id.Value);
            if (club == null)
            {
                return NotFound();
            }
            await GenerateSignedUrl(club);
            return View(club);
        }

        // POST: Clubs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Club club)
        {
            if (id != club.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ReplacePhoto(club);
                    await _clubRepository.UpdateClubAsync(club);
                }
                catch (KeyNotFoundException)
                {
                    if (!await _clubRepository.ClubExistsAsync(club.Id))
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
            return View(club);
        }

        // GET: Clubs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var club = await _clubRepository.GetClubByIdAsync(id.Value);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _clubRepository.DeleteClubAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        private bool ClubExists(int id)
        {
            return _clubRepository.GetClubByIdAsync(id).Result != null;
        }
    }
}
