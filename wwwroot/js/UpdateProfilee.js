document.addEventListener('DOMContentLoaded', function () {
    const fileInput = document.querySelector('input[type="file"]');
    const previewContainer = document.getElementById('image-preview');
    const previewImage = document.createElement('img');
    previewContainer.appendChild(previewImage);

    fileInput.addEventListener('change', function (e) {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();

            reader.onload = function (e) {
                previewImage.src = e.target.result;
                previewContainer.style.display = 'block';
            }

            reader.readAsDataURL(file);
        } else {
            previewContainer.style.display = 'none';
        }
    });

    document.querySelector('.btn-file-input').addEventListener('click', function () {
        fileInput.click();
    });
});