namespace BRIX.GameService.Contracts.Common
{
    /// <summary>
    /// Контракт, встречающий ProblemDetails с сервера. Знает про расширение ProblemDetalization, можно расширить 
    /// при необходимости и другими свойствами из ProblemDetails. Этот контракт, как обобщённое решение для обработки
    /// ответов с ошибками, скорее всего переберётся в проект общий для нескольких сервисов.
    /// </summary>
    public class ProblemResponse
    {
        public string Title { get; set; } = string.Empty;

        public int Status {  get; set; }

        public ProblemDetalization Detalization { get; set; } = new();
    }
}
