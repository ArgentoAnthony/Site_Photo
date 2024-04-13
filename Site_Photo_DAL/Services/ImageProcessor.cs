using Site_Photo_DAL.Interface;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site_Photo_DAL.Services
{
    public class ImageProcessor: IImageProcessor
    {
        public void ProcessImage(Stream input, Stream output, int maxWidth, int maxHeight, int quality)
        {
            using (Image image = Image.Load(input))
            {
                // Calculer les nouvelles dimensions en maintenant le rapport hauteur/largeur
                double ratioX = (double)maxWidth / image.Width;
                double ratioY = (double)maxHeight / image.Height;
                double ratio = Math.Min(ratioX, ratioY); // Utiliser le ratio le plus petit pour éviter toute déformation
                int newWidth = (int)(image.Width * ratio);
                int newHeight = (int)(image.Height * ratio);

                // Redimensionner l'image
                image.Mutate(x => x.Resize(newWidth, newHeight));

                // Encoder l'image en JPEG avec la qualité spécifiée
                var encoder = new JpegEncoder { Quality = quality };
                image.Save(output, encoder);
            }
        }
    }
}
