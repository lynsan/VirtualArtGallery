﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSU21_5.Data;
using DSU21_5.Models;
using DSU21_5.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace DSU21_5.Controllers
{
    public class ArtworkController : Controller
    {
        public IArtRepository ArtRepository { get; set; }
        public ArtworkViewModel ArtworkViewModel;

        public ArtworkController(IArtRepository artRepository)
        {
            ArtRepository = artRepository;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Artwork> listOfMovies = await ArtRepository.GetArtThatsPosted();
            ArtworkViewModel = new ArtworkViewModel(listOfMovies);
            return View(ArtworkViewModel);
        }
    }
}