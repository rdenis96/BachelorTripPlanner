(function () {
    'use strict';

    var globalModule = angular.module('globalModule', [
        // Angular modules
        'ngRoute',
        'ngResource'

    ]);

    globalModule.config(function ($routeProvider) {
        $routeProvider
            .when('/home', {
                templateUrl: 'AppViews/Home/home.html',
                controller: 'HomeController'
            })
            .otherwise({
                redirectTo: '/'
            });
    });
})();