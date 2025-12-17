document.addEventListener('DOMContentLoaded', () => {
    const cabinGrid = document.getElementById('cabinGrid');
    const tooltip = document.getElementById('seatTooltip');
    const selectedSeatDisplay = document.getElementById('selectedSeatDisplay');
    const totalPriceDisplay = document.getElementById('totalPrice');
    const btnBuy = document.getElementById('btnBuy');

    const rows = 20; 
    //HTML'deki gizli kutudan gerçek fiyatı alıyoruz
    const basePriceInput = document.getElementById('serverBasePrice');

    //Sayıya çeviriyoruz (parseFloat)
    //Eğer bir sorun olursa 0 kabul et diyoruz (|| 0)
    const basePrice = parseFloat(basePriceInput.value) || 0;
    let currentSelectedSeat = null;

    //Create a single seat element
    function createSeat(rowNum, letter, type = 'standard') {
        const seat = document.createElement('div');
        seat.classList.add('seat');
        seat.dataset.seat = `${rowNum}${letter}`;

        //Prices
        let seatPrice = 0;
        if (type === 'extra') {
            seat.classList.add('extra');
            seatPrice = 500; //büyük koltuk
            seat.innerHTML = '<i class="fa-solid fa-star" style="font-size:8px; color:#ff9800"></i>';
        } else {
            seatPrice = 150; //standard koltuk
        }
        seat.dataset.price = seatPrice;


        if (Math.random() < 0.2) {
            seat.classList.add('occupied');
            seat.innerHTML = '<i class="fa-solid fa-xmark"></i>';
        }

        return seat;
    }

    for (let i = 1; i <= rows; i++) {
        const rowDiv = document.createElement('div');
        rowDiv.classList.add('seat-row');

        //Left Block (ABC)
        ['A', 'B', 'C'].forEach(l => rowDiv.appendChild(createSeat(i, l, i === 10 ? 'extra' : 'standard'))); // Row 10 is exit/extra
        const aisle1 = document.createElement('div');
        aisle1.classList.add('aisle-spacer');
        rowDiv.appendChild(aisle1);

        //Center Block (DEF)
        ['D', 'E', 'F'].forEach(l => rowDiv.appendChild(createSeat(i, l, i === 10 ? 'extra' : 'standard')));
        const aisle2 = document.createElement('div');
        aisle2.classList.add('aisle-spacer');
        rowDiv.appendChild(aisle2);

        //Right Block (GHJ)
        ['G', 'H', 'J'].forEach(l => rowDiv.appendChild(createSeat(i, l, i === 10 ? 'extra' : 'standard')));

        cabinGrid.appendChild(rowDiv);
    }

    cabinGrid.addEventListener('mouseover', (e) => {
        const seat = e.target.closest('.seat');
        if (seat && !seat.classList.contains('occupied')) {
            tooltip.style.opacity = '1';
            tooltip.querySelector('.tooltip-seat-num').textContent = seat.dataset.seat;
            tooltip.querySelector('.tooltip-price').textContent = `+ TRY ${seat.dataset.price}`;

            const rect = seat.getBoundingClientRect();
            tooltip.style.top = `${rect.top - 60}px`;
            tooltip.style.left = `${rect.left + (rect.width / 2) - (tooltip.offsetWidth / 2)}px`;
        }
    });

    cabinGrid.addEventListener('mouseout', (e) => {
        tooltip.style.opacity = '0';
    });

    cabinGrid.addEventListener('click', (e) => {
        const seat = e.target.closest('.seat');
        if (!seat || seat.classList.contains('occupied')) return;

        if (currentSelectedSeat) {
            currentSelectedSeat.classList.remove('selected');
        }

        seat.classList.add('selected');
        currentSelectedSeat = seat;

        //Update UI
        const seatPrice = parseFloat(seat.dataset.price);
        const finalPrice = basePrice + seatPrice;

        selectedSeatDisplay.textContent = seat.dataset.seat;
        totalPriceDisplay.textContent = `TRY ${finalPrice.toLocaleString('tr-TR', { minimumFractionDigits: 2 })}`;

        btnBuy.disabled = false;
        btnBuy.classList.add('active');
        btnBuy.innerHTML = `Buy Ticket (Seat ${seat.dataset.seat}) <i class="fa-solid fa-check"></i>`;

        document.getElementById('SelectedSeat').value = seat.dataset.seat;

        // Fiyatı güncelle
        document.getElementById('FinalPrice').value = finalPrice;

    });

    //Buy Button
    btnBuy.addEventListener('click', () => {
        if (currentSelectedSeat) {
            document.getElementById('seatForm').submit();
        }
    });
});