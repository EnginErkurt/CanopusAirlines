let flights = [];
let bookings = [];

//Login
const loginForm = document.getElementById('loginForm');
const errorMsg = document.getElementById('error-msg');
const loginSection = document.getElementById('login-section');
const dashboardSection = document.getElementById('dashboard-section');

loginForm.addEventListener('submit', function (e) {
    e.preventDefault();
    const userIn = document.getElementById('username').value;
    const passIn = document.getElementById('password').value;


    fetch('/Admin/Login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username: userIn, password: passIn })
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                errorMsg.style.display = 'none';
                loadDashboard({ name: data.name, role: data.role });
            } else {
                errorMsg.style.display = 'block';
                errorMsg.innerText = "Email veya şifre hatalı!";
            }
        })
        .catch(err => console.error(err));
});

function loadDashboard(user) {
    loginSection.classList.add('hidden');
    dashboardSection.classList.remove('hidden');

    document.getElementById('welcome-text').innerText = `Welcome, ${user.name}`;
    document.getElementById('admin-name').innerText = user.name;
    document.getElementById('admin-role').innerText = user.role;

    refreshData();
}

function refreshData() {
    fetch('/Admin/GetAllData')
        .then(res => res.json())
        .then(data => {
            flights = data.flights; 
            bookings = data.bookings; 

            renderFlights();
            renderPassengers();
            calculateStats();
        });
}

function calculateStats() {
    const totalTickets = bookings.length;

    const activeFlights = flights.filter(f => f.status && f.status !== "Cancelled" && f.status !== "Landed").length;
    let totalRevenue = 0;

    bookings.forEach(booking => {
        const flight = flights.find(f => f.no === booking.flight);
        if (flight) {
            totalRevenue += flight.price; 
        }
    });

    document.getElementById('total-revenue').innerText = "$" + totalRevenue.toLocaleString();
    document.getElementById('total-tickets').innerText = totalTickets;
    document.getElementById('active-flights').innerText = activeFlights;
}

function renderFlights(dataToRender) {
    const tbody = document.getElementById('flight-table-body');
    tbody.innerHTML = "";

    const data = dataToRender || flights;

    data.forEach(f => {
        let statusColor = "color:green";
        if (f.status === "Delayed") statusColor = "color:orange";
        if (f.status === "Cancelled") statusColor = "color:red";
        if (f.status === "Gate Closed" || f.status === "Landed") statusColor = "color:gray";

        let row = `
            <tr>
                <td><b>${f.no}</b></td>
                <td>${f.from}</td>
                <td>${f.to}</td>
                <td>${f.date}</td>
                <td style="${statusColor} font-weight:bold;">${f.status}</td>
                <td><button class="action-btn" onclick="editFlight('${f.no}')">Edit</button></td>
            </tr>
        `;
        tbody.innerHTML += row;
    });
}

function renderPassengers() {
    const tbody = document.getElementById('passenger-table-body');
    tbody.innerHTML = "";

    bookings.forEach(b => {
        let row = `
            <tr>
                <td><code>${b.pnr}</code></td>
                <td>${b.name}</td>
                <td>${b.flight}</td>
                <td>${b.seat}</td>
                <td><span style="padding: 2px 8px; background: #eee; border-radius: 4px;">${b.flight_class}</span></td>
                <td>
                    <button class="btn-small-delete" onclick="deleteBooking('${b.pnr}')">
                        <i class="fas fa-trash"></i> Cancel
                    </button>
                </td>
            </tr>
        `;
        tbody.innerHTML += row;
    });
}

function filterFlights() {
    const query = document.getElementById('searchInput').value.toLowerCase();
    const filteredFlights = flights.filter(f =>
        f.no.toLowerCase().includes(query) ||
        f.from.toLowerCase().includes(query) ||
        f.to.toLowerCase().includes(query)
    );
    renderFlights(filteredFlights);
}

function deleteBooking(pnr) {
    if (confirm(`PNR: ${pnr} numaralı bileti iptal etmek istiyor musunuz?`)) {
        fetch('/Admin/CancelTicket', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ pnr: pnr })
        }).then(res => res.json()).then(data => {
            if (data.success) {
                refreshData(); 
            } else {
                alert("Hata oluştu!");
            }
        });
    }
}

function showPage(pageId) {
    document.getElementById('flights-page').classList.add('hidden');
    document.getElementById('passengers-page').classList.add('hidden');
    document.getElementById(pageId + '-page').classList.remove('hidden');

    const links = document.querySelectorAll('.nav-links li');
    links.forEach(l => l.classList.remove('active-link'));

}

function logout() { location.reload(); }

let currentFlightNo = null;

function editFlight(flightNo) {
    const flight = flights.find(f => f.no === flightNo);
    currentFlightNo = flightNo;

    if (flight) {
        document.getElementById('edit-no').value = flight.no;

        document.getElementById('edit-from').value = flight.from;
        document.getElementById('edit-to').value = flight.to;
        document.getElementById('edit-status').value = flight.status;


        let dateForInput = flight.date.replace(' ', 'T');
        document.getElementById('edit-date').value = dateForInput;

        document.getElementById('editModal').classList.remove('hidden');
    }
}

function saveFlight() {
    const newStatus = document.getElementById('edit-status').value;
    const newDateRaw = document.getElementById('edit-date').value; 
    const newFrom = document.getElementById('edit-from').value; 
    const newTo = document.getElementById('edit-to').value; 

    fetch('/Admin/EditFlight', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            no: currentFlightNo,
            from: newFrom,
            to: newTo,
            date: newDateRaw,
            status: newStatus
        })
    }).then(res => res.json()).then(data => {
        if (data.success) {
            refreshData();
            closeModal();
        } else {
            alert("Update failed!");
        }
    });
}

function deleteFlight() {
    if (confirm(`Caution: You are about to delete ${currentFlightNo} flight. All tickets for this flight will also be deleted!`)) {
        fetch('/Admin/DeleteFlight', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ flightNo: currentFlightNo })
        }).then(res => res.json()).then(data => {
            if (data.success) {
                refreshData();
                closeModal();
            } else {
                alert("Silme başarısız!");
            }
        });
    }
}

function closeModal() {
    document.getElementById('editModal').classList.add('hidden');
    currentFlightNo = null;
}

//Add new flight
function openAddModal() {
    document.getElementById('addForm').reset();
    document.getElementById('addModal').classList.remove('hidden');
}

function closeAddModal() {
    document.getElementById('addModal').classList.add('hidden');
}

function addNewFlight() {
    const no = document.getElementById('add-no').value;
    const from = document.getElementById('add-from').value;
    const to = document.getElementById('add-to').value; 
    const price = document.getElementById('add-price').value;
    const seats = document.getElementById('add-seats').value;
    const status = document.getElementById('add-status').value;
    const dateRaw = document.getElementById('add-date').value;

    if (no === "" || from === "" || to === "" || dateRaw === "") {
        alert("Please fill in all fields!");
        return;
    }

    fetch('/Admin/AddFlight', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            no: no,
            fromIata: from.toUpperCase(), 
            toIata: to.toUpperCase(), 
            price: price,
            seats: seats,
            date: dateRaw,
            status: status
        })
    }).then(res => res.json()).then(data => {
        if (data.success) {
            refreshData();
            closeAddModal();
        } else {
            alert("Error: " + (data.message || "The flight could not be added. Please ensure you have entered the IATA codes (IST, ESB, etc.) correctly."));
        }
    });
}

window.onclick = function (event) {
    if (event.target == document.getElementById('editModal')) closeModal();
    if (event.target == document.getElementById('addModal')) closeAddModal();
}