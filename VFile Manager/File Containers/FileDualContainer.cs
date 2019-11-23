using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VFile_Manager.File_Containers
{
    static class FileDualContainer
    {
        private static FileContainer FC_Left = new FileContainer(), FC_Right = new FileContainer();
        public static FileContainer ChooseContainer(FileOperator.Side s)
        {
            if (s == FileOperator.Side.Left)
                return FC_Left;
            else
                return FC_Right;
        }
    }
}
