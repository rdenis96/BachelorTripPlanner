var globalModule = angular.module('globalModule', [
    // Angular modules
    'ngRoute', 'ngAnimate', 'ngCookies', 'ngResource', 'ngSanitize', 'ngTouch', 'ngStorage', 'ui.bootstrap', 'ui.select', 'toastr', 'cgBusy'
]);

globalModule.config([
    '$routeProvider', '$locationProvider', 'toastrConfig',
    function ($routeProvider, $locationProvider, toastrConfig) {
        $routeProvider
            .when('/', {
                templateUrl: 'AppViews/Home/home.html',
                controller: 'HomeController'
            })
            .when('/home', {
                templateUrl: 'AppViews/Home/home.html',
                controller: 'HomeController'
            })
            .when('/welcome', {
                templateUrl: 'AppViews/LandingPage/landingPage.html',
                controller: 'LandingPageController'
            })
            .when('/account/interests', {
                templateUrl: 'AppViews/Account/interests.html',
                controller: 'AccountController'
            })
            .when('/account/editAccount', {
                templateUrl: 'AppViews/Account/edit-account.html',
                controller: 'AccountController'
            })
            .otherwise({
                redirectTo: '/'
            });

        angular.extend(toastrConfig, {
            autoDismiss: false,
            maxOpened: 1,
            newestOnTop: true,
            positionClass: 'toast-top-center',
            preventDuplicates: false,
            preventOpenDuplicates: false,
            target: 'body',
            closeButton: true,
            extendedTimeOut: 5000,
            timeOut: 5000
        });

        $locationProvider.html5Mode(true);
    }]);