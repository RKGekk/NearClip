using MathematicalEntities;
using ObjReader.Data.Elements;
using ObjReader.Loaders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicRender.Engine {

    public class RenderObjectLoader {

        public LoadResult loadResult;

        public RenderObjectLoader(LoadResult loadResult) {
            this.loadResult = loadResult;
        }

        public RenderObjectLoader(string objFileName) {

            this.loadResult = loadObject(objFileName);
        }

        public static LoadResult loadObject(string fileName) {

            return ObjLoaderFactory.Create().Load(File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None));
        }

        public RObject GetRObject() {
            RObject result = new RObject();

            result.id = lastId++;

            RTexture oneTexture = new RTexture(
                loadResult.Materials[0].DiffuseTextureMap,
                loadResult.Materials[0].Name
            );

            result.texture = oneTexture;

            foreach (Group group in loadResult.Groups) {
                foreach (Face face in group.Faces) {

                    if (face.Count == 3) {

                        Vec3f point1Original = new Vec3f(
                            loadResult.Vertices[face[0].VertexIndex - 1].X,
                            loadResult.Vertices[face[0].VertexIndex - 1].Y,
                            loadResult.Vertices[face[0].VertexIndex - 1].Z
                        );
                                                
                        Vec3f color1 = new Vec3f(
                            loadResult.Vertices[face[0].VertexIndex - 1].R,
                            loadResult.Vertices[face[0].VertexIndex - 1].G,
                            loadResult.Vertices[face[0].VertexIndex - 1].B
                        );

                        RPolygon.RasterType rasterType1 = face[0].TextureIndex != 0 ? RPolygon.RasterType.Textured : RPolygon.RasterType.InterpolatedColor;

                        Vec2f texColor1 = new Vec2f(
                            face[0].TextureIndex != 0 ? loadResult.Textures[face[0].TextureIndex - 1].X : 0.0f,
                            face[0].TextureIndex != 0 ? loadResult.Textures[face[0].TextureIndex - 1].Y : 0.0f
                        );


                        Vec3f point2Original = new Vec3f(
                            loadResult.Vertices[face[1].VertexIndex - 1].X,
                            loadResult.Vertices[face[1].VertexIndex - 1].Y,
                            loadResult.Vertices[face[1].VertexIndex - 1].Z
                        );
                        
                        Vec3f color2 = new Vec3f(
                            loadResult.Vertices[face[1].VertexIndex - 1].R,
                            loadResult.Vertices[face[1].VertexIndex - 1].G,
                            loadResult.Vertices[face[1].VertexIndex - 1].B
                        );

                        RPolygon.RasterType rasterType2 = face[1].TextureIndex != 0 ? RPolygon.RasterType.Textured : RPolygon.RasterType.InterpolatedColor;

                        Vec2f texColor2 = new Vec2f(
                            face[1].TextureIndex != 0 ? loadResult.Textures[face[1].TextureIndex - 1].X : 0.0f,
                            face[1].TextureIndex != 0 ? loadResult.Textures[face[1].TextureIndex - 1].Y : 0.0f
                        );


                        Vec3f point3Original = new Vec3f(
                            loadResult.Vertices[face[2].VertexIndex - 1].X,
                            loadResult.Vertices[face[2].VertexIndex - 1].Y,
                            loadResult.Vertices[face[2].VertexIndex - 1].Z
                        );
                        
                        Vec3f color3 = new Vec3f(
                            loadResult.Vertices[face[2].VertexIndex - 1].R,
                            loadResult.Vertices[face[2].VertexIndex - 1].G,
                            loadResult.Vertices[face[2].VertexIndex - 1].B
                        );

                        RPolygon.RasterType rasterType3 = face[2].TextureIndex != 0 ? RPolygon.RasterType.Textured : RPolygon.RasterType.InterpolatedColor;

                        Vec2f texColor3 = new Vec2f(
                            face[2].TextureIndex != 0 ? loadResult.Textures[face[2].TextureIndex - 1].X : 0.0f,
                            face[2].TextureIndex != 0 ? loadResult.Textures[face[2].TextureIndex - 1].Y : 0.0f
                        );

                        result.polygons.Add(
                            new RPolygon(
                                new RVertex(
                                    point1Original,
                                    color1,
                                    texColor1
                                ),
                                new RVertex(
                                    point2Original,
                                    color2,
                                    texColor2
                                ),
                                new RVertex(
                                    point3Original,
                                    color3,
                                    texColor3
                                ),
                                rasterType1,
                                oneTexture
                            )
                        );
                        result.name = group.Name;
                    }
                }
            }

            return result;
        }

        private static int lastId = 0;
    }
}
