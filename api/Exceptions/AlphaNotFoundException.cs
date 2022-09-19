using System.Net;

namespace Alpha.Pesagem.Api.Exceptions
{
  public class AlphaNotFoundException : AlphaException
  {
    public AlphaNotFoundException(string message) : base(message, HttpStatusCode.NotFound)
    {
    }
  }
}