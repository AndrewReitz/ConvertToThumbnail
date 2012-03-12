using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace ConvertToThumbnail
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                help();
                return;
            }

            int height = 64;
            int width = 64;
            string prop = "same";

            string folder = null;

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-h": height = Convert.ToInt32(args[++i]); break;
                    case "-w": width = Convert.ToInt32(args[++i]); break;
                    case "-f": folder = Convert.ToString(args[++i]); break;
                    case "-p": prop = args[++i]; break;
                }
            }

            if (string.IsNullOrEmpty(folder))
            {
                Console.WriteLine("No Folder Path Specified");
                help();
                return;
            }

            int newHeight = 0; //setting to zero to get rid of unassigned var prob
            int newWidth = 0;

            Directory.CreateDirectory(Path.Combine(folder, "thumbs"));

            foreach (string pic in Directory.GetFiles(folder))
            {
                switch (Path.GetExtension(pic.ToLower()))
                {
                    case ".jpg": break;
                    case ".bmp": break;
                    case ".gif": break;
                    case ".exif": break;
                    case ".png": break;
                    case ".tiff": break;
                    default: continue; // not a recognized format by System.Drawing.Bitmap so skip and go to the next
                }
                using (Bitmap source = new Bitmap(pic))
                {
                    switch (prop)
                    {
                        case "same":
                            {
                                newHeight = height;
                                newWidth = width;
                            } break;
                        case "height":
                            {
                                newHeight = height;
                                newWidth = (height * source.Width) / source.Height;
                            } break;
                        case "width":
                            {
                                newWidth = width;
                                newHeight = (width * source.Height) / source.Width;
                            } break;
                        default: Console.WriteLine("Unrecognized proportion, options are height, width or same"); return;
                    }

                    using (Bitmap newImage = new Bitmap(newWidth, newHeight))
                    {
                        using (Graphics gr = Graphics.FromImage(newImage))
                        {
                            gr.SmoothingMode = SmoothingMode.AntiAlias;
                            gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                            gr.DrawImage(new Bitmap(pic), new Rectangle(0, 0, newWidth, newHeight));
                        }

                        string imgName = Path.GetFileName(pic); //get the images name
                        //write the image to thumbs directory with the thumb-imageName
                        newImage.Save(Path.Combine(folder, "thumbs\\thumb-" + imgName));
                    }
                }
            }
        }

        static void help()
        {
            Console.WriteLine("This program takes in a folder path, and reads all the images");
            Console.WriteLine("in that folder and converts them to thumbnails");
            Console.WriteLine("The output will be in a subfolder thumbs, of path given");
            Console.WriteLine("You must provide the -f [path] option");
            Console.WriteLine();
            Console.WriteLine("Other options include -h [height] to specify the height");
            Console.WriteLine("of the thumbnail, -w [width] to specify the width of");
            Console.WriteLine("the thumbnail, and lastly -p [options: same, height, or width]");
            Console.WriteLine("to specify if you want the thumbnails to be scaled to the");
            Console.WriteLine("height or width specified.  The default is same meaning");
            Console.WriteLine("that the thumbnail will be scaled to the height and");
            Console.WriteLine("width input, default height and width is 64");
        }
    }
}
