define([
    'require',
    'angular',
    'angular-route'
    ], function(require, angular) {

    angular.module('app', ['ngRoute'])
        .config(['$routeProvider', function($routeProvider) {


            }
        ]);


    // bootstrap Angular after require.js and DOM are ready
    angular.element(document).ready(function () {
        angular.bootstrap(document, ['app']);
    });
});