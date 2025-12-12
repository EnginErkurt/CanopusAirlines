document.addEventListener('DOMContentLoaded', () => {
    const nextBtn = document.getElementById('btnNext');
    const cardNumberInput = document.getElementById('cardNumber');
    const expiryInput = document.getElementById('expiry');

    // 1. Credit Card Formatting (Adds space every 4 digits)
    cardNumberInput.addEventListener('input', (e) => {
        let value = e.target.value.replace(/\D/g, ''); // Remove non-digits
        value = value.replace(/(.{4})/g, '$1 ').trim(); // Add space every 4 chars
        e.target.value = value;
    });

    // 2. Expiry Date Formatting (MM/YY)
    expiryInput.addEventListener('input', (e) => {
        let value = e.target.value.replace(/\D/g, '');
        if (value.length >= 2) {
            value = value.substring(0, 2) + '/' + value.substring(2, 4);
        }
        e.target.value = value;
    });

    // 3. Next Button Logic
    nextBtn.addEventListener('click', () => {
        // Basic Validation
        const inputs = document.querySelectorAll('input[required]');
        let isValid = true;

        inputs.forEach(input => {
            if (!input.value) {
                isValid = false;
                input.style.borderColor = "#e74c3c"; // Red border for error
            } else {
                input.style.borderColor = "#cbd5e1"; // Reset border
            }
        });

        if (isValid) {
            // Here you would normally redirect to the Seat Selection page
            console.log("Form Validated. Proceeding to Seat Selection...");
            alert("Passenger details saved! Redirecting to Seat Selection...");

            // Example redirection (uncomment when you have the next page):
            // window.location.href = "seat-selection.html";
        } else {
            alert("Please fill in all required fields.");
        }
    });
});