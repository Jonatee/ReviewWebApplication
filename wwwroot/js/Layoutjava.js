﻿document.getElementById('toggle-sidebar').addEventListener('click', function () {
    const sidebar = document.querySelector('.sidebar');
    sidebar.classList.toggle('expanded');
    this.textContent = sidebar.classList.contains('expanded') ? 'X' : '☰';
});

function showToast(message) {
    const toast = document.getElementById('toast');
    toast.textContent = message;
    toast.className = 'show';
    setTimeout(() => {
        toast.className = toast.className.replace('show', '');
    }, 3000);
}
