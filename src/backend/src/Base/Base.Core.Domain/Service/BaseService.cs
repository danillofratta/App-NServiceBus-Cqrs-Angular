using Base.Common.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Core.Domain.Service
{
    /// <summary>
    /// TODO create default method to execute the process
    /// </summary>
    public class BaseService
    {
        public List<ValidationErrorDetail> _ListError { get; set; } = new();
    }
}
