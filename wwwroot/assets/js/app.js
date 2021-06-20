function CreateTriangles() {
    // Header background                        
    var colors = Please.make_color({
        colors_returned: 3,
        saturation: .6
    });
    var t = new Trianglify({
        x_gradient: colors,
        y_gradient: ["#FFFFFF"]
    });
    var header = document.getElementById("intro-header");
    var pattern = t.generate(header.clientWidth, header.clientHeight);
    header.setAttribute('style', 'background-image: ' + pattern.dataUrl);
}

function SetPageTitle(newTitle) {
    this.window.document.title = newTitle;
}


// TODO: Find out what this is for
function EnableBackgroundCheck() {
    BackgroundCheck.init({
        targets: '.intro-header',
        images: '.intro-header'
    });
}
