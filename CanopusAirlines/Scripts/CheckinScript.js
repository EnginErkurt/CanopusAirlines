let currentStep = 1;
let baggagePrice = 0;

function selectBaggage(index) {
    const options = document.querySelectorAll('.baggage-option');
    options.forEach(opt => opt.classList.remove('selected'));
    options[index].classList.add('selected');

    baggagePrice = parseFloat(options[index].dataset.price);
    updatePricing();
}

function updatePricing() {
    document.getElementById('baggageFee').textContent = `$${baggagePrice.toFixed(2)}`;
}

function nextStep(step) {
    //Reservation control
    if (step === 1) {
        const bookingRef = document.getElementById('bookingRef').value.trim();
        const lastName = document.getElementById('lastName').value.trim();

        if (!bookingRef || !lastName) {
            alert('Lütfen PNR Kodu ve Soyad alanlarını doldurunuz.');
            return;
        }

        const btn = document.querySelector('#step1 .btn-primary');
        const originalText = btn.innerText;
        btn.innerText = "Sorgulanıyor...";
        btn.disabled = true;

        //Ajax (Fetch)
        fetch('/Home/GetTicketCheckIn', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ pnr: bookingRef, lastname: lastName })
        })
            .then(response => response.json())
            .then(data => {

                btn.innerText = originalText;
                btn.disabled = false;

                if (data.success) {

                    //Boarding Pass
                    document.getElementById('passengerName').textContent = data.passengerName;
                    document.getElementById('seatNumber').textContent = data.seat;

                    //Flight
                    document.querySelector('.bp-flight').textContent = "Flight " + data.flightNo;

                    //Route(Departure)
                    document.querySelector('.bp-route .bp-location:first-child .bp-code').textContent = data.originCode;
                    document.querySelector('.bp-route .bp-location:first-child .bp-city').textContent = data.originCity;

                    //Route(Arrival)
                    document.querySelector('.bp-route .bp-location:last-child .bp-code').textContent = data.destCode;
                    document.querySelector('.bp-route .bp-location:last-child .bp-city').textContent = data.destCity;

                    //Flight Date
                    document.getElementById('flightDate').textContent = data.date;

                    document.querySelector('.bp-detail:nth-child(2) .bp-value').textContent = data.time;

                    //Gate (rastgele koyduk)
                    document.querySelector('.bp-detail:last-child .bp-value').textContent = data.gate;

                    goToStep(2);
                } else {
                    alert(data.message); //Alert 
                }
            })
            .catch(error => {
                console.error('Hata:', error);
                alert("Bir hata oluştu. Lütfen tekrar deneyin.");
                btn.innerText = originalText;
                btn.disabled = false;
            });

        return; 
    }

    if (step === 2) {
        goToStep(3);
    }
}

function goToStep(targetStep) {
    const currentStep = targetStep - 1;


    document.getElementById('step' + currentStep).classList.remove('active');

    document.getElementById('step' + targetStep).classList.add('active');

  
    document.getElementById('step' + currentStep + '-progress').classList.remove('active');
    document.getElementById('step' + currentStep + '-progress').classList.add('completed');
    document.getElementById('step' + targetStep + '-progress').classList.add('active');

    window.scrollTo(0, 0);
}
function prevStep(step) {

    document.getElementById('step' + step).classList.remove('active');

    document.getElementById('step' + (step - 1)).classList.add('active');

    document.getElementById('step' + step + '-progress').classList.remove('active');
    document.getElementById('step' + (step - 1) + '-progress').classList.add('active');
    document.getElementById('step' + (step - 1) + '-progress').classList.remove('completed');

    currentStep = step - 1;
    window.scrollTo(0, 0);
}

//Sayfa yüklendiğinde çalışacaklar
document.addEventListener('DOMContentLoaded', function () {

    selectBaggage(0);

    //Current date
    const today = new Date();
    const months = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
    const dateStr = today.getDate() + ' ' + months[today.getMonth()];

    const dateEl = document.getElementById('flightDate');
    if (dateEl) {
        dateEl.textContent = dateStr;
    }
});