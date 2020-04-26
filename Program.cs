using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_info_S4
{
    class Program
    {
        static void Main(string[] args)
        {
            //MyImage image = new MyImage("coco.bmp");
            //image.Rotation1(27);
            //image.From_Image_To_File("COCOrot.bmp");
            //MyImage fract = new MyImage(4000, 4000);
            //fract.From_Image_To_File("fract.bmp");
            //MyImage cache = new MyImage("coco.bmp");
            //cache.ImageDansImage("fract.bmp");
            //cache.From_Image_To_File("imagecodee.bmp");
            //cache.Décodage();
            //cache.From_Image_To_File("decode.bmp");
            MyImage histo = new MyImage("coco.bmp");
            histo.Histogramme();
            histo.From_Image_To_File("histogramme.bmp");
            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
}
