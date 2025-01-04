namespace BRIX.GameService.Contracts.Common
{
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
}
