using System;
using System.Net;

namespace Alpha.Vendas.Api.Exceptions
{
  public class AlphaException : Exception
  {
    public HttpStatusCode StatusCode { get; set; }

    public AlphaException(string message, HttpStatusCode? statusCode) : base(message)
    {
      this.StatusCode = statusCode ?? HttpStatusCode.InternalServerError;
    }
  }
}