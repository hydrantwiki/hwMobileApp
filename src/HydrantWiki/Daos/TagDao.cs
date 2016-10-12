using System;
using System.Linq;
using System.Collections.Generic;
using HydrantWiki.Objects;
using LiteDB;

namespace HydrantWiki.Daos
{
    public class TagDao : AbstractDao<Tag>
    {
        public TagDao(LiteDatabase _db)
            : base(_db)
        {
        }

        public override string CollectionName
        {
            get
            {
                return "Tags";
            }
        }

        public override void BuildIndexes()
        {
            m_Collection.EnsureIndex("TagTime", new IndexOptions { Unique = false });
            m_Collection.EnsureIndex("SentToServer", new IndexOptions { Unique = false });
        }

        public List<Tag> GetRecent()
        {
            Query query = Query.All("TagTime", Query.Descending);
            return m_Collection.Find(query, limit: 50).ToList();
        }

        public List<Tag> GetNotSentToServer()
        {
            Query query = Query.EQ("SentToServer", false);

            return m_Collection.Find(query, limit: 50).OrderBy((Tag arg) => arg.TagTime).ToList();
        }
    }
}
