using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace inquizitor2
{
    public interface IVote : IComparer<IVote>
    {
        int Upvotes
        {
            get; set;
        }

    }
}