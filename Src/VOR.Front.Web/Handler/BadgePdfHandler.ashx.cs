
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Linq;
using System.IO;
using VOR.Core.Domain;
using VOR.Utils;
using VOR.Core.Model;
using VOR.Core.Enum;

namespace VOR.Front.Web.Handler
{
    /// <summary>
    /// 
    /// </summary>
    public class BadgePdfHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/pdf";
            int? eventId = GetEventId(context);
            int? agenceId = GetAgenceId(context);


            if (!eventId.HasValue || !agenceId.HasValue)
            {
                Logger.Current.Error("Une erreur a été rencontrée lors du téléchargement du badge");
                context.Response.ContentType = "text/html";
                context.Response.Write("Une erreur a été rencontrée: ce token n'est pas valable");
                return;
            }

            try
            {
                IList<Pelerin> lstPelerin = Global.Container.Resolve<PelerinModel>().GetPelerinToDownloadBadge(agenceId.Value);
                Global.Container.Resolve<PelerinModel>().SetBadgeToDownload(false, getLstIdPelerin(lstPelerin));

                FileInfo modele = this.TemplateFolderBadge.GetFiles().FirstOrDefault();
                string templatePath = modele.FullName;
                if (modele != null)
                {
                    IList<PelerinBadge> lstPerlerinBadge = new List<PelerinBadge>();

                    foreach (Pelerin pelerin in lstPelerin)
                    {
                        PelerinBadge pelerinBadge = new PelerinBadge();
                        pelerinBadge.Nom = pelerin.NomArabe;
                        pelerinBadge.Prenom = pelerin.PrenomArabe;
                        pelerinBadge.Photo = pelerin.Photo;
                        pelerinBadge.Visa = pelerin.Visa != null ? pelerin.Visa.Image : new byte[5];
                        pelerinBadge.PelerinsMakkah = pelerin.ChambreMakkah != null ? pelerin.ChambreMakkah.PelerinsMakkah.Select(n => n).Where(n => n.ID != pelerin.ID).ToList() : null;
                        pelerinBadge.PelerinsMedine = pelerin.ChambreMedine != null ? pelerin.ChambreMedine.PelerinsMedine.Select(n => n).Where(n => n.ID != pelerin.ID).ToList() : null;
                        //pelerinBadge.DateNaissance = pelerin.DateNaissance;
                        //pelerinBadge.Accompagnant = pelerin.Personnes.Select(n => n.Personne).Where(n => n.TypePersonne.ID == (int) EnumTypePersonne.ACCOMPAGNANT).ToArray().Count() != 0 ?
                        //pelerin.Personnes.Select(n => n.Personne).Where(n => n.TypePersonne.ID == (int) EnumTypePersonne.ACCOMPAGNANT).FirstOrDefault().NomPrenom : "";
                        //pelerinBadge.Passeport = pelerin.NumPassport;
                        //pelerinBadge.Telef = pelerin.Telephone1;
                        pelerinBadge.HotelMakkah = pelerin.Hotels.Select(n => n.Hotel).Where(n => n.Ville.ID == (int) EnumVille.MAKKAH).ToArray().Count() != 0 ? pelerin.Hotels.Select(n => n.Hotel).Where(n => n.Ville.ID == (int) EnumVille.MAKKAH).FirstOrDefault().NomFr : "";
                        pelerinBadge.HotelMedine = pelerin.Hotels.Select(n => n.Hotel).Where(n => n.Ville.ID == (int) EnumVille.MEDINE).ToArray().Count() != 0 ? pelerin.Hotels.Select(n => n.Hotel).Where(n => n.Ville.ID == (int) EnumVille.MEDINE).FirstOrDefault().NomFr : "";
                        pelerinBadge.TypeChambre = pelerin.TypeChambre.Code;
                        lstPerlerinBadge.Add(pelerinBadge);
                    }

                    Evenement evenement = Global.Container.Resolve<EvenementModel>().GetByID(eventId.Value);

                    string fileName = string.Empty;
                    if (lstPelerin.Count == 1)
                        fileName = string.Format("BADGE_{0}_{1}_{2}", evenement.Nom, lstPelerin[0].NomFrancais, lstPelerin[0].PrenomFrancais);
                    else
                        fileName = string.Format("BADGES_{0}", evenement.Nom);


                    HttpResponse Response = context.Response;
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}.pdf", fileName));
                    PdfHelper.GenerateBadgePelerinDocument(templatePath, lstPerlerinBadge, Response.OutputStream);
                }
            }
            catch (Exception e)
            {
                Logger.Current.Error("Une erreur a été rencontrée  lors de la génération du pdf", e);

                context.Response.ContentType = "text/html";
                context.Response.Write("Une erreur a été rencontrée lors de la génération du pdf");
                return;
            }

        }

        private IList<int> getLstIdPelerin(IList<Pelerin> lstPelerin)
        {
            IList<int> lstIdPelerin = new List<int>();
            foreach (Pelerin pelerin in lstPelerin)
            {
                lstIdPelerin.Add(pelerin.ID);
            }
            return lstIdPelerin;
        }

        private DirectoryInfo TemplateFolderBadge
        {
            get
            {
                var dir = new DirectoryInfo(ConfigurationManager.AppSettings["BADGE_TEMPLATE_FOLDER"]);

                if (!dir.Exists)
                    throw new IOException("Le répertoire '" + dir.FullName + "' n'existe pas.");

                return dir;
            }
        }

        private int? GetEventId(HttpContext context)
        {
            int? eventId = null;

            if (context.Request.QueryString["EventID"] != null)
            {
                eventId = int.Parse(context.Request.QueryString["EventID"]);
            }

            return eventId;
        }

        private int? GetAgenceId(HttpContext context)
        {
            int? agenceId = null;

            if (context.Request.QueryString["AgenceID"] != null)
            {
                agenceId = int.Parse(context.Request.QueryString["AgenceID"]);
            }

            return agenceId;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}