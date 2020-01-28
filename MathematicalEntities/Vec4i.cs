using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalEntities {

    public class Vec4i {

        public int x;
        public int y;
        public int z;
        public int w;

        public int r { get => x; set => x = value; }
        public int g { get => y; set => y = value; }
        public int b { get => z; set => z = value; }
        public int a { get => z; set => z = value; }

        public Vec4i() {
            this.x = 0;
            this.y = 0;
            this.z = 0;
            this.w = 0;
        }

        public Vec4i(Vec2i other) {
            this.x = other.x;
            this.y = other.y;
            this.z = 0;
            this.w = 0;
        }

        public Vec4i(Vec4i other) {
            this.x = other.x;
            this.y = other.y;
            this.z = other.z;
            this.w = other.w;
        }

        public Vec4i(int x, int y, int z, int w) {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public Vec4i(int value) {
            this.x = value;
            this.y = value;
            this.z = value;
            this.w = value;
        }
    }
}
