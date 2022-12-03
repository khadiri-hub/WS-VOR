<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IDPhotoMakerUC.ascx.cs" Inherits="VOR.Front.Web.UserControls.Pelerin.IDPhotoMakerUC" %>

<script src="<%=ResolveUrl("~/Plugins/IDPhotoMaker/IDPhotoMaker.js")%>" type="text/javascript"></script>

<script type="text/javascript">
    $(document).ready(function () {
        var oWin = GetRadWindow();
        oWin.Center();
        oWin.add_close(function () {
            try {
                setTimeout(function () {
                    IDPhotoMaker.dispose();
                }, 500);
            } catch (e) {
            } 
        });

        var lang = "<%= this.Lang %>";
            var cropWidth = <%= this.CropWidth %>;
            var cropHeight = <%= this.CropHeight %>;            
            var refWin = "<%= this.RefWin %>";
            var refWinMan = "<%= this.RefWinMan %>";

            IDPhotoMaker.init('#IDPhotoMaker_container', {
                lang: lang,
                imageMaxSize: 3, //Mo
                imageColorRequired: true,
                crop: {
                    width: cropWidth,
                    height: cropHeight
                },
                save: function (data) {
                    oWin.BrowserWindow.getIDPhotoMakerResult(data, refWin, refWinMan);
                    setTimeout(function() {
                        oWin.close();
                    }, 500);                   
                },
                error: function (message) {
                    alert(message);
                },
                exit: function () {
                    oWin.close();
                    setTimeout(function () {
                        IDPhotoMaker.dispose();
                    }, 500);
                }
            });
        });
</script>

<div id="IDPhotoMaker_container"></div>
