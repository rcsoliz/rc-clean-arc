window.getSelectedOptions = () => {
    const element = document.getElementById('selectCategorias');
    const result = [];

    if (!element) return result;

    for (let option of element.selectedOptions) {
        result.push(option.value);
    }

    return result;
};
