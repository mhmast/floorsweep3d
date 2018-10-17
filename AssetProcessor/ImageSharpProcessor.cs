using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Transforms;
using SixLabors.ImageSharp.Processing.Transforms.Resamplers;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Veldrid;
using AssetPrimitives;

namespace AssetProcessor
{
    public class ImageSharpProcessor : BinaryAssetProcessor<ProcessedTexture>
    {
        public unsafe override ProcessedTexture ProcessT(Stream stream, string extension)
        {
            Image<Rgba32> image = Image.Load(stream);
            var mipmaps = GenerateMipmaps(image, out var totalSize);

            var allTexData = new byte[totalSize];
            long offset = 0;
            fixed (byte* allTexDataPtr = allTexData)
            {
                foreach (var mipmap in mipmaps)
                {
                    long mipSize = mipmap.Width * mipmap.Height * sizeof(Rgba32);
                    fixed (Rgba32* pixelPtr = &mipmap.DangerousGetPinnableReferenceToPixelBuffer())
                    {
                        Buffer.MemoryCopy(pixelPtr, allTexDataPtr + offset, mipSize, mipSize);
                    }

                    offset += mipSize;
                }
            }

            var texData = new ProcessedTexture(
                    PixelFormat.R8_G8_B8_A8_UNorm, TextureType.Texture2D,
                    (uint)image.Width, (uint)image.Height, 1,
                    (uint)mipmaps.Length, 1,
                    allTexData);
            return texData;
        }

        // Taken from Veldrid.ImageSharp

        private static readonly IResampler s_resampler = new Lanczos3Resampler();

        private static Image<T>[] GenerateMipmaps<T>(Image<T> baseImage, out int totalSize) where T : struct, IPixel<T>
        {
            var mipLevelCount = ComputeMipLevels(baseImage.Width, baseImage.Height);
            var mipLevels = new Image<T>[mipLevelCount];
            mipLevels[0] = baseImage;
            totalSize = baseImage.Width * baseImage.Height * Unsafe.SizeOf<T>();
            var i = 1;

            var currentWidth = baseImage.Width;
            var currentHeight = baseImage.Height;
            while (currentWidth != 1 || currentHeight != 1)
            {
                var newWidth = Math.Max(1, currentWidth / 2);
                var newHeight = Math.Max(1, currentHeight / 2);
                var newImage = baseImage.Clone(context => context.Resize(newWidth, newHeight, s_resampler));
                Debug.Assert(i < mipLevelCount);
                mipLevels[i] = newImage;

                totalSize += newWidth * newHeight * Unsafe.SizeOf<T>();
                i++;
                currentWidth = newWidth;
                currentHeight = newHeight;
            }

            Debug.Assert(i == mipLevelCount);

            return mipLevels;
        }

        public static int ComputeMipLevels(int width, int height)
        {
            return 1 + (int)Math.Floor(Math.Log(Math.Max(width, height), 2));
        }
    }
}
