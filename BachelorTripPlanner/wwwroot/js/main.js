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
    ['$scope', '$window', '$localStorage', 'homeRepository',
        function ($scope, $window, $localStorage, homeRepository) {
            $scope.init = function () {
            };
        }

    ]);
globalModule.controller("HeaderController",
    ['$scope', '$window', '$localStorage', '$uibModal', 'homeRepository',
        function ($scope, $window, $localStorage, $uibModal, homeRepository) {
            $scope.isLogged = $localStorage.TPUserId !== null && $localStorage.TPUserId !== undefined;
            $scope.init = function () {
                if ($scope.isLogged === true)
                    $scope.userId = $localStorage.TPUserId;
                else {
                    if ($window.location.href.indexOf('/welcome') == -1) {
                        $window.location.href = '/welcome';
                    }
                }
            };

            $scope.login = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'AppViews/LandingPage/login-modal.html',
                    controller: 'LoginModalController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg'
                });

                modalInstance.result.then(function () {
                }, function () {
                });
            };

            $scope.logout = function () {
                $localStorage.TPUserId = null;
                $window.location.href = '/welcome';

            };
        }

    ]);
globalModule.controller("LandingPageController",
    ['$scope', '$window', '$http', '$localStorage', 'landingPageRepository', 'toastr',
        function ($scope, $window, $http, $localStorage, landingPageRepository, toastr) {
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
                    toastr.success(result.message);
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
globalModule.controller("AccountController",
    ['$scope', '$localStorage', '$uibModal', 'accountRepository', 'toastr',
        function ($scope, $localStorage, $uibModal, accountRepository, toastr) {
            $scope.user = {};
            $scope.userInterests = {};

            $scope.initEditAccount = function () {
                $scope.userId = $localStorage.TPUserId;
                var getUserPromise = accountRepository.getUser({ userId: $scope.userId }).$promise;
                getUserPromise.then(function (result) {
                    $scope.user = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.initInterests = function () {
                $scope.userId = $localStorage.TPUserId;
                $scope.getUserInterestPromise = accountRepository.getUserInterest({ userId: $scope.userId }).$promise;
                $scope.getUserInterestPromise.then(function (result) {
                    $scope.userInterest = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.openCountryAndCityModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'AppViews/Account/interest-country-city-modal.html',
                    controller: 'AccountInterestsController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg'
                });

                modalInstance.result.then(function () {
                }, function () {
                });
            }

            //update functions
            $scope.update = function () {
                if ($scope.newPassword != $scope.confPassword) {
                    toastr.warning('The password does not match, please type the same password in Confirm Password field!');
                    return;
                }

                var userUpdateParam = {
                    email: $scope.user.email,
                    password: $scope.newPassword
                };

                var userUpdatePromise = accountRepository.update({ userId: $scope.userId }, userUpdateParam).$promise;
                userUpdatePromise.then(function (result) {
                    toastr.success('The account was updated successfuly!');
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };
        }

    ]);
globalModule.factory('accountRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/account", {},
            {
                getUser: {
                    method: 'GET',
                    url: 'api/account/getUser'
                },
                getUserInterest: {
                    method: 'GET',
                    url: 'api/account/getUserInterest'
                },
                getAvailableCountries: {
                    method: 'GET',
                    url: 'api/account/GetAvailableCountries',
                    isArray: true
                },
                update: {
                    method: 'PUT',
                    url: 'api/account/update'
                },
                updateCountriesAndCities: {
                    method: 'PUT',
                    url: 'api/account/updateCountriesAndCities'
                }
            });
    }

]);
globalModule.controller("AccountInterestsController",
    ['$scope', '$localStorage', 'accountRepository', 'toastr', '$uibModalInstance',
        function ($scope, $localStorage, accountRepository, toastr, $uibModalInstance) {
            $scope.userInterest = {
                countries: [],
                cities: [],
                weather: []
            };
            $scope.countriesList = [];

            $scope.initCountryCity = function () {
                $scope.userId = $localStorage.TPUserId;

                $scope.getAvailableCountriesPromise = accountRepository.getAvailableCountries({ userId: $scope.userId }).$promise;
                $scope.getAvailableCountriesPromise.then(function (result) {
                    $scope.countriesList = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            //update functions
            $scope.updateCountryCity = function () {
                if ($scope.newPassword != $scope.confPassword) {
                    toastr.warning('The password does not match, please type the same password in Confirm Password field!');
                    return;
                }

                var userUpdateParam = {
                    email: $scope.user.email,
                    password: $scope.newPassword
                };

                var userUpdatePromise = accountRepository.updateCountriesAndCities({ userId: $scope.userId }, userUpdateParam).$promise;
                userUpdatePromise.then(function (result) {
                    toastr.success('The account was updated successfuly!');
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.close = function () {
                $uibModalInstance.close();
            };
        }

    ]);
globalModule.controller("LoginModalController",
    ['$scope', '$window', '$localStorage', '$uibModalInstance', 'landingPageRepository', 'toastr',
        function ($scope, $window, $localStorage, $uibModalInstance, landingPageRepository, toastr) {
            $scope.login = function () {
                var loginParamModel = {
                    email: $scope.loginEmail,
                    password: $scope.loginPassword
                };

                var loginUserPromise = landingPageRepository.login(loginParamModel).$promise;
                loginUserPromise.then(function (result) {
                    $uibModalInstance.close();
                    $localStorage.TPUserId = result.userId;
                    $window.location.href = '/home';
                    toastr.success(result.message);
                }).catch(function (result) {
                    console.log(result);
                    toastr.warning(result.data);
                });
            };

            $scope.close = function () {
                $uibModalInstance.close();
            };
        }

    ]);