using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing;


namespace Projet_info_S4
{
    class MyImage
    {
        //Attributs
        string type;
        int taille;
        int large;
        int haut;
        int offset;
        int couleur;
        Pixel[,] image;
        byte[] header;

        //Constructeur
        public MyImage(string fichier)
        {
            byte[] image = File.ReadAllBytes(fichier);
            this.type = Encoding.ASCII.GetString(new byte[] { image[0], image[1] });
            this.taille = image.Count();
            this.large = Convertir_Endian_To_Int(new byte[] { image[18], image[19], image[20], image[21] });
            this.haut = Convertir_Endian_To_Int(new byte[] { image[22], image[23], image[24], image[25] });
            this.offset = Convertir_Endian_To_Int(new byte[] { image[14], image[15], image[16], image[17] });
            this.couleur = Convertir_Endian_To_Int(new byte[] { image[28], image[29] }) / 8;
            this.image = GoImage(image, large, haut, offset);
            this.header = new byte[54];
            for (int i = 0; i < 54; i++) { header[i] = image[i]; }
        }

        //Constructeur
        public MyImage(int large, int haut)
        {
            this.large = large;
            this.haut = haut;
            type = "bmp";
            taille = haut * large * 3 + 54;
            offset = 40;
            couleur = 3;
            byte[] tailletab = Convertir_Int_To_Endian(taille, 4);
            byte[] hauttab = Convertir_Int_To_Endian(haut, 4);
            byte[] largetab = Convertir_Int_To_Endian(large, 4);
            header = new byte[54] { 66, 77, tailletab[0], tailletab[1], tailletab[2], tailletab[3], 0, 0, 0, 0, 54, 0, 0, 0, 40, 0, 0, 0, largetab[0], largetab[1], largetab[2], largetab[3], hauttab[0], hauttab[1], hauttab[2], hauttab[3], 1, 0, 24, 0, 0, 0, 0, 0, 176, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            image = new Pixel[haut, large];
            Fractale();
        }

        //constructeur pour le qrcode
        public MyImage(int cote, Pixel[,] code)
        {
            type = "bmp";
            this.haut = cote;
            this.large = cote;
            this.taille = cote*cote + 54;
            this.offset = 54;
            this.couleur = 3;
            this.image = code;
            byte[] tailletab = Convertir_Int_To_Endian(taille, 4);
            byte[] hauttab = Convertir_Int_To_Endian(haut, 4);
            byte[] largetab = Convertir_Int_To_Endian(large, 4);
            header = new byte[54] { 66, 77, tailletab[0], tailletab[1], tailletab[2], tailletab[3], 0, 0, 0, 0, 54, 0, 0, 0, 40, 0, 0, 0, largetab[0], largetab[1], largetab[2], largetab[3], hauttab[0], hauttab[1], hauttab[2], hauttab[3], 1, 0, 24, 0, 0, 0, 0, 0, 176, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        }

        //Métohde pour passer d'un tableau de byte en entier avec une boucle for sur tous les bytes du tableau
        private int Convertir_Endian_To_Int(byte[] tab)
        {
            int i = 1;
            int tot = 0;
            foreach (int a in tab)
            {
                tot += a * i;
                i *= 256;
            }
            return tot;
        }

        //Méthode pour passer d'un entier en un tableau de bytes avec une boucle for sur le nombre d'octets sur lequel on veut faire la conversion
        private byte[] Convertir_Int_To_Endian(int val, int nboctet)
        {
            byte[] res = new byte[nboctet];
            int reste = val % 256;
            res[0] = (byte)reste;
            val -= reste;
            for (int i = nboctet - 1; i > 0; i--)
            {
                int temp = val / ((int)Math.Pow(256, i));
                res[i] = (byte)temp;
                val -= ((int)Math.Pow(256, i) * temp);
            }
            return res;
        }

        //Méthode pour accéder aux pixels de l'image sans le header
        private Pixel[,] GoImage(byte[] image, int large, int haut, int offset)
        {
            Pixel[,] tab = new Pixel[haut, large];
            int i = offset + 14;
            for (int y = 0; y < haut; y++)
            {
                for (int x = 0; x < large; x++)
                {
                    tab[y, x] = new Pixel(image[i], image[i + 1], image[i + 2]);
                    i += 3;
                }
            }
            return tab;
        }

        //Méthode pour passer d'une image à un fichier en recréant le header puis l'image et en le mettant dans un fichier 
        public void From_Image_To_File(string file)
        {
            byte[] myfile = new byte[this.taille];
            for (int i = 0; i < 54; i++)
            {
                myfile[i] = header[i];
            }
            int index = 54;
            for (int y = 0; y < haut; y++)
            {
                for (int x = 0; x < large; x++)
                {
                    if (index < taille) { myfile[index] = image[y, x].Red; }
                    if (index + 1 < taille) myfile[index + 1] = image[y, x].Green;
                    if (index + 2 < taille) myfile[index + 2] = image[y, x].Blue;
                    index += 3;
                }
            }
            File.WriteAllBytes(file, myfile);
        }

        //Méthode pour convertir une image colorée en image de nuances de gris
        public void ConvertToGris()
        {
            for (int j = 0; j < this.haut; j++)
            {
                for (int i = 0; i < this.large; i++)
                {
                    image[j, i].ConvertToGris();
                }
            }
        }

        //Methode qui agrandit les dimensions de l'image 
        public void Agrandissement(int fact)
        {
            Console.WriteLine();
            Pixel[,] image1 = new Pixel[haut * fact, large * fact];
            for (int j = 0; j < this.haut; j++)
            {
                for (int i = 0; i < this.large; i++)
                {
                    for (int ii = 0; ii < fact; ii++)
                    {
                        for (int jj = 0; jj < fact; jj++)
                        {
                            image1[j * fact + jj, i * fact + ii] = image[j, i];
                        }
                    }
                }
            }
            taille = (taille - 54) * fact * fact + 54;
            image = image1;
            haut *= fact;
            large *= fact;
            byte[] temp = Convertir_Int_To_Endian(large, 4);
            for (int i = 0; i < 4; i++) { header[18 + i] = temp[i]; }
            temp = Convertir_Int_To_Endian(haut, 4);
            for (int i = 0; i < 4; i++) { header[22 + i] = temp[i]; }
            temp = Convertir_Int_To_Endian(taille , 4);
            for (int i = 0; i < 4; i++) { header[2 + i] = temp[i]; }
        }

        //De même, méthode qui à l'inverse réduit les dimensions de l'image
        public void Reduction(int fact)
        {
            Console.WriteLine();
            Pixel[,] image1 = new Pixel[haut / fact, large / fact];
            for (int j = 0; j < this.haut / fact; j++)
            {
                for (int i = 0; i < this.large / fact; i++)
                {
                    for (int ii = 0; ii < fact; ii++)
                    {
                        image1[j, i] = image[j * fact, i * fact];
                    }

                }
            }
            taille = (taille - 54) / (fact * fact) + 54;
            image = image1;
            haut /= fact;
            large /= fact;
            byte[] temp = Convertir_Int_To_Endian(large, 4);
            for (int i = 0; i < 4; i++) { header[18 + i] = temp[i]; }
            temp = Convertir_Int_To_Endian(haut, 4);
            for (int i = 0; i < 4; i++) { header[22 + i] = temp[i]; }
            temp = Convertir_Int_To_Endian(taille, 4);
            for (int i = 0; i < 4; i++) { header[2 + i] = temp[i]; }
        }

        //Méthode qui renvoie l'image mais comme si c'était son miroir c'est à dire inverser l'image soit de façon veticale soit de façon horizontale
        public void Miroir(char x)
        {
            Pixel[,] image1 = new Pixel[image.GetLength(0), image.GetLength(1)];
            if (x == 'H')
            {
                for (int j = 0; j < this.haut; j++)
                {
                    for (int i = 0; i < this.large; i++)
                    {
                        image1[j, i] = image[j, this.large - 1 - i];
                    }
                }
                image = image1;
            }



            else if (x == 'V')
            {
                for (int i = 0; i < this.large; i++)
                {
                    for (int j = 0; j < this.haut; j++)
                    {
                        image1[j, i] = image[this.haut - 1 - j, i];
                    }
                }
                image = image1;
            }



            else
            {
                Console.WriteLine("Vous n'avez pas choisi V ou H");
            }
        }

        //Méthode qui permet de tourner une image selon n'importe quel angle
        public void Rotation(int angleint)
        {
            int taille1 = Convert.ToInt32(Math.Sqrt(haut * haut + large * large)) + 1;
            Pixel[,] image1 = new Pixel[taille1, taille1];
            Pixel[,] imageTemp = new Pixel[taille1, taille1];
            int X = (taille1 - haut) / 2;
            int Y = (taille1 - large) / 2;
            double angle = (angleint * Math.PI) / 180;
            for (int k = 0; k < taille1; k++)
            {
                for (int l = 0; l < taille1; l++)
                {
                    image1[l, k] = new Pixel((byte)0, (byte)0, (byte)0);
                    if (l >= X && l < X + haut && k >= Y && k < Y + large)
                    { imageTemp[l, k] = image[l - X, k - Y]; }
                    else { imageTemp[l, k] = new Pixel((byte)0, (byte)0, (byte)0); }
                }
            }
            image = imageTemp;
            X = taille1 / 2;
            Y = taille1 / 2;
            for (int j = 0; j < taille1; j++)
            {
                for (int i = 0; i < taille1; i++)
                {
                    int tempi = Convert.ToInt32((i-X) * Math.Cos(angle) - (j-Y) * Math.Sin(angle)+X);
                    int tempj = Convert.ToInt32((i-X) * Math.Sin(angle) + (j-Y) * Math.Cos(angle)+Y);
                    if (tempj < taille1 && tempi < taille1 && 0 <= tempj && 0 <= tempi) { image1[tempj, tempi] = image[j, i]; }
                    
                }
            }
            image = image1;
            haut = taille1;
            large = taille1;
            taille = haut * large * 3 + 54;
            byte[] temp = Convertir_Int_To_Endian(large, 4);
            for (int i = 0; i < 4; i++) { header[18 + i] = temp[i]; }
            temp = Convertir_Int_To_Endian(haut, 4);
            for (int i = 0; i < 4; i++) { header[22 + i] = temp[i]; }
            temp = Convertir_Int_To_Endian(taille, 4);
            for (int i = 0; i < 4; i++) { header[2 + i] = temp[i]; }
            Agrandissement(4);
        }

        //Méthode permettant de retrouver la matrice de convolution de l'image
        public void Convolution(int[,] matrice)
        {
            Pixel[,] temp = new Pixel[haut, large];
            if (matrice.GetLength(0) == 3 && matrice.GetLength(1) == 3)
            {
                for (int i = 0; i < large; i++)
                {
                    for (int j = 0; j < haut; j++)
                    {
                        temp[j, i] = Operation(matrice, i, j);
                    }
                }
                image = temp;
            }
            else { Console.WriteLine("erreur"); }
        }

        //Méthode permettant de donner un asspect flouté à l'image en utilisant la matrice de convolution
        public void Flou()
        {
            int[,] matrice = new int[3, 3] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
            Convolution(matrice);
        }

        //Méthode permettant de détecter les bords toujours à l'aide de la matrice de convolution
        public void DetecBord()
        {
            int[,] matrice = new int[3, 3] { { 0, 1, 0 }, { 1, -4, 1 }, { 0, 1, 0 } };
            Convolution(matrice);
        }

        //Méthode qui renforce les bords de l'image
        public void RenfocementBords()
        {
            int[,] matrice = new int[3, 3] { { 0, 0, 0 }, { -1, 1, 0 }, { 0, 0, 0 } };
            Convolution(matrice);
        }

        //Méthode qui repousse les bords de l'image
        public void Repoussage()
        {
            int[,] matrice = new int[3, 3] { { -2, -1, 0 }, { -1, 1, 1 }, { 0, 1, 2 } };
            Convolution(matrice);
        }

        //Méthode qui créé une image de fractale
        private void Fractale()
        {
            double l = 2 / (double)large;
            double h = 2 / (double)haut;
            for (int i = 0; i < large; i++)
            {
                for (int j = 0; j < haut; j++)
                {
                    double X0 = -1.5 + (double)i * l;
                    double Y0 = -1 + (double)j * h;
                    double X = X0 * X0 - Y0 * Y0 + X0;
                    double Y = 2 * X0 * Y0 + Y0;
                    double mod = Math.Sqrt(X * X + Y * Y);
                    bool test = true;
                    int essai = 1;
                    while (test && essai < 20)
                    {
                        double Temp = X * X - Y * Y + X0;
                        Y = 2 * X * Y + Y0;
                        X = Temp;
                        mod = Math.Sqrt(X * X + Y * Y);
                        test = mod < 2;
                        essai++;
                    }
                    if (test) { image[j, i] = new Pixel((byte)0, (byte)0, (byte)0); }
                    else { image[j, i] = new Pixel((byte)255, (byte)0, (byte)0); }
                }
            }
        }

        //Méthode qui permet de faire la matrice de convolution et qui est réutilisée dans la méthode Convolution
        private Pixel Operation(int[,] matrice, int i, int j)
        {
            byte r;
            byte g;
            byte b;
            if (i == 0)
            {
                if (j == 0)
                {
                    r = (byte)((image[j, i].Red * matrice[1, 1] + image[j, i + 1].Red * matrice[1, 2] + image[j + 1, i].Red * matrice[2, 1] + image[j + 1, i + 1].Red * matrice[2, 2]) / 4);
                    g = (byte)((image[j, i].Green * matrice[1, 1] + image[j, i + 1].Green * matrice[1, 2] + image[j + 1, i].Green * matrice[2, 1] + image[j + 1, i + 1].Green * matrice[2, 2]) / 4);
                    b = (byte)((image[j, i].Blue * matrice[1, 1] + image[j, i + 1].Blue * matrice[1, 2] + image[j + 1, i].Blue * matrice[2, 1] + image[j + 1, i + 1].Blue * matrice[2, 2]) / 4);
                }
                else if (j == haut - 1)
                {
                    r = (byte)((image[j - 1, i].Red * matrice[0, 1] + image[j - 1, i + 1].Red * matrice[0, 2] + image[j, i].Red * matrice[1, 1] + image[j, i + 1].Red * matrice[1, 2]) / 4);
                    g = (byte)((image[j - 1, i].Green * matrice[0, 1] + image[j - 1, i + 1].Green * matrice[0, 2] + image[j, i].Green * matrice[1, 1] + image[j, i + 1].Green * matrice[1, 2]) / 4);
                    b = (byte)((image[j - 1, i].Blue * matrice[0, 1] + image[j - 1, i + 1].Blue * matrice[0, 2] + image[j, i].Blue * matrice[1, 1] + image[j, i + 1].Blue * matrice[1, 2]) / 4);
                }
                else
                {
                    r = (byte)((image[j - 1, i].Red * matrice[0, 1] + image[j - 1, i + 1].Red * matrice[0, 2] + image[j, i].Red * matrice[1, 1] + image[j, i + 1].Red * matrice[1, 2] + image[j + 1, i].Red * matrice[2, 1] + image[j + 1, i + 1].Red * matrice[2, 2]) / 6);
                    g = (byte)((image[j - 1, i].Green * matrice[0, 1] + image[j - 1, i + 1].Green * matrice[0, 2] + image[j, i].Green * matrice[1, 1] + image[j, i + 1].Green * matrice[1, 2] + image[j + 1, i].Green * matrice[2, 1] + image[j + 1, i + 1].Green * matrice[2, 2]) / 6);
                    b = (byte)((image[j - 1, i].Blue * matrice[0, 1] + image[j - 1, i + 1].Blue * matrice[0, 2] + image[j, i].Blue * matrice[1, 1] + image[j, i + 1].Blue * matrice[1, 2] + image[j + 1, i].Blue * matrice[2, 1] + image[j + 1, i + 1].Blue * matrice[2, 2]) / 6);
                }
            }
            else if (i == large - 1)
            {
                if (j == 0)
                {
                    r = (byte)((image[j, i - 1].Red * matrice[1, 0] + image[j, i].Red * matrice[1, 1] + image[j + 1, i - 1].Red * matrice[2, 0] + image[j + 1, i].Red * matrice[2, 1]) / 4);
                    g = (byte)((image[j, i - 1].Green * matrice[1, 0] + image[j, i].Green * matrice[1, 1] + image[j + 1, i - 1].Green * matrice[2, 0] + image[j + 1, i].Green * matrice[2, 1]) / 4);
                    b = (byte)((image[j, i - 1].Blue * matrice[1, 0] + image[j, i].Blue * matrice[1, 1] + image[j + 1, i - 1].Blue * matrice[2, 0] + image[j + 1, i].Blue * matrice[2, 1]) / 4);

                }
                else if (j == haut - 1)
                {
                    r = (byte)((image[j - 1, i - 1].Red * matrice[0, 0] + image[j - 1, i].Red * matrice[0, 1] + image[j, i - 1].Red * matrice[1, 0] + image[j, i].Red * matrice[1, 1]) / 4);
                    g = (byte)((image[j - 1, i - 1].Green * matrice[0, 0] + image[j - 1, i].Green * matrice[0, 1] + image[j, i - 1].Green * matrice[1, 0] + image[j, i].Green * matrice[1, 1]) / 4);
                    b = (byte)((image[j - 1, i - 1].Blue * matrice[0, 0] + image[j - 1, i].Blue * matrice[0, 1] + image[j, i - 1].Blue * matrice[1, 0] + image[j, i].Blue * matrice[1, 1]) / 4);
                }
                else
                {
                    r = (byte)((image[j - 1, i - 1].Red * matrice[0, 0] + image[j - 1, i].Red * matrice[0, 1] + image[j, i - 1].Red * matrice[1, 0] + image[j, i].Red * matrice[1, 1] + image[j + 1, i - 1].Red * matrice[2, 0] + image[j + 1, i].Red * matrice[2, 1]) / 6);
                    g = (byte)((image[j - 1, i - 1].Green * matrice[0, 0] + image[j - 1, i].Green * matrice[0, 1] + image[j, i - 1].Green * matrice[1, 0] + image[j, i].Green * matrice[1, 1] + image[j + 1, i - 1].Green * matrice[2, 0] + image[j + 1, i].Green * matrice[2, 1]) / 6);
                    b = (byte)((image[j - 1, i - 1].Blue * matrice[0, 0] + image[j - 1, i].Blue * matrice[0, 1] + image[j, i - 1].Blue * matrice[1, 0] + image[j, i].Blue * matrice[1, 1] + image[j + 1, i - 1].Blue * matrice[2, 0] + image[j + 1, i].Blue * matrice[2, 1]) / 6);

                }
            }
            else if (j == 0)
            {
                r = (byte)((image[j, i - 1].Red * matrice[1, 0] + image[j, i].Red * matrice[1, 1] + image[j, i + 1].Red * matrice[1, 2] + image[j + 1, i - 1].Red * matrice[2, 0] + image[j + 1, i].Red * matrice[2, 1] + image[j + 1, i + 1].Red * matrice[2, 2]) / 6);
                g = (byte)((image[j, i - 1].Green * matrice[1, 0] + image[j, i].Green * matrice[1, 1] + image[j, i + 1].Green * matrice[1, 2] + image[j + 1, i - 1].Green * matrice[2, 0] + image[j + 1, i].Green * matrice[2, 1] + image[j + 1, i + 1].Green * matrice[2, 2]) / 6);
                b = (byte)((image[j, i - 1].Blue * matrice[1, 0] + image[j, i].Blue * matrice[1, 1] + image[j, i + 1].Blue * matrice[1, 2] + image[j + 1, i - 1].Blue * matrice[2, 0] + image[j + 1, i].Blue * matrice[2, 1] + image[j + 1, i + 1].Blue * matrice[2, 2]) / 6);

            }
            else if (j == haut - 1)
            {
                r = (byte)((image[j - 1, i - 1].Red * matrice[0, 0] + image[j - 1, i].Red * matrice[0, 1] + image[j - 1, i + 1].Red * matrice[0, 2] + image[j, i - 1].Red * matrice[1, 0] + image[j, i].Red * matrice[1, 1] + image[j, i + 1].Red * matrice[1, 2]) / 6);
                g = (byte)((image[j - 1, i - 1].Green * matrice[0, 0] + image[j - 1, i].Green * matrice[0, 1] + image[j - 1, i + 1].Green * matrice[0, 2] + image[j, i - 1].Green * matrice[1, 0] + image[j, i].Green * matrice[1, 1] + image[j, i + 1].Green * matrice[1, 2]) / 6);
                b = (byte)((image[j - 1, i - 1].Blue * matrice[0, 0] + image[j - 1, i].Blue * matrice[0, 1] + image[j - 1, i + 1].Blue * matrice[0, 2] + image[j, i - 1].Blue * matrice[1, 0] + image[j, i].Blue * matrice[1, 1] + image[j, i + 1].Blue * matrice[1, 2]) / 6);
            }
            else
            {
                r = (byte)((image[j - 1, i - 1].Red * matrice[0, 0] + image[j - 1, i].Red * matrice[0, 1] + image[j - 1, i + 1].Red * matrice[0, 2] + image[j, i - 1].Red * matrice[1, 0] + image[j, i].Red * matrice[1, 1] + image[j, i + 1].Red * matrice[1, 2] + image[j + 1, i - 1].Red * matrice[2, 0] + image[j + 1, i].Red * matrice[2, 1] + image[j + 1, i + 1].Red * matrice[2, 2]) / 9);
                g = (byte)((image[j - 1, i - 1].Green * matrice[0, 0] + image[j - 1, i].Green * matrice[0, 1] + image[j - 1, i + 1].Green * matrice[0, 2] + image[j, i - 1].Green * matrice[1, 0] + image[j, i].Green * matrice[1, 1] + image[j, i + 1].Green * matrice[1, 2] + image[j + 1, i - 1].Green * matrice[2, 0] + image[j + 1, i].Green * matrice[2, 1] + image[j + 1, i + 1].Green * matrice[2, 2]) / 9);
                b = (byte)((image[j - 1, i - 1].Blue * matrice[0, 0] + image[j - 1, i].Blue * matrice[0, 1] + image[j - 1, i + 1].Blue * matrice[0, 2] + image[j, i - 1].Blue * matrice[1, 0] + image[j, i].Blue * matrice[1, 1] + image[j, i + 1].Blue * matrice[1, 2] + image[j + 1, i - 1].Blue * matrice[2, 0] + image[j + 1, i].Blue * matrice[2, 1] + image[j + 1, i + 1].Blue * matrice[2, 2]) / 9);
            }
            Pixel res = new Pixel(r, g, b);
            return res;
        }


        //Méthode qui permet de réaliser l'histogramme des couleurs d'une image
        public void Histogramme()
        {
            int taille2 = image.GetLength(0) * image.GetLength(1);
            int[] R = new int[256];
            int[] V = new int[256];
            int[] B = new int[256];
            byte val1 = 0;
            byte val2 = 0;
            byte val3 = 0;
            for (int k = 0; k < 256; k++)
            {
                R[k] = 0;
                V[k] = 0;
                B[k] = 0;
            }

            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    val1 = image[i, j].Red;
                    val2 = image[i, j].Green;
                    val3 = image[i, j].Blue;
                    R[val1] += 1;
                    V[val2] += 1;
                    B[val3] += 1;
                }
            }

            haut = 0;
            for (int l = 0; l < 256; l++)
            {
                if (R[l] >= haut)
                {
                    haut = R[l];
                }
                if (V[l] >= haut)
                {
                    haut = V[l];
                }
                if (B[l] >= haut)
                {
                    haut = B[l];
                }
            }

            large = 788;
            Pixel[,] hist = new Pixel[haut, large];
            for (int i = 0; i < haut; i++)
            {
                for (int j = 0; j < large; j++)
                {
                    hist[i, j] = new Pixel(0, 0, 0);
                }
            }

            for (int a = 0; a < haut; a++)
            {
                for (int b = 0; b < large; b++)
                {
                    hist[a, b].Red = 255;
                    hist[a, b].Blue = 255;
                    hist[a, b].Green = 255;
                }
            }

            for (int m = 0; m < 255; m++)
            {
                for (int n = 0; n < R[m]; n++)
                {
                    hist[haut - n - 1, m] = new Pixel(0, 0, 0);
                    hist[haut - n - 1, m].Red = 255;
                    hist[haut - n - 1, m].Blue = 0;
                    hist[haut - n - 1, m].Green = 0;
                }
            }

            for (int m = 266; m < 521; m++)
            {
                for (int n = 0; n < B[m - 266]; n++)
                {
                    hist[haut - n - 1, m] = new Pixel(0, 0, 0);
                    hist[haut - n - 1, m].Red = 0;
                    hist[haut - n - 1, m].Blue = 255;
                    hist[haut - n - 1, m].Green = 0;
                }
            }

            for (int m = 532; m < 788; m++)
            {
                for (int n = 0; n < V[m - 532]; n++)
                {
                    hist[haut - n - 1, m] = new Pixel(0, 0, 0);
                    hist[haut - n - 1, m].Red = 0;
                    hist[haut - n - 1, m].Blue = 0;
                    hist[haut - n - 1, m].Green = 255;
                }
            }

            Pixel[,] Histogramme = new Pixel[haut, large];
            for (int i = 0; i < haut; i++)
            {
                for (int j = 0; j < large; j++)
                {
                    Histogramme[i, j] = new Pixel(0, 0, 0);
                }
            }

            for (int i = 0; i < haut; i++)
            {
                for (int j = 0; j < large; j++)
                {
                    Histogramme[i, j].Red = 255;
                    Histogramme[i, j].Blue = 255;
                    Histogramme[i, j].Green = 255;
                }
            }

            for (int i = 0; i < haut; i++)
            {
                for (int j = 0; j < large; j++)
                {
                    Histogramme[haut - i - 1, j] = hist[i, j];
                }
            }

            image = Histogramme;
            haut = 256;
            large = 788;
            taille = haut * large * 3 + 54;
            byte[] temp = Convertir_Int_To_Endian(large, 4);
            for (int i = 0; i < 4; i++) { header[18 + i] = temp[i]; }
            temp = Convertir_Int_To_Endian(haut, 4);
            for (int i = 0; i < 4; i++) { header[22 + i] = temp[i]; }
            temp = Convertir_Int_To_Endian(taille, 4);
            for (int i = 0; i < 4; i++) { header[2 + i] = temp[i]; }
        }

        //Méthode permettant de coder une image dans une autre image
        public void ImageDansImage(string fichier1)
        {
            byte[] file2 = File.ReadAllBytes(fichier1);
            int l = large;
            int h = haut;
            int compt = 0;
            Pixel[,] im2 = new Pixel[h, l];


            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    im2[i, j] = new Pixel(0, 0, 0);
                    im2[i, j].Red = file2[54 + compt];
                    im2[i, j].Green = file2[54 + 1 + compt];
                    im2[i, j].Blue = file2[54 + 2 + compt];
                    compt = compt + 3;
                }
            }

            int hbis = h;
            int lbis = l;
            Pixel[,] ImageFinale = new Pixel[haut, large];
            for (int i = 0; i < haut; i++)
            {
                for (int j = 0; j < large; j++)
                {
                    ImageFinale[i, j] = image[i, j];
                }
            }

            for (int i = 0; i < hbis; i++)
            {
                for (int j = 0; j < lbis; j++)
                {
                    string R = Convert.ToString(image[i, j].Red, 2);
                    string B = Convert.ToString(image[i, j].Blue, 2);
                    string V = Convert.ToString(image[i, j].Green, 2);
                    string R2 = Convert.ToString(im2[i, j].Red, 2);
                    string B2 = Convert.ToString(im2[i, j].Blue, 2);
                    string V2 = Convert.ToString(im2[i, j].Green, 2);
                    char[] Rbis = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
                    char[] Vbis = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
                    char[] Bbis = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
                    char[] Rbis2 = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
                    char[] Vbis2 = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
                    char[] Bbis2 = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
                    char[] Rbis3 = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
                    char[] Vbis3 = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
                    char[] Bbis3 = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
                    int compteur = 0;
                    for (int u = R.Length - 1; u >= 0; u--)
                    {
                        Rbis[7 - compteur] = Convert.ToChar(R[u]);
                        compteur++;

                    }
                    compteur = 0;
                    for (int u = V.Length - 1; u >= 0; u--)
                    {
                        Vbis[7 - compteur] = Convert.ToChar(V[u]);
                        compteur++;
                    }
                    compteur = 0;
                    for (int u = B.Length - 1; u >= 0; u--)
                    {
                        Bbis[7 - compteur] = Convert.ToChar(B[u]);
                        compteur++;
                    }

                    compteur = 0;
                    for (int u = R2.Length - 1; u >= 0; u--)
                    {
                        Rbis2[7 - compteur] = Convert.ToChar(R2[u]);
                        compteur++;
                    }
                    compteur = 0;
                    for (int u = V2.Length - 1; u >= 0; u--)
                    {

                        Vbis2[7 - compteur] = Convert.ToChar(V2[u]);
                        compteur++;
                    }
                    compteur = 0;
                    for (int u = B2.Length - 1; u >= 0; u--)
                    {
                        Bbis2[7 - compteur] = Convert.ToChar(B2[u]);
                        compteur++;
                    }
                    for (int y = 0; y < 4; y++)
                    {
                        Rbis3[y] = Rbis[y];
                        Vbis3[y] = Vbis[y];
                        Bbis3[y] = Bbis[y];
                        Rbis3[4 + y] = Rbis2[y];
                        Vbis3[4 + y] = Vbis2[y];
                        Bbis3[4 + y] = Bbis2[y];
                    }
                    string Rouge = new string(Rbis3);
                    string Vert = new string(Vbis3);
                    string Bleu = new string(Bbis3);
                    ImageFinale[i, j].Red = (byte)Convert.ToInt32(Rouge, 2);
                    ImageFinale[i, j].Green = (byte)Convert.ToInt32(Vert, 2);
                    ImageFinale[i, j].Blue = (byte)Convert.ToInt32(Bleu, 2);


                }
            }
            image = ImageFinale;
        }

        //Méthode permettant de décoder une image dans une autre image
        public void Décodage()
        {
            char[] Rbis = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
            char[] Vbis = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
            char[] Bbis = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
            char[] Rbis2 = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
            char[] Vbis2 = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
            char[] Bbis2 = new char[8] { '0', '0', '0', '0', '0', '0', '0', '0' };
            Pixel[,] im2 = new Pixel[haut, large];
            for (int i = 0; i < haut; i++)
            {
                for (int j = 0; j < large; j++)
                {
                    im2[i, j] = new Pixel(0, 0, 0);
                }
            }
            for (int i = 0; i < haut; i++)
            {
                for (int j = 0; j < large; j++)
                {
                    string rouge = Convert.ToString(image[i, j].Red, 2);
                    string bleu = Convert.ToString(image[i, j].Blue, 2);
                    string vert = Convert.ToString(image[i, j].Green, 2);

                    int compteur = 0;
                    for (int u = rouge.Length - 1; u >= 0; u--)
                    {
                        Rbis[7 - compteur] = Convert.ToChar(rouge[u]);
                        compteur++;

                    }
                    compteur = 0;
                    for (int u = vert.Length - 1; u >= 0; u--)
                    {
                        Vbis[7 - compteur] = Convert.ToChar(vert[u]);
                        compteur++;
                    }
                    compteur = 0;
                    for (int u = bleu.Length - 1; u >= 0; u--)
                    {
                        Bbis[7 - compteur] = Convert.ToChar(bleu[u]);
                        compteur++;
                    }
                    for (int y = 0; y < 4; y++)
                    {
                        Rbis2[y] = Rbis[y + 4];
                        Bbis2[y] = Bbis[y + 4];
                        Vbis2[y] = Vbis[y + 4];
                    }
                    string Rouge = new string(Rbis2);
                    string Vert = new string(Vbis2);
                    string Bleu = new string(Bbis2);
                    im2[i, j].Red = (byte)Convert.ToInt32(Rouge, 2);
                    im2[i, j].Green = (byte)Convert.ToInt32(Vert, 2);
                    im2[i, j].Blue = (byte)Convert.ToInt32(Bleu, 2);
                }
            }
            image = im2;
        }
    }
}
