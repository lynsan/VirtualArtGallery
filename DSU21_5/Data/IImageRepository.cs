﻿using DSU21_5.Models;
using DSU21_5.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DSU21_5.Data
{
    public interface IImageRepository
    {
        ImageModel GetImageFromDb(string Id);
        ImageModel RemoveImageFromDb(IWebHostEnvironment hostEnvironment, ImageModel imgModel);
        Task<ImageModel> CreateNewProfilePicture(ImageDbContext context, IWebHostEnvironment hostEnvironment, ImageModel imageModel, string Id);
        Task<ShowroomViewModel> GetShowroomImages();
    }
        
}