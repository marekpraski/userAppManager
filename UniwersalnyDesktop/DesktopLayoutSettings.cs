using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniwersalnyDesktop
{
    
    class DesktopLayoutSettings
    {
        public enum ButtonType { square, rectangular }
        public enum GroupboxType { squareButtons, rectangularButtons}

        public static int numberOfSquareButtonsInOneBlock = 4;

        //rozmiary buttonów
        public static int squareButtonWidth = 110;
        public static int squareButtonHeigth = 90;
        public static int rectangularButtonWidth = 350;
        public static int rectangularButtonHeigth = 40;

        //rozlokowanie buttonów w groupboxie
        public static int horizontalButtonPadding = 10;
        public static int verticalButtonPadding = 10;

        //rozlokowanie groupboxów
        public static int firstGroupboxVerticalLocation = 10;
        public static int horizontalGroupboxPadding = 10;
        public static int verticalGroupboxPadding = 0;

        //korekta do wymiarów desktopu, żeby uwzględnić grubość obrzeży
        public static int desktopFormHorizontalPadding = 30;
        public static int desktopFormVerticalPadding = 40;
    }
}
