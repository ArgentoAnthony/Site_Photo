using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Site_Photo_DAL.Interface
{
    public interface IImageProcessor
    {
        void ProcessImage(Stream input, Stream output, int maxWidth, int maxHeight, int quality);
    }
}
