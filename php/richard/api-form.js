<script>
  // Function to handle form submission
  function submitForm() {
    var clientCount = document.getElementById("client-toggle").checked ? 2 : 1;
    var firstName1 = document.getElementById("first-name-1").value;
    var lastName1 = document.getElementById("last-name-1").value;
    var firstName2 = "";
    var lastName2 = "";

    // If two clients are selected, retrieve the values of the second client
    if (clientCount === 2) {
      firstName2 = document.getElementById("first-name-2").value;
      lastName2 = document.getElementById("last-name-2").value;
    }

    // Prepare the data to be sent in the API request
    var requestData = {
      clientCount: clientCount,
      firstName1: firstName1,
      lastName1: lastName1,
      firstName2: firstName2,
      lastName2: lastName2
    };

    // Send the API request
    fetch("api-url", {
      method: "POST",
      body: JSON.stringify(requestData)
    })
      .then(response => response.json())
      .then(data => {
        // Process the response data
        console.log(data);
      })
      .catch(error => {
        // Handle any errors
        console.error("Error:", error);
      });
  }

  // Function to toggle the second client input fields
  function toggleSecondClient() {
    var clientToggle = document.getElementById("client-toggle");
    var clientInfo2 = document.getElementById("client-info-2");

    if (clientToggle.checked) {
      clientInfo2.style.display = "block";
    } else {
      clientInfo2.style.display = "none";
    }
  }

  // Add event listener to the client toggle checkbox
  document.getElementById("client-toggle").addEventListener("change", toggleSecondClient);
</script>
