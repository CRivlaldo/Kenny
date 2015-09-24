module WebCam
    
    open Emgu.CV
    open Emgu.CV.UI
    open Emgu.CV.Structure
    open System.Drawing

//    let viewer : ImageViewer = new ImageViewer()
    let mutable internal image : Image<Bgr, byte> = null
    let mutable internal capture : Capture = null
//Application.Idle += new EventHandler(delegate(object sender, EventArgs e)
//{  //run this until application closed (close button click on image viewer)
//   viewer.Image = capture.QueryFrame(); //draw the image obtained from camera
//});

    let Init() =
        capture <- new Capture(0)

    let Shutdown() =
        if capture <> null then capture.Dispose()
        capture <- null

        if image <> null then image.Dispose()
        image <- null

    let Capture() : Image<Bgr, byte> =
        if capture <> null then
            image <- capture.QueryFrame()
            //printfn "%d x %d" image.Width image.Height
            image
        else null

    let GetFrame() = image.ToBitmap(620, 480)