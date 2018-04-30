var map, geocoder, infowindow, bounds;
var resetMap = false;

function initializeMap() {
	if (!!document.getElementById('mapcanvas')) {
		map = new google.maps.Map(document.getElementById('mapcanvas'),
			{
				zoom: 20,
				center: { lat: 38.21561, lng: -85.76199 },
				mapTypeId: google.maps.MapTypeId.roadmap	
			});
		geocoder = new google.maps.Geocoder();
		infowindow = new google.maps.InfoWindow;
		bounds = new google.maps.LatLngBounds();

		window.setInterval(refreshMap, 50000);
	}
}

function refreshMap() {
	$.post($url("/Location/GetLocationForMap"), function (data) {
		if (!!data && data.DriverId > 0) {
			resetMap = true;
			var latlng = { lat: parseFloat(data.Latitude), lng: parseFloat(data.Longitude) };
			geocoder.geocode({ 'location': latlng }, function (results, status) {
				if (status === google.maps.GeocoderStatus.OK) {
					if (results[1]) {
						map.setZoom(12);
						map.setCenter(latlng);

						var image = '~/Content/grapesicon.jpg';
						var marker = new google.maps.Marker({
							position: latlng,
							map: map,
							title: data.UserId,
							icon: image

						});
						infowindow.setContent('<p>' + data.wineryName + '<br />' + results[1].formatted_address + '</p>');
						infowindow.open(map, marker);

					} else {
						window.alert('No results found');
					}
				} else {
					window.alert('Geocoder failed due to: ' + status);
				}
			});
		} else {
			if (resetMap) {
				map = new google.maps.Map(document.getElementById('mapcanvas'), {
					zoom: 20,
					center: { lat: 38.21561, lng: -85.76199 },
				});
				resetMap = false;
			}
		}
	});
}

/********************************/
/*** Get lat/lng from address ***/
/********************************/
 
function geocodeAddress(resultsMap, address) {
	var deferred = $.Deferred(),
		geocoder = new google.maps.Geocoder();

	geocoder.geocode({ 'address': address }, function (results, status) {
		if (status === google.maps.GeocoderStatus.OK) {
			if (!!resultsMap) {
				// show on map if provided
				resultsMap.setCenter(results[0].geometry.location);
				var marker = new google.maps.Marker({
					map: resultsMap,
					position: results[0].geometry.location
				});
			}

			// return the location object
			deferred.resolve(results[0].geometry.location);
		} else {
			//alert('Geocode was not successful for the following reason: ' + status);
			deferred.reject(status);
		}
	});

	return deferred.promise();
}

/***********************************/
/*** Return Address from lat/lng ***/
/***********************************/

function geocodeLatLng() {
	var input = document.getElementById('latlng').value;
	var latlngStr = input.split(',', 2);
	var latlng = { lat: parseFloat(latlngStr[0]), lng: parseFloat(latlngStr[1]) };
	geocoder.geocode({ 'location': latlng }, function (results, status) {
		if (status === google.maps.GeocoderStatus.OK) {
			if (results[1]) {
				map.setZoom(20);
				var marker = new google.maps.Marker({
					position: latlng,
					map: map
				});
				infowindow.setContent(results[1].formatted_address);
				infowindow.open(map, marker);
			} else {
				window.alert('No results found');
			}
		} else {
			window.alert('Geocoder failed due to: ' + status);
		}
	});
}

/********************************/
/*** Time At Location lat/lng ***/
/********************************/

function getTimeZoneByLatLng(lat, lng) {
	var deferred = $.Deferred();
	$.ajax({
		url: "https://maps.googleapis.com/maps/api/timezone/json?location=" + lat + "," + lng + "&timestamp=" + (Math.round((new Date().getTime()) / 1000)).toString() + "&sensor=false&key=AIzaSyAxCMbyRjM4ScTIIaZM3uX6Jb9FPicOEMA"
	}).done(function (response) {
		if (response.timeZoneId !== null) {
			//do something
			deferred.resolve(response);
		} else {
			deferred.reject(response);
		}
	});

	return deferred.promise();
}



function createMarker(latlng, name, address) {
	var html = "<b>" + name + "</b> <br/>" + address;
	var marker = new google.maps.Marker({
		map: map,
		position: latlng
	});
	google.maps.event.addListener(marker, 'click', function () {
		infoWindow.setContent(html);
		infoWindow.open(map, marker);
	});
	markers.push(marker);
}



function createOption(name, distance, num) {
	var option = document.createElement("option");
	option.value = num;
	option.innerHTML = name;
	locationSelect.append(option);
}



function clearLocations() {
	infoWindow.close();
	for (var i = 0; i < markers.length; i++) {
		markers[i].setMap(null);
	}
	markers.length = 0;

	locationSelect.innerHTML = "";
	var option = document.createElement("option");
	option.value = "none";
	option.innerHTML = "See all results:";
	locationSelect.append(option);
	locationSelect.style.visibility = "visible";
}
