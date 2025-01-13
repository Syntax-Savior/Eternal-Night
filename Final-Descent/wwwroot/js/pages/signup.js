
// Password Limit Check Start.
document.addEventListener('input', function(event) {
    if (event.target.name === 'password' && event.target.value.length === 39) {
        alert('Maximum password length reached (40 characters).');
    }
});
// Password Limit Check End.

// Password Strength Indicator Animation Start.
document.addEventListener('DOMContentLoaded', function() {
  const passwordInput = document.getElementById('password');
  const strengthBar = document.getElementById('passwordStrengthBar');
  const strengthText = document.getElementById('passwordStrengthText');
  
  passwordInput.addEventListener('input', function(e) {
      const passwordValue = e.target.value;
      let strength = 0;

      // Reset display styles if the password is empty
      if (passwordValue.length === 0) {
          strengthBar.style.display = 'none';
          strengthText.style.display = 'none';
          return; // Exit early if no password has been typed
      }

      // Only display the bar when there's input
      strengthBar.style.display = 'block';
      strengthText.style.display = 'block';

      if (passwordValue.length >= 6) {
          strength += 1; // Length 6 or more
      }
      if (passwordValue.match(/[a-z]+/)) {
          strength += 1; // Lowercase letters
      }
      if (passwordValue.match(/[A-Z]+/)) {
          strength += 1; // Uppercase letters
      }
      if (passwordValue.match(/[0-9]+/)) {
          strength += 1; // Numbers
      }
      if (passwordValue.match(/[\W]+/)) {
          strength += 1; // Special characters
      }

      switch(strength) {
          case 1:
              strengthBar.style.width = "20%";
              strengthBar.style.backgroundColor = "red";
              strengthText.textContent = "Weak";
              break;
          case 2:
              strengthBar.style.width = "40%";
              strengthBar.style.backgroundColor = "orange";
              strengthText.textContent = "Fair";
              break;
          case 3:
              strengthBar.style.width = "60%";
              strengthBar.style.backgroundColor = "yellow";
              strengthText.textContent = "Medium";
              break;
          case 4:
              strengthBar.style.width = "80%";
              strengthBar.style.backgroundColor = "lightgreen";
              strengthText.textContent = "Strong";
              break;
          case 5:
              strengthBar.style.width = "100%";
              strengthBar.style.backgroundColor = "green";
              strengthText.textContent = "Very Strong";
              break;
          default:
              strengthBar.style.width = "0%";
              strengthBar.style.backgroundColor = "red";
              strengthText.textContent = "Weak";
      }
  });
});
// Password Strength Indicator Animation End.

// Client Side Password Confirmation Check Start.
document.addEventListener('DOMContentLoaded', function() {
  const form = document.querySelector('.sign-up-form');
  const passwordInput = document.getElementById('password');
  const confirmPasswordInput = document.getElementById('confirm_password');

  form.addEventListener('submit', function(event) {
      if (passwordInput.value !== confirmPasswordInput.value) {
          event.preventDefault(); // Prevent form submission
          // Display an error message
          alert('The passwords do not match.');
      }
  });
});
// Client Side Password Confirmation Check End.
