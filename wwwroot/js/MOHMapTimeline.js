var map;
var infobox;

export function loadMap(bingMapsKey) {
    map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        credentials: bingMapsKey,
        center: new Microsoft.Maps.Location(1.290160, 103.852000) // Lat, Lon of "Singapore"
    });

    // Setup Infobox
    infobox = new Microsoft.Maps.Infobox(map.getCenter(), {
        visible: false
    });
    infobox.setMap(map);
}

export function pushPin(newPin) {
    console.log(newPin);

    // Create custom Pushpin
    var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(newPin.latitude, newPin.longitude), {
        title: newPin.title,
        color: newPin.color,
        subTitle: newPin.subtitle
    });

    pin.metadata = {
        pdfUrl: newPin.pdfUrl,
        articleUrl: newPin.articleUrl,
        location: newPin.locationFormatted,
        time: newPin.time
    }

    // Add Click Event Handler
    Microsoft.Maps.Events.addHandler(pin, 'click', displayInfobox);

    // Add the pushpin to the map
    map.entities.push(pin);    
}

export function removePin(selectedPin) {
    for (var i = map.entities.getLength() - 1; i >= 0; i--) {
        var pushpin = map.entities.get(i);
        if (pushpin instanceof Microsoft.Maps.Pushpin) {
            if (pushpin.entity.title == selectedPin.title) {
                map.entities.removeAt(i);
            }
        }
    }
}

export function removeAllPins() {
    var count = 0;
    for (var i = map.entities.getLength() - 1; i >= 0; i--) {
        var pushpin = map.entities.get(i);
        if (pushpin instanceof Microsoft.Maps.Pushpin) {
            map.entities.removeAt(i);
            count++;
        }
    }
    console.log(count + ' Pins Removed');

    infobox.setOptions({ visible: false });
}

export function displayInfobox(e) {
    if (e.target.metadata) {
        infobox.setOptions({
            maxHeight: 640,
            maxWidth: 640,
            location: e.target.getLocation(),
            title: e.target.metadata.time,
            description: e.target.metadata.location,
            visible: true,
            actions: [{
                label: 'Press Release',
                eventHandler: function () {
                    window.open(e.target.metadata.articleUrl, '_blank');
                }
            },
            {
                label: 'PDF',
                eventHandler: function () {
                    window.open(e.target.metadata.pdfUrl, '_blank');
                }
            }]
        });
    }
}