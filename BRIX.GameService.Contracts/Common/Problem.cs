namespace BRIX.GameService.Contracts.Common
{
    /// <summary>
    /// Проблема с сообщением и кодом ошибки. Коды используются в том числе для локализации.
    /// </summary>
    public class Problem
    {
        public string Code { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}
