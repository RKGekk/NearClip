using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalEntities {

    public class Line3f {

        public Vec3f t0 { get; set; }
        public Vec3f t1 { get; set; }

        public float x0 { get => t0.x; set => t0.x = value; }
        public float y0 { get => t0.y; set => t0.y = value; }
        public float z0 { get => t0.z; set => t0.z = value; }
        public float x1 { get => t1.x; set => t1.x = value; }
        public float y1 { get => t1.y; set => t1.y = value; }
        public float z1 { get => t1.z; set => t1.z = value; }

        public Line3f(float x0, float y0, float z0, float x1, float y1, float z1) {

            this.t0 = new Vec3f();
            this.t0.x = x0;
            this.t0.y = y0;
            this.t0.z = z0;

            this.t1 = new Vec3f();
            this.t1.x = x1;
            this.t1.y = y1;
            this.t1.z = z1;
        }

        public Line3f(Vec4f t0, Vec4f t1) {

            this.t0 = new Vec3f(t0);
            this.t1 = new Vec3f(t1);
        }

        public Line3f(Vec3f t0, Vec3f t1) {

            this.t0 = t0;
            this.t1 = t1;
        }

        public Line3f(Vec2f t0, Vec2f t1) {

            this.t0 = new Vec3f(t0);
            this.t1 = new Vec3f(t1);
        }

        public Vec3f direction() {
            return t1 - t0;
        }

        public Vec3f normalizedDirection() {
            return direction().normal();
        }

        public float segmentD(Triangle3f triangle) {

            float result = float.NaN;

            Vec3f planeNorm = triangle.normal();
            Vec3f lineNormDirection = direction().normal();

            float denom = lineNormDirection.dot(planeNorm);
            if (!GeneralVariables.FCMP(denom, 0.0f)) {

                float oneOverDenom = 1.0f / denom;
                result = (triangle.v0 - t0).dot(planeNorm) * oneOverDenom;
            }

            return result;
        }

        public Vec3f intersect(Triangle3f triangle) {

            Vec3f result = null;
            
            Mat3f eqi = new Mat3f(
                -1.0f * (t1 - t0),
                triangle.v1 - triangle.v0,
                triangle.v2 - triangle.v0
            ).inverse();

            result = eqi.transpose() * (t0 - triangle.v0);

            return result;
        }
    }
}
