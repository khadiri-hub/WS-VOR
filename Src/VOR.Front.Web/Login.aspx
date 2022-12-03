<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VOR.Front.Web.Login" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Voyage Or - BackOffice</title>
    <link rel="shortcut icon" type="image/png" href="Images/imagesBack/favicon.ico" />
    <script>

</script>
    <style>
        .login-page {
            width: 360px;
            padding: 8% 0 0;
            margin: auto;
        }

        .form {
            position: relative;
            z-index: 1;
            background: #FFFFFF;
            width: 400px;
            margin: 20px -100px 100px;
            padding: 45px;
            text-align: center;
            box-shadow: 0 0 20px 0 rgba(0, 0, 0, 0.2), 0 5px 5px 0 rgba(0, 0, 0, 0.24);
        }

            .form .inputText {
                font-family: "Roboto", sans-serif;
                outline: 0;
                background: #f2f2f2;
                width: 350px;
                border: 0;
                margin: 0 0 15px;
                padding: 15px;
                box-sizing: border-box;
            }

            .form .button {
                font-family: "Trebuchet MS", Arial, Helvetica, sans-serif;
                text-transform: uppercase;
                outline: 0;
                background: #edd902;
                width: 352px;
                border: 0;
                padding: 15px;
                color: #fff;
                font-size: 14px;
                -webkit-transition: all 0.3 ease;
                transition: all 0.3 ease;
                cursor: pointer;
                font-weight: bold;
                background: #5a569c;
            }

                .form .button:hover, .form .button:active, .form .button:focus {
                    background: #6a65c2;
                }

            .form .message {
                margin: 15px 0 0;
                color: #b3b3b3;
                font-size: 12px;
            }

                .form .message a {
                    color: #4CAF50;
                    text-decoration: none;
                }

            .form .register-form {
                display: none;
            }

        .container {
            position: relative;
            z-index: 1;
            max-width: 300px;
            margin: 0 auto;
        }

            .container:before, .container:after {
                content: "";
                display: block;
                clear: both;
            }

            .container .info {
                margin: 50px auto;
                text-align: center;
            }

                .container .info h1 {
                    margin: 0 0 15px;
                    padding: 0;
                    font-size: 36px;
                    font-weight: 300;
                    color: #1a1a1a;
                }

                .container .info span {
                    color: #4d4d4d;
                    font-size: 12px;
                }

                    .container .info span a {
                        color: #000000;
                        text-decoration: none;
                    }

                    .container .info span .fa {
                        color: #EF3B3A;
                    }

        body {
            /*background: url(../images/imagesBack/bgMakkah.jpg);*/
            background-repeat: no-repeat;
            background-size: cover;
            background-color: #5a569c;
        }
    </style>
</head>
<body>
    <div class="login-page">
        <div class="form">
            <%-- <div style="width: 70px; height: 70px; position: relative; float: right; top: -53px; left: 64px;">
                <img runat="server" src="~/Images/imagesBack/logoLogin.png" width="60" height="60" />
            </div>--%>
            <form class="login-form" runat="server">
                <!--style="height: 255px; top: -50px; position: relative;-->
                <img runat="server" src="~/Images/imagesBack/bigLogo.png" />
                <asp:TextBox CssClass="inputText" ID="_txtLogin" runat="server" placeholder="Login"></asp:TextBox>
                <asp:TextBox CssClass="inputText" ID="_txtPwd" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
                <asp:DropDownList CssClass="inputText" ID="_ddlEvenement" runat="server"
                    AppendDataBoundItems="true" AutoPostBack="False" ClientIDMode="AutoID">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="_rfvEvenement" CssClass="required" runat="server" ControlToValidate="_ddlEvenement" ErrorMessage="&nbsp;" Display="Dynamic" EnableClientScript="False" Style="position: absolute;" />
                <asp:Button CssClass="button" ID="_btnValid" runat="server" Text="Valider" OnClick="_btnValid_Click" />
            </form>
        </div>
    </div>

    <div class="footer">
        <span>&copy; Copyright 2020 Voyage Or, Lot Sala Aljadida nº 1148 bis hssain Sala Aljadida, Sal&eacute;. T&eacute;l. :
            0537857475 / 0645232617 / 0624990203- Fax. : 0537857475</span>
    </div>
</body>
</html>


<style>
.footer {
  position: fixed;
  left: 0;
  bottom: 10px;
  width: 100%;
  color: white;
  text-align: center;
}
</style>
