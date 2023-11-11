using PasswordManager.Shared.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Entity.Dtos
{
    public class ResponseDto<T>
    {
        public T Data { get; set; }
        public ResultStatus ResultStatus { get; set; }
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> ErrorMessages { get; set; } // Birden fazla hata mesajı için

        public ResponseDto()
        {

        }

        public ResponseDto(T data, ResultStatus resultStatus = 0, string successMessage = "")
        {
            Data = data;
            ResultStatus = resultStatus;
            SuccessMessage = successMessage;
        }

        public ResponseDto(string errorMessage)
        {
            ResultStatus = ResultStatus.Error;
            ErrorMessage = errorMessage;
        }

        public ResponseDto(List<string> errorMessages)
        {
            ResultStatus = ResultStatus.Error;
            ErrorMessages = errorMessages;
        }
    }
}
