using System;
using System.Collections;
using System.Collections.Generic;

namespace Wool
{
    public class WoolTagContainer
    {
        public WoolTagContainer()
        {
            tags = new List<WoolTagToType>(Utils.GetWoolTags());
        }

        public Type GetTag(string tag)
        {
            WoolTagToType t = tags.Find(x => x.tag == tag);
            if (t == null)
            {
                return null;
            }

            return t.type;
        }

        List<WoolTagToType> tags = new List<WoolTagToType>();
    }

    public class WoolTagToType
    {
        public WoolTagToType(string tag, Type type)
        {
            this.tag = tag;
            this.type = type;
        }

        public string tag;
        public Type type;
    }
}