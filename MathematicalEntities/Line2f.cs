using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalEntities {

    public class Line2f {

        public Vec2f t0 { get; set; }
        public Vec2f t1 { get; set; }

        public float x0 { get => t0.x; set => t0.x = value; }
        public float y0 { get => t0.y; set => t0.y = value; }
        public float x1 { get => t1.x; set => t1.x = value; }
        public float y1 { get => t1.y; set => t1.y = value; }

        public Line2f(float x0, float y0, float x1, float y1) {

            this.t0 = new Vec2f();
            this.t0.x = x0;
            this.t0.y = y0;

            this.t1 = new Vec2f();
            this.t1.x = x1;
            this.t1.y = y1;
        }

        public Line2f(Vec4f t0, Vec4f t1) {

            this.t0 = new Vec2f(t0);
            this.t1 = new Vec2f(t1);
        }

        public Line2f(Vec3f t0, Vec3f t1) {

            this.t0 = new Vec2f(t0);
            this.t1 = new Vec2f(t1);
        }

        public Line2f(Vec2f t0, Vec2f t1) {

            this.t0 = t0;
            this.t1 = t1;
        }

        public Vec2f intersect(Line2f other) {

            float x1 = t0.x;
            float y1 = t0.y;

            float x2 = t1.x;
            float y2 = t1.y;

            float x3 = other.x0;
            float y3 = other.y0;

            float x4 = other.x1;
            float y4 = other.y1;

            Vec2f result = null;

            float denom = (x1 - x2) * (y3 - y4) - (y1 - y2)*(x3 - x4);
            if (!GeneralVariables.FCMP(denom, 0.0f)) {

                float oneOverDenom = 1.0f / denom;
                float numX = (x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4);
                float numY = (x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4);
                result = new Vec2f(numX * oneOverDenom, numY * oneOverDenom);
            }

            return result;
        }

        public float segmentT(Line2f other) {

            float x1 = t0.x;
            float y1 = t0.y;

            float x2 = t1.x;
            float y2 = t1.y;

            float x3 = other.x0;
            float y3 = other.y0;

            float x4 = other.x1;
            float y4 = other.y1;

            float result = float.NaN;

            float denom = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (!GeneralVariables.FCMP(denom, 0.0f)) {

                float oneOverDenom = 1.0f / denom;
                float num = (x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4);
                result = num * oneOverDenom;
            }

            return result;
        }

        public float segmentU(Line2f other) {

            float x1 = t0.x;
            float y1 = t0.y;

            float x2 = t1.x;
            float y2 = t1.y;

            float x3 = other.x0;
            float y3 = other.y0;

            float x4 = other.x1;
            float y4 = other.y1;

            float result = float.NaN;

            float denom = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
            if (!GeneralVariables.FCMP(denom, 0.0f)) {

                float oneOverDenom = 1.0f / denom;
                float num = -1.0f * ((x1 - x2) * (y1 - y3) - (y1 - y2) * (x1 - x3));
                result = num * oneOverDenom;
            }

            return result;
        }

        public float denom(Line2f other) {

            float x1 = t0.x;
            float y1 = t0.y;

            float x2 = t1.x;
            float y2 = t1.y;

            float x3 = other.x0;
            float y3 = other.y0;

            float x4 = other.x1;
            float y4 = other.y1;

            return (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
        }
    }
}
