using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalEntities {

    public class CohenSutherlandClip {

        public CohenSutherlandClip(Vec2f screenLeftTop, Vec2f screenRightBottom) {
            this.screenLeftTop = screenLeftTop;
            this.screenRightBottom = screenRightBottom;

            this.xmin = screenLeftTop.x;
            this.xmax = screenRightBottom.x;
            this.ymin = screenLeftTop.y;
            this.ymax = screenRightBottom.y;
        }

        public Vec2f screenLeftTop;
        public Vec2f screenRightBottom;

        public float xmin;
        public float xmax;
        public float ymin;
        public float ymax;

        public const int INSIDE = 0; // 0000
        public const int LEFT   = 1; // 0001
        public const int RIGHT  = 2; // 0010
        public const int BOTTOM = 4; // 0100
        public const int TOP    = 8; // 1000

        public int ComputeOutCode(float x, float y) {

            int code;

            code = INSIDE;          // initialised as being inside of [[clip window]]

            if (x < xmin)           // to the left of clip window
                code |= LEFT;
            else if (x > xmax)      // to the right of clip window
                code |= RIGHT;
            if (y < ymin)           // below the clip window
                code |= BOTTOM;
            else if (y > ymax)      // above the clip window
                code |= TOP;

            return code;
        }

        public Line2f Clip(Line2f line) {

            Line2f result = null;

            float x0 = line.x0;
            float y0 = line.y0;
            float x1 = line.x1;
            float y1 = line.y1;

            // compute outcodes for P0, P1, and whatever point lies outside the clip rectangle
            int outcode0 = ComputeOutCode(x0, y0);
            int outcode1 = ComputeOutCode(x1, y1);
            bool accept = false;

            while (true) {

                if ((outcode0 | outcode1) == 0) {
                    // bitwise OR is 0: both points inside window; trivially accept and exit loop
                    accept = true;
                    break;
                }
                else if ((outcode0 & outcode1) != 0) {
                    // bitwise AND is not 0: both points share an outside zone (LEFT, RIGHT, TOP,
                    // or BOTTOM), so both must be outside window; exit loop (accept is false)
                    break;
                }
                else {
                    // failed both tests, so calculate the line segment to clip
                    // from an outside point to an intersection with clip edge
                    float x = 0.0f;
                    float y = 0.0f;

                    // At least one endpoint is outside the clip rectangle; pick it.
                    int outcodeOut = outcode0 != 0 ? outcode0 : outcode1;

                    // Now find the intersection point;
                    // use formulas:
                    //   slope = (y1 - y0) / (x1 - x0)
                    //   x = x0 + (1 / slope) * (ym - y0), where ym is ymin or ymax
                    //   y = y0 + slope * (xm - x0), where xm is xmin or xmax
                    // No need to worry about divide-by-zero because, in each case, the
                    // outcode bit being tested guarantees the denominator is non-zero
                    if ((outcodeOut & TOP) != 0) {         // point is above the clip window
                        x = x0 + (x1 - x0) * (ymax - y0) / (y1 - y0);
                        y = ymax;
                    }
                    else if ((outcodeOut & BOTTOM) != 0) { // point is below the clip window
                        x = x0 + (x1 - x0) * (ymin - y0) / (y1 - y0);
                        y = ymin;
                    }
                    else if ((outcodeOut & RIGHT) != 0) {  // point is to the right of clip window
                        y = y0 + (y1 - y0) * (xmax - x0) / (x1 - x0);
                        x = xmax;
                    }
                    else if ((outcodeOut & LEFT) != 0) {   // point is to the left of clip window
                        y = y0 + (y1 - y0) * (xmin - x0) / (x1 - x0);
                        x = xmin;
                    }

                    // Now we move outside point to intersection point to clip
                    // and get ready for next pass.
                    if (outcodeOut == outcode0) {
                        x0 = x;
                        y0 = y;
                        outcode0 = ComputeOutCode(x0, y0);
                    }
                    else {
                        x1 = x;
                        y1 = y;
                        outcode1 = ComputeOutCode(x1, y1);
                    }
                }
            }
            if (accept) {
                // Following functions are left for implementation by user based on
                // their platform (OpenGL/graphics.h etc.)
                result = new Line2f(x0, y0, x1, y1);
            }

            return result;
        }
    }
}