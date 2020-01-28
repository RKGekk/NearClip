using ObjReader.Data.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjReader.Data.Elements {

    public class ObjectContainer : IFaceGroup {

        private readonly List<Face> _faces = new List<Face>();
        private readonly List<Group> _groups = new List<Group>();

        public ObjectContainer(string name) {
            Name = name;
        }

        public string Name { get; private set; }
        public Material Material { get; set; }

        public IList<Face> Faces { get { return _faces; } }
        public IList<Group> Groups { get { return _groups; } }

        public void AddFace(Face face) {
            _faces.Add(face);
        }

        public void AddGroup(Group group) {
            _groups.Add(group);
        }
    }
}
