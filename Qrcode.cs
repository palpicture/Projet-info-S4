using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_info_S4
{
    class Qrcode
    {

        int version = 0;
        byte[] data;
        static string masque = "111011111000100";

        public Qrcode (string mot)
        {
            int taille = mot.Length;
            if(mot.Length < 26)
            {
                version = 1;
                data = new byte[208];
                data[0] = 0;
                data[1] = 0;
                data[2] = 1;
                data[3] = 0;
                string binary = Convert.ToString(taille, 2);
                for(int i = 0; i<9;i++)
                {
                    if (binary.Length > i) { data[12 - i] = (byte)binary[i]; }
                    else { data[12 - i] = 0; }
                }
                int index;
                int temp;
                int compt = 0;
                for(index=0;index<taille-1;index+=2)
                {
                    temp = ConvertToInt(mot[index]) * 45 + ConvertToInt(mot[index + 1]);
                    binary = Convert.ToString(temp, 2);
                    for (int i = 0; i < 11; i++)
                    {
                        if (binary.Length > i) { data[compt*11 + 21 - i] = (byte)binary[binary.Length -1 -i]; }
                        else { data[compt*11 +21-i] = 0; }
                    }
                    compt++;
                }
                compt--;
                index--;
                compt = compt * 11 + 21;
                if (taille%2==1)
                {
                    temp = ConvertToInt(mot[index + 1]);
                    binary = Convert.ToString(temp, 2);
                    for (int i = 0; i < 6; i++)
                    {
                        if (binary.Length > i) { data[compt + 6 - i] = (byte)binary[binary.Length - 1 - i]; }
                        else { data[compt  +6 - i] = 0; }
                    }
                    compt += 7;
                }
                index = 0;
                while(compt<152 && index<4)
                {
                    data[compt] = 0;
                    compt++;
                    index++;
                }
                while(compt<152&& compt%8==1)
                {
                    data[compt] = 0;
                    compt++;
                }
                byte[] fin = { 1, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1 };
                index = 0;
                while(compt<152)
                {
                    data[compt] = fin[index];
                    compt++;
                    index++;
                    if (index == 16) { index = 0; }
                }
                Encoding u8 = Encoding.UTF8;
                int iBC = u8.GetByteCount(mot);
                byte[] bytesa = u8.GetBytes(mot);
                byte[] result = ReedSolomonAlgorithm.Encode(bytesa, 7, ErrorCorrectionCodeType.QRCode);
                foreach (byte val in result)
                {
                    binary = Convert.ToString(val, 2);
                    for (int i = 0; i < binary.Length; i++)
                    {
                        if (binary.Length > i) { data[compt + 7 - i] = (byte)binary[binary.Length - 1 - i]; }
                        else { data[compt + 7 - i] = 0; }
                    }
                    compt += 8;
                }

            }
            else if (mot.Length <48)
            {
                version = 2;
                data = new byte[368];
                data[0] = 0;
                data[1] = 0;
                data[2] = 1;
                data[3] = 0;
                string binary = Convert.ToString(taille, 2);
                for (int i = 0; i < 9; i++)
                {
                    if (binary.Length > i) { data[12 - i] = (byte)binary[i]; }
                    else { data[12 - i] = 0; }
                }
                int index;
                int temp;
                int compt = 0;
                for (index = 0; index < taille - 1; index += 2)
                {
                    temp = ConvertToInt(mot[index]) * 45 + ConvertToInt(mot[index + 1]);
                    binary = Convert.ToString(temp, 2);
                    for (int i = 0; i < 11; i++)
                    {
                        if (binary.Length > i) { data[compt * 11 + 21 - i] = (byte)binary[binary.Length - 1 - i]; }
                        else { data[compt * 11 + 21 - i] = 0; }
                    }
                    compt++;
                }
                compt--;
                index--;
                compt = compt * 11 + 21;
                if (taille % 2 == 1)
                {
                    temp = ConvertToInt(mot[index + 1]);
                    binary = Convert.ToString(temp, 2);
                    for (int i = 0; i < 6; i++)
                    {
                        if (binary.Length > i) { data[compt + 6 - i] = (byte)binary[binary.Length - 1 - i]; }
                        else { data[compt + 6 - i] = 0; }
                    }
                    compt += 7;
                }
                index = 0;
                while (compt < 272 && index < 4)
                {
                    data[compt] = 0;
                    compt++;
                    index++;
                }
                while (compt < 272 && compt % 8 == 1)
                {
                    data[compt] = 0;
                    compt++;
                }
                byte[] fin = { 1, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1 };
                index = 0;
                while (compt < 272)
                {
                    data[compt] = fin[index];
                    compt++;
                    index++;
                    if (index == 16) { index = 0; }
                }
                Encoding u8 = Encoding.UTF8;
                int iBC = u8.GetByteCount(mot);
                byte[] bytesa = u8.GetBytes(mot);
                byte[] result = ReedSolomonAlgorithm.Encode(bytesa, 10, ErrorCorrectionCodeType.QRCode);
                foreach (byte val in result)
                {
                    binary = Convert.ToString(val, 2);
                    for (int i = 0; i < binary.Length; i++)
                    {
                        if (binary.Length > i) { data[compt + 7 - i] = (byte)binary[binary.Length - 1 - i]; }
                        else { data[compt + 7 - i] = 0; }
                    }
                    compt += 8;
                }
            }
            else { Console.WriteLine("erreur, chaine de carractere trop longue"); }
        }

        public int Version { get { return version; } }


        public void generation(string nom)
        {
            switch(this.version)
            {
                case (1):
                    Pixel[,] code = new Pixel[21, 21];
                    //tracage de tous les separateurs
                    for(int i =0;i<7;i++)
                    {
                        code[i, 0] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[0 , i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[6, i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[i, 6] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[20 - i, 0] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[20, i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[14, i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[20 - i, 6] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[i, 20] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[0, 20 - i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[6,20 - i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[i, 14] = new Pixel((byte)0, (byte)0, (byte)0);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        code[1 + i, 1] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[1 , 1 +  i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[5,1+ i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[1+i, 5] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[19 - i, 1] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[19,1+ i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[15,1+ i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[19 - i, 5] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[1+i, 19] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[1, 19 - i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[5, 19 - i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[1+i, 15] = new Pixel((byte)255, (byte)255, (byte)255);
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        code[2+i, 2] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[2,2+ i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[4,2+ i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[2+i, 4] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[18 - i, 2] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[18,2 + i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[16, 2+i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[18 - i, 4] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[2+i, 18] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[2, 18 - i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[4, 18 - i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[2+i, 16] = new Pixel((byte)0, (byte)0, (byte)0);
                    }
                    code[3 , 3] = new Pixel((byte)0, (byte)0, (byte)0);
                    code[17 , 3] = new Pixel((byte)0, (byte)0, (byte)0);
                    code[ 3, 17] = new Pixel((byte)0, (byte)0, (byte)0);

                    //patern de séparation
                    for (int i = 0; i < 8; i++)
                    {
                        code[7, i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[i, 7] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[13, i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[20 - i, 7] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[i, 13] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[7, 20 - i] = new Pixel((byte)255, (byte)255, (byte)255);
                    }

                    //motifs de synchronisation
                    for(int i =8;i<13;i++)
                    {
                        if (i % 2 == 0) 
                        {
                            code[i, 6] = new Pixel((byte)0, (byte)0, (byte)0);
                            code[6, i] = new Pixel((byte)0, (byte)0, (byte)0);
                        }
                        else
                        {
                            code[6, i] = new Pixel((byte)255, (byte)255, (byte)255);
                            code[i, 6] = new Pixel((byte)255, (byte)255, (byte)255);
                        }
                    }

                    //implantation du masque
                    for(int i = 0;i<15;i++)
                    {
                        if (masque[i]==0)
                        {
                            if (i < 6) { code[8,i] = new Pixel((byte)0, (byte)0, (byte)0); }
                            else if(i<8) { code[8, 1 + i] = new Pixel((byte)0, (byte)0, (byte)0); }
                            else if(i==8) { code[7, 8] = new Pixel((byte)0, (byte)0, (byte)0); }
                            else { code[14-i, 8] = new Pixel((byte)0, (byte)0, (byte)0); };
                            if (i < 7) { code[20-i, 8] = new Pixel((byte)0, (byte)0, (byte)0); }
                            else { code[8, 6+i] = new Pixel((byte)0, (byte)0, (byte)0); }
                        }
                        else
                        {
                            if (i < 6) { code[8, i] = new Pixel((byte)255, (byte)255, (byte)255); }
                            else if (i < 8) { code[8, 1 + i] = new Pixel((byte)255, (byte)255, (byte)255); }
                            else if (i == 8) { code[7, 8] = new Pixel((byte)255, (byte)255, (byte)255); }
                            else { code[14 - i, 8] = new Pixel((byte)255, (byte)255, (byte)255); };
                            if (i < 7) { code[20 - i, 8] = new Pixel((byte)255, (byte)255, (byte)255); }
                            else { code[8, 6 + i] = new Pixel((byte)255, (byte)255, (byte)255); }
                        }
                    }

                    //dark module
                    code[13, 8] = new Pixel((byte)255, (byte)255, (byte)255);

                    //ecriture du code
                    bool montee = true;
                    int index = 0;
                    for (int x =20;x>0;x-=2)
                    {
                        if (x == 6) { x--; }
                        if (montee)
                        {
                            for (int y = 20; y >= 0; y --)
                            {

                                if (code[y, x] == null) 
                                {
                                    if (data[index] == (byte)0) { code[y, x] = new Pixel((byte)255, (byte)255, (byte)255); }
                                    else { code[y, x] = new Pixel((byte)0, (byte)0, (byte)0); }
                                    index++;
                                }
                                if (code[y, x - 1] == null)
                                {
                                    if (data[index] == (byte)0) { code[y, x - 1] = new Pixel((byte)255, (byte)255, (byte)255); }
                                    else { code[y, x - 1] = new Pixel((byte)0, (byte)0, (byte)0); }
                                    index++;
                                }
                            }
                            montee = false;
                        }
                        else 
                        {
                            for (int y = 0; y < 21; y ++)
                            {
                                if (code[y, x] == null)
                                {
                                    if (data[index] == (byte)0) { code[y, x] = new Pixel((byte)255, (byte)255, (byte)255); }
                                    else { code[y, x] = new Pixel((byte)0, (byte)0, (byte)0); }
                                    index++;
                                }
                                if (code[y, x - 1] == null)
                                {
                                    if (data[index] == (byte)0) { code[y, x - 1] = new Pixel((byte)255, (byte)255, (byte)255); }
                                    else { code[y, x - 1] = new Pixel((byte)0, (byte)0, (byte)0); }
                                    index++;
                                }
                            }
                            montee = true;
                        }

                    }
                    MyImage QR = new MyImage(21, code);
                    QR.Agrandissement(4);
                    QR.From_Image_To_File(nom);
                    break;

                case (2):
                    code = new Pixel[25, 25];
                    //tracage de tous les separateurs
                    for (int i = 0; i < 7; i++)
                    {
                        code[i, 0] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[0, i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[6, i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[i, 6] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[24 - i, 0] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[24, i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[18, i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[24 - i, 6] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[i, 24] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[0, 24 - i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[6, 24 - i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[i, 18] = new Pixel((byte)0, (byte)0, (byte)0);
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        code[1 + i, 1] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[1, 1 + i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[5, 1 + i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[1 + i, 5] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[23 - i, 1] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[23, 1 + i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[15, 1 + i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[23 - i, 5] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[1 + i, 23] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[1, 23 - i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[5, 23 - i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[1 + i, 15] = new Pixel((byte)255, (byte)255, (byte)255);
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        code[2 + i, 2] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[2, 2 + i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[4, 2 + i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[2 + i, 4] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[22 - i, 2] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[22, 2 + i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[20, 2 + i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[22 - i, 4] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[2 + i, 22] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[2, 22 - i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[4, 22 - i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[2 + i, 20] = new Pixel((byte)0, (byte)0, (byte)0);
                    }
                    code[3, 3] = new Pixel((byte)0, (byte)0, (byte)0);
                    code[21, 3] = new Pixel((byte)0, (byte)0, (byte)0);
                    code[3, 21] = new Pixel((byte)0, (byte)0, (byte)0);

                    //patern de séparation
                    for (int i = 0; i < 8; i++)
                    {
                        code[7, i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[i, 7] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[13, i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[24 - i, 7] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[i, 13] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[7, 24 - i] = new Pixel((byte)255, (byte)255, (byte)255);
                    }

                    //motifs de synchronisation
                    for (int i = 8; i < 17; i++)
                    {
                        if (i % 2 == 0)
                        {
                            code[i, 6] = new Pixel((byte)0, (byte)0, (byte)0);
                            code[6, i] = new Pixel((byte)0, (byte)0, (byte)0);
                        }
                        else
                        {
                            code[6, i] = new Pixel((byte)255, (byte)255, (byte)255);
                            code[i, 6] = new Pixel((byte)255, (byte)255, (byte)255);
                        }
                    }

                    //implantation du masque
                    for (int i = 0; i < 15; i++)
                    {
                        if (masque[i] == 0)
                        {
                            if (i < 6) { code[8, i] = new Pixel((byte)0, (byte)0, (byte)0); }
                            else if (i < 8) { code[8, 1 + i] = new Pixel((byte)0, (byte)0, (byte)0); }
                            else if (i == 8) { code[7, 8] = new Pixel((byte)0, (byte)0, (byte)0); }
                            else { code[14 - i, 8] = new Pixel((byte)0, (byte)0, (byte)0); }
                            if (i < 7) { code[24 - i, 8] = new Pixel((byte)0, (byte)0, (byte)0); }
                            else { code[8, 10 + i] = new Pixel((byte)0, (byte)0, (byte)0); }
                        }
                        else
                        {
                            if (i < 6) { code[8, i] = new Pixel((byte)255, (byte)255, (byte)255); }
                            else if (i < 8) { code[8, 1 + i] = new Pixel((byte)255, (byte)255, (byte)255); }
                            else if (i == 8) { code[7, 8] = new Pixel((byte)255, (byte)255, (byte)255); }
                            else { code[14 - i, 8] = new Pixel((byte)255, (byte)255, (byte)255); }
                            if (i < 7) { code[24 - i, 8] = new Pixel((byte)255, (byte)255, (byte)255); }
                            else { code[8, 10 + i] = new Pixel((byte)255, (byte)255, (byte)255); }
                        }
                    }

                    //dark module
                    code[8, 13] = new Pixel((byte)255, (byte)255, (byte)255);

                    // module d'alignement
                    for (int i = 0; i < 5; i++)
                    {
                        code[16+i, 16] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[16,16+ i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[20, 16+i] = new Pixel((byte)0, (byte)0, (byte)0);
                        code[16+i, 20] = new Pixel((byte)0, (byte)0, (byte)0);
                    }
                    for (int i = 0; i < 3; i++)
                    {
                        code[17 + i, 17] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[17, 17 + i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[19, 17 + i] = new Pixel((byte)255, (byte)255, (byte)255);
                        code[17 + i, 19] = new Pixel((byte)255, (byte)255, (byte)255);
                    }
                    code[18, 18] = new Pixel((byte)0, (byte)0, (byte)0);

                    //ecriture du code
                    montee = true;
                    index = 0;
                    for (int x = 24; x > 0; x -= 2)
                    {
                        if (x == 6) { x--; }
                        if (montee)
                        {
                            for (int y = 24; y >= 0; y--)
                            {
                                if (code[y, x] == null)
                                {
                                    if (data[index] == (byte)0) { code[y, x] = new Pixel((byte)255, (byte)255, (byte)255); }
                                    else { code[y, x] = new Pixel((byte)0, (byte)0, (byte)0); }
                                    index++;
                                }
                                if (code[y, x - 1] == null)
                                {
                                    if (data[index] == (byte)0) { code[y, x - 1] = new Pixel((byte)255, (byte)255, (byte)255); }
                                    else { code[y, x - 1] = new Pixel((byte)0, (byte)0, (byte)0); }
                                    index++;
                                }
                            }
                            montee = false;
                        }
                        else
                        {
                            for (int y = 0; y < 25; y++)
                            {
                                if (code[y, x] == null)
                                {
                                    if (data[index] == (byte)0) { code[y, x] = new Pixel((byte)255, (byte)255, (byte)255); }
                                    else { code[y, x] = new Pixel((byte)0, (byte)0, (byte)0); }
                                    index++;
                                }
                                if (code[y, x - 1] == null)
                                {
                                    if (data[index] == (byte)0) { code[y, x - 1] = new Pixel((byte)255, (byte)255, (byte)255); }
                                    else { code[y, x - 1] = new Pixel((byte)0, (byte)0, (byte)0); }
                                    index++;
                                }
                            }
                            montee = true;
                        }

                    }
                    QR = new MyImage(25, code);
                    QR.Agrandissement(4);
                    QR.From_Image_To_File(nom);
                    break;
            }
            
        }

        private int ConvertToInt (char lettre)
        {
            int res = 36;
            lettre = Char.ToUpper(lettre);
            switch(lettre)
            {
                case ('0'):
                    res = 0;
                    break;
                case ('1'):
                    res = 1;
                    break;
                case ('2'):
                    res = 2;
                    break;
                case ('3'):
                    res = 3;
                    break;
                case ('4'):
                    res = 4;
                    break;
                case ('5'):
                    res = 5;
                    break;
                case ('6'):
                    res = 6;
                    break;
                case ('7'):
                    res = 7;
                    break;
                case ('8'):
                    res = 8;
                    break;
                case ('9'):
                    res = 9;
                    break;
                case ('A'):
                    res = 10;
                    break;
                case ('B'):
                    res = 11;
                    break;
                case ('C'):
                    res = 12;
                    break;
                case ('D'):
                    res = 13;
                    break;
                case ('E'):
                    res = 14;
                    break;
                case ('F'):
                    res = 15;
                    break;
                case ('G'):
                    res = 16;
                    break;
                case ('H'):
                    res = 17;
                    break;
                case ('I'):
                    res = 18;
                    break;
                case ('J'):
                    res = 19;
                    break;
                case ('K'):
                    res = 20;
                    break;
                case ('L'):
                    res = 21;
                    break;
                case ('M'):
                    res = 22;
                    break;
                case ('N'):
                    res = 23;
                    break;
                case ('O'):
                    res = 24;
                    break;
                case ('P'):
                    res = 25;
                    break;
                case ('Q'):
                    res = 26;
                    break;
                case ('R'):
                    res = 27;
                    break;
                case ('S'):
                    res = 28;
                    break;
                case ('T'):
                    res = 29;
                    break;
                case ('U'):
                    res = 30;
                    break;
                case ('V'):
                    res = 31;
                    break;
                case ('W'):
                    res = 32;
                    break;
                case ('X'):
                    res = 33;
                    break;
                case ('Y'):
                    res = 34;
                    break;
                case ('Z'):
                    res = 35;
                    break;
                case (' '):
                    res = 36;
                    break;
                case ('$'):
                    res = 37;
                    break;
                case ('%'):
                    res = 38;
                    break;
                case ('*'):
                    res = 39;
                    break;
                case ('+'):
                    res = 40;
                    break;
                case ('-'):
                    res = 41;
                    break;
                case ('.'):
                    res = 42;
                    break;
                case ('/'):
                    res = 43;
                    break;
                case (':'):
                    res = 44;
                    break;
                default:
                    res = 36;
                    break;
            }
            return res;
        }
    }
}
