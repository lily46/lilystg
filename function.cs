using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DxLibDLL;

namespace DxlibCs
{
    //色々便利関数
    class function
    {
        //funcs
        static public void AlphaSet(int n = -1)
        {
            if (n < -1)
                n = 0;

            if (n != -1)
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_ALPHA, n);
            else
                DX.SetDrawBlendMode(DX.DX_BLENDMODE_NOBLEND, 0);


        }
    }
}
