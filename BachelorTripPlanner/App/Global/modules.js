﻿var globalModule = angular.module('globalModule', [
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
            .when('/account/planningHistory', {
                templateUrl: 'AppViews/Account/planning-history.html',
                controller: 'PlanningHistoryController'
            })
            .when('/account/editAccount', {
                templateUrl: 'AppViews/Account/edit-account.html',
                controller: 'AccountController'
            })
            .when('/trip/createTrip', {
                templateUrl: 'AppViews/Trip/trip-create.html',
                controller: 'TripCreateController'
            })
            .when('/trip/tripPlanner/:id', {
                templateUrl: 'AppViews/Trip/trip-planner.html',
                controller: 'TripPlannerController'
            })
            .when('/account/friends', {
                templateUrl: 'AppViews/Account/friends.html',
                controller: 'FriendsController'
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