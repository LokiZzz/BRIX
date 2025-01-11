var splash = document.getElementById('splash');
splash.style.display = 'flex';

window.hideSplash = () => {
    var splash = document.getElementById('splash');
    splash.style.display = 'none';
};

window.blazorCulture = {
    get: () => localStorage['BlazorCulture'],
    set: (value) => localStorage['BlazorCulture'] = value
};