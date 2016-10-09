using System;
using HydrantWiki.Objects;
using LiteDB;

namespace HydrantWiki.Daos
{
    public class SettingDao : AbstractDao<Setting>
    {
        public SettingDao(LiteDatabase _db)
            : base(_db)
        {
        }

        public override void BuildIndexes()
        {
            m_Collection.EnsureIndex("Name");
        }

        public override string CollectionName
        {
            get
            {
                return "Settings";
            }
        }

        public Setting GetSetting(string _name)
        {
            Query query = Query.EQ("Name", _name);
            return m_Collection.FindOne(query);
        }
    }
}

