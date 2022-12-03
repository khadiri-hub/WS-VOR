using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Collections;
using System.Web.Configuration;
using iTextSharp.text.pdf.parser;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Reflection;

namespace VOR.Utils
{
    public class PdfHelper
    {
        #region constants

        public const int BADGE_OF_PAGE = 4;

        #endregion

        public static void GenerateBadgePelerinDocument(string templatePath, IList<PelerinBadge> lstPelerinBadge, Stream OutputStream)
        {
            using (Document document = new Document())
            {
                using (PdfSmartCopy copy = new PdfSmartCopy(
                  document, OutputStream))
                {
                    document.Open();

                    if (lstPelerinBadge.Count > 1)
                    {
                        for (int i = 0; i < lstPelerinBadge.Count; i++)
                        {
                            IList<PelerinBadge> lstPelerinPerPage = lstPelerinBadge.Skip(i * BADGE_OF_PAGE).Take(BADGE_OF_PAGE).ToList();
                            if (lstPelerinPerPage.Count > 0)
                            {
                                PdfReader reader = new PdfReader(GenerateBadgePelerin(templatePath, lstPelerinPerPage));
                                for (int j = 1; j <= reader.NumberOfPages; j++)
                                    copy.AddPage(copy.GetImportedPage(reader, j));
                            }
                            else
                                break;
                        }
                    }
                    else
                    {
                        PdfReader reader = new PdfReader(GenerateBadgePelerin(templatePath, lstPelerinBadge));
                        for (int j = 1; j <= reader.NumberOfPages; j++)
                            copy.AddPage(copy.GetImportedPage(reader, j));
                    }
                }
            }
        }

        public static byte[] GenerateBadgePelerin(string templatePath, IList<PelerinBadge> lstPelerinBadge)
        {
            Hashtable htValue = new Hashtable();

            for (int i = 0; i < lstPelerinBadge.Count; i++)
            {
                PelerinBadge pelerin = lstPelerinBadge[i];

                int index = i + 1;
                htValue.Add("NOM_PRENOM_" + index, string.Format("{0} {1}", pelerin.Nom, pelerin.Prenom));

                if (pelerin.Photo != null)
                {
                    System.Drawing.Image photo = ScaleImage(byteArrayToImage(pelerin.Photo), 87, 105);
                    photo.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                    htValue.Add("P-" + index, photo);
                }


                if (pelerin.Visa != null)
                {
                    System.Drawing.Image badge = ScaleImage(byteArrayToImage(pelerin.Visa), 388, 260);
                    badge.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                    htValue.Add("V-" + index, badge);
                }

                htValue.Add("HOTEL_MAKKAH_" + index, pelerin.HotelMakkah);
                htValue.Add("HOTEL_MEDINE_" + index, pelerin.HotelMedine);

                if (pelerin.PelerinsMakkah != null)
                {
                    for (int j = 0; j < pelerin.PelerinsMakkah.Count; j++)
                    {
                        htValue.Add(string.Format("ACC_MAKKAH_{0}_{1}", index, (j + 1)), string.Format("{0} {1}", pelerin.PelerinsMakkah[j].NomFrancais, pelerin.PelerinsMakkah[j].PrenomFrancais));
                    }
                }

                if (pelerin.PelerinsMedine != null)
                {
                    for (int k = 0; k < pelerin.PelerinsMedine.Count; k++)
                    {
                        htValue.Add(string.Format("ACC_MEDINE_{0}_{1}", index, (k + 1)), string.Format("{0} {1}", pelerin.PelerinsMedine[k].NomFrancais, pelerin.PelerinsMedine[k].PrenomFrancais));
                    }
                }

                htValue.Add("CHAMBRE_" + index, pelerin.TypeChambre.ToString());
            }

            return GenerateDocumentWithAnnotations(templatePath, htValue);
        }

        private static System.Drawing.Image DrawText(System.Drawing.Font font, string text)
        {
            System.Drawing.PointF firstLocation = new System.Drawing.PointF(10f, 10f);
            System.Drawing.PointF secondLocation = new System.Drawing.PointF(10f, 50f);


            string fileName = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), string.Format("{0}{1}", ConfigurationManager.AppSettings["IMAGE_FOLDER"], "background.png"));
            System.Drawing.Bitmap bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(fileName);//load the image file

            bitmap = (System.Drawing.Bitmap)ScaleImage(bitmap, 110, 18);

            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bitmap))
            {
                //create a brush for the text
                System.Drawing.Brush textBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                // Set the TextRenderingHint property.
                graphics.TextRenderingHint =
                    System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;

                // Set the text contrast to a high-contrast setting.
                graphics.TextContrast = 0;

                //System.Drawing.Font font2 = new System.Drawing.Font("Segoe WP", 14);
                System.Drawing.StringFormat format = new System.Drawing.StringFormat(System.Drawing.StringFormatFlags.DirectionRightToLeft);
                //graphics.DrawString(text, SystemFonts.DefaultFont, Brushes.Black, 110f, 0, format);
                graphics.DrawString(text, font, textBrush, 110f, 0, format);


                graphics.Save();

                // textBrush.Dispose();
                graphics.Dispose();
                // bitmap.Dispose();
            }

            return bitmap;
        }


        public static System.Drawing.Image ScaleImage(System.Drawing.Image image, int newWidth, int newHeight)
        {
            var newImage = new System.Drawing.Bitmap(newWidth, newHeight);

            using (var graphics = System.Drawing.Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        public static System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }

        private static byte[] GenerateDocumentWithAnnotations(string templatePath, Hashtable annotationsValues)
        {
            PdfReader reader = new PdfReader(templatePath);
            IList<Commentaire> coms = PdfHelper.ChangeMemo(templatePath);
            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                PdfDictionary pagedic = reader.GetPageN(i);
                pagedic.Remove(PdfName.ANNOTS);
            }
            try
            {
                //return AddTextTasyeer(coms, annotationsValues, reader).ToArray();
                return AddText(coms, annotationsValues, reader).ToArray();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private static Image AddBar(PdfContentByte cb, string text)
        {
            Image code128 = null;
            if (!string.IsNullOrEmpty(text))
            {
                Barcode128 code = new Barcode128();
                code.BarHeight = 35;
                code.X = 1.2f;
                code.Size = 11;
                code.Code = text;

                //code.CodeType = Barcode.CODE128_UCC;

                code128 = code.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
            }

            return code128;
        }

        public static MemoryStream AddTextTasyeer(IList<Commentaire> coms, Hashtable htValue, PdfReader reader)
        {
            BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                //
                // PDFStamper is the class we use from iTextSharp to alter an existing PDF.
                //

                PdfStamper pdfStamper = new PdfStamper(reader, memoryStream);

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfContentByte overContent = pdfStamper.GetOverContent(i);
                    overContent.SaveState();

                    var lstTempCom = coms.Where(n => n.NumPage == i).Select(n => n).ToList();

                    #region Annotations Text FR

                    overContent.BeginText();

                    foreach (Commentaire com in lstTempCom)
                    {
                        Annotations annotation = ParserMemo(com.Name);

                        if (annotation.Type == AnnotationType.BD)
                        {
                            overContent.SetFontAndSize(annotation.FontType, annotation.FontSize);
                            //overContent.SetTextMatrix(com.Rect.Left, com.Rect.Bottom + 11f);
                            overContent.SetColorFill(annotation.FontColor);

                            if (htValue.ContainsKey(annotation.Text))
                            {
                                if (htValue[annotation.Text] != null)
                                {
                                    overContent.ShowTextAligned(annotation.FontAlign, htValue[annotation.Text].ToString(), com.Rect.Left, com.Rect.Bottom + 11f, 270);
                                }
                            }
                        }
                        else
                        {
                            overContent.SetFontAndSize(annotation.FontType, annotation.FontSize);
                            //overContent.SetTextMatrix(com.Rect.Left, com.Rect.Bottom + 11f);
                            overContent.SetColorFill(annotation.FontColor);
                            overContent.ShowTextAligned(annotation.FontAlign, string.Empty, com.Rect.Left, com.Rect.Bottom + 11f, 270);
                        }
                    }

                    overContent.EndText();

                    #endregion

                    #region Annotations Text AR

                    foreach (Commentaire com in lstTempCom)
                    {
                        Annotations annotation = ParserMemo(com.Name);

                        if (annotation.Type == AnnotationType.BD_AR)
                        {
                            overContent.SetFontAndSize(annotation.FontType, annotation.FontSize);
                            overContent.SetColorFill(annotation.FontColor);

                            if (htValue.ContainsKey(annotation.Text))
                            {
                                if (htValue[annotation.Text] != null)
                                {
                                    var el = new Chunk();
                                    Font f2 = new Font(f_cb, annotation.FontSize, -1, annotation.FontColor);
                                    el.Font = f2;
                                    PdfPTable table = new PdfPTable(1);
                                    table.WidthPercentage = 100;
                                    table.TotalWidth = 250;
                                    table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;


                                    var str = htValue[annotation.Text].ToString();
                                    PdfPCell cell = new PdfPCell(new Phrase(str, f2));
                                    cell.Border = PdfPCell.NO_BORDER;
                                    table.AddCell(cell);

                                    table.WriteSelectedRows(0, 1, com.Rect.Left, com.Rect.Bottom + 11f, overContent);
                                    //overContent.ShowTextAligned(annotation.FontAlign, htValue[annotation.Text].ToString(), com.Rect.Left, com.Rect.Bottom + 11f, 270);
                                }
                            }
                        }
                    }

                    #endregion

                    #region Annotations Code à barre

                    foreach (Commentaire com in lstTempCom)
                    {
                        Annotations annotation = ParserMemo(com.Name);
                        if (annotation.Type == AnnotationType.Image)
                        {
                            if (htValue.ContainsKey(annotation.Text))
                            {
                                BaseColor color = BaseColor.WHITE;
                                Image image = Image.GetInstance((System.Drawing.Image)htValue[annotation.Text], System.Drawing.Imaging.ImageFormat.Png);
                                if (image != null)
                                {
                                    image.SetAbsolutePosition(com.Rect.Left, com.Rect.Bottom - com.Rect.Height);
                                    if (annotation.Width > 0)
                                    {
                                        image.ScaleAbsoluteWidth(annotation.Width);
                                    }
                                    if (annotation.Height > 0)
                                    {
                                        image.ScaleAbsoluteHeight(annotation.Height);
                                    }
                                    if (annotation.Degree > 0)
                                    {
                                        image.RotationDegrees = annotation.Degree;
                                    }

                                    //overContent.SetTextMatrix(com.Rect.Left, com.Rect.Bottom + 11f);
                                    overContent.AddImage(image);
                                }
                                image = null;
                            }
                        }
                    }

                    #endregion

                    overContent.RestoreState();

                    PdfDestination dest = new PdfDestination(PdfDestination.XYZ, 0, reader.GetPageSizeWithRotation(i).Height, 0.9f);
                    pdfStamper.SetPageAction(PdfWriter.PAGE_OPEN, PdfAction.GotoLocalPage(i, dest, pdfStamper.Writer), i);
                }

                pdfStamper.Close();
                reader.Close();

                return memoryStream;
            }
        }

        public static MemoryStream AddText(IList<Commentaire> coms, Hashtable htValue, PdfReader reader)
        {
            BaseFont f_cb = BaseFont.CreateFont("c:\\windows\\fonts\\arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                //
                // PDFStamper is the class we use from iTextSharp to alter an existing PDF.
                //

                PdfStamper pdfStamper = new PdfStamper(reader, memoryStream);

                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    PdfContentByte overContent = pdfStamper.GetOverContent(i);
                    overContent.SaveState();

                    var lstTempCom = coms.Where(n => n.NumPage == i).Select(n => n).ToList();

                    #region Annotations Text FR

                    overContent.BeginText();

                    foreach (Commentaire com in lstTempCom)
                    {
                        Annotations annotation = ParserMemo(com.Name);

                        if (annotation.Type == AnnotationType.BD)
                        {
                            overContent.SetFontAndSize(annotation.FontType, annotation.FontSize);
                            //overContent.SetTextMatrix(com.Rect.Left, com.Rect.Bottom + 11f);
                            overContent.SetColorFill(annotation.FontColor);

                            if (htValue.ContainsKey(annotation.Text))
                            {
                                if (htValue[annotation.Text] != null)
                                {
                                    overContent.ShowTextAligned(annotation.FontAlign, htValue[annotation.Text].ToString(), com.Rect.Left, com.Rect.Bottom + 11f, 270);
                                }
                            }
                        }
                        else
                        {
                            overContent.SetFontAndSize(annotation.FontType, annotation.FontSize);
                            //overContent.SetTextMatrix(com.Rect.Left, com.Rect.Bottom + 11f);
                            overContent.SetColorFill(annotation.FontColor);
                            overContent.ShowTextAligned(annotation.FontAlign, string.Empty, com.Rect.Left, com.Rect.Bottom + 11f, 270);
                        }
                    }

                    overContent.EndText();

                    #endregion

                    #region Annotations Text AR

                    foreach (Commentaire com in lstTempCom)
                    {
                        Annotations annotation = ParserMemo(com.Name);

                        if (annotation.Type == AnnotationType.BD_AR)
                        {
                            overContent.SetFontAndSize(annotation.FontType, annotation.FontSize);
                            overContent.SetColorFill(annotation.FontColor);

                            if (htValue.ContainsKey(annotation.Text))
                            {
                                if (htValue[annotation.Text] != null)
                                {
                                    var el = new Chunk();
                                    Font f2 = new Font(f_cb, annotation.FontSize, -1, annotation.FontColor);
                                    el.Font = f2;
                                    PdfPTable table = new PdfPTable(1);
                                    table.WidthPercentage = 100;
                                    table.TotalWidth = 30;
                                    table.HorizontalAlignment = annotation.FontAlign;
                                    table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;


                                    var str = htValue[annotation.Text].ToString();
                                    PdfPCell cell = new PdfPCell(new Phrase(str, f2));
                                    cell.Rotation = 270;
                                    cell.Border = 0;
                                    table.AddCell(cell);

                                    table.WriteSelectedRows(0, 1, com.Rect.Left, com.Rect.Bottom + 11f, overContent);
                                    //overContent.ShowTextAligned(annotation.FontAlign, htValue[annotation.Text].ToString(), com.Rect.Left, com.Rect.Bottom + 11f, 270);
                                }
                            }
                        }
                    }

                    #endregion

                    #region Annotations Code à barre

                    foreach (Commentaire com in lstTempCom)
                    {
                        Annotations annotation = ParserMemo(com.Name);
                        if (annotation.Type == AnnotationType.Image)
                        {
                            if (htValue.ContainsKey(annotation.Text))
                            {
                                BaseColor color = BaseColor.WHITE;
                                Image image = Image.GetInstance((System.Drawing.Image)htValue[annotation.Text], System.Drawing.Imaging.ImageFormat.Png);
                                if (image != null)
                                {
                                    image.SetAbsolutePosition(com.Rect.Left, com.Rect.Bottom - com.Rect.Height);
                                    if (annotation.Width > 0)
                                    {
                                        image.ScaleAbsoluteWidth(annotation.Width);
                                    }
                                    if (annotation.Height > 0)
                                    {
                                        image.ScaleAbsoluteHeight(annotation.Height);
                                    }
                                    if (annotation.Degree > 0)
                                    {
                                        image.RotationDegrees = annotation.Degree;
                                    }

                                    //overContent.SetTextMatrix(com.Rect.Left, com.Rect.Bottom + 11f);
                                    overContent.AddImage(image);
                                }
                                image = null;
                            }
                        }
                    }

                    #endregion

                    overContent.RestoreState();

                    PdfDestination dest = new PdfDestination(PdfDestination.XYZ, 0, reader.GetPageSizeWithRotation(i).Height, 0.9f);
                    pdfStamper.SetPageAction(PdfWriter.PAGE_OPEN, PdfAction.GotoLocalPage(i, dest, pdfStamper.Writer), i);
                }

                pdfStamper.Close();
                reader.Close();

                return memoryStream;
            }
        }

        private static Annotations ParserMemo(string pdfMemo)
        {
            string defaultEncoding = BaseFont.CP1252;
            BaseFont defaultFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, defaultEncoding, false);
            BaseColor defaultColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"));

            Annotations anno = new Annotations();
            anno.Type = AnnotationType.Text;
            anno.FontType = defaultFont;
            anno.FontSize = 8.0f;
            anno.FontColor = defaultColor;
            anno.FontAlign = PdfContentByte.ALIGN_LEFT;

            try
            {
                pdfMemo = pdfMemo.Replace("\n", "\r");
                string[] pm = pdfMemo.Split('\r');
                int count = pm.Length;

                #region Type

                anno.Text = pm[0].ToString();

                string typeInfo = pm[1].ToString().ToLower();  //Type, Width, Height, Degree
                string[] typeInfos = typeInfo.Split(',');
                string type;

                if (typeInfos.Length > 0)
                {
                    type = typeInfos[0];

                    if (typeInfos.Length > 1)
                    {
                        if (typeInfos[1].Trim() != "")
                        {
                            anno.Width = Convert.ToSingle(typeInfos[1]);
                        }
                    }
                    if (typeInfos.Length > 2)
                    {
                        if (typeInfos[2].Trim() != "")
                        {
                            anno.Height = Convert.ToSingle(typeInfos[2]);
                        }
                    }
                    if (typeInfos.Length > 3)
                    {
                        if (typeInfos[3].Trim() != "")
                        {
                            anno.Degree = Convert.ToSingle(typeInfos[3]);
                        }
                    }
                }
                else
                {
                    type = typeInfo;
                }

                switch (type)
                {
                    case "text":
                    case "texte":
                        anno.Type = AnnotationType.Text;
                        break;

                    case "bd":
                    case "db":
                        anno.Type = AnnotationType.BD;
                        break;

                    case "bd_ar":
                        anno.Type = AnnotationType.BD_AR;
                        break;

                    case "codebar":
                    case "codebar_128b":
                        anno.Type = AnnotationType.CodeBar_128B;
                        break;

                    case "image":
                        anno.Type = AnnotationType.Image;
                        break;

                    default:
                        anno.Type = AnnotationType.Text;
                        break;
                }

                #endregion Type

                #region Font Name

                if (count > 2)
                {
                    string fontName = pm[2].ToString();
                    if (fontName.Trim() != "")
                    {
                        if (!string.IsNullOrEmpty(WebConfigurationManager.AppSettings["MAQUETTE_FONT"]))
                        {
                            string fontDossier = string.Format("{0}{1}", ConfigurationManager.AppSettings["PHYSICAL_BASE_PATH"], WebConfigurationManager.AppSettings["MAQUETTE_FONT"].Trim());
                            string fontPlace = string.Format(@"{0}{1}.ttf", fontDossier, fontName);

                            if (File.Exists(fontPlace))
                            {
                                if (FontFactory.GetFont(fontName).BaseFont == null)
                                {
                                    FontFactory.Register(fontPlace, fontName);
                                }

                                anno.FontType = FontFactory.GetFont(fontName, BaseFont.IDENTITY_H, BaseFont.EMBEDDED).BaseFont;
                            }

                            if (anno.FontType == null)
                            {
                                anno.FontType = BaseFont.CreateFont(BaseFont.COURIER, defaultEncoding, false);
                            }
                        }
                    }
                }

                #endregion Font Name

                if (count > 3)
                {
                    anno.FontSize = Convert.ToSingle(pm[3]);
                }

                if (count > 4)
                {
                    string fontColor = pm[4].ToString().Replace("#", "");
                    anno.FontColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml(string.Format("#{0}", fontColor)));
                }

                if (count > 5)
                {
                    string fontAlign = pm[5].ToString().ToLower();
                    if (fontAlign == "centre" || fontAlign == "center")
                    {
                        anno.FontAlign = PdfContentByte.ALIGN_CENTER;
                    }
                    else if (fontAlign == "right" || fontAlign == "droit")
                    {
                        anno.FontAlign = PdfContentByte.ALIGN_RIGHT;
                    }
                }
            }
            catch
            { }

            return anno;
        }

        public static IList<Commentaire> ChangeMemo(string inPath)
        {
            PdfReader reader = new PdfReader(inPath);
            List<Commentaire> coms = new List<Commentaire>();

            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                IList<Commentaire> lstComInPage = ChangeOnePageMemo(reader, i);
                if (lstComInPage != null && lstComInPage.Count > 0)
                    coms.AddRange(lstComInPage);
            }
            reader.Close();
            return coms;
        }

        private static IList<Commentaire> ChangeOnePageMemo(PdfReader reader, int numPage)
        {
            IList<Commentaire> coms = new List<Commentaire>();
            PdfDictionary pagedic = reader.GetPageN(numPage);
            if (pagedic.Contains(PdfName.ANNOTS))
            {
                PdfDictionary page = reader.GetPageN(numPage);
                PdfArray annotsArray = page.GetAsArray(PdfName.ANNOTS);

                if (annotsArray != null)
                {
                    for (int k = 0; k < annotsArray.Size; k++)
                    {
                        PdfDictionary annot = (PdfDictionary)PdfReader.GetPdfObject(annotsArray[k]);
                        PdfString content = (PdfString)PdfReader.GetPdfObject(annot.Get(PdfName.CONTENTS));

                        if (content != null)
                        {
                            Commentaire com = new Commentaire();
                            com.NumPage = numPage;
                            PdfObject X = PdfReader.GetPdfObject(annot.Get(PdfName.RECT));
                            if (X.IsArray())
                            {
                                PdfArray alRect = (PdfArray)X;
                                List<PdfObject> arrayList = alRect.ArrayList;

                                PdfNumber pn1 = (PdfNumber)arrayList[0];
                                PdfNumber pn2 = (PdfNumber)arrayList[1];
                                PdfNumber pn3 = (PdfNumber)arrayList[2];
                                PdfNumber pn4 = (PdfNumber)arrayList[3];

                                Rectangle rect = new Rectangle(Convert.ToInt32(pn1.FloatValue), Convert.ToInt32(pn2.FloatValue),
                                    Convert.ToInt32(pn3.FloatValue), Convert.ToInt32(pn4.FloatValue));
                                com.Rect = rect;
                                com.Height = pn4.FloatValue - pn2.FloatValue;
                            }

                            com.Name = content.ToString();
                            coms.Add(com);
                        }
                    }
                }
            }
            return coms;
        }


        private static IList<Commentaire> GetListComsText(string templatePath)
        {
            PdfReader reader = new PdfReader(templatePath);

            IList<Commentaire> coms = PdfHelper.ChangeMemo(templatePath);
            PdfDictionary pagedic = reader.GetPageN(1);
            pagedic.Remove(PdfName.ANNOTS);

            IList<Commentaire> comsText = new List<Commentaire>();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                foreach (Commentaire com in coms)
                {
                    Annotations annotation = ParserMemo(com.Name);
                    if (annotation.Type == AnnotationType.Text)
                    {
                        comsText.Add(com);
                    }
                }

            }

            return comsText;
        }

        private static void LoadParagraphByCoOrdinate(string templatePath, ref Hashtable annotationsValues, IList<Commentaire> listComText)
        {
            PdfReader reader = new PdfReader(templatePath);

            foreach (Commentaire com in listComText)
            {
                RenderFilter[] renderFilter = new RenderFilter[1];
                renderFilter[0] = new RegionTextRenderFilter(com.Rect);

                ITextExtractionStrategy textExtractionStrategy = new FilteredTextRenderListener(new LocationTextExtractionStrategy(), renderFilter);
                string text = PdfTextExtractor.GetTextFromPage(reader, 1, textExtractionStrategy);

                annotationsValues.Add(com.Name, text);
            }
        }

        public static MemoryStream GeneratePdfFromTemplate(string templateFile, Hashtable htValue)
        {
            byte[] bytes;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (Document document = new Document())
                    {
                        using (PdfSmartCopy copy = new PdfSmartCopy(document, ms))
                        {
                            document.Open();
                            bytes = GenerateDocumentWithAnnotations(templateFile, htValue);

                            PdfReader reader = new PdfReader(bytes);
                            copy.AddPage(copy.GetImportedPage(reader, 1));
                            copy.FreeReader(reader);
                        }
                    }
                    // Must be here.
                    bytes = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new MemoryStream(bytes);

        }

        public static MemoryStream GeneratePdfParking(string templatePath, Hashtable htValue)
        {
            byte[] bytes;

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (Document document = new Document())
                    {
                        using (PdfSmartCopy copy = new PdfSmartCopy(document, ms))
                        {
                            document.Open();
                            bytes = GenerateDocumentWithAnnotations(templatePath, htValue);
                            PdfReader reader = new PdfReader(bytes);
                            copy.AddPage(copy.GetImportedPage(reader, 1));
                            copy.FreeReader(reader);

                        }
                    }
                    // Must be here.
                    bytes = ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return new MemoryStream(bytes);
        }

        public static void DownloadPdf(string FileName)
        {
            try
            {
                if (File.Exists(FileName))
                {
                    using (System.IO.FileStream r = new FileStream(FileName, System.IO.FileMode.Open))
                    {
                        System.Web.HttpContext.Current.Response.Buffer = false;
                        System.Web.HttpContext.Current.Response.AddHeader("Connection", "Keep-Alive");
                        System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                        System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + System.IO.Path.GetFileName(FileName));
                        System.Web.HttpContext.Current.Response.AddHeader("Content-Length", r.Length.ToString());

                        while (true)
                        {
                            byte[] buffer = new byte[1024];
                            int leng = r.Read(buffer, 0, 1024);

                            if (leng == 0)
                            {
                                break;
                            }

                            if (leng == 1024)
                            {
                                System.Web.HttpContext.Current.Response.BinaryWrite(buffer);
                            }
                            else
                            {
                                byte[] b = new byte[leng];
                                for (int i = 0; i < leng; i++)
                                {
                                    b = buffer;
                                }

                                System.Web.HttpContext.Current.Response.BinaryWrite(b);
                            }
                        }
                    }

                    File.Delete(FileName);

                    System.Web.HttpContext.Current.Response.End();
                }
            }
            catch (Exception ex)
            {
                //Logger.Current.Error(string.Format("Erreur d'envoi d'email de validation charte à {0}", vuePers.Email), ex);
                throw ex;
            }
        }

        public static void SaveBytesToFile(string filename, byte[] bytesToWrite)
        {
            if (filename != null && filename.Length > 0 && bytesToWrite != null)
            {
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));

                FileStream file = File.Create(filename);

                file.Write(bytesToWrite, 0, bytesToWrite.Length);

                file.Close();
            }
        }

        public static void SaveStreamToFile(string destinationFile, Stream pdfStream)
        {
            using (System.IO.FileStream output = new System.IO.FileStream(destinationFile, FileMode.OpenOrCreate))
            {
                pdfStream.CopyTo(output);
            }
        }
    }
}