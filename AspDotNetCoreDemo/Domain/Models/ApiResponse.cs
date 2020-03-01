using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace AspDotNetCoreDemo.Domain.Models
{
    public class ApiResponse<T>
        where T : class
    {
        public ApiResponse()
        {
            ErrorMessages = new List<string>();
        }

        public bool HasError 
        {
            get
            {
                return ErrorMessages.Count > 0;
            }
        }

        public T Data { get; private set; }

        public List<string> ErrorMessages { get; private set; }

        public void AddError(string message)
        {
            ErrorMessages.Add(message);
        }

        public void AddError(string[] messages)
        {
            ErrorMessages.AddRange(messages);
        }

        public void AddError(ModelStateDictionary modelStateDictionary)
        {
            var errorsCollection = modelStateDictionary.Values.Select(v => v.Errors);

            foreach (var errorCollection in errorsCollection)
            {
                foreach (var error in errorCollection)
                {
                    ErrorMessages.Add(error.ErrorMessage);
                }
            }
        }

        public void AddError(IEnumerable<IdentityError> identityErrors)
        {
            foreach (var identityError in identityErrors)
            {
                ErrorMessages.Add(identityError.Description);
            }
        }

        public void AddData(T data)
        {
            Data = data;
        }

    }
}
