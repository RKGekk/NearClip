using MathematicalEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicRender.Engine {

    public class RPolygon {

        public int id;
        public bool visible = true;
        public bool sorted = false;
        public int[] order = new int[3] { 0, 1, 2 };
        public RasterType rasterType;
        public RTexture texture;
        public RVertex vertex0;
        public RVertex vertex1;
        public RVertex vertex2;

        public Vec3f v0 { get => vertex0.position; set => vertex0.position = value; }
        public Vec3f v1 { get => vertex1.position; set => vertex1.position = value; }
        public Vec3f v2 { get => vertex2.position; set => vertex2.position = value; }

        public Vec4f c0 { get => vertex0.color; set => vertex0.color = value; }
        public Vec4f c1 { get => vertex1.color; set => vertex1.color = value; }
        public Vec4f c2 { get => vertex2.color; set => vertex2.color = value; }

        public Vec2f t0 { get => vertex0.textureCoordinates; set => vertex0.textureCoordinates = value; }
        public Vec2f t1 { get => vertex1.textureCoordinates; set => vertex1.textureCoordinates = value; }
        public Vec2f t2 { get => vertex2.textureCoordinates; set => vertex2.textureCoordinates = value; }

        public Vec3f n0 { get => vertex0.normal; set => vertex0.normal = value; }
        public Vec3f n1 { get => vertex1.normal; set => vertex1.normal = value; }
        public Vec3f n2 { get => vertex2.normal; set => vertex2.normal = value; }

        public float x0 { get => v0.x; set => v0.x = value; }
        public float y0 { get => v0.y; set => v0.y = value; }
        public float z0 { get => v0.z; set => v0.z = value; }

        public float x1 { get => v1.x; set => v1.x = value; }
        public float y1 { get => v1.y; set => v1.y = value; }
        public float z1 { get => v1.z; set => v1.z = value; }

        public float x2 { get => v2.x; set => v2.x = value; }
        public float y2 { get => v2.y; set => v2.y = value; }
        public float z2 { get => v2.z; set => v2.z = value; }

        public float r0 { get => vertex0.r; set => vertex0.r = value; }
        public float g0 { get => vertex0.g; set => vertex0.g = value; }
        public float b0 { get => vertex0.b; set => vertex0.b = value; }

        public float r1 { get => vertex1.r; set => vertex1.r = value; }
        public float g1 { get => vertex1.g; set => vertex1.g = value; }
        public float b1 { get => vertex1.b; set => vertex1.b = value; }

        public float r2 { get => vertex2.r; set => vertex2.r = value; }
        public float g2 { get => vertex2.g; set => vertex2.g = value; }
        public float b2 { get => vertex2.b; set => vertex2.b = value; }

        public float tu0 { get => vertex0.u; set => vertex0.u = value; }
        public float tv0 { get => vertex0.v; set => vertex0.v = value; }

        public float tu1 { get => vertex1.u; set => vertex1.u = value; }
        public float tv1 { get => vertex1.v; set => vertex1.v = value; }

        public float tu2 { get => vertex2.u; set => vertex2.u = value; }
        public float tv2 { get => vertex2.v; set => vertex2.v = value; }

        public float nx0 { get => vertex0.nx; set => vertex0.nx = value; }
        public float ny0 { get => vertex0.ny; set => vertex0.ny = value; }
        public float nz0 { get => vertex0.nz; set => vertex0.nz = value; }

        public float nx1 { get => vertex1.nx; set => vertex1.nx = value; }
        public float ny1 { get => vertex1.ny; set => vertex1.ny = value; }
        public float nz1 { get => vertex1.nz; set => vertex1.nz = value; }

        public float nx2 { get => vertex2.nx; set => vertex2.nx = value; }
        public float ny2 { get => vertex2.ny; set => vertex2.ny = value; }
        public float nz2 { get => vertex2.nz; set => vertex2.nz = value; }

        public Vec3f normal;
        public Vec3f center;

        public enum RasterType : int {
            FlatColor,
            InterpolatedColor,
            Textured
        }

        public RPolygon() {
            this.id = ++lastId;
        }

        public RPolygon(RPolygon other) {

            this.vertex0 = new RVertex(other.vertex0);
            this.vertex1 = new RVertex(other.vertex1);
            this.vertex2 = new RVertex(other.vertex2);
            this.rasterType = other.rasterType;
            this.texture = other.texture;
            this.normal = other.normal;
            this.center = other.center;
            this.id = ++lastId;
        }

        public RPolygon(RVertex vertex1, RVertex vertex2, RVertex vertex3) {

            this.vertex0 = vertex1;
            this.vertex1 = vertex2;
            this.vertex2 = vertex3;

            this.center = centerCalc();
            this.id = ++lastId;
        }

        public RPolygon(RVertex vertex1, RVertex vertex2, RVertex vertex3, bool isNormalCalc) {

            this.vertex0 = vertex1;
            this.vertex1 = vertex2;
            this.vertex2 = vertex3;

            if (isNormalCalc) {
                this.normal = normalizedNormalCalc();
                this.vertex0.normal = this.normal;
                this.vertex1.normal = this.normal;
                this.vertex2.normal = this.normal;
            }

            this.center = centerCalc();
            this.id = ++lastId;
        }

        public RPolygon(RVertex vertex1, RVertex vertex2, RVertex vertex3, RasterType rasterType) {

            this.vertex0 = vertex1;
            this.vertex1 = vertex2;
            this.vertex2 = vertex3;

            this.rasterType = rasterType;

            this.center = centerCalc();
            this.id = ++lastId;
        }

        public RPolygon(RVertex vertex1, RVertex vertex2, RVertex vertex3, RasterType rasterType, RTexture texture) {

            this.vertex0 = vertex1;
            this.vertex1 = vertex2;
            this.vertex2 = vertex3;

            this.rasterType = rasterType;
            this.texture = texture;

            this.center = centerCalc();
            this.id = ++lastId;
        }

        public RPolygon(RVertex vertex1, RVertex vertex2, RVertex vertex3, RasterType rasterType, bool isNormalCalc) {

            this.vertex0 = vertex1;
            this.vertex1 = vertex2;
            this.vertex2 = vertex3;

            this.rasterType = rasterType;

            if (isNormalCalc) {
                this.normal = normalizedNormalCalc();
                this.vertex0.normal = this.normal;
                this.vertex1.normal = this.normal;
                this.vertex2.normal = this.normal;
            }

            this.center = centerCalc();
            this.id = ++lastId;
        }

        public RPolygon reSort() {
            RPolygon result = new RPolygon(this);
            result.sort();
            return result;
        }

        public void sort() {

            RVertex A = vertex0;
            RVertex B = vertex1;
            RVertex C = vertex2;
            RVertex Temp = new RVertex();
            int t;
            bool isSwitchAction = false;

            if (B.y < A.y) {

                Temp = A;
                A = B;
                B = Temp;

                t = order[0];
                order[0] = order[1];
                order[1] = t;

                isSwitchAction = true;
            }

            if (C.y < A.y) {

                Temp = A;
                A = C;
                C = Temp;

                t = order[0];
                order[0] = order[2];
                order[2] = t;

                isSwitchAction = true;
            }

            if (A.y > C.y) {

                Temp = C;
                C = A;
                A = Temp;

                t = order[2];
                order[2] = order[0];
                order[0] = t;

                isSwitchAction = true;
            }

            if (B.y > C.y) {

                Temp = C;
                C = B;
                B = Temp;

                t = order[2];
                order[2] = order[1];
                order[1] = t;

                isSwitchAction = true;
            }

            if (isSwitchAction) {
                vertex0 = A;
                vertex1 = B;
                vertex2 = C;
            }
            sorted = true;
        }

        public RPolygon reClock() {

            return new RPolygon(vertex0, vertex2, vertex1);
        }

        public bool isEpsilonGeometry() {
            if ((GeneralVariables.FCMP(v0.x, v1.x) && GeneralVariables.FCMP(v1.x, v2.x)) || (GeneralVariables.FCMP(v0.y, v1.y) && GeneralVariables.FCMP(v1.y, v2.y)))
                return true;
            else
                return false;
        }

        public bool isOffScreen(Vec2f leftTop, Vec2f rightBottom) {
            if (sorted) {
                if (v2.y < leftTop.y || v0.y > rightBottom.y || (v0.x < leftTop.x && v1.x < leftTop.x && v2.x < leftTop.x) || (v0.x > rightBottom.x && v1.x > rightBottom.x && v2.x > rightBottom.x))
                    return true;
                else
                    return false;
            }
            else {
                RPolygon temp = reSort();
                if (temp.v2.y < leftTop.y || temp.v0.y > rightBottom.y || (temp.v0.x < leftTop.x && temp.v1.x < leftTop.x && temp.v2.x < leftTop.x) || (temp.v0.x > rightBottom.x && temp.v1.x > rightBottom.x && temp.v2.x > rightBottom.x))
                    return true;
                else
                    return false;
            }
        }

        public bool isFlatTop() {
            if (GeneralVariables.FCMP(v0.y, v1.y))
                return true;
            else {
                return false;
            }
        }

        public bool isFlatBottom() {
            if (GeneralVariables.FCMP(v1.y, v2.y))
                return true;
            else {
                return false;
            }
        }

        public Vec2f flatCoeff() {
            if (sorted) {
                float new_x = v0.x + (v1.y - v0.y) * (v2.x - v0.x) / (v2.y - v0.y);
                return new Vec2f(new_x, v1.y);
            }
            else {
                RPolygon temp = reSort();
                float new_x = temp.v0.x + (temp.v1.y - temp.v0.y) * (temp.v2.x - temp.v0.x) / (temp.v2.y - temp.v0.y);
                return new Vec2f(new_x, temp.v1.y);
            }
        }

        public bool clockWise() {

            if (sorted) {
                return edgeFunction(v0, v2, v1) > 0.0f ? true : false;
            }
            else {
                RPolygon temp = reSort();
                return edgeFunction(temp.v0, temp.v2, temp.v1) > 0.0f ? true : false;
            }
        }

        public Vec3f normalCalc() {

            return (v1 - v0).cross(v2 - v0);
        }

        public Vec3f normalizedNormalCalc() {

            return normalCalc().unit();
        }

        public Vec3f centerCalc() {
            return new Vec3f(
                (v0.x + v1.x + v2.x) / 3.0f,
                (v0.y + v1.y + v2.y) / 3.0f,
                (v0.z + v1.z + v2.z) / 3.0f
            );
        }

        public float screenArea() {
            return edgeFunction(v0, v1, v2);
        }

        public float screenAreaCCW() {
            return edgeFunction(v0, v2, v1);
        }

        public static float edgeFunction(Vec2f a, Vec2f b, Vec2f c) {
            //return (c[0] - a[0]) * (b[1] - a[1]) - (c[1] - a[1]) * (b[0] - a[0]);
            return (c.x - a.x) * (b.y - a.y) - (c.y - a.y) * (b.x - a.x);
            //return (c - a).crossLenght(b - a);
        }

        public static float edgeFunction(Vec3f a, Vec3f b, Vec3f c) {
            //return edgeFunction(new Vec2f(a), new Vec2f(b), new Vec2f(c));
            return (c.x - a.x) * (b.y - a.y) - (c.y - a.y) * (b.x - a.x);
        }

        public Vec3f lambdas(Vec2f p) {

            //float w0 = edgeFunction(v1, v2, new Vec3f(p));
            float w0 = (p.x - v1.x) * (v2.y - v1.y) - (p.y - v1.y) * (v2.x - v1.x);

            //float w1 = edgeFunction(v2, v0, new Vec3f(p));
            float w1 = (p.x - v2.x) * (v0.y - v2.y) - (p.y - v2.y) * (v0.x - v2.x);

            //float w2 = edgeFunction(v0, v1, new Vec3f(p));
            float w2 = (p.x - v0.x) * (v1.y - v0.y) - (p.y - v0.y) * (v1.x - v0.x);

            return new Vec3f(w0, w1, w2);
        }

        public Vec3f nlambdas(Vec2f p) {

            float area = this.screenArea();
            float oneOverArea = 1.0f / area;

            //float w0 = edgeFunction(v1, v2, new Vec3f(p));
            float w0 = (p.x - v1.x) * (v2.y - v1.y) - (p.y - v1.y) * (v2.x - v1.x);

            //float w1 = edgeFunction(v2, v0, new Vec3f(p));
            float w1 = (p.x - v2.x) * (v0.y - v2.y) - (p.y - v2.y) * (v0.x - v2.x);

            //float w2 = edgeFunction(v0, v1, new Vec3f(p));
            float w2 = (p.x - v0.x) * (v1.y - v0.y) - (p.y - v0.y) * (v1.x - v0.x);

            return new Vec3f(w0 * oneOverArea, w1 * oneOverArea, w2 * oneOverArea);
        }

        public bool test(Vec2f p) {

            Vec3f w = lambdas(p);
            float w0 = w.x;
            float w1 = w.y;
            float w2 = w.z;
            if (w0 >= 0 && w1 >= 0 && w2 >= 0) {
                return true;
            }
            else {
                return false;
            }
        }

        public static bool test(float w0, float w1, float w2) {

            if (w0 >= 0 && w1 >= 0 && w2 >= 0) {
                return true;
            }
            else {
                return false;
            }
        }

        public static bool test(Vec3f w) {

            if (w.x >= 0 && w.y >= 0 && w.z >= 0) {
                return true;
            }
            else {
                return false;
            }
        }

        public float minX() {
            if (x0 <= x1 && x0 < x2) return x0;
            if (x1 <= x0 && x1 < x2) return x1;
            return x2;
        }

        public float maxX() {
            if (x0 >= x1 && x0 > x2) return x0;
            if (x1 >= x0 && x1 > x2) return x1;
            return x2;
        }

        public float minY() {
            if (y0 <= y1 && y0 < y2) return y0;
            if (y1 <= y0 && y1 < y2) return y1;
            return y2;
        }

        public float maxY() {
            if (y0 >= y1 && y0 > y2) return y0;
            if (y1 >= y0 && y1 > y2) return y1;
            return y2;
        }

        public Vec2i topLeft() {
            return new Vec2i((int)minX(), (int)minY());
        }

        public Vec2i bottomRight() {
            return new Vec2i((int)maxX(), (int)maxY());
        }

        public Vec3f getOriginal(int key) {
            int i = order[key];
            Vec3f result;
            switch (i) {
                case 0: {
                        result = v0;
                    }
                    break;
                case 1: {
                        result = v1;
                    }
                    break;
                case 2: {
                        result = v2;
                    }
                    break;
                default: {
                        result = v0;
                    }
                    break;
            }
            return result;
        }

        public void setOriginal(int key, Vec3f value) {
            int i = order[key];
            switch (i) {
                case 0: {
                        v0 = value;
                    }
                    break;
                case 1: {
                        v1 = value;
                    }
                    break;
                case 2: {
                        v2 = value;
                    }
                    break;
                default: {
                        v0 = value;
                    }
                    break;
            }
        }

        public Vec3f this[int key] {
            get {
                Vec3f result;
                switch (key) {
                    case 0: {
                            result = v0;
                        }
                        break;
                    case 1: {
                            result = v1;
                        }
                        break;
                    case 2: {
                            result = v2;
                        }
                        break;
                    default: {
                            result = v0;
                        }
                        break;
                }
                return result;
            }
            set {
                switch (key) {
                    case 0: {
                            v0 = value;
                        }
                        break;
                    case 1: {
                            v1 = value;
                        }
                        break;
                    case 2: {
                            v2 = value;
                        }
                        break;
                    default: {
                            v0 = value;
                        }
                        break;
                }
            }
        }

        private static int lastId = 0;
    }
}
