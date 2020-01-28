using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalEntities {

    public class Quad2f {

        public Quad2f(float x0, float y0, float x1, float y1, float x2, float y2, float x3, float y3) {

            this.v0 = new Vec2f(x0, y0);
            this.v1 = new Vec2f(x1, y1);
            this.v2 = new Vec2f(x2, y2);
        }

        public Quad2f(Quad2f other) {

            this.v0 = new Vec2f(other.v0);
            this.v1 = new Vec2f(other.v1);
            this.v2 = new Vec2f(other.v2);
            this.v3 = new Vec2f(other.v3);
        }

        public Quad2f(Vec2f v0, Vec2f v1, Vec2f v2, Vec2f v3) {

            this.v0 = new Vec2f(v0);
            this.v1 = new Vec2f(v1);
            this.v2 = new Vec2f(v2);
            this.v3 = new Vec2f(v3);
        }

        public Quad2f(Vec3f v0, Vec3f v1, Vec3f v2, Vec3f v3) {

            this.v0 = new Vec2f(v0);
            this.v1 = new Vec2f(v1);
            this.v2 = new Vec2f(v2);
            this.v3 = new Vec2f(v3);
        }

        public Quad2f(Vec4f v0, Vec4f v1, Vec4f v2, Vec4f v3) {

            this.v0 = new Vec2f(v0);
            this.v1 = new Vec2f(v1);
            this.v2 = new Vec2f(v2);
            this.v3 = new Vec2f(v3);
        }

        public Quad2f(Triangle2f t0, Triangle2f t1) {

            this.v0 = new Vec2f(t0.v0);
            this.v1 = new Vec2f(t0.v1);
            this.v2 = new Vec2f(t0.v2);
            this.v3 = new Vec2f(t1.v2);
        }

        public Quad2f() {

            this.v0 = new Vec2f();
            this.v1 = new Vec2f();
            this.v2 = new Vec2f();
        }

        public Triangle2f toTriangle1() {
            return new Triangle2f(v0, v1, v2);
        }

        public Triangle2f toTriangle2() {
            return new Triangle2f(v0, v2, v3);
        }

        public int[] order = new int[4] { 0, 1, 2, 3 };
        public bool sorted = false;
        public CohenSutherlandClip clip;

        public Vec2f v0;
        public Vec2f v1;
        public Vec2f v2;
        public Vec2f v3;

        public float x0 { get => v0.x; set => v0.x = value; }
        public float y0 { get => v0.y; set => v0.y = value; }

        public float x1 { get => v1.x; set => v1.x = value; }
        public float y1 { get => v1.y; set => v1.y = value; }

        public float x2 { get => v2.x; set => v2.x = value; }
        public float y2 { get => v2.y; set => v2.y = value; }

        public float x3 { get => v3.x; set => v3.x = value; }
        public float y3 { get => v3.y; set => v3.y = value; }

        public Line2f trimOuter(Line2f line) {

            CohenSutherlandClip clip = new CohenSutherlandClip(v0, v2);
            Line2f result = clip.Clip(line);
            return result;
        }
    }
}
