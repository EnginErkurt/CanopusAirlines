document.addEventListener("DOMContentLoaded", function () {
    const tripRadios = document.querySelectorAll('input[name="trip_type"]');
    const returnGroup = document.getElementById('return-date-group');
    const returnInput = document.getElementById('return');

    //Return tarihini açıp kapatan fonksiyon
    function toggleReturnDate() {

        const selectedRadio = document.querySelector('input[name="trip_type"]:checked');

        if (selectedRadio && selectedRadio.value === 'oneway') {
            returnGroup.style.opacity = '0.3';
            returnGroup.style.pointerEvents = 'none';
            returnInput.value = '';
            returnInput.required = false; 
        } else {
            returnGroup.style.opacity = '1';
            returnGroup.style.pointerEvents = 'auto'; 
            returnInput.required = true;
        }
    }

    tripRadios.forEach(radio => {
        radio.addEventListener('change', toggleReturnDate);
    });

    toggleReturnDate();
});