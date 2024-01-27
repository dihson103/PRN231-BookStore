using App_client.Controllers;

namespace App_client.Dtos.Common
{
    public class SuccessResponse<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
