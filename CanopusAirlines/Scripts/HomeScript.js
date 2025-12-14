document.addEventListener("DOMContentLoaded", function () {
    // Elementleri seçiyoruz
    const tripRadios = document.querySelectorAll('input[name="trip_type"]');
    const returnGroup = document.getElementById('return-date-group');
    const returnInput = document.getElementById('return');

    // Return tarihini açıp kapatan fonksiyon
    function toggleReturnDate() {
        // Seçili olan radyo butonunu bul
        // (Eğer sayfa ilk açıldığında seçili yoksa hata vermesin diye kontrol ekleyebiliriz)
        const selectedRadio = document.querySelector('input[name="trip_type"]:checked');

        if (selectedRadio && selectedRadio.value === 'oneway') {
            // Tek yön ise: Soluklaştır, tıklanmasını engelle ve içini temizle
            returnGroup.style.opacity = '0.3';
            returnGroup.style.pointerEvents = 'none'; // Tıklamayı engeller
            returnInput.value = '';
            returnInput.required = false; // Zorunluluğu kaldır
        } else {
            // Gidiş-Dönüş ise: Canlandır
            returnGroup.style.opacity = '1';
            returnGroup.style.pointerEvents = 'auto'; // Tıklamayı aç
            returnInput.required = true; // Seçilmesini zorunlu yap
        }
    }

    // Radyo butonlarına tıklanınca fonksiyonu çalıştır
    tripRadios.forEach(radio => {
        radio.addEventListener('change', toggleReturnDate);
    });

    // Sayfa ilk açıldığında durumu kontrol et (Otomatik ayarla)
    toggleReturnDate();
});