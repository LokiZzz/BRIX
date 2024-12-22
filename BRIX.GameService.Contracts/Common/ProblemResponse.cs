namespace BRIX.GameService.Contracts.Common
{
    /// <summary>
    /// Контракт, встречающий ProblemDetails с сервера. Знает про расширение ProblemDetalization, можно расширить 
    /// при необходимости и другими свойствами из ProblemDetails.
    /// </summary>
    public class ProblemResponse
    {
        public string Title { get; set; } = string.Empty;

        public int Status {  get; set; }

        public ProblemDetalization Detalization { get; set; } = new();
    }

    /// <summary>
    /// Детализация возникших проблем.
    /// </summary>
    public class ProblemDetalization
    {
        public ProblemDetalization() { }

        public ProblemDetalization((string Code, string Message)[] messages)
        {
            Problems = messages
                .Select(x => new Problem() { Code = x.Code, Message = x.Message })
                .ToList();
        }

        public List<Problem> Problems { get; set; } = [];
    }

    /// <summary>
    /// Проблема с сообщением и кодом ошибки. Коды используются в том числе для локализации.
    /// </summary>
    public class Problem
    {
        public string Code { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}
