        // Slideshow (Welcoming in different languages)
        var slideIndex = 0;
        showSlides();

        function showSlides() {
            var slides = document.getElementsByClassName("slide");
            for (var i = 0; i < slides.length; i++) {
                slides[i].classList.remove("active");
            }
            slideIndex++;
            if (slideIndex > slides.length) {
                slideIndex = 1;
            }
            slides[slideIndex - 1].classList.add("active");
            setTimeout(showSlides, 4000);
        }

        document.addEventListener('DOMContentLoaded', function() {
    
        
                var slideIndex = 0;
                showSlides();

                function showSlides() {
                    var slides = document.getElementsByClassName("slide");
                    for (var i = 0; i < slides.length; i++) {
                        slides[i].classList.remove("active");
                    }
                    slideIndex++;
                    if (slideIndex > slides.length) {
                        slideIndex = 1;
                    }
                    slides[slideIndex - 1].classList.add("active");
                    setTimeout(showSlides, 4000);
                }

                //Trip type 
                const tripTypeRadios = document.querySelectorAll('input[name="trip"]');
                const returnDateGroup = document.getElementById('return-date-group');
                const returnInput = document.getElementById('return');

                tripTypeRadios.forEach(radio => {
                    radio.addEventListener('change', function() {
                        if (this.value === 'oneway') {
                            returnDateGroup.style.display = 'none';
                            returnInput.value = '';
                        } else {
                            returnDateGroup.style.display = 'flex';
                        }
                    });
                });

                var today = new Date().toISOString().split('T')[0];
                document.getElementById("departure").setAttribute('min', today);
                document.getElementById("return").setAttribute('min', today);
            });
        
        
        // Search flights
        function searchFlights() {
    var from = document.getElementById("from").value;
    var to = document.getElementById("to").value;
    var departure = document.getElementById("departure").value;
    var returnDate = document.getElementById("return").value;
    var passengers = document.getElementById("passengers").value;
    var flightClass = document.getElementById("class").value;
    var tripType = document.querySelector('input[name="trip"]:checked').value;

    if (!from || !to || !departure) {
        alert("Please fill all the required fields!");
        return;
    }

    if (tripType === 'round' && !returnDate) {
        alert("Please choose return date!");
        return;
    }

    var message = "Searching for flight:\n" +
                  "From: " + from + "\n" +
                  "To: " + to + "\n" +
                  "Departure: " + departure + "\n";
    
    if (tripType === 'round') {
        message += "Return: " + returnDate + "\n";
    }
    
    message += "Passengers: " + passengers + "\n" +
               "Class: " + flightClass;

    alert(message);
}



        // Set min date to today
        var today = new Date().toISOString().split('T')[0];
        document.getElementById("departure").setAttribute('min', today);
        document.getElementById("return").setAttribute('min', today);