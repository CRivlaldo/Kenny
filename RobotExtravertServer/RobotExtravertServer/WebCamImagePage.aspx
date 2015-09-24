<%@ Page Language="C#" %>

<%@ Import namespace="System.Drawing" %>
<%@ Import namespace="KennyServer" %>

<script runat="server" language="c#">    
    private Bitmap GetBlankBitmap(int width, int height)
    {
        Bitmap bitmap = new System.Drawing.Bitmap(640, 480);

        using (Graphics graphics = Graphics.FromImage(bitmap))
        {
            graphics.FillRectangle(Brushes.Gray, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
        }

        return bitmap;
    }

    private Bitmap GetFrameBitmap(int width, int height)
    {
        if (KennyApp.Instance == null)
            return GetBlankBitmap(width, height);

        Bitmap frameBitmap = null;

        try
        {
            frameBitmap = KennyApp.Instance.PipeProxy.GetWebCameraFrame();
        }
        catch 
        {
            frameBitmap = GetBlankBitmap(width, height);
        }

        return frameBitmap;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentType = "image/bmp";
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");

        using (Bitmap frameBitmap = GetFrameBitmap(640, 480))//!!!vladimir: magic constants
        {
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                frameBitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                memoryStream.WriteTo(Response.OutputStream);
            }
        }

        Response.OutputStream.Flush(); 
        Response.End();
    }   
</script>