using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace HackerNews.Domain.Exceptions
{
    public class CosmosDbException : DbException
    {
        public CosmosDbException(string message) : base(message)
        {
        }
    }
}
