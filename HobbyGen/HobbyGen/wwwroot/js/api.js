(function () {
	// register events to get buttons

	// GETS
	// Attach buttons to
	// Get all users
	// Search user
	// Search user by hobbies
	attach("btn-get-all-users", "api/user", "GET", null, showUsers);
	attach("btn-get-user", "api/user/name/$(NAME)", "GET", getSearchQuery, showUsers);
	attach("btn-get-userhobby", "api/user/hobby", "POST", getSearchQueryHobby, showUsers);
	attach("btn-add-user", "api/user", "POST", getUserQuery, showAddedUser);

	// TODO
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

		// Attach anon function to button
		addEvent("click", element, function () {
			var tdata = data;
			var turl = url;

			// Fill in url with data from data, if data is present
			if (tdata != null) {
				// is data a function?
				if (typeof data === "function") {
					tdata = data();
				}

				// Loop through properties of data
				for (var property in tdata) {
					if (tdata.hasOwnProperty(property)) {
						// find property in upper casing in data
						var upperProp = property.toUpperCase();
						var value = tdata[property];

						// replace all in url
						turl = url.replace("\$\(" + upperProp + "\)", value);
					}
				}
			}

			// transform url to prevent XSS and injection
			turl = encodeURIComponent(turl);
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
			xhr.open(type, turl, true);
			xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");
			xhr.send(JSON.stringify(tdata));
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

			wrapperDiv.id = user.id;
			wrapperDiv.appendChild(head);
			wrapperDiv.appendChild(body);
			resultEle.appendChild(wrapperDiv);
		}
	}

	/*
	 * Shows feedback of added user
	 * 
	 * returns nothing
	*/ 
	function showAddedUser(user) {
		showUsers([user]);
	}

	/*
	 * Gets the filled in value for the search query
	 * 
	 * returns string
	*/
	function getSearchQuery() {
		// get element with query
		var element = document.getElementById("search-query");

		return { name: element.value };
	}

	/*
	 * Gets the filled in value for the user query
	 * 
	 * returns string
	*/
	function getUserQuery() {
		// get element with query
		var element = document.getElementById("user-query");

		return {
			name: element.value,
			hobbies: []
		};
	}

	/*
	 * Gets the filled in value for the search query for hobbies
	 * 
	 * returns array of hobbies
	*/
	function getSearchQueryHobby() {
		// get element with query
		var element = document.getElementById("search-queryhobby");

		return element.value.split(", ");
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