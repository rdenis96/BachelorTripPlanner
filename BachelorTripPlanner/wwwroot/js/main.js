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
        .when('/welcome', {
            templateUrl: 'AppViews/LandingPage/landingPage.html',
            controller: 'LandingPageController'
        })
        .otherwise({
            redirectTo: '/'
        });
    $locationProvider.html5Mode(true);
});

globalModule.factory('homeRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/home", {},
            {
                getAll: {
                    method: 'GET',
                    url: 'api/home/getAll',
                    isArray: true
                }
            });
    }

]);
globalModule.controller("HomeController",
    ['$scope', 'homeRepository',
        function ($scope, homeRepository) {
            $scope.testScope = 5;

            $scope.getAllUsers = function () {
                console.log(homeRepository);
                var getAllUsersPromise = homeRepository.getAll().$promise;
                getAllUsersPromise.then(function (result) {
                    console.log(result);
                });
            };
        }

    ]);
globalModule.controller("LandingPageController",
    ['$scope', 'homeRepository',
        function ($scope, homeRepository) {
            $scope.landingPage = true;
        }

    ]);