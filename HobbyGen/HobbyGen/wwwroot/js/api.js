(function () {
	// register events to get buttons

	// GETS
	// Attach buttons to
	// Get all users
	// Search user
	// Search user by hobbies
	attach("btn-get-all-users", "api/user", "GET", null, showUsers);
	attach("btn-get-user", "api/user/name/$(NAME)", "GET", getSearchQuery, showUsers);

	// UPDATES (dynamically)
	// Attach buttons to
	// Delete user
	// Delete hobby
	// Update user

	//
	// FUNCTIONS
	//

	/*
	 * Attach a click to an element to trigger an event
	 * id= id of element
	 * url= url of api call
	 * type= HTTP method
	 * data= JSON data
	 * 
	 * returns nothing
	*/
	function attach(id, url, type, data, callback) {
		// Get element
		var element = document.getElementById(id);

		// Fill in url with data from data, if data is present
		if (data != null) {
			// Loop through properties of data
			for (var property in data) {
				if (object.hasOwnProperty(property)) {
					// find property in upper casing in data
					var upperProp = property.toUpperCase();
					var value = data[property];

					// replace all in url
					url = url.replaceAll("$(" + upperProp + ")", value);
				}
			}
		}
		
		// transform url to prevent XSS and injection
		url = encodeURIComponent(url);

		// Attach anon function to button
		addEvent("click", element, function () {
			// Call callback with data (from url, if present)
			var xhr = new XMLHttpRequest();
			xhr.onreadystatechange = function () {
				if (this.readyState === 4 && this.status === 200) {
					// send JSON data
					var json = JSON.parse(this.responseText);
					if (callback) {
						callback(json);
					}
				}
			};
			xhr.open(type, url, true);
			xhr.send();
		});
	}

	/*
	 * Shows the users given on the website
	 * 
	 * returns nothing
	*/
	function showUsers(users) {
		// Get body to insert results into
		var resultEle = document.getElementById("result");

		// Delete all children
		while (resultEle.hasChildNodes()) {
			resultEle.removeChild(resultEle.lastChild);
		}

		// Loop through every user
		while (users.length) {
			var user = users.pop();

			// Create structure
			var wrapperDiv = document.createElement("div");
			var head = document.createElement("h2");
			var body = document.createElement("ul");

			head.innerHTML = user.name;

			while (user.hobbies.length) {
				var hobby = user.hobbies.pop();
				var tail = document.createElement("li");
				tail.innerHTML = hobby;
				body.appendChild(tail);
			}

			wrapperDiv.appendChild(head);
			wrapperDiv.appendChild(body);
			resultEle.appendChild(wrapperDiv);
		}
	}

	/*
	 * Gets the filled in value for the search query
	 * 
	 * returns string
	*/
	function getSearchQuery() {
		return "";
	}

	/*
	 * Adds an eventlistener to the element
	 * name= name of event
	 * element= HTMLElement
	 * func= function to execute on event
	 * bubble= bubble the event (false by default)
	 * 
	 * returns nothing
	*/
	function addEvent(name, element, func, bubble) {
		if (!bubble) bubble = false;

		if (document.body['on' + name]) {
			element.addEventListener('on' + name, func, bubble);
		} else {
			element.addEventListener(name, func, bubble);
		}
	}
})();

String.prototype.replaceAll = function (search, replacement) {
	var target = this;
	return target.replace(new RegExp(search, 'g'), replacement);
};