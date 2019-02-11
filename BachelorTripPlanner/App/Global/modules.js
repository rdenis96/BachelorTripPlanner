var globalModule = angular.module('globalModule', [
    // Angular modules
    'ngRoute', 'ngAnimate', 'ngCookies', 'ngResource', 'ngSanitize', 'ngTouch', 'ui.bootstrap'

]);

globalModule.config(function ($routeProvider, $locationProvider) {
    $routeProvider
        .when('/', {
            templateUrl: 'AppViews/Home/home.html',
            controller: 'HomeController'
        })
        .when('/home', {
            templateUrl: 'AppViews/Home/home.html',
            controller: 'HomeController'
        })
        .otherwise({
            redirectTo: '/'
        });
    $locationProvider.html5Mode(true);
});