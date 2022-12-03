using iTextSharp.text;
using iTextSharp.text.pdf;

namespace VOR.Utils
{
    public enum AnnotationType
    {
        Text, BD, BD_AR, CodeBar_128B, CodeBar_QR, CodeBar_417, Image
    }

    public class Annotations
    {
        public AnnotationType Type { get; set; }
        public string Text { get; set; }
        public BaseFont FontType { get; set; }
        public float FontSize { get; set; }
        public BaseColor FontColor { get; set; }
        public int FontAlign { get; set; }

        public float Width { get; set; }
        public float Height { get; set; }
        public float Degree { get; set; }
    }

    public class Commentaire
    {
        public string Name { get; set; }
        public Rectangle Rect { get; set; }
        public float Height { get; set; }
        public int NumPage { get; set; }
    }
}
