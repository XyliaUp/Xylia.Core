using System.Drawing;

namespace Xylia.Extension;
public static partial class GraphicsEnetension
{
    public static SizeF MeasureString(this char Txt, Font Font) => Txt.ToString().MeasureString(Font);

    public static SizeF MeasureString(this string Txt, Font Font)
    {
        using Graphics g = Graphics.FromHwnd(IntPtr.Zero);

        var sf = StringFormat.GenericTypographic;
        sf.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

        var Result = g.MeasureString(Txt, Font ?? SystemFonts.DefaultFont, PointF.Empty, sf);

        return Result;
    }
}
