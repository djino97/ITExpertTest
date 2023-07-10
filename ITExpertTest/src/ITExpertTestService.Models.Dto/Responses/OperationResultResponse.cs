using System.Collections.Generic;

namespace ExchangeRate.Models
{
    public record OperationResultResponse<T>
    {
        public T Result { get; set; }
        public List<string> Error { get; set; }

        public OperationResultResponse(T result = default, List<string> error = default)
        {
            Result = result;
            Error = error;
        }
    }
}