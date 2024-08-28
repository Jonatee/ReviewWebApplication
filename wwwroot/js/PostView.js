document.addEventListener('DOMContentLoaded', function () {
    const commentBtn = document.getElementById('commentBtn');
    const commentOverlay = document.getElementById('commentOverlay');
    const closeBtn = document.getElementById('closeBtn');
    const cancelBtn = document.getElementById('cancelBtn');
    const fileInput = document.getElementById('fileInput');
    const fileLabel = document.getElementById('fileLabel');

    // Open the overlay
    commentBtn.addEventListener('click', function () {
        commentOverlay.classList.add('show');
    });

    // Close the overlay
    closeBtn.addEventListener('click', function () {
        commentOverlay.classList.remove('show');
    });

    // Close the overlay when Cancel button is clicked
    cancelBtn.addEventListener('click', function () {
        commentOverlay.classList.remove('show');
    });

    // Update file input label when file is selected
    fileInput.addEventListener('change', function () {
        if (fileInput.files.length > 0) {
            fileLabel.textContent = fileInput.files[0].name;
        } else {
            fileLabel.textContent = "Choose file";
        }
    });
});
