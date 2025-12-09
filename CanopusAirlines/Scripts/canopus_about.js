        // Page animations
        function animateValue(id, start, end, duration) {
            var obj = document.getElementById(id);
            var range = end - start;
            var current = start;
            var increment = end > start ? 1 : -1;
            var stepTime = Math.abs(Math.floor(duration / range));
            
            var timer = setInterval(function() {
                current += increment;
                obj.innerHTML = current;
                if (current == end) {
                    clearInterval(timer);
                }
            }, stepTime);
        }

        window.onload = function() {
            animateValue("flights", 0, 500, 2000);
            animateValue("destinations", 0, 150, 2000);
            animateValue("passengers", 0, 25, 2000);
        };