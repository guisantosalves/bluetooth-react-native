using System.Net;

namespace Alpha.Pesagem.Api.Exceptions
{
  ///<summary>
  /// Representa uma exceção de requisição inválida, quando por responsabilidade do usuário
  /// a mesma não pôde ser completada.
  ///</summary>
  public class AlphaBadRequestException : AlphaException
  {
    ///<summary>
    /// Construtor de AlphaBadRequestException. Deve conter a mensagem de erro.
    ///</summary>
    public AlphaBadRequestException(string message) : base(message, HttpStatusCode.BadRequest)
    {
    }
  }
}