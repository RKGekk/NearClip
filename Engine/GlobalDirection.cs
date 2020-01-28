using MathematicalEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicRender.Engine {

    public static class GlobalDirection {

        public static Vec3f forward3f = new Vec3f(0.0f, 0.0f, 1.0f);
        public static Vec3f up3f = new Vec3f(0.0f, 1.0f, 0.0f);
        public static Vec3f left3f = new Vec3f(1.0f, 0.0f, 0.0f);
    }
}
