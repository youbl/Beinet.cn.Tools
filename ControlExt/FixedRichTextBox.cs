using System;
using System.Windows.Forms;

namespace Beinet.cn.Tools.ControlExt
{
    /// <summary>
    /// 本类，用于修正RichTextBox的一个BUG，就是自动选中整个单词的问题
    /// 如：\d+(\d+)\d+，鼠标从左括号后面的\d开始选择时，会自动选中前面的左括号
    /// 重写OnHandleCreated方法后，就不会了
    /// 另一个方法，在Form的OnLoad事件里设置这个属性也是ok的
    /// </summary>
    public class FixedRichTextBox : RichTextBox
    {
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e); 
            if (!AutoWordSelection) 
            {
                AutoWordSelection = true; 
                AutoWordSelection = false; 
            }
        }
    }
}
