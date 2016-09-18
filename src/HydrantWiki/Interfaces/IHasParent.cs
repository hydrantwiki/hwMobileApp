using System;

namespace HydrantWiki.Interfaces
{
    public interface IHasParent
    {
        Guid ParentId { get; set; }
    }
}

