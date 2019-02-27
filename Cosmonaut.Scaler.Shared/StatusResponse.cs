namespace Cosmonaut.Scaler.Shared
{
    public class StatusResponse
    {
        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public StatusResponse()
        {
            
        }

        public StatusResponse(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public StatusResponse(string errorMessage)
        {
            IsSuccess = false;
            ErrorMessage = errorMessage;
        }
    }
}