document.addEventListener('DOMContentLoaded', () => {
    const nextBtn = document.getElementById('btnNext');
    const cardNumberInput = document.getElementById('cardNumber');
    const expiryInput = document.getElementById('expiry');

    //Credit Card
    cardNumberInput.addEventListener('input', (e) => {
        let value = e.target.value.replace(/\D/g, ''); 
        value = value.replace(/(.{4})/g, '$1 ').trim(); 
        e.target.value = value;
    });

    //Expiry Date
    expiryInput.addEventListener('input', (e) => {
        let value = e.target.value.replace(/\D/g, '');
        if (value.length >= 2) {
            value = value.substring(0, 2) + '/' + value.substring(2, 4);
        }
        e.target.value = value;
    });

    nextBtn.addEventListener('click', () => {
        const inputs = document.querySelectorAll('input[required]');
        let isValid = true;

        inputs.forEach(input => {
            if (!input.value) {
                isValid = false;
                input.style.borderColor = "#e74c3c"; 
            } else {
                input.style.borderColor = "#cbd5e1";
            }
        });

        if (isValid) {
            //redirect to seat selection
            console.log("Form Validated. Proceeding to Seat Selection...");
            alert("Passenger details saved! Redirecting to Seat Selection...");

        } else {
            alert("Please fill in all required fields.");
        }
    });
});