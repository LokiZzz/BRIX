namespace BRIX.Web.Client.Models.Common
{
    public class OperationResult
    {
        public bool Successfull { get; set; }

        public List<string> Errors { get; set; } = [];
    }
}
