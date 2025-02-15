function autoExpand(textarea) {
    textarea.style.height = 'auto'; // Сброс высоты
    textarea.style.height = (textarea.scrollHeight) + 'px'; // Установка новой высоты
}

// Экспортируем функцию для использования в Blazor
window.initializeAutoExpand = function (textareaId) {
    const textarea = document.getElementById(textareaId); // Находим элемент по ID
    if (textarea) {
        textarea.addEventListener('input', () => autoExpand(textarea));
        autoExpand(textarea); // Инициализация при загрузке
    }
};

window.updateTextarea = function (id, value) {
    const textarea = document.getElementById(id);
    if (textarea) {
        textarea.value = value;
    }
}