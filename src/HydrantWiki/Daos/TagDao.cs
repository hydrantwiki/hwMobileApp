using System;
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
            m_Collection.EnsureIndex("TagTime");
        }
    }
}
