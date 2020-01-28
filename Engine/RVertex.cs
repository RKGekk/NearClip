using MathematicalEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicRender.Engine {

    public class RVertex {

        public Vec3f position;
        public Vec4f color;
        public Vec3f normal;
        public Vec3f tangentU;
        public Vec2f textureCoordinates;

        public float x { get => position.x; set => position.x = value; }
        public float y { get => position.y; set => position.y = value; }
        public float z { get => position.z; set => position.z = value; }

        public float r { get => color.x; set => color.x = value; }
        public float g { get => color.y; set => color.y = value; }
        public float b { get => color.z; set => color.z = value; }

        public float nx { get => normal.x; set => normal.x = value; }
        public float ny { get => normal.y; set => normal.y = value; }
        public float nz { get => normal.z; set => normal.z = value; }

        public float tx { get => tangentU.x; set => tangentU.x = value; }
        public float ty { get => tangentU.y; set => tangentU.y = value; }
        public float tz { get => tangentU.z; set => tangentU.z = value; }

        public float u { get => textureCoordinates.x; set => textureCoordinates.x = value; }
        public float v { get => textureCoordinates.y; set => textureCoordinates.y = value; }

        public RVertex() { }

        public RVertex(float x, float y, float z, float r, float g, float b, float nx, float ny, float nz, float u, float v) {

            this.position = new Vec3f(x, y, z);
            this.color = new Vec4f(r, g, b, 1.0f);
            this.normal = new Vec3f(nx, ny, nz);
            this.tangentU = new Vec3f();
            this.textureCoordinates = new Vec2f(u, v);
        }

        public RVertex(Vec3f pos, Vec3f color, Vec3f norm, Vec2f texPos) {

            this.position = new Vec3f(pos);
            this.color = new Vec4f(color);
            this.normal = new Vec3f(norm);
            this.tangentU = new Vec3f();
            this.textureCoordinates = new Vec2f(texPos);
        }

        public RVertex(Vec3f pos, Vec3f color, Vec2f texPos) {

            this.position = new Vec3f(pos);
            this.color = new Vec4f(color);
            this.normal = new Vec3f();
            this.tangentU = new Vec3f();
            this.textureCoordinates = new Vec2f(texPos);
        }

        public RVertex(RVertex other) {

            this.position = new Vec3f(other.position);
            this.color = new Vec4f(other.color);
            this.normal = new Vec3f(other.normal);
            this.tangentU = new Vec3f(other.tangentU);
            this.textureCoordinates = new Vec2f(other.textureCoordinates);
        }
    }
}
