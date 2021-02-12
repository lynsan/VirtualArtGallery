﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DSU21_5.Areas.Identity.Data;
using DSU21_5.Data;
using DSU21_5.Models;
using DSU21_5.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DSU21_5.Controllers
{
    public class CommunityController : Controller
    {
        public IMemberRepository MemberRepository {get; set;}
        public IImageRepository ImageRepository { get; set; }
        public IArtRepository ArtRepository { get; set; }
        public ProfileViewModel ProfileViewModel { get; set; }
        public CommunityViewModel CommunityViewModel { get; set; }


        public CommunityController(IMemberRepository memberRepository, IImageRepository imageRepository, IArtRepository artRepository, IRelationshipRepository relationshipRepository)
        {
            MemberRepository = memberRepository;
            ImageRepository = imageRepository;
            ArtRepository = artRepository;
           
        }

        public async Task<IActionResult> Index()
        {
            //List<Member> members = await MemberRepository.GetAllMembers();
            List<Member> listOfMembers = await MemberRepository.GetAllMembers();
            List<Image> listOfImages = ImageRepository.GetAllImagesFromDbConnectedToUsers(listOfMembers);




            //var viewModel = new CommunityViewModel(listOfImages, members)
            //{
            //    Members = members
            //};

            //return View(viewModel);



            CommunityViewModel = new CommunityViewModel(listOfImages, listOfMembers);
            return View(CommunityViewModel);
        
        }
    
        public async Task<IActionResult> Profile(string Id)
        {
            Image image = ImageRepository.GetImageFromDb(Id);
            Member member = await MemberRepository.GetMember(Id);
            IEnumerable<Artwork> artwork = await ArtRepository.GetPostedArtFromUniqueUser(Id);
            ProfileViewModel = new ProfileViewModel(artwork, member, image);
            return View(ProfileViewModel);
        }
    }
}
