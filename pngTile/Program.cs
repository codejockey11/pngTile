using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace pngTile
{
    class Program
    {
        static Bitmap image;
        static StreamWriter sw;

        static void ProcessImage(String filename)
        {
            image = (Bitmap)Bitmap.FromFile(filename);

            Boolean wasFound = false;

            for (Int32 x = 0; x < image.Width; x++)
            {
                for (Int32 y = 0; y < image.Height; y++)
                {
                    Color clr = image.GetPixel(x, y);
                    if (((clr.R == 255) && (clr.G == 0) && (clr.B == 255)) || (clr.A < 255))
                    {
                        wasFound = true;
                        
                        image.SetPixel(x, y, Color.FromArgb(0, 87, 91, 79));
                    }
                }
            }

            if (wasFound)
            {
                // have to write to a different file name due to the file lock
                String[] str = filename.Split('.');
                
                String[] path = str[0].Split('\\');

                image.Save(str[0] + "a." + str[1], System.Drawing.Imaging.ImageFormat.Png);

                image.Dispose();

                // Write bat file to do renames
                sw.WriteLine("DEL \"" + str[0] + "." + str[1] + "\"");
                sw.WriteLine("REN \"" + str[0] + "a." + str[1] + "\" \"" + path[path.Length - 1] + "." + str[1] + "\"");
            }

        }

        static void Main(string[] args)
        {
            String[] s = args[0].Split('\\');

            sw = new StreamWriter(s[s.Length - 1] + "Rename.bat");

            String[] subdirectoryEntries = Directory.GetDirectories(args[0]);

            foreach (String subdirectory in subdirectoryEntries)
            {
                String[] directories = Directory.GetDirectories(subdirectory);
                
                foreach (String directory in directories)
                {
                    String[] fileEntries = Directory.GetFiles(directory);
                    
                    foreach(String file in fileEntries)
                    {
                        String[] f = file.Split('.');
                        
                        if(String.Compare(f[f.Length - 1], "png") == 0)
                        {
                            ProcessImage(file);
                        }
                    }
                }
            }

            sw.Close();

        }
    }
}
