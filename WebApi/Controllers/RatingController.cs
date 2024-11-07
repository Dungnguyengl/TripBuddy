using CommonService.Services;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController(IInternalService internalService) : ControllerBase
    {
        //[HttpPost]
        //public Task<Response<CreateReviewDto>> CreateReview(CreateReviewCommand command)
        //{
        //    return 
        //}
    }
}
