using System.Net;

namespace Alpha.Vendas.Api.Exceptions
{
  public class AlphaNotFoundException : AlphaException
  {
    public AlphaNotFoundException(string message) : base(message, HttpStatusCode.NotFound)
    {
    }
  }
}