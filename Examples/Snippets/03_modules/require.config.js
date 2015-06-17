requirejs.config({
    paths: {
        'jquery': '../../js/jquery/jquery-1.11.1',
        'knockout': '../../js/knockout-3.1.0/knockout.debug',
        'domReady': '../../js/domReady',
        'angular': '../../js/angular-1.3.0/angular'
    },
    
    shim: {
        'angular': {
            exports: 'angular'
        }
    }
});