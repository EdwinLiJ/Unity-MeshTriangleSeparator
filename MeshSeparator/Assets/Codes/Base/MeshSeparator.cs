using UnityEngine;

namespace Codes.Base {
    public static class MeshSeparator {
        public static Mesh GenerateSeparateTriangleMesh(this Mesh mesh) {
            var result = new Mesh();
            result.Clear();

            var sourceTriangleNum = mesh.triangles.Length;
            var triangles = new int[sourceTriangleNum];
            var vertices = new Vector3[sourceTriangleNum * 3];
            var uvs = new Vector2[sourceTriangleNum * 3];
            var colors = new Color[sourceTriangleNum * 3];
            
            var stepCount = mesh.triangles.Length / 3f;
            var stepIndex = 0;
            for (int i = 0; i < sourceTriangleNum; i += 3) {
                var index = mesh.triangles[i];
                var index1 = mesh.triangles[i + 1];
                var index2 = mesh.triangles[i + 2];
                
                var uv0 = mesh.uv[index];
                var uv1 = mesh.uv[index1];
                var uv2 = mesh.uv[index2];

                uvs[i] = uv0;
                uvs[i + 1] = uv1;
                uvs[i + 2] = uv2;
                
                var vertex0 = mesh.vertices[index];
                var vertex1 = mesh.vertices[index1];
                var vertex2 = mesh.vertices[index2];

                vertices[i] = vertex0;
                vertices[i + 1] = vertex1;
                vertices[i + 2] = vertex2;

                // var uvPoint01 = (uv0 + uv1) * 0.5f;
                // var uvPoint02 = (uv0 + uv2) * 0.5f;
                // var center = (uvPoint01 + uvPoint02) * 0.5f;

                // You dont have to tint vertex color
                var aValue = stepIndex / (stepCount + 1);
                var color = new Color(aValue, 0, 0, 1);
                colors[i] = color;
                colors[i + 1] = color;
                colors[i + 2] = color;
                stepIndex++;

                triangles[i] = i;
                triangles[i + 1] = i + 1;
                triangles[i + 2] = i + 2;
            }

            result.vertices = vertices;
            result.uv = uvs;
            result.triangles = triangles;
            result.colors = colors;

            result.RecalculateBounds();
            result.RecalculateNormals();

            return result;
        }
    }
}