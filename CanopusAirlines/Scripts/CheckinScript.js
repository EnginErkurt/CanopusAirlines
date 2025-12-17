let currentStep = 1;
let baggagePrice = 0;

function selectBaggage(index) {
    const options = document.querySelectorAll('.baggage-option');
    options.forEach(opt => opt.classList.remove('selected'));
    options[index].classList.add('selected');

    // Fiyatı al (data-price attribute'undan)
    baggagePrice = parseFloat(options[index].dataset.price);
    updatePricing();
}

function updatePricing() {
    document.getElementById('baggageFee').textContent = `$${baggagePrice.toFixed(2)}`;
}

function nextStep(step) {
    // --- ADIM 1: REZERVASYON KONTROLÜ ---
    if (step === 1) {
        const bookingRef = document.getElementById('bookingRef').value.trim();
        const lastName = document.getElementById('lastName').value.trim();

        if (!bookingRef || !lastName) {
            alert('Lütfen PNR Kodu ve Soyad alanlarını doldurunuz.');
            return;
        }

        // BUTONU KİLİTLE (Çift tıklamayı önle)
        const btn = document.querySelector('#step1 .btn-primary');
        const originalText = btn.innerText;
        btn.innerText = "Sorgulanıyor...";
        btn.disabled = true;

        // AJAX (FETCH) İSTEĞİ - SUNUCUYA SORUYORUZ
        fetch('/Home/GetTicketCheckIn', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ pnr: bookingRef, lastname: lastName })
        })
            .then(response => response.json())
            .then(data => {
                // Butonu eski haline getir
                btn.innerText = originalText;
                btn.disabled = false;

                if (data.success) {
                    // VERİLER GELDİ, EKRANI DOLDUR

                    // 1. Boarding Pass Bilgilerini Doldur
                    document.getElementById('passengerName').textContent = data.passengerName;
                    document.getElementById('seatNumber').textContent = data.seat;

                    // HTML'de bu ID'lere sahip elementler yoksa eklememiz gerekecek (Adım 3'te bakacağız)
                    // Şimdilik class veya mevcut yapıyı kullanarak dolduralım

                    // Uçuş No
                    document.querySelector('.bp-flight').textContent = "Flight " + data.flightNo;

                    // Rota (Kalkış)
                    document.querySelector('.bp-route .bp-location:first-child .bp-code').textContent = data.originCode;
                    document.querySelector('.bp-route .bp-location:first-child .bp-city').textContent = data.originCity;

                    // Rota (Varış)
                    document.querySelector('.bp-route .bp-location:last-child .bp-code').textContent = data.destCode;
                    document.querySelector('.bp-route .bp-location:last-child .bp-city').textContent = data.destCity;

                    // Tarih ve Saat
                    document.getElementById('flightDate').textContent = data.date;
                    // Boarding saatini uçuş saatinden 30 dk önce gösterelim
                    document.querySelector('.bp-detail:nth-child(2) .bp-value').textContent = data.time;

                    // Kapı
                    document.querySelector('.bp-detail:last-child .bp-value').textContent = data.gate;

                    // SONRAKİ ADIMA GEÇ
                    goToStep(2);
                } else {
                    alert(data.message); // Hata mesajı (Bilet bulunamadı)
                }
            })
            .catch(error => {
                console.error('Hata:', error);
                alert("Bir hata oluştu. Lütfen tekrar deneyin.");
                btn.innerText = originalText;
                btn.disabled = false;
            });

        return; // Fetch bitene kadar fonksiyonu burada kesiyoruz
    }

    // --- DİĞER ADIMLAR (Step 2 -> Step 3) ---
    if (step === 2) {
        goToStep(3);
    }
}

// Yardımcı Fonksiyon: Adım Değiştirme (Kod tekrarını önlemek için ayırdım)
function goToStep(targetStep) {
    const currentStep = targetStep - 1;

    // Mevcut adımı gizle
    document.getElementById('step' + currentStep).classList.remove('active');
    // Hedef adımı göster
    document.getElementById('step' + targetStep).classList.add('active');

    // Progress Bar güncelleme
    document.getElementById('step' + currentStep + '-progress').classList.remove('active');
    document.getElementById('step' + currentStep + '-progress').classList.add('completed');
    document.getElementById('step' + targetStep + '-progress').classList.add('active');

    window.scrollTo(0, 0);
}
function prevStep(step) {
    // Mevcut adımı gizle
    document.getElementById('step' + step).classList.remove('active');
    // Önceki adımı göster
    document.getElementById('step' + (step - 1)).classList.add('active');

    // Progress Bar geri alma
    document.getElementById('step' + step + '-progress').classList.remove('active');
    document.getElementById('step' + (step - 1) + '-progress').classList.add('active');
    document.getElementById('step' + (step - 1) + '-progress').classList.remove('completed');

    currentStep = step - 1;
    window.scrollTo(0, 0);
}

// Sayfa yüklendiğinde çalışacaklar
document.addEventListener('DOMContentLoaded', function () {
    // Varsayılan bagajı seç
    selectBaggage(0);

    // Güncel tarihi ayarla
    const today = new Date();
    const months = ['JAN', 'FEB', 'MAR', 'APR', 'MAY', 'JUN', 'JUL', 'AUG', 'SEP', 'OCT', 'NOV', 'DEC'];
    const dateStr = today.getDate() + ' ' + months[today.getMonth()];

    // Eğer flightDate elementi varsa doldur
    const dateEl = document.getElementById('flightDate');
    if (dateEl) {
        dateEl.textContent = dateStr;
    }
});