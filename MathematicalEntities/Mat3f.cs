using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalEntities {

    public class Mat3f {

        public Vec3f[] data;

        public Mat3f() {
            data = new Vec3f[3];

            data[0] = new Vec3f(1.0f, 0.0f, 0.0f);
            data[1] = new Vec3f(0.0f, 1.0f, 0.0f);
            data[2] = new Vec3f(0.0f, 0.0f, 1.0f);
        }

        public Mat3f(float s) {
            data = new Vec3f[3];

            data[0] = new Vec3f(s);
            data[1] = new Vec3f(s);
            data[2] = new Vec3f(s);
        }

        public Mat3f(Vec3f row1, Vec3f row2, Vec3f row3) {
            data = new Vec3f[3];

            data[0] = row1;
            data[1] = row2;
            data[2] = row3;
        }

        public Mat3f transpose() {

            Mat3f transpMat = new Mat3f();

            transpMat.data[0][0] = data[0][0];
            transpMat.data[0][1] = data[1][0];
            transpMat.data[0][2] = data[2][0];

            transpMat.data[1][0] = data[0][1];
            transpMat.data[1][1] = data[1][1];
            transpMat.data[1][2] = data[2][1];

            transpMat.data[2][0] = data[0][2];
            transpMat.data[2][1] = data[1][2];
            transpMat.data[2][2] = data[2][2];

            return transpMat;
        }

        public float determinant() {

            return
                + data[0][0] * (data[1][1] * data[2][2] - data[1][2] * data[2][1])
                - data[0][1] * (data[1][0] * data[2][2] - data[1][2] * data[2][0])
                + data[0][2] * (data[1][0] * data[2][1] - data[1][1] * data[2][0]);
        }

        public Mat3f inverse() {

            float Determinant = determinant();

            Mat3f Inverse = new Mat3f();
            Inverse.data[0][0] = +(data[1][1] * data[2][2] - data[2][1] * data[1][2]);
            Inverse.data[0][1] = -(data[1][0] * data[2][2] - data[2][0] * data[1][2]);
            Inverse.data[0][2] = +(data[1][0] * data[2][1] - data[2][0] * data[1][1]);
            Inverse.data[1][0] = -(data[0][1] * data[2][2] - data[2][1] * data[0][2]);
            Inverse.data[1][1] = +(data[0][0] * data[2][2] - data[2][0] * data[0][2]);
            Inverse.data[1][2] = -(data[0][0] * data[2][1] - data[2][0] * data[0][1]);
            Inverse.data[2][0] = +(data[0][1] * data[1][2] - data[1][1] * data[0][2]);
            Inverse.data[2][1] = -(data[0][0] * data[1][2] - data[1][0] * data[0][2]);
            Inverse.data[2][2] = +(data[0][0] * data[1][1] - data[1][0] * data[0][1]);
            Inverse *= 1.0f / Determinant;

            return Inverse;
        }

        public static Vec3f operator *(Mat3f m1, Vec3f v1) {

            Vec3f outRes = new Vec3f();

            outRes.x = v1.x * m1.data[0].x + v1.y * m1.data[1].x + v1.z * m1.data[2].x;
            outRes.y = v1.x * m1.data[0].y + v1.y * m1.data[1].y + v1.z * m1.data[2].y;
            outRes.z = v1.x * m1.data[0].z + v1.y * m1.data[1].z + v1.z * m1.data[2].z;

            return outRes;
        }

        public static Mat3f operator *(Mat3f m1, float s1) {
            return new Mat3f(
                new Vec3f(m1.data[0] * s1),
                new Vec3f(m1.data[1] * s1),
                new Vec3f(m1.data[2] * s1)
            );
        }

        public static Mat3f operator +(Mat3f m1, Mat3f m2) {

            Mat3f res = new Mat3f();
            res.data[0] = m1.data[0] + m2.data[0];
            res.data[1] = m1.data[1] + m2.data[1];
            res.data[2] = m1.data[2] + m2.data[2];

            return res;
        }

        public float this[int i] {
            get {
                if (i == 0)
                    return this.data[0][0];
                if (i == 1)
                    return this.data[0][1];
                if (i == 2)
                    return this.data[0][2];
                if (i == 3)
                    return this.data[1][0];
                if (i == 4)
                    return this.data[1][1];
                if (i == 5)
                    return this.data[1][2];
                if (i == 6)
                    return this.data[2][0];
                if (i == 7)
                    return this.data[2][1];
                return this.data[2][2];
            }
            set {
                if (i == 0)
                    this.data[0][0] = value;
                else if (i == 1)
                    this.data[0][1] = value;
                else if (i == 2)
                    this.data[0][2] = value;
                else if (i == 3)
                    this.data[1][0] = value;
                else if (i == 4)
                    this.data[1][1] = value;
                else if (i == 5)
                    this.data[1][2] = value;
                else if (i == 6)
                    this.data[2][0] = value;
                else if (i == 7)
                    this.data[2][1] = value;
                else 
                    this.data[2][2] = value;
            }
        }
    }
}
