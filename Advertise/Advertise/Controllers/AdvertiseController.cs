using AdvertisePublish.Models;
using AdvertisePudlish.Exceptions;
using AdvertisePudlish.Helper;
using AdvertisePudlish.Models;
using AdvertisePudlish.Services.Abstractions;
using AutoMapper;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Imaging;

namespace AdvertisePudlish.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
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
        /// <response code="200">Returns the newly created item</response>
        /// <response code="400">If not valid firlds does exist into boby</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("create")]
        public IActionResult CreateAdvertise([FromBody] CreateAdvertiseViewModel model)
        {
            var newItem = new Advertise();
            newItem.Title = model.Title;
            newItem.Description = model.Description;
            newItem.Price = model.Price;
           
            _context.Advertises.Add(newItem);
            _context.SaveChanges();

            var images = new List<Image>();

            foreach (var item in model.Images)
            {
                var img = _imageconverter.FromBase64StringToImage(item.Name);
                string randomFilename = Path.GetRandomFileName() + ".jpg";
                var dir = Path.Combine(Directory.GetCurrentDirectory(), "images", randomFilename);
                img.Save(dir, ImageFormat.Jpeg);


                Image pImage = new Image();
                pImage.Name = randomFilename;
                pImage.Advertise = newItem;
                
                images.Add(pImage);
            }
            _context.Images.AddRange(images);
            _context.SaveChanges();
            
            return Ok(new { message = "Success" });
        }

        /// <summary>
        /// Get list all of advertise
        /// </summary>
        /// <response code="200">Returns the list of items</response>
        /// <response code="400">If item is null</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("list")]
        public async Task<IActionResult> GetItemList([FromQuery] PageParamsModel pageParams)
        {
            var res = await _context.Advertises.Include(i => i.Images.Take(1))
                     .Select(item => _mapper.Map<AdvertiseViewModel>(item)).ToListAsync();

            if (res == null)
            {
                throw new NotFoundException("There is no item for display");
            }

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
        /// Get list items sorted by descending price
        /// </summary>
        /// <response code="200">Returns the list of items</response>
        /// <response code="400">If item search data is null</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("descending/price")]
        public async Task<IActionResult> DescendingByPrice()
        {
            var res = await _context.Advertises.Include(i => i.Images.Take(1))
                     .Select(item => _mapper.Map<AdvertiseViewModel>(item)).ToListAsync();
            res.Sort(new PriceComparer());
            return Ok(res);

        }

        /// <summary>
        /// Get list items sorted by ascending price
        /// </summary>
        /// <response code="200">Returns the list of items</response>
        /// <response code="400">If item search data is null</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("ascending/price")]
        public async Task<IActionResult> AscendingByPrice()
        {
            var res = await _context.Advertises.Include(i => i.Images.Take(1)).OrderBy(u => u.Price)
                     .Select(item => _mapper.Map<AdvertiseViewModel>(item)).ToListAsync();
            return Ok(res);

        }

        /// <summary>
        /// Search advertise by title
        /// </summary>
        /// <param name="choise"></param>
        /// <response code="200">Returns the list of items</response>
        /// <response code="400">If item search data is null</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("title")]
        public IActionResult GetItemByTitle([FromQuery] OperationType choise)
        {
            string search = choise.Title ?? "";
            var resTitle = _context.Advertises.Where(x => x.Title.Contains(search)).Include(i => i.Images)
                .Select(item => _mapper.Map<AdvertiseViewModel>(item));

            if (String.IsNullOrEmpty(search))
            {
                throw new NotFoundException("Title field is empty");
            }
            return Ok(resTitle);
        }

        /// <summary>
        /// Search advertise by Id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Returns the list of items</response>
        /// <response code="400">If item search data is null</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("id")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var res = await _context.Advertises.Where(c => c.Id == id).Include(i => i.Images)
                     .Select(item => _mapper.Map<AdvertiseViewModel>(item)).FirstOrDefaultAsync();

            if (res == null)
            {
                throw new NotFoundException("Not found item with such Id");
            }
            return Ok(res);

        }

        /// <summary>
        /// Sorted item lisy by ascending date
        /// </summary>
        /// <response code="200">Returns the list of items</response>
        /// <response code="400">If item search data is null</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("ascending/date")]
        public async Task<IActionResult> AscendingByDate()
        {
            var res = await _context.Advertises.Include(i => i.Images.Take(1))
                     .Select(item => _mapper.Map<AdvertiseViewModel>(item)).ToListAsync();
            res.Sort((x, y) => x.DateCreate.CompareTo(y.DateCreate));
            return Ok(res);

        }

        /// <summary>
        /// Sorted item lisy by descending date
        /// </summary>
        /// <response code="200">Returns the list of items</response>
        /// <response code="400">If item search data is null</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("descending/date")]
        public async Task<IActionResult> DescendingByDate()
        {
            var res = await _context.Advertises.Include(i => i.Images.Take(1))
                     .Select(item => _mapper.Map<AdvertiseViewModel>(item)).ToListAsync();
            res.Sort(new DateComparer());
            return Ok(res);

        }
    }
}
