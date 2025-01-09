window.hideSplash = () => {
    var btn = document.getElementById('splash');
    btn.style.display = 'none';
};

window.blazorCulture = {
    get: () => localStorage['BlazorCulture'],
    set: (value) => localStorage['BlazorCulture'] = value
};
