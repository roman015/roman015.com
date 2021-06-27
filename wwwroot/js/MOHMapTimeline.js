var map;

export function loadMap(bingMapsKey) {
    map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        credentials: bingMapsKey,
        center: new Microsoft.Maps.Location(1.290160, 103.852000) // Lat, Lon of "Singapore"
    });
}

export function pushPin(newPin) {
    console.log(newPin);
    //Create custom Pushpin
    var pin = new Microsoft.Maps.Pushpin(new Microsoft.Maps.Location(newPin.latitude, newPin.longitude), {
        title: newPin.title,
        color: newPin.color,
        subTitle: newPin.subtitle
    });

    //Add the pushpin to the map
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
}