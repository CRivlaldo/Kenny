<%@ Page Language="C#" Inherits="KennyServer.Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head runat="server">
	<title>Default</title>
    <script type="text/javascript" language="javascript">
        var isDrawing = false;
        var closeFlag = false;

        window.onbeforeunload = confirmClose;
        //window.onunload = handleOnClose;

        function refreshWebCamImage() {
            if (isDrawing) {
                setTimeout(refreshWebCamImage, 20);
                return;
            }

            isDrawing = true;
            var canvas = document.getElementById('canvas');
            var context = canvas.getContext("2d");
            var img = new Image();
            img.src = "WebCamImagePage.aspx";
            img.onload = function () {
                context.drawImage(img, 0, 0);
                isDrawing = false;
            }
            setTimeout(refreshWebCamImage, 100);
        }

        function confirmClose() {
            //if (event.clientY < 0) {
            if(PageMethods.IsShutdown() == false) {
                return 'You have to shutdown the connection before close.';
            }
        }

        //function handleOnClose() {
        //    alert('handleOnClose');
        //    if (closeFlag == true) {
        //        PageMethods.OnClose();
        //    }
        //}
  </script>
</head>
<body onload="refreshWebCamImage()">
	<form id="mainForm" runat="server">       

        <asp:Timer ID="Timer1" runat="server" Interval="1000" ontick="Timer1_Tick"></asp:Timer>
        
        <div>
            <asp:ScriptManager ID="ScriptManager" runat="server" EnablePageMethods="true" />
            <asp:UpdatePanel ID="UpdatePanelButtons" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger controlid="Timer1" eventname="Tick" />
                </Triggers>
                <ContentTemplate>
                    <asp:Table ID="TableButtons" runat="server" Width="470px" style="margin-bottom: 0px">
                        <asp:TableRow>
                            <asp:TableCell><asp:Button id="buttonStartStopConnection" runat="server" Text="Start Connection" OnClick="buttonStartStopConnectionClicked" Width="470px" Height="100px" Font-Size="Large" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell><asp:Button id="buttonEnableManualMode" runat="server" Text="Enable Manual Mode" OnClick="buttonEnableManualModeClicked" Width="470px" Height="100px" Font-Size="Large" /></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:Table ID="TableMoveButtons" runat="server" Width="198px" style="margin-bottom: 0px">
                                    <asp:TableRow>
                                        <asp:TableCell><asp:Button id="buttonLeft" runat="server" Text="<" Enabled="false" OnClick="buttonLeftClicked" Width="153px" Height="100px" Font-Size="Large" /></asp:TableCell>
                                        <asp:TableCell>
                                            <asp:Table ID="TableForwardBackwardButtons" runat="server" Width="70px" style="margin-bottom: 0px">
                                                <asp:TableRow>
                                                    <asp:TableCell><asp:Button id="buttonForward" runat="server" Text="Fwd" Enabled="false" OnClick="buttonForwardClicked" Width="153px" Height="100px" Font-Size="Large" /></asp:TableCell>
                                                </asp:TableRow>
                                                <asp:TableRow>
                                                    <asp:TableCell><asp:Button id="buttonBackward" runat="server" Text="Back" Enabled="false" OnClick="buttonBackwardClicked" Width="153px" Height="100px" Font-Size="Large" /></asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:TableCell>
                                        <asp:TableCell><asp:Button id="buttonRight" runat="server" Text="&gt;" Enabled="false" OnClick="buttonRightClicked" Width="153px" Height="100px" Font-Size="Large" /></asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>

                    <asp:Label runat="server" id="DateTimeLabel2" Text="XXX" /> <%--!!!!!!vladimir: do wee need this???--%>

                    <asp:Table ID="TableTelemetry" runat="server" Width="470px" style="margin-bottom: 0px">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Font-Size="Larger">Property</asp:TableHeaderCell>
                            <asp:TableHeaderCell Font-Size="Larger">Value</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                        <asp:TableRow>
                            <asp:TableCell Font-Size="Larger">Ultrasonic Sensor #0, mm</asp:TableCell>
                            <asp:TableCell><asp:TextBox runat="server" ID="textBoxUltrasoniSensor0" ReadOnly="true" Font-Size="Larger">0</asp:TextBox></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Font-Size="Larger">Magnetometer Vector</asp:TableCell>
                            <asp:TableCell><asp:TextBox runat="server" ID="textBoxMagnetometerVector" ReadOnly="true" Font-Size="Larger">0.0 0.0 0.0</asp:TextBox></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Font-Size="Larger">Accelerometer Vector</asp:TableCell>
                            <asp:TableCell><asp:TextBox runat="server" ID="textBoxAccelerometerVector" ReadOnly="true" Font-Size="Larger">0.0 0.0 0.0</asp:TextBox></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Font-Size="Larger">Gyroscope Vector</asp:TableCell>
                            <asp:TableCell><asp:TextBox runat="server" ID="textBoxGyroscopeVector" ReadOnly="true" Font-Size="Larger">0.0 0.0 0.0</asp:TextBox></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Font-Size="Larger">Battery Voltage, V</asp:TableCell>
                            <asp:TableCell><asp:TextBox runat="server" ID="textBoxBatteryVoltage" ReadOnly="true" Font-Size="Larger">0</asp:TextBox></asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell Font-Size="Larger">Charger Voltage, V</asp:TableCell>
                            <asp:TableCell><asp:TextBox runat="server" ID="textBoxChargerVoltage" ReadOnly="true" Font-Size="Larger">0</asp:TextBox></asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <canvas id="canvas" width="640" height="480">Canvas isn't supported</canvas>

    </form>
</body>
</html>
