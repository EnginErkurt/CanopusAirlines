        // Contact Form
        function handleSubmit(event) {
            event.preventDefault();
            
            var name = document.getElementById('name').value;
            var email = document.getElementById('email').value;
            var phone = document.getElementById('phone').value;
            var subject = document.getElementById('subject').value;
            var message = document.getElementById('message').value;

            if (!name || !email || !subject || !message) {
                alert('Please fill all the required fields (*)');
                return false;
            }

            alert('Your message has been sent.\n\n' +
                  'Name Surname: ' + name + '\n' +
                  'E-mail: ' + email + '\n' +
                  'Tel: ' + (phone || 'Belirtilmedi') + '\n' +
                  'Subject: ' + subject + '\n' +
                  'Message: ' + message + '\n\n' +
                  'We will reply as soon as possible.');

            // Reset form
            event.target.reset();
            return false;
        }

        //FAQ toggle
        function toggleFaq(element) {
            var allItems = document.querySelectorAll('.faq-item');
            allItems.forEach(function(item) {
                if (item !== element) {
                    item.classList.remove('active');
                }
            });
            element.classList.toggle('active');
        }