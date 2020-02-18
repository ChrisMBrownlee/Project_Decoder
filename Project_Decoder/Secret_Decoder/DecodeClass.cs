using System.Drawing;
using System.Text;

namespace Secret_Decoder {
    class DecodeClass {
        //NOTE: X: 20 , Y: 390
        //VARS
        private int xPos;
        private int yPos;
        private char tempChar;
        private Bitmap bmp;
        private char stopChar = (char)4;
        //private char startChar = (char)2;
        private char[] startChar = { (char)1 , (char)2 , (char)1 , (char)2 , (char)1 };

        //CONSTRUCTOR
        public DecodeClass() {
            //construct
        }//end constructor

        //METHODS
        public StringBuilder Decoder(Bitmap bitmap, StringBuilder stringBuilder) {
            //NOTE: DESIGNATED STOP GLYPH - GLYPH: ¸ | CODE: U+00B8 | DECIMAL: 0184 | HTML: &cedil; | NUMBER: 0120
            //SET FOR GLOBAL
            bmp = bitmap;

            //GET START POS
            SetStart();

            //IF SETSTART RETURNS 0 , 0 SEND ERROR TO OUTPUT BOX
            if(xPos == 0 && yPos == 0) {
                stringBuilder.Clear();
                stringBuilder.Append("ERROR: THE IMAGE YOU DECODED DOESN'T HAVE A MESSAGE");
                return stringBuilder;
            }//end if

            //IF STRINGBUILDER HAS WORDS CLEAR
            if(stringBuilder.Length > 0) {
                stringBuilder.Clear();
            }//end if

            //GRAB FIRST LETTER
            stringBuilder.Append(GrabLetter());

            for(int index = 0; index <= 255; index++) {
                //GRAB NEXT POINT
                NextPoint();

                if(tempChar == stopChar) {
                    //IF CHAR IS STOP CHAR END STRING
                    return stringBuilder;
                } else {
                    //APPEND(STORE) GRABBED LETTERS
                    stringBuilder.Append(GrabLetter());
                } //end if                
            }//end for            

            return stringBuilder;
        }//end method

        private string GrabLetter() {
            //START NEW STRING BUILDER
            StringBuilder temp = new StringBuilder();

            //GRAB COLORS PER PIXEL
            Color col = bmp.GetPixel(xPos, yPos);

            //SPLIT COLORS
            int r = col.R;
            int g = col.G;
            int b = col.B;

            //STRIP THE HUNDREDS PLACE OFF
            int totalSubHundred = StripHundredsPlace(r + g + b);

            tempChar = (char)totalSubHundred;

            //RETURN CHAR VERSION OF THE INT            
            return $"{(char)totalSubHundred}";
        }//end method

        private void NextPoint() {
            //X POSITION PLUS 300
            xPos += 300;

            if(xPos >= bmp.Width) {
                xPos = (xPos - bmp.Width);
                //IF Y POSITION >= BITMAP HEIGHT MINUS ONE SET Y POSITION TO ZERO, ELSE ADD ONE TO THE POSITION
                yPos = yPos >= bmp.Height - 1 ? yPos = 0 : yPos += 1;
            }//end if
        }//end method

        private void SetStart() {
            int count = 0;

            for(int Y = 0; Y < bmp.Height; Y++) {                
                for(int X = 0; X < bmp.Width; X++) {
                    if(count == 5) {
                        xPos = X;
                        yPos = Y;
                        return;
                    }//end if

                    if(GetStart(X,Y) == startChar[count]) {
                        count++;
                    } else {
                        count = 0;
                    }//end if
                }//end for                
            }//end for
        }//end method

        private char GetStart(int x, int y) {
            //SET COLOR FROM X & Y IN LOOP
            Color col = bmp.GetPixel(x, y);

            //SET RGB
            int r = col.R;
            int g = col.G;
            int b = col.B;

            //STRIP HUNDREDS PLACE
            int totalSubHundred = StripHundredsPlace(r + g + b);

            return (char)totalSubHundred;
        }//end method

        private int StripHundredsPlace(int num) {
            if(num < 100) {
                return num;
            } else {
                int remove = num.ToString().Length;
                string str = "";

                //GRAB LAST TWO NUMBERS ONLY
                str += num.ToString()[remove - 2];
                str += num.ToString()[remove - 1];
                
                return int.Parse(str);
            }//end if
        }//end method

    }//end class
}//end namespace
