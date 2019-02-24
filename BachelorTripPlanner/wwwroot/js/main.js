var globalModule = angular.module('globalModule', [
    // Angular modules
    'ngRoute', 'ngAnimate', 'ngCookies', 'ngResource', 'ngSanitize', 'ngTouch', 'ngStorage', 'ui.bootstrap', 'toastr'
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
var landingPageTabsEnum = {
    Welcome: 'Welcome',
    Register: 'Register',
    Login: 'Login'
};

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
globalModule.controller("HeaderController",
    ['$scope', '$localStorage', 'homeRepository',
        function ($scope, $localStorage, homeRepository) {
            $scope.isLogged = $localStorage.TPUserId !== null && $localStorage.TPUserId !== undefined;
            $scope.init = function () {
                if ($scope.isLogged == true)
                    $scope.userId = $localStorage.TPUserId;
            };
        }

    ]);
globalModule.controller("LandingPageController",
    ['$scope', '$http', '$localStorage', 'landingPageRepository', 'toastr',
        function ($scope, $http, $localStorage, landingPageRepository, toastr) {
            $scope.landingPage = true;
            $scope.landingPageTabsEnum = landingPageTabsEnum;
            $scope.selectedTab = landingPageTabsEnum.Welcome;

            $scope.changeTab = function (tab) {
                switch (tab) {
                    case landingPageTabsEnum.Welcome: {
                        $scope.selectedTab = landingPageTabsEnum.Welcome;
                        break;
                    }
                    case landingPageTabsEnum.Login: {
                        $scope.selectedTab = landingPageTabsEnum.Login;
                        break;
                    }
                    case landingPageTabsEnum.Register: {
                        $scope.selectedTab = landingPageTabsEnum.Register;
                        break;
                    }
                }
            };

            $scope.register = function () {
                $http.get('https://ipapi.co/json/').success(function (response) {
                    $scope.registerIp = response.ip;

                    var registerParamModel = {
                        email: $scope.registerEmail,
                        password: $scope.registerPassword,
                        registerDate: new Date(),
                        lastOnline: null,
                        ip: $scope.registerIp,
                        phone: $scope.registerPhoneNumber != undefined && $scope.registerPhoneNumber.length > 0 ? $scope.registerPhoneNumber : null
                    };

                    var registerUserPromise = landingPageRepository.register(registerParamModel).$promise;
                    registerUserPromise.then(function (result) {
                        toastr.success(result.message);
                        $scope.changeTab(landingPageTabsEnum.Login);
                    }).catch(function (result) {
                        toastr.warning(result.data);
                    });
                });
            };

            $scope.login = function () {
                var loginParamModel = {
                    email: $scope.loginEmail,
                    password: $scope.loginPassword
                };

                var loginUserPromise = landingPageRepository.login(loginParamModel).$promise;
                loginUserPromise.then(function (result) {
                    $localStorage.TPUserId = result.userId;
                    $window.location.href = '/home';
                    toastr.success(result.message + $localStorage.TPUserId);
                }).catch(function (result) {
                    console.log(result);
                    toastr.warning(result.data);
                });
            };
        }

    ]);
globalModule.factory('landingPageRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/landingpage", {},
            {
                register: {
                    method: 'POST',
                    url: 'api/landingpage/register'
                },
                login: {
                    method: 'GET',
                    url: 'api/landingpage/login'
                }
            });
    }

]);