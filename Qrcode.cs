using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_info_S4
{
    class Qrcode
    {

        int version;
        byte[] data;
        string masque = "111011111000100";

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
                        else { data[compt+21-i] = 0; }
                    }
                    compt++;
                }
                compt--;
                compt = compt * 11 + 21;
                if (taille%2==1)
                {
                    temp = ConvertToInt(mot[index + 1]);
                    binary = Convert.ToString(temp, 2);
                    for (int i = 0; i < 6; i++)
                    {
                        if (binary.Length > i) { data[compt + 6 - i] = (byte)binary[binary.Length - 1 - i]; }
                        else { data[compt*11 + 21 - i] = 0; }
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
                    for (int i = 0; i < ; i++)
                    {
                        if (binary.Length > i) { data[compt + 6 - i] = (byte)binary[binary.Length - 1 - i]; }
                        else { data[compt * 11 + 21 - i] = 0; }
                    }
                    compt += 7;
                }

            }
            else if (mot.Length <48)
            {
                version = 2;
                data = new byte[368];
            }
            else { Console.WriteLine("erreur, chaine de carractere trop longue"); }
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
