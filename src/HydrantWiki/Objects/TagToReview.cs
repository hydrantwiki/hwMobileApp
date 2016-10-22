using System;
using System.Collections.Generic;

namespace HydrantWiki.Objects
{
    public class TagToReview : Tag
    {
        public string Username { get; set; }

        public int UserTagsApproved { get; set; }

        public int UserTagsRejected { get; set; }

        public string Status { get; set; }

        public List<Hydrant> NearbyHydrants { get; set; }

        public string ImageUrl { get; set; }

        public string ThumbnailUrl { get; set; }

        public string DisplayUsername
        {
            get
            {
                return string.Format("Username: {0}", Username);
            }
        }

        public string UserInfo
        {
            get
            {
                return string.Format(
                    "Approved: {0}, Rejected: {1}",
                    UserTagsApproved,
                    UserTagsRejected);
            }
        }
    }
}
