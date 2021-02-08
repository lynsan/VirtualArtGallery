﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DSU21_5.Data;
using DSU21_5.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using DSU21_5.Areas.Identity.Data;
using DSU21_5.Models.ViewModel;

namespace DSU21_5.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ImageDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public IImageRepository ImageRepository { get; set; }
        public IMemberRepository MemberRepository { get; set; }
        public IArtRepository ArtRepository { get; set; }
        public ProfileViewModel ProfileViewModel { get; set; }

        //TODO: create try-catch

        public ProfileController(ImageDbContext context, IWebHostEnvironment hostEnvironment, IImageRepository imageRepository, IMemberRepository memberRepository, IArtRepository artRepository)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            ImageRepository = imageRepository;
            MemberRepository = memberRepository;
            ArtRepository = artRepository;
        }

        [Route("/Profile/Index/{Id}")]
        public async Task<IActionResult> Index(string Id)
        {
            var art = await ArtRepository.GetArtFromExhibit(Id);
            var test = await ArtRepository.GetUniqueIdsConnectedToExhibit();
            var test1 = await ArtRepository.GetArtConnectedToExhibit(test);
            Image image = ImageRepository.GetImageFromDb(Id);
            Member member = await MemberRepository.GetMember(Id);
            IEnumerable<Artwork> artwork = await ArtRepository.GetPostedArtFromUniqueUser(Id);
            ProfileViewModel = new ProfileViewModel(artwork, member, image, test1, art);
            return View(ProfileViewModel);
        }

        public IActionResult Create(string Id)
        {
            var image = ImageRepository.GetImageFromDb(Id);
            return View(image);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageId,ImageFile,UserId")] Image imageModel, string Id)
        {
            var image = imageModel;
            try
            {
                if (ModelState.IsValid)
                {
                    var checkIfUserHadProfilePictureAlready = ImageRepository.GetImageFromDb(Id);
                    if (checkIfUserHadProfilePictureAlready != null)
                    {
                        ImageRepository.RemoveImageFromDb(_hostEnvironment, checkIfUserHadProfilePictureAlready);
                    }
                    image = await ImageRepository.CreateNewProfilePicture(_context, _hostEnvironment, imageModel, Id);
                }
            }
            catch (Exception ex)
            {
                //TODO: Fixa en errorsida
                return View("Error", ex);
            }
            return RedirectToAction($"Index", new { Id });

        }
        public async Task<IActionResult> CreateArt(string Id)
        {
            var art = await ArtRepository.GetArtFromExhibit(Id);
            var test = await ArtRepository.GetUniqueIdsConnectedToExhibit();
            var test1 = await ArtRepository.GetArtConnectedToExhibit(test);
            Image image = ImageRepository.GetImageFromDb(Id);
            Member member = await MemberRepository.GetMember(Id);
            IEnumerable<Artwork> artwork = await ArtRepository.GetPostedArtFromUniqueUser(Id);
            ProfileViewModel = new ProfileViewModel(artwork, member, image, test1, art);
            return View(ProfileViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateArt([Bind("Artwork")] ProfileViewModel profileView, string Id)
        {
            Artwork imageModel = profileView.Artwork;
            Member member = await MemberRepository.GetMember(Id);
            Exhibit exhibit = null;
            try
            {
                if (ModelState.IsValid)
                { 
                        var artwork = await ArtRepository.AddArt(_context, _hostEnvironment, imageModel, member, exhibit);
                }
            }
            catch (Exception ex)
            {
                //TODO: Fixa en errorsida

                return View("Error", ex);

            }
            return RedirectToAction($"Index", new { Id });
        }

        [HttpPost, ActionName("DeleteArt")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteArtConfirm(int Id)
        {
            try
            {
                Artwork artwork = ArtRepository.GetArtworkThatsGonnaBeDeleted(Id);
                await ArtRepository.DeleteArtworkFromArtworkTable(_hostEnvironment, artwork);
            }
            catch (Exception ex)
            {
                //TODO: Fixa en errorsida
                return View(ex);
            }
            return Json(Id);
        }

        public async Task<IActionResult> CreateExhibition(string Id)
        {
            //TODO: måste vi ropa på allt de här? Kolla om det går att lösa de här snyggare
            var art = await ArtRepository.GetArtFromExhibit(Id);
            var test = await ArtRepository.GetUniqueIdsConnectedToExhibit();
            var test1 = await ArtRepository.GetArtConnectedToExhibit(test);
            Image image = ImageRepository.GetImageFromDb(Id);
            Member member = await MemberRepository.GetMember(Id);
            IEnumerable<Artwork> artwork = await ArtRepository.GetPostedArtFromUniqueUser(Id);
            ProfileViewModel = new ProfileViewModel(artwork, member, image, test1, art);
            return View(ProfileViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExhibition([Bind("Artwork")] ProfileViewModel profileView, string Id)
        {
            Artwork imageModel = profileView.Artwork;
            Member member = await MemberRepository.GetMember(Id);
            bool exist = ArtRepository.CheckIfIdExists(Id);
            string selected = Request.Form["exhibit-checkbox"];
            //string test = Request.Form.Files["files[]"].ToString();

            //List<Artwork> lista = new List<Artwork>();
            //foreach (var item in Request.Form.Files)
            //{
            //    lista.Add(item.FileName);

            //}

            Exhibit exhibit = null;
            try
            {
                if (ModelState.IsValid)
                { //TODO: CHECK IF WORKING
                    if (exist == true)
                    {
                        var exhibitId = ArtRepository.GetExhibitId(Id);
                        var artwork = await ArtRepository.AddArtWithExistingExhibitId(_context, _hostEnvironment, imageModel, member, exhibitId);
                    }
                    else if (exist == false)
                    {
                        exhibit = await ArtRepository.CreateExhibit(_context, member);
                        var artwork = await ArtRepository.AddArt(_context, _hostEnvironment, imageModel, member, exhibit);
                    }
                    else
                    {
                        var artwork = await ArtRepository.AddArt(_context, _hostEnvironment, imageModel, member, exhibit);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: Fixa en errorsida

                return View("Error", ex);

            }
            return RedirectToAction($"Index", new { Id });
        }
    }
}


