using AssetPrimitives;
using Assimp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Veldrid;
using aiMatrix4x4 = Assimp.Matrix4x4;

namespace AssetProcessor
{
    public class AssimpProcessor : BinaryAssetProcessor<ProcessedModel>
    {
        public unsafe override ProcessedModel ProcessT(Stream stream, string extension)
        {
            var ac = new AssimpContext();
            Scene scene = ac.ImportFileFromStream(
                stream, 
                PostProcessSteps.FlipWindingOrder | PostProcessSteps.GenerateNormals | PostProcessSteps.FlipUVs,
                extension);
            aiMatrix4x4 rootNodeInverseTransform = scene.RootNode.Transform;
            rootNodeInverseTransform.Inverse();

            var parts = new List<ProcessedMeshPart>();
            var animations = new List<ProcessedAnimation>();

            var encounteredNames = new HashSet<string>();
            for (var meshIndex = 0; meshIndex < scene.MeshCount; meshIndex++)
            {
                Mesh mesh = scene.Meshes[meshIndex];
                var meshName = mesh.Name;
                if (string.IsNullOrEmpty(meshName))
                {
                    meshName = $"mesh_{meshIndex}";
                }
                var counter = 1;
                while (!encounteredNames.Add(meshName))
                {
                    meshName = mesh.Name + "_" + counter.ToString();
                    counter += 1;
                }
                var vertexCount = mesh.VertexCount;

                var positionOffset = 0;
                var normalOffset = 12;
                var texCoordsOffset = -1;
                var boneWeightOffset = -1;
                var boneIndicesOffset = -1;

                var elementDescs = new List<VertexElementDescription>();
                elementDescs.Add(new VertexElementDescription("Position", VertexElementSemantic.Position, VertexElementFormat.Float3));
                elementDescs.Add(new VertexElementDescription("Normal", VertexElementSemantic.Normal, VertexElementFormat.Float3));
                normalOffset = 12;

                var vertexSize = 24;

                var hasTexCoords = mesh.HasTextureCoords(0);
                elementDescs.Add(new VertexElementDescription("TexCoords", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2));
                texCoordsOffset = vertexSize;
                vertexSize += 8;

                var hasBones = mesh.HasBones;
                if (hasBones)
                {
                    elementDescs.Add(new VertexElementDescription("BoneWeights", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4));
                    elementDescs.Add(new VertexElementDescription("BoneIndices", VertexElementSemantic.TextureCoordinate, VertexElementFormat.UInt4));

                    boneWeightOffset = vertexSize;
                    vertexSize += 16;

                    boneIndicesOffset = vertexSize;
                    vertexSize += 16;
                }

                var vertexData = new byte[vertexCount * vertexSize];
                var builder = new VertexDataBuilder(vertexData, vertexSize);
                var min = vertexCount > 0 ? mesh.Vertices[0].ToSystemVector3() : Vector3.Zero;
                var max = vertexCount > 0 ? mesh.Vertices[0].ToSystemVector3() : Vector3.Zero;

                for (var i = 0; i < vertexCount; i++)
                {
                    Vector3 position = mesh.Vertices[i].ToSystemVector3();
                    min = Vector3.Min(min, position);
                    max = Vector3.Max(max, position);

                    builder.WriteVertexElement(
                        i,
                        positionOffset,
                        position);

                    Vector3 normal = mesh.Normals[i].ToSystemVector3();
                    builder.WriteVertexElement(i, normalOffset, normal);

                    if (mesh.HasTextureCoords(0))
                    {
                        builder.WriteVertexElement(
                            i,
                            texCoordsOffset,
                            new Vector2(mesh.TextureCoordinateChannels[0][i].X, mesh.TextureCoordinateChannels[0][i].Y));
                    }
                    else
                    {
                        builder.WriteVertexElement(
                            i,
                            texCoordsOffset,
                            new Vector2());
                    }
                }

                var indices = new List<int>();
                foreach (Face face in mesh.Faces)
                {
                    if (face.IndexCount == 3)
                    {
                        indices.Add(face.Indices[0]);
                        indices.Add(face.Indices[1]);
                        indices.Add(face.Indices[2]);
                    }
                }

                var boneIDsByName = new Dictionary<string, uint>();
                var boneOffsets = new System.Numerics.Matrix4x4[mesh.BoneCount];

                if (hasBones)
                {
                    var assignedBoneWeights = new Dictionary<int, int>();
                    for (uint boneID = 0; boneID < mesh.BoneCount; boneID++)
                    {
                        Bone bone = mesh.Bones[(int)boneID];
                        var boneName = bone.Name;
                        var suffix = 1;
                        while (boneIDsByName.ContainsKey(boneName))
                        {
                            boneName = bone.Name + "_" + suffix.ToString();
                            suffix += 1;
                        }

                        boneIDsByName.Add(boneName, boneID);
                        foreach (VertexWeight weight in bone.VertexWeights)
                        {
                            var relativeBoneIndex = GetAndIncrementRelativeBoneIndex(assignedBoneWeights, weight.VertexID);
                            builder.WriteVertexElement(weight.VertexID, boneIndicesOffset + (relativeBoneIndex * sizeof(uint)), boneID);
                            builder.WriteVertexElement(weight.VertexID, boneWeightOffset + (relativeBoneIndex * sizeof(float)), weight.Weight);
                        }

                        System.Numerics.Matrix4x4 offsetMat = bone.OffsetMatrix.ToSystemMatrixTransposed();
                        System.Numerics.Matrix4x4.Decompose(offsetMat, out var scale, out var rot, out var trans);
                        offsetMat = System.Numerics.Matrix4x4.CreateScale(scale)
                            * System.Numerics.Matrix4x4.CreateFromQuaternion(rot)
                            * System.Numerics.Matrix4x4.CreateTranslation(trans);

                        boneOffsets[boneID] = offsetMat;
                    }
                }
                builder.FreeGCHandle();

                var indexCount = (uint)indices.Count;

                var int32Indices = indices.ToArray();
                var indexData = new byte[indices.Count * sizeof(uint)];
                fixed (byte* indexDataPtr = indexData)
                {
                    fixed (int* int32Ptr = int32Indices)
                    {
                        Buffer.MemoryCopy(int32Ptr, indexDataPtr, indexData.Length, indexData.Length);
                    }
                }

                var part = new ProcessedMeshPart(
                    vertexData,
                    elementDescs.ToArray(),
                    indexData,
                    IndexFormat.UInt32,
                    (uint)indices.Count,
                    boneIDsByName,
                    boneOffsets);
                parts.Add(part);
            }

            // Nodes
            Node rootNode = scene.RootNode;
            var processedNodes = new List<ProcessedNode>();
            ConvertNode(rootNode, -1, processedNodes);

            var nodes = new ProcessedNodeSet(processedNodes.ToArray(), 0, rootNodeInverseTransform.ToSystemMatrixTransposed());

            for (var animIndex = 0; animIndex < scene.AnimationCount; animIndex++)
            {
                Animation animation = scene.Animations[animIndex];
                var channels = new Dictionary<string, ProcessedAnimationChannel>();
                for (var channelIndex = 0; channelIndex < animation.NodeAnimationChannelCount; channelIndex++)
                {
                    NodeAnimationChannel nac = animation.NodeAnimationChannels[channelIndex];
                    channels[nac.NodeName] = ConvertChannel(nac);
                }

                var baseAnimName = animation.Name;
                if (string.IsNullOrEmpty(baseAnimName))
                {
                    baseAnimName = "anim_" + animIndex;
                }

                var animationName = baseAnimName;


                var counter = 1;
                while (!encounteredNames.Add(animationName))
                {
                    animationName = baseAnimName + "_" + counter.ToString();
                    counter += 1;
                }
            }

            return new ProcessedModel()
            {
                MeshParts = parts.ToArray(),
                Animations = animations.ToArray(),
                Nodes = nodes
            };
        }

        private int GetAndIncrementRelativeBoneIndex(Dictionary<int, int> assignedBoneWeights, int vertexID)
        {
            var currentCount = 0;
            assignedBoneWeights.TryGetValue(vertexID, out currentCount);
            assignedBoneWeights[vertexID] = currentCount + 1;
            return currentCount;
        }

        private ProcessedAnimationChannel ConvertChannel(NodeAnimationChannel nac)
        {
            var nodeName = nac.NodeName;
            var positions = new AssetPrimitives.VectorKey[nac.PositionKeyCount];
            for (var i = 0; i < nac.PositionKeyCount; i++)
            {
                Assimp.VectorKey assimpKey = nac.PositionKeys[i];
                positions[i] = new AssetPrimitives.VectorKey(assimpKey.Time, assimpKey.Value.ToSystemVector3());
            }

            var scales = new AssetPrimitives.VectorKey[nac.ScalingKeyCount];
            for (var i = 0; i < nac.ScalingKeyCount; i++)
            {
                Assimp.VectorKey assimpKey = nac.ScalingKeys[i];
                scales[i] = new AssetPrimitives.VectorKey(assimpKey.Time, assimpKey.Value.ToSystemVector3());
            }

            var rotations = new AssetPrimitives.QuaternionKey[nac.RotationKeyCount];
            for (var i = 0; i < nac.RotationKeyCount; i++)
            {
                Assimp.QuaternionKey assimpKey = nac.RotationKeys[i];
                rotations[i] = new AssetPrimitives.QuaternionKey(assimpKey.Time, assimpKey.Value.ToSystemQuaternion());
            }

            return new ProcessedAnimationChannel(nodeName, positions, scales, rotations);
        }

        private int ConvertNode(Node node, int parentIndex, List<ProcessedNode> processedNodes)
        {
            var currentIndex = processedNodes.Count;
            var childIndices = new int[node.ChildCount];
            var nodeTransform = node.Transform.ToSystemMatrixTransposed();
            var pn = new ProcessedNode(node.Name, nodeTransform, parentIndex, childIndices);
            processedNodes.Add(pn);

            for (var i = 0; i < childIndices.Length; i++)
            {
                var childIndex = ConvertNode(node.Children[i], currentIndex, processedNodes);
                childIndices[i] = childIndex;
            }

            return currentIndex;
        }

        private unsafe struct VertexDataBuilder
        {
            private readonly GCHandle _gch;
            private readonly unsafe byte* _dataPtr;
            private readonly int _vertexSize;

            public VertexDataBuilder(byte[] data, int vertexSize)
            {
                _gch = GCHandle.Alloc(data, GCHandleType.Pinned);
                _dataPtr = (byte*)_gch.AddrOfPinnedObject();
                _vertexSize = vertexSize;
            }

            public void WriteVertexElement<T>(int vertex, int elementOffset, ref T data)
            {
                var dst = _dataPtr + (_vertexSize * vertex) + elementOffset;
                Unsafe.Copy(dst, ref data);
            }

            public void WriteVertexElement<T>(int vertex, int elementOffset, T data)
            {
                var dst = _dataPtr + (_vertexSize * vertex) + elementOffset;
                Unsafe.Copy(dst, ref data);
            }

            public void FreeGCHandle()
            {
                _gch.Free();
            }
        }
    }

    public static class AssimpExtensions
    {
        public static unsafe System.Numerics.Matrix4x4 ToSystemMatrixTransposed(this aiMatrix4x4 mat)
        {
            return System.Numerics.Matrix4x4.Transpose(Unsafe.Read<System.Numerics.Matrix4x4>(&mat));
        }

        public static System.Numerics.Quaternion ToSystemQuaternion(this Assimp.Quaternion quat)
        {
            return new System.Numerics.Quaternion(quat.X, quat.Y, quat.Z, quat.W);
        }

        public static Vector3 ToSystemVector3(this Assimp.Vector3D v3)
        {
            return new Vector3(v3.X, v3.Y, v3.Z);
        }
    }
}
