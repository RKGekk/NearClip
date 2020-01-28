using MathematicalEntities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicRender.Engine {

    public class RTexture {

        public int id;
        public string name;

        public byte[] texture;
        public int textureStride;

        public int textureWidth;
        public float textureWidthf;
        
        public int textureHeight;
        public float textureHeightf;

        public RTexture(string bmpFileName, string name) {

            this.name = name;
            this.id = lastId++;

            Bitmap bmp = new Bitmap(bmpFileName);
            this.textureWidth = bmp.Width;
            this.textureWidthf = this.textureWidth;

            this.textureStride = (textureWidth * 32) / 8;
            this.textureHeight = bmp.Height;
            this.textureHeightf = this.textureHeight;

            this.texture = new byte[this.textureWidth * this.textureHeight * 4];

            for (int x = 0; x < bmp.Width; x++) {
                for (int y = 0; y < bmp.Height; y++) {

                    Color pxl = bmp.GetPixel(x, y);

                    byte red = pxl.R;
                    byte green = pxl.G;
                    byte blue = pxl.B;
                    byte alpha = pxl.A;

                    int pixelOffset = (x + y * textureWidth) * 32 / 8;
                    this.texture[pixelOffset] = blue;
                    this.texture[pixelOffset + 1] = green;
                    this.texture[pixelOffset + 2] = red;
                    this.texture[pixelOffset + 3] = alpha;
                }
            }
        }

        public Vec3f sampleTexture(Vec2f UV) {

            if (UV.u * this.textureWidthf > this.textureWidthf)
                UV.u = 1.0f;

            if (UV.u < 0.0f)
                UV.u = 0.0f;

            if (UV.v * this.textureHeightf > this.textureHeightf)
                UV.v = 1.0f;

            if (UV.v < 0.0f)
                UV.v = 0.0f;

            int pixelOffset = ((int)((UV.u) * textureWidthf) + (int)((1.0f - UV.v) * textureHeightf) * textureWidth) * 32 / 8;

            byte red = texture[pixelOffset + 2];
            byte green = texture[pixelOffset + 1];
            byte blue = texture[pixelOffset];

            return new Vec3f((float)red / 255.0f, (float)green / 255.0f, (float)blue / 255.0f);
        }

        public Vec4f sampleTextureA(Vec2f UV) {

            if (UV.u > 1.0f)
                UV.u = 1.0f;

            if (UV.u < 0.0f)
                UV.u = 0.0f;

            if (UV.v > 1.0f)
                UV.v = 1.0f;

            if (UV.v < 0.0f)
                UV.v = 0.0f;

            int x = (int)((UV.u) * (textureWidthf - 1.0f));
            int y = (int)((1.0f - UV.v) * (textureHeightf - 1.0f));

            int pixelOffset = (x + y * textureWidth) * 32 / 8;

            byte alpha = texture[pixelOffset + 3];
            byte red = texture[pixelOffset + 2];
            byte green = texture[pixelOffset + 1];
            byte blue = texture[pixelOffset];

            return new Vec4f((float)red / 255.0f, (float)green / 255.0f, (float)blue / 255.0f, (float)alpha / 255.0f);
        }

        private static int lastId = 0;
    }
}
