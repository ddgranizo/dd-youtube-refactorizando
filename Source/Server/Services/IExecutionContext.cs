using System;
using System.Collections.Generic;
using System.Text;

namespace Refactorizando.Server.Services
{
    public interface IExecutionContext
    {
        string GetUserId();
    }
}
