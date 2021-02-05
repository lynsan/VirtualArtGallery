﻿using DSU21_5.Models;
using DSU21_5.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DSU21_5.Data
{
    public class ArtRepository : IArtRepository
    {
        ImageDbContext db;
        public List<ArtworkInformation> ArtworkInformation { get; set; } = new List<ArtworkInformation>();
        public IEnumerable<Artwork> ArtToExhibits { get; set; }

        public ArtRepository(ImageDbContext context)
        {
            db = context;
        }

        public bool CheckIfIdExists(string id)
        {
            if (db.Artworks.Any(x => x.UserId == id))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public int? GetExhibitId(string id)
        {
            var getId = db.Artworks.Where(x => x.UserId == id).FirstOrDefault();
            int? exhibitId = getId.ExhibitId;
            return exhibitId;
            
        }
        /// <summary>
        /// Get all ids from artwork-table
        /// </summary>
        /// <returns>list of user-ids</returns>
        //public async Task<IEnumerable<string>> GetAllUserIdFromArtwork()
        //{
        //    IEnumerable<string> userId = db.Artworks.Select(x => x.UserId);
        //   await db.SaveChangesAsync();
        //    return userId;
        //}

        //public IEnumerable<string> GetListOfUniqueUserId(IEnumerable<string> listOfIds)
        //{
        //    List<string> uniqueIds = new List<string>();
        //    foreach (var item in listOfIds)
        //    {
        //        if (!uniqueIds.Contains(item))
        //        {
        //            uniqueIds.Add(item);
        //        }
        //    }
        //    return uniqueIds;
        //}

        
        public async Task<ArtworkViewModel> GetViewModel(List<Member> members)
        {
            var list = await GetArtThatsPosted();
            var model = new ArtworkViewModel(list, members);
            return model;
        }
        public async Task<IEnumerable<Artwork>> GetArtThatsPosted()
        {
            IEnumerable<Artwork> images = db.Artworks;
            await db.SaveChangesAsync();
            return images;
        }
        public async Task<Exhibit> CreateExhibit(ImageDbContext context, Member member)
        {
            Exhibit exhibit = new Exhibit()
            {
                
                Date = "2020-10-12",
                Name = "test"
            };
            context.Add(exhibit);
            await db.SaveChangesAsync();
            return exhibit;
        }
        public async Task<Artwork> AddArt(ImageDbContext context,IWebHostEnvironment hostEnvironment, Artwork artworkModel, Member member, Exhibit exhibit)
        {
            string wwwRootPath = hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(artworkModel.ImageFile.FileName);
            string extention = Path.GetExtension(artworkModel.ImageFile.FileName);
            artworkModel.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extention;
            artworkModel.UserId = member.MemberId;
            if (exhibit != null)
            {
            artworkModel.ExhibitId = exhibit.Id;

            }
        
           // artworkModel.Firstname = member.Firstname;
            //artworkModel.Lastname = member.Lastname;
            
            string path = Path.Combine(wwwRootPath + "/imagesArt/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await artworkModel.ImageFile.CopyToAsync(fileStream);
            }
            context.Add(artworkModel);
            await db.SaveChangesAsync();
            return artworkModel;
        }
        public async Task<Artwork> AddArtWithExistingExhibitId(ImageDbContext context, IWebHostEnvironment hostEnvironment, Artwork artworkModel, Member member, int? exhibit)
        {
            string wwwRootPath = hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(artworkModel.ImageFile.FileName);
            string extention = Path.GetExtension(artworkModel.ImageFile.FileName);
            artworkModel.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extention;
            artworkModel.UserId = member.MemberId;
            if (exhibit != null)
            {
                artworkModel.ExhibitId = exhibit;

            }

            // artworkModel.Firstname = member.Firstname;
            //artworkModel.Lastname = member.Lastname;

            string path = Path.Combine(wwwRootPath + "/imagesArt/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await artworkModel.ImageFile.CopyToAsync(fileStream);
            }
            context.Add(artworkModel);
            await db.SaveChangesAsync();
            return artworkModel;
        }
        public async Task<IEnumerable<Artwork>> GetPostedArtFromUniqueUser(string Id)
        {
            IEnumerable<Artwork> art = db.Artworks.Where(x => x.UserId == Id && x.ExhibitId == null);
            await db.SaveChangesAsync();
            return art;
        }
        public async Task<IEnumerable<Artwork>> GetArtToExhibitions(string id)
        {
            IEnumerable<Artwork> art = db.Artworks.Where(x => x.UserId == id && x.ExhibitId != null);
            await db.SaveChangesAsync();
            return art;
        }
        public async Task<IEnumerable<Artwork>> GetAllArtToExhibitions()
        {
            IEnumerable<Artwork> art = db.Artworks.Where(x => x.ExhibitId != null);
            await db.SaveChangesAsync();
            ArtToExhibits = art;
            return ArtToExhibits;
        }
        //public async Task<IEnumerable<ArtworkInformation>> GetAllInformationToExhibit(IEnumerable<ArtworkInformation> art)
        //{
        //    foreach (var item in art)
        //    {

        //    }
        //}

        //public async Task<Exhibit> GetExhibit(string id)
        //{
        //    Exhibit exhibit = db.Exhibit.Where(x => x.MemberId == id).FirstOrDefault();
        //    await db.SaveChangesAsync();
        //    return exhibit;
        //}
        //public async Task<List<Exhibit>> GetAllExhibits()
        //{
        //    List<Exhibit> exhibits = new List<Exhibit>();

        //    List<String> Ids = db
        //    .Exhibit
        //    .Select(u => u.MemberId)
        //    .ToList();

        //    int counter = 0;
        //    foreach (string id in Ids)
        //    {
        //        foreach (var item in exhibits)
        //        {
        //            if (item.MemberId == id) // om id redan finns, gå vidare. 
        //            {
        //                counter++;
        //            }

        //        }
        //        if (counter == 0) // om countern är noll ska en 
        //        {
        //            Exhibit exhibit = await GetExhibit(id);
        //            exhibits.Add(exhibit); 
        //        }
        //    }

        //    return exhibits;
        //}
        public async Task<List<ArtworkInformation>> GetAllInformation(string Id)
        {

            IEnumerable<Artwork> artwork = db.Artworks.Where(x => x.UserId == Id);
          
            await db.SaveChangesAsync();
            foreach (var item in artwork)
            {
                ArtworkInformation.Add(new ArtworkInformation()
                {
                    Description = item.Description,
                    Source = item.ImageName,

                }) ;
            }
            
            return ArtworkInformation.ToList();
        }
        public Artwork GetArtworkThatsGonnaBeDeleted(int id)
        {
            Artwork artwork = db.Artworks.Where(x => x.ArtworkId == id).FirstOrDefault();
            return artwork;
        }
        public async Task<Artwork> DeleteArtworkFromArtworkTable(IWebHostEnvironment hostEnvironment, Artwork artwork)
        {
            db.Artworks.Remove(artwork);
            await db.SaveChangesAsync();
            string wwwRootPath = hostEnvironment.WebRootPath;
            string path = Path.Combine(wwwRootPath + "/imagesArt/", artwork.ImageName);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                File.Delete(path);
            }
           
            return artwork;
        }

    }
}
