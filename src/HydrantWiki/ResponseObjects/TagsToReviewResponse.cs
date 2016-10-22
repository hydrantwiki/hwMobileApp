using System.Collections.Generic;
using HydrantWiki.Objects;

namespace HydrantWiki.ResponseObjects
{
    public class TagsToReviewResponse
    {
        public bool Success { get; set; }
        public List<TagToReview> Tags { get; set; }
    }
}
