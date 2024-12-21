document.addEventListener("DOMContentLoaded", function () {
    const alert = document.getElementById("timer-alert");
    if (alert) {
        setTimeout(() => {
            alert.style.transition = "opacity 0.5s ease";
            alert.style.opacity = "0";
            setTimeout(() => alert.remove(), 500);
        }, 5000); // 5000ms = 5 segundos
    }
});

document.getElementById("changePasswordCheckbox").addEventListener("change", function () {
    const passwordInput = document.getElementById("passwordInput");
    passwordInput.disabled = !this.checked;
    if (!this.checked) {
        passwordInput.value = ""; // Limpa o campo se desmarcado
    }
});