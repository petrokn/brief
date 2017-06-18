﻿namespace brief.Library
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using Helpers;

    public abstract class BaseImageService
    {
        private readonly ImageFormat _mainTransformerFormat;

        protected BaseImageService(BaseTransformerSettings settings)
        {
            _mainTransformerFormat = settings.MainTransformerFormat;
        }

        public virtual string ConvertToAppropirateFormat(string existingFilePath, bool deleteOriginal)
        {
            var image = Image.FromFile(existingFilePath);

            if (image.RawFormat.Equals(_mainTransformerFormat))
            {
                return existingFilePath;
            }

            var newPath = existingFilePath.Substring(0, existingFilePath.LastIndexOf(".", StringComparison.Ordinal)) + "." +
                          _mainTransformerFormat;

            image.Save(newPath, _mainTransformerFormat);
            image.Dispose();

            if (deleteOriginal)
            {
                File.Delete(existingFilePath);
            }

            return newPath;
        }

        public void CleanUp(string imagePath)
        {
            File.Delete(imagePath);
        }
    }
}
