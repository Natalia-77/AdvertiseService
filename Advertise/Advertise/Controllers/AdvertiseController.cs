using AdvertisePublish.Models;
using AdvertisePudlish.Exceptions;
using AdvertisePudlish.Helper;
using AdvertisePudlish.Models;
using AdvertisePudlish.Services.Abstractions;
using AdvertisePudlish.Services.Implementation;
using AutoMapper;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Drawing.Imaging;

namespace AdvertisePudlish.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertiseController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IImageConverter _imageconverter;
        private readonly AppDbContext _context;

        public AdvertiseController(IMapper mapper, IImageConverter imageconverter, AppDbContext context)
        {
            _mapper = mapper;
            _imageconverter = imageconverter;
            _context = context;
        }

        /// <summary>
        /// Add new advertise
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAdvertiseAsync([FromBody] CreateAdvertiseViewModel model)
        {
            var images = new List<Image>();
            var adv = _mapper.Map<Advertise>(model);
            foreach (var item in model.Images)
            {
                var img = _imageconverter.FromBase64StringToImage(item.Name);
                string randomFilename = Path.GetRandomFileName() + ".jpg";
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "images", randomFilename);
                img.Save(dir, ImageFormat.Jpeg);

                var im = _mapper.Map<Image>(item);
                im.Name = randomFilename;
                im.Advertise = adv;
                images.Add(im);
            }

            adv.Images = images;
            _context.Images.AddRange(images);
            _context.SaveChanges();

            return Ok(new { adv.Id });
        }

        /// <summary>
        /// Get list all of advertise
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("list")]
        public async Task<IActionResult> GetItemList([FromQuery] PageParamsModel pageParams)
        {
            var res = await _context.Advertises.Include(i => i.Images.Take(1))
                     .Select(item => _mapper.Map<AdvertiseViewModel>(item)).ToListAsync();

            int count = 10;
            decimal maxPageNumber = (Math.Ceiling((decimal)res.Count / count));
            int currentPage = pageParams.Page;
            if (currentPage <= 0 || currentPage > maxPageNumber)
            {
                pageParams.Page = 1;                              
            }

            return Ok(new
            {
                data = new
                {
                    current_page = pageParams.Page,
                    data = res.Select(x => x).ToList().Skip(count * (pageParams.Page - 1)).Take(count),
                    last_page = (Math.Ceiling((decimal)res.Count / count)),
                    total_items = res.Count,
                }

            });            
        }

        /// <summary>
        /// Get list items sorted by descending 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("descendingPrice")]
        public async Task<IActionResult> DescendingByPrice()
        {
            var res = await _context.Advertises.Include(i => i.Images.Take(1))
                     .Select(item => _mapper.Map<AdvertiseViewModel>(item)).ToListAsync();
            res.Sort(new PriceComparer());
            return Ok(res);
            
        }

        /// <summary>
        /// Get list items sorted by descending 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ascendingPrice")]
        public async Task<IActionResult> AscendingByPrice()
        {
            var res = await _context.Advertises.Include(i => i.Images.Take(1)).OrderBy(u=>u.Price)
                     .Select(item => _mapper.Map<AdvertiseViewModel>(item)).ToListAsync();           
            return Ok(res);

        }

        /// <summary>
        /// Search advertise by title
        /// </summary>
        /// <param name="choise"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("title")]
        public async Task<IActionResult> GetItemByTitle([FromQuery] OperationType choise)
        {
            string search = choise.Title ?? "";
            var res2 = _context.Advertises.Where(x => x.Title.Contains(search)).Include(i => i.Images)
                .Select(item => _mapper.Map<AdvertiseViewModel>(item));
            return Ok(res2);
        }

        /// <summary>
        /// Search advertise by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("id")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var res = await _context.Advertises.Where(c => c.Id == id).Include(i => i.Images)
                     .Select(item => _mapper.Map<AdvertiseViewModel>(item)).FirstAsync();
            return Ok(res);

        }
    }
}
