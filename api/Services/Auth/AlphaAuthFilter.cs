using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alpha.Pesagem.Api.Services.Auth
{
  public class ApiConsumerFilter : ActionFilterAttribute
  {
    public override void OnActionExecuting(ActionExecutingContext context)
    {
      var token = context.HttpContext.Request.Headers["Alpha"];

      if (token != "0EACD71FD4BE0008C5658DACC419F5386ABDF5D34A3B2468B1906AAEC2B14F9D")
      {
        context.Result = new ForbidResult();
      }
    }
  }
}
