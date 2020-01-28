using ObjReader.Data.DataStore;
using ObjReader.TypeParsers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjReader.TypeParsers {

    public class ObjectParser : TypeParserBase, IObjectParser {

        private readonly IObjectDataStore _objectDataStore;

        public ObjectParser(IObjectDataStore objectDataStore) {
            _objectDataStore = objectDataStore;
        }

        protected override string Keyword {
            get { return "o"; }
        }

        public override void Parse(string line) {
            _objectDataStore.PushObject(line);
        }
    }
}
