namespace BRIX.Web.Client.Models.Common
{
    public class OperationResult
    {
        public bool Successfull { get; set; }

        public string[] Errors { get; set; } = [];
    }
}
