using MathematicalEntities;
using ObjReader.Data.Elements;
using ObjReader.Loaders;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicRender.Engine {

    public class Renderer {

        public byte[] buf;
        public float[] zbuf;
        public int pixelWidth;
        public float pixelWidthf;
        public int stride;
        public int pixelHeight;
        public float pixelHeightf;
        public Vec2f pixelLeftTop;
        public Vec2f pixelRightBottom;
        public CohenSutherlandClip clip;

        public RObject renderObject;
        public GameTimer timer;
        public Camera camera;

        public Renderer(GameTimer timer, byte[] buf, float[] zbuf, int pixelWidth) {

            this.buf = buf;
            this.zbuf = zbuf;
            this.pixelWidth = pixelWidth;
            this.pixelWidthf = this.pixelWidth;

            this.stride = (pixelWidth * 32) / 8;
            this.pixelHeight = buf.Length / stride;
            this.pixelHeightf = this.pixelHeight;

            this.pixelLeftTop = new Vec2f(0.0f, 0.0f);
            this.pixelRightBottom = new Vec2f(this.pixelWidthf - 1.0f, this.pixelHeightf - 1.0f);

            this.clip = new CohenSutherlandClip(this.pixelLeftTop, this.pixelRightBottom);

            this.camera = new Camera(new Vec3f(0.0f, 0.0f, 0.0f), new Vec3f(0.0f, 1.0f, 0.0f), -90.0f, 0.0f);

            this.timer = timer;
        }

        public void setObject(string fileName) {

            RenderObjectLoader objectLoader = new RenderObjectLoader(fileName);
            this.renderObject = objectLoader.GetRObject();
        }

        public void printPixel(int x, int y, Vec3f color) {

            byte red = (byte)(color.x * 255.0f);
            byte green = (byte)(color.y * 255.0f);
            byte blue = (byte)(color.z * 255.0f);
            byte alpha = 255;

            int pixelOffset = (x + y * pixelWidth) * 32 / 8;
            buf[pixelOffset] = blue;
            buf[pixelOffset + 1] = green;
            buf[pixelOffset + 2] = red;
            buf[pixelOffset + 3] = alpha;
        }

        public void printPixelZ(int x, int y, float z, Vec3f color) {

            byte red = (byte)(color.x * 255.0f);
            byte green = (byte)(color.y * 255.0f);
            byte blue = (byte)(color.z * 255.0f);
            byte alpha = 255;

            int offset = (x + y * pixelWidth);
            float oneOverZ = 1.0f / z;

            if (zbuf[offset] > oneOverZ) {

                zbuf[offset] = oneOverZ;

                int pixelOffset = (x + y * pixelWidth) * 32 / 8;
                buf[pixelOffset] = blue;
                buf[pixelOffset + 1] = green;
                buf[pixelOffset + 2] = red;
                buf[pixelOffset + 3] = alpha;
            }
        }

        public void fillScreen(Vec3f color) {

            for (int y = 0; y < pixelHeight; y++)
                for (int x = 0; x < pixelWidth; x++)
                    printPixel(x, y, color);
        }

        public void fillZBuff(float z) {

            for (int y = 0; y < pixelHeight; y++) {
                for (int x = 0; x < pixelWidth; x++) {
                    int offset = (x + y * pixelWidth);
                    zbuf[offset] = z;
                }
            }
        }

        public void lmoveScreen(Vec3f fillColor, int moveAmt) {

            for (int y = 0; y < pixelHeight; y++) {
                for (int x = 0; x < pixelWidth; x++) {

                    int nextPixel = x + moveAmt;
                    if (nextPixel < pixelWidth) {
                        int pixelOffset = (x + y * pixelWidth) * 32 / 8;
                        int pixelOffsetNew = (nextPixel + y * pixelWidth) * 32 / 8;
                        buf[pixelOffset] = buf[pixelOffsetNew];
                        buf[pixelOffset + 1] = buf[pixelOffsetNew + 1];
                        buf[pixelOffset + 2] = buf[pixelOffsetNew + 2];
                        buf[pixelOffset + 3] = buf[pixelOffsetNew + 3];
                    }
                    else {
                        printPixel(x, y, fillColor);
                    }
                }
            }
        }

        public void printLineCut(Line2f lineCoords, Vec3f color) {

            int stride = (pixelWidth * 32) / 8;
            int pixelHeight = buf.Length / stride;

            Line2f line = clip.Clip(lineCoords);
            if (line == null)
                return;

            int x0 = (int)line.x0;
            int y0 = (int)line.y0;
            int x1 = (int)line.x1;
            int y1 = (int)line.y1;

            int dx = Math.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;

            int dy = Math.Abs(y1 - y0);
            int sy = y0 < y1 ? 1 : -1;

            int err = (dx > dy ? dx : -dy) / 2;
            int e2;

            for (; ; ) {

                printPixel(x0, y0, color);

                if (x0 == x1 && y0 == y1)
                    break;

                e2 = err;

                if (e2 > -dx) {
                    err -= dy;
                    x0 += sx;
                }

                if (e2 < dy) {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        public void printTriangleWireframe(Triangle3f triangle, Vec3f color) {

            printLineCut(new Line2f(triangle.v0, triangle.v1), color);
            printLineCut(new Line2f(triangle.v1, triangle.v2), color);
            printLineCut(new Line2f(triangle.v2, triangle.v0), color);
        }

        public void printTriangleWireframe(RPolygon polygon, Vec3f color) {

            printLineCut(new Line2f(polygon.v0, polygon.v1), color);
            printLineCut(new Line2f(polygon.v1, polygon.v2), color);
            printLineCut(new Line2f(polygon.v2, polygon.v0), color);
        }

        public void printPolyZ(RPolygon pIn) {

            RPolygon polygon = pIn.reSort();
            float area = polygon.screenArea();
            float oneOverArea = 1.0f / area;

            Vec3f v0 = polygon.v0;
            Vec3f v1 = polygon.v1;
            Vec3f v2 = polygon.v2;

            if (polygon.isEpsilonGeometry())
                return;

            if (polygon.isOffScreen(this.pixelLeftTop, this.pixelRightBottom))
                return;

            if (area < 1.0f && area > -1.0f)
                return;

            Vec2i leftTop = polygon.topLeft();
            Vec2i bottomRight = polygon.bottomRight();

            if (leftTop.x < 0) {
                leftTop.x = 0;
            }

            if (leftTop.y < 0) {
                leftTop.y = 0;
            }

            if (bottomRight.x >= pixelWidth) {
                bottomRight.x = pixelWidth - 1;
            }

            if (bottomRight.y >= pixelHeight) {
                bottomRight.y = pixelHeight - 1;
            }

            for (int x = leftTop.x; x < bottomRight.x; x++) {
                for (int y = leftTop.y; y < bottomRight.y; y++) {

                    Vec2f p = new Vec2f(x, y);
                    Vec3f w = polygon.lambdas(p);

                    if ((w.x >= 0.0f && w.y >= 0.0f && w.z >= 0.0f) || (w.x <= 0.0f && w.y <= 0.0f && w.z <= 0.0f)) {

                        w *= oneOverArea;

                        float z = 1.0f / (v0.z * w.x + v1.z * w.y + v2.z * w.z);
                        Vec2f UV = new Vec2f(
                            (polygon.vertex0.textureCoordinates.x * w.x + polygon.vertex1.textureCoordinates.x * w.y + polygon.vertex2.textureCoordinates.x * w.z) * z,
                            (polygon.vertex0.textureCoordinates.y * w.x + polygon.vertex1.textureCoordinates.y * w.y + polygon.vertex2.textureCoordinates.y * w.z) * z
                        );
                        Vec4f texColor = polygon.texture.sampleTextureA(UV);
                        printPixelZ(x, y, z, new Vec3f(texColor));
                    }
                }
            }
        }

        public void renderPolySolidColorZ(bool isWireframe) {

            renderObject.model = new Mat4f(
                new Vec4f(1.0f, 0.0f, 0.0f, 0.0f),
                new Vec4f(0.0f, 1.0f, 0.0f, 0.0f),
                new Vec4f(0.0f, 0.0f, 1.0f, 0.0f),
                new Vec4f(0.0f, 0.0f, 0.0f, 1.0f)
            );
            renderObject.calculateModel();

            renderObject.world = new Mat4f(
                new Vec4f(1.0f, 0.0f, 0.0f, 0.0f),
                new Vec4f(0.0f, 1.0f, 0.0f, 0.0f),
                new Vec4f(0.0f, 0.0f, 1.0f, 0.0f),
                new Vec4f(0.0f, 0.0f, 6.0f, 1.0f)
            );
            renderObject.calculateWorld();

            renderObject.view = camera.GetViewMatrix();
            renderObject.calculateView(false);

            renderObject.proj = camera.GetProjMatrix();
            renderObject.calculateProj();

            renderObject.calculateRaster(pixelWidthf, pixelHeightf, true);

            foreach (RPolygon polygon in renderObject.polygons_raster) {
                printPolyZ(polygon);
            }

            if (isWireframe) {
                foreach (RPolygon polygon in renderObject.polygons_raster) {
                    printTriangleWireframe(polygon, new Vec3f(1.0f, 0.0f, 0.0f));
                }
            }

            string ffds = "sfv";
        }
    }
}
