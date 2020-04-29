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
            bool fin = true;
            while(fin)
            {
                Console.WriteLine("\n\n           Menu"
                            + "\n\n 1/ Charger un fichier"
                            + "\n 2/ Utiliser coco"
                            + "\n 3/ Créer un Fractale"
                            + "\n 4/ Générer un QrCode"
                            +"\n 5/ Exit");

                Console.Write("\n Entrez le programme souhaité  > ");
                int choix = Convert.ToInt32(Console.ReadLine());
                string chemin = "";
                if (choix == 1)
                {
                    Console.Write("\n\n Entrez le chemin du fichier souhaité > ");
                    chemin = Console.ReadLine();
                }
                else if (choix == 2) { chemin = "coco.bmp"; }
                switch (choix)
                {
                    case 1:
                    case 2:
                        bool fin2 = true;
                        MyImage image = new MyImage(chemin);
                        while (fin2)
                        {
                            Console.WriteLine("\n\n        Menu Image"
                                            + "\n\n 1/ Sauvegarder le fichier fichier"
                                            + "\n 2/ Convertir en Noir et blanc"
                                            + "\n 3/ Agrandire l'image"
                                            + "\n 4/ Réduire l'image"
                                            + "\n 5/ Effet Miroir"
                                            + "\n 6/ Rotation"
                                            + "\n 7/ Flouter"
                                            + "\n 8/ Detection des bords"
                                            + "\n 9/ Renforcement des bords"
                                            + "\n 10/ Repoussage des bords"
                                            + "\n 11/ Créer l'histogramme"
                                            + "\n 12/ Cacher une image dans une image"
                                            + "\n 13/ Décoder une image"
                                            + "\n 14/ Exit");

                            Console.Write("\n Entrez le programme souhaité  > ");
                            int choix2 = Convert.ToInt32(Console.ReadLine());
                            switch (choix2)
                            {
                                case 1:
                                    Console.Write("\n\n Entrez le chemin souhaitée pour sauvegarder la fractale (n'oubliez pas le '.bmp') > ");
                                    chemin = Console.ReadLine();
                                    image.From_Image_To_File(chemin);
                                    Console.WriteLine("\n\n fichier enregistré");
                                    break;

                                case 2:
                                    image.ConvertToGris();
                                    Console.WriteLine("\n\n image convertie en nuance de gris");
                                    break;

                                case 3:
                                    Console.Write("\n\n Entrez le facteur d'agrandissement > ");
                                    int fact = Convert.ToInt32(Console.ReadLine());
                                    image.Agrandissement(fact);
                                    Console.WriteLine("\n\n Image Agrandie");
                                    break;

                                case 4:
                                    Console.Write("\n\n Entrez le facteur de réduction > ");
                                    fact = Convert.ToInt32(Console.ReadLine());
                                    image.Agrandissement(fact);
                                    Console.WriteLine("\n\n Image Agrandie");
                                    break;

                                case 5:
                                    Console.Write("\n\n Entrez H pour un miroir horizontal et V pour un miroir Vertical > ");
                                    char caract = char.ToUpper(Convert.ToChar(Console.ReadLine()));
                                    image.Miroir(caract);
                                    Console.WriteLine("\n\n Effet Appliqué");
                                    break;

                                case 6:
                                    Console.Write("\n\n Entrez le'angle de rotation > ");
                                    fact = Convert.ToInt32(Console.ReadLine());
                                    image.Rotation(fact);
                                    Console.WriteLine("\n\n Rotation effectuée");
                                    break;

                                case 7:
                                    image.Flou();
                                    Console.WriteLine("\n\n Effet Appliqué");
                                    break;

                                case 8:
                                    image.DetecBord();
                                    Console.WriteLine("\n\n Effet Appliqué");
                                    break;

                                case 9:
                                    image.RenfocementBords();
                                    Console.WriteLine("\n\n Effet Appliqué");
                                    break;

                                case 10:
                                    image.Repoussage();
                                    Console.WriteLine("\n\n Effet Appliqué");
                                    break;

                                case 11:
                                    Console.Write("\n\n Entrez le chemin souhaitée pour sauvegarder l'histogramme (n'oubliez pas le '.bmp') > ");
                                    chemin = Console.ReadLine();
                                    image.Histogramme();
                                    image.From_Image_To_File(chemin);
                                    break;

                                case 12:
                                    Console.Write("\n\n Entrez le chemin de l'image à cacher > ");
                                    chemin = Console.ReadLine();
                                    image.ImageDansImage(chemin);
                                    Console.WriteLine("\n\n Image cachée");
                                    break;

                                case 13:
                                    image.Décodage();
                                    Console.WriteLine("\n\n Image décodée");
                                    break;

                                case 14:
                                    fin2 = false;
                                    break;




                            }
                        }
                        break;
                    case 3:
                        Console.Write("\n\n Entrez la largeur souhaitée pour la fractale > ");
                        int large = Convert.ToInt32(Console.ReadLine());
                        Console.Write("\n\n Entrez la hauteur souhaitée pour la fractale > ");
                        int haut = Convert.ToInt32(Console.ReadLine());
                        MyImage fract = new MyImage(large, haut);
                        Console.Write("\n\n Entrez le chemin souhaitée pour sauvegarder la fractale (n'oubliez pas le '.bmp') > ");
                        chemin = Console.ReadLine();
                        fract.From_Image_To_File(chemin);
                        break;

                    case 4:
                        Console.Write("\n\n Entre le message que vous voulez coder dans le QRcode > ");
                        string message = Console.ReadLine();
                        Qrcode qr = new Qrcode(message);
                        if (qr.Version != 0)
                        {
                            Console.Write("\n\n Entrez le chemin souhaitée pour sauvegarder le QRcode (n'oubliez pas le '.bmp') > ");
                            chemin = Console.ReadLine();
                            qr.generation(chemin);
                        }
                        break;

                    case 5:
                        fin = false;
                        break;
                }
            }
            
            Console.ReadKey();
        }
    }
}
