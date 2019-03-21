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

	// Fetch dummy data
	attach("btn-fetch-dummy-data", "api/dummy", "POST", null, function () {
		showMessage("Dummy data fetched!", "Hit get users to see a massive list");
		var e = document.getElementById("btn-fetch-dummy-data");
		e.innerHTML = "Dummy data loaded!";
	});
	addEvent("click", document.getElementById("btn-fetch-dummy-data"), function () {
		// disable self
		var e = document.getElementById("btn-fetch-dummy-data");
		e.innerHTML = "Loading dummy data..."
		e.disabled = true;
	});

	showEmpty();
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
					if (callback) {
						var json;
						// send JSON data, if any
						if (this.responseText != "") {
							json = JSON.parse(this.responseText);
						}
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

		clearResults();

		if (users.length == 0) {
			showEmpty();
		} else {
			// Loop through every user
			while (users.length) {
				var user = users.pop();

				// Call function to remove scoping
				appendUser(user);
			}
		}

		// Append a user to the result bar
		function appendUser(user) {
			// Create structure
			var wrapperDiv = document.createElement("div");
			var head = document.createElement("h2");
			var body = document.createElement("ul");

			// Add button to remove users
			var deluser = document.createElement("button");
			deluser.innerHTML = "Delete user";
			deluser.id = user.id + "-del-user";

			// Add hr
			var hr = document.createElement("hr");

			// Add input for adding hobbies
			var howrapper = document.createElement("div");
			var hobadd = document.createElement("input");
			hobadd.type = "text";
			hobadd.placeholder = "add hobby...";
			var hobutton = document.createElement("button");
			hobutton.innerHTML = "Add hobby";

			howrapper.appendChild(hobadd);
			hobutton.id = user.id + "-hobby-add";
			howrapper.appendChild(hobutton);

			// Add input to remove hobbies
			var how2rapper = document.createElement("div");
			var hobrem = document.createElement("input");
			hobrem.type = "text";
			hobrem.placeholder = "remove hobby...";
			var ho2button = document.createElement("button");
			ho2button.innerHTML = "Remove hobby";

			how2rapper.appendChild(hobrem);
			ho2button.id = user.id + "-hobby-rem";
			how2rapper.appendChild(ho2button);

			// Append and create hobbies
			head.innerHTML = user.name;

			while (user.hobbies.length) {
				var hobby = user.hobbies.pop();
				if (hobby != "") {
					var tail = document.createElement("li");
					tail.innerHTML = hobby;
					body.appendChild(tail);
				}
			}
			
			wrapperDiv.appendChild(head);
			wrapperDiv.appendChild(deluser);

			wrapperDiv.appendChild(hr);

			wrapperDiv.appendChild(howrapper);
			wrapperDiv.appendChild(how2rapper);
			wrapperDiv.appendChild(body);
			resultEle.appendChild(wrapperDiv);

			// Attach listeners
			// Allow for hobby creation
			attach(hobutton.id, "api/user/" + user.id, "PUT", function () {
				return {
					hobbiesAdded: hobadd.value.split(", "),
					hobbiesRemoved: []
				};
			}, showAddedUser);

			// Allow for hobby deletion
			attach(ho2button.id, "api/user/" + user.id, "PUT", function () {
				return {
					hobbiesAdded: [],
					hobbiesRemoved: hobrem.value.split(", ")
				};
			}, showAddedUser);

			// Allow for user deletion
			attach(deluser.id, "api/user/" + user.id, "DELETE", null, function () {
				clearResults();
				showEmpty();
			});
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
	 * Clears all results
	 * 
	 * returns nothing
	*/ 
	function clearResults() {
		var resultEle = document.getElementById("result");

		// Delete all children
		while (resultEle.hasChildNodes()) {
			resultEle.removeChild(resultEle.lastChild);
		}
	}

	/*
	 * Sets the results bar to empty and shows the user to interact
	 * 
	 * returns nothing
	*/ 
	function showEmpty() {
		showMessage("No users shown!", "Either search for users of get all users.");
	}

	/*
	 * Shows a message in the results bar
	 * 
	 * returns nothing
	*/
	function showMessage(h2, p) {
		var resultEle = document.getElementById("result");

		// Create structure
		var wrapperDiv = document.createElement("div");
		var head = document.createElement("h2");
		head.innerHTML = h2;
		var text = document.createElement("p");
		text.innerHTML = p;

		wrapperDiv.appendChild(head);
		wrapperDiv.appendChild(text);
		resultEle.appendChild(wrapperDiv);
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