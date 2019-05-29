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

var tripMainPageTabsEnum = {
    MainTab: 'Main',
    SingleTrip: 'Single Trip',
    GroupTrip: 'Group Trip'
};

var tripTypeEnum = {
    Single: 'Single',
    Group: 'Group'
};
globalModule.directive('homeInterestsSlider', function ($timeout, $window) {
    return {
        restrict: 'E',
        scope: {
            interests: '='
        },
        link: function (scope, elem, attrs) {
            scope.currentIndex = 0; // Initially the index is at the first interest

            scope.next = function () {
                scope.currentIndex < scope.interests.length - 1 ? scope.currentIndex++ : scope.currentIndex = 0;
            };

            scope.prev = function () {
                scope.currentIndex > 0 ? scope.currentIndex-- : scope.currentIndex = scope.interests.length - 1;
            };

            scope.openWikipediaTab = function (link) {
                $window.open(link, '_blank');
            };

            scope.$watch('currentIndex', function () {
                scope.interests.forEach(function (interest) {
                    interest.visible = false; // make every interest invisible
                });

                scope.interests[scope.currentIndex].visible = true; // make the current interest visible
            });

            var timer;
            var sliderFunc = function () {
                timer = $timeout(function () {
                    scope.next();
                    timer = $timeout(sliderFunc, 2000);
                }, 2000);
            };

            sliderFunc();

            scope.$on('$destroy', function () {
                $timeout.cancel(timer); // when the scope is getting destroyed, cancel the timer
            });
        },
        templateUrl: 'AppViews/Home/home-suggested-interests-slidetemplate.html'
    };
});
globalModule.factory('homeRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/home", {},
            {
                getSuggestedInterests: {
                    method: 'GET',
                    url: 'api/home/getSuggestedInterests',
                    isArray: true
                },
                getRandomInterests: {
                    method: 'GET',
                    url: 'api/home/getRandomInterests',
                    isArray: true
                }
            });
    }

]);
globalModule.controller("HomeController",
    ['$scope', '$window', '$localStorage', 'homeRepository', 'toastr',
        function ($scope, $window, $localStorage, homeRepository, toastr) {
            $scope.userId = undefined;
            $scope.suggestedInterests = [];
            $scope.suggestedInterestsLoaded = false;
            $scope.randomInterests = [];
            $scope.randomInterestsLoaded = false;

            $scope.init = function () {
                $scope.userId = $localStorage.TPUserId;
                $scope.initSuggestedInterests();
            };

            $scope.initSuggestedInterests = function () {
                var getSuggestedInterestsPromise = homeRepository.getSuggestedInterests({ userId: $scope.userId }).$promise;
                getSuggestedInterestsPromise.then(function (result) {
                    $scope.suggestedInterests = result;
                    $scope.suggestedInterestsLoaded = true;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });

                var getRandomInterestsPromise = homeRepository.getRandomInterests().$promise;
                getRandomInterestsPromise.then(function (result) {
                    $scope.randomInterests = result;
                    $scope.randomInterestsLoaded = true;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.init();
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
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userInterest: $scope.userInterest
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            };

            $scope.openWeatherModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'AppViews/Account/interest-weather-modal.html',
                    controller: 'AccountInterestsController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userInterest: $scope.userInterest
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            };

            $scope.openTransportsModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'AppViews/Account/interest-transport-modal.html',
                    controller: 'AccountInterestsController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userInterest: $scope.userInterest
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            };

            $scope.openTouristAttractionsModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'AppViews/Account/interest-tourist-attractions-modal.html',
                    controller: 'AccountInterestsController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userInterest: $scope.userInterest
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            };

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
                getAvailableCities: {
                    method: 'GET',
                    url: 'api/account/GetAvailableCities',
                    isArray: true
                },
                getUserCitiesByUserCountriesAndAvailableCities: {
                    method: 'GET',
                    url: 'api/account/GetUserCitiesByUserCountriesAndAvailableCities'
                },
                getAvailableWeather: {
                    method: 'GET',
                    url: 'api/account/GetAvailableWeather',
                    isArray: true
                },
                getAvailableTransport: {
                    method: 'GET',
                    url: 'api/account/GetAvailableTransport',
                    isArray: true
                },
                getAvailableTouristAttractions: {
                    method: 'GET',
                    url: 'api/account/GetAvailableTouristAttractions',
                    isArray: true
                },
                update: {
                    method: 'PUT',
                    url: 'api/account/update'
                },
                updateCountriesAndCities: {
                    method: 'PUT',
                    url: 'api/account/updateInterestByCountryAndCity'
                },
                updateWeather: {
                    method: 'PUT',
                    url: 'api/account/updateInterestByWeather'
                },
                updateTransport: {
                    method: 'PUT',
                    url: 'api/account/updateInterestByTransport'
                },
                updateTouristAttractions: {
                    method: 'PUT',
                    url: 'api/account/updateInterestByTouristAttractions'
                }
            });
    }

]);
globalModule.controller("AccountInterestsController",
    ['$scope', 'data', '$localStorage', 'accountRepository', 'toastr', '$uibModalInstance',
        function ($scope, data, $localStorage, accountRepository, toastr, $uibModalInstance) {
            $scope.userInterest = data.userInterest;
            $scope.countriesList = [];
            $scope.citiesList = [];
            $scope.weatherList = [];
            $scope.transportList = [];
            $scope.touristAttractionsList = [];
            $scope.userWeather = [];
            $scope.userTransport = [];
            $scope.userTouristAttractions = [];

            $scope.initCountriesCities = function () {
                $scope.userId = $localStorage.TPUserId;

                $scope.getAvailableCountriesPromise = accountRepository.getAvailableCountries({ userId: $scope.userId }).$promise;
                $scope.getAvailableCountriesPromise.then(function (result) {
                    $scope.countriesList = result;
                    $scope.reInitCities();
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.reInitCities = function () {
                var queryParam = {
                    userId: $scope.userId,
                    userCountries: $scope.userInterest.countries
                };
                $scope.getUserCitiesByUserCountriesAndAvailableCitiesPromise = accountRepository.getUserCitiesByUserCountriesAndAvailableCities(queryParam).$promise;
                $scope.getUserCitiesByUserCountriesAndAvailableCitiesPromise.then(function (result) {
                    $scope.citiesList = result.availableCities;
                    $scope.userInterest.cities = result.userCities;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.initWeather = function () {
                $scope.userId = $localStorage.TPUserId;

                $scope.getAvailableWeatherPromise = accountRepository.getAvailableWeather({ userId: $scope.userId }).$promise;
                $scope.getAvailableWeatherPromise.then(function (result) {
                    $scope.weatherList = result;
                    $scope.userWeather = $scope.extractListFromString($scope.userInterest.weather);
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.initTransport = function () {
                $scope.userId = $localStorage.TPUserId;

                $scope.getAvailableTransportPromise = accountRepository.getAvailableTransport({ userId: $scope.userId }).$promise;
                $scope.getAvailableTransportPromise.then(function (result) {
                    $scope.transportList = result;
                    $scope.userTransport = $scope.extractListFromString($scope.userInterest.transports);
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.initTouristAttractions = function () {
                $scope.userId = $localStorage.TPUserId;

                $scope.getAvailableTouristAttractionsPromise = accountRepository.getAvailableTouristAttractions({ userId: $scope.userId }).$promise;
                $scope.getAvailableTouristAttractionsPromise.then(function (result) {
                    $scope.touristAttractionsList = result;
                    $scope.userTouristAttractions = $scope.userInterest.touristAttractions;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.addCountry = function () {
                $scope.userInterest.countries.push($scope.countriesList.selected);
                var index = $scope.countriesList.indexOf($scope.countriesList.selected);
                $scope.countriesList.splice(index, 1);
                $scope.countriesList.selected = undefined;
                $scope.reInitCities();
            };

            $scope.removeCountry = function () {
                $scope.countriesList.push($scope.userInterest.countries.selected);
                var index = $scope.userInterest.countries.indexOf($scope.userInterest.countries.selected);
                $scope.userInterest.countries.splice(index, 1);
                $scope.userInterest.countries.selected = undefined;
                $scope.reInitCities();
            };

            $scope.addCity = function () {
                $scope.userInterest.cities.push($scope.citiesList.selected);
                var index = $scope.citiesList.indexOf($scope.citiesList.selected);
                $scope.citiesList.splice(index, 1);
                $scope.citiesList.selected = undefined;
            };

            $scope.removeCity = function () {
                $scope.citiesList.push($scope.userInterest.cities.selected);
                var index = $scope.userInterest.cities.indexOf($scope.userInterest.cities.selected);
                $scope.userInterest.cities.splice(index, 1);
                $scope.userInterest.cities.selected = undefined;
            };

            $scope.addWeather = function () {
                $scope.userWeather.push($scope.weatherList.selected);
                var index = $scope.weatherList.indexOf($scope.weatherList.selected);
                $scope.weatherList.splice(index, 1);
                $scope.weatherList.selected = undefined;
            };

            $scope.removeWeather = function () {
                $scope.weatherList.push($scope.userWeather.selected);
                var index = $scope.userWeather.indexOf($scope.userWeather.selected);
                $scope.userWeather.splice(index, 1);
                $scope.userWeather.selected = undefined;
            };

            $scope.addTransport = function () {
                $scope.userTransport.push($scope.transportList.selected);
                var index = $scope.transportList.indexOf($scope.transportList.selected);
                $scope.transportList.splice(index, 1);
                $scope.transportList.selected = undefined;
            };

            $scope.removeTransport = function () {
                $scope.transportList.push($scope.userTransport.selected);
                var index = $scope.userTransport.indexOf($scope.userTransport.selected);
                $scope.userTransport.splice(index, 1);
                $scope.userTransport.selected = undefined;
            };

            $scope.addTouristAttractions = function () {
                $scope.userTouristAttractions.push($scope.touristAttractionsList.selected);
                var index = $scope.touristAttractionsList.indexOf($scope.touristAttractionsList.selected);
                $scope.touristAttractionsList.splice(index, 1);
                $scope.touristAttractionsList.selected = undefined;
            };

            $scope.removeTouristAttractions = function () {
                $scope.touristAttractionsList.push($scope.userTouristAttractions.selected);
                var index = $scope.userTouristAttractions.indexOf($scope.userTouristAttractions.selected);
                $scope.userTouristAttractions.splice(index, 1);
                $scope.userTouristAttractions.selected = undefined;
            };

            $scope.submitCountriesAndCities = function () {
                var queryParam = {
                    countries: $scope.userInterest.countries,
                    cities: $scope.userInterest.cities
                };
                $scope.updateCountriesAndCitiesForUserPromise = accountRepository.updateCountriesAndCities({ userId: $scope.userId }, queryParam).$promise;
                $scope.updateCountriesAndCitiesForUserPromise.then(function (result) {
                    if (result == null) {
                        toastr.warning("Changes were not made, please try again!");
                    }
                    $uibModalInstance.close();
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.submitWeather = function () {
                var queryParam = {
                    weather: $scope.userWeather
                };
                $scope.updateWeatherForUserPromise = accountRepository.updateWeather({ userId: $scope.userId }, queryParam).$promise;
                $scope.updateWeatherForUserPromise.then(function (result) {
                    if (result == null) {
                        toastr.warning("Changes were not made, please try again!");
                    }
                    $uibModalInstance.close();
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.submitTransport = function () {
                var queryParam = {
                    transport: $scope.userTransport
                };
                $scope.updateTransportForUserPromise = accountRepository.updateTransport({ userId: $scope.userId }, queryParam).$promise;
                $scope.updateTransportForUserPromise.then(function (result) {
                    if (result == null) {
                        toastr.warning("Changes were not made, please try again!");
                    }
                    $uibModalInstance.close();
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.submitTouristAttractions = function () {
                var queryParam = {
                    touristAttractions: $scope.userTouristAttractions
                };
                $scope.updateTouristAttractionsForUserPromise = accountRepository.updateTouristAttractions({ userId: $scope.userId }, queryParam).$promise;
                $scope.updateTouristAttractionsForUserPromise.then(function (result) {
                    if (result == null) {
                        toastr.warning("Changes were not made, please try again!");
                    }
                    $uibModalInstance.close();
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.extractListFromString = function (stringEnum) {
                var list = [];
                var array = stringEnum.replace(/[\s]/g, '').split(',');
                angular.forEach(array, function (value, key) {
                    list.push(value);
                });
                return list;
            };

            $scope.close = function () {
                $uibModalInstance.close();
            };
        }

    ]);
globalModule.controller("PlanningHistoryController",
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
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userInterest: $scope.userInterest
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            };

            $scope.openWeatherModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'AppViews/Account/interest-weather-modal.html',
                    controller: 'AccountInterestsController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userInterest: $scope.userInterest
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            };

            $scope.openTransportsModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'AppViews/Account/interest-transport-modal.html',
                    controller: 'AccountInterestsController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userInterest: $scope.userInterest
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            };

            $scope.openTouristAttractionsModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'AppViews/Account/interest-tourist-attractions-modal.html',
                    controller: 'AccountInterestsController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userInterest: $scope.userInterest
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            };

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
globalModule.controller("TripCreateController",
    ['$scope', '$window', '$http', '$localStorage', 'tripCreateRepository', 'toastr',
        function ($scope, $window, $http, $localStorage, tripCreateRepository, toastr) {
            $scope.invitedPersonEmail = "";
            $scope.invitedPeople = [];

            $scope.tripMainPageTabsEnum = tripMainPageTabsEnum;
            $scope.tripTypeEnum = tripTypeEnum;
            $scope.selectedTab = tripMainPageTabsEnum.MainTab;

            $scope.init = function () {
                $scope.userId = $localStorage.TPUserId;
            };

            $scope.changeTab = function (tab) {
                switch (tab) {
                    case tripMainPageTabsEnum.MainTab: {
                        $scope.selectedTab = tripMainPageTabsEnum.MainTab;
                        break;
                    }
                    case tripMainPageTabsEnum.SingleTrip: {
                        $scope.selectedTab = tripMainPageTabsEnum.SingleTrip;
                        break;
                    }
                    case tripMainPageTabsEnum.GroupTrip: {
                        $scope.selectedTab = tripMainPageTabsEnum.GroupTrip;
                        break;
                    }
                }
            };

            $scope.goBackToMainTab = function () {
                $scope.selectedTab = tripMainPageTabsEnum.MainTab;
            };

            $scope.addInvitedPerson = function () {
                $scope.invitedPeople.push($scope.invitedPersonEmail);
                $scope.invitedPersonEmail = "";
            };

            $scope.removeInvitedPerson = function () {
                var index = $scope.invitedPeople.indexOf($scope.invitedPeople.selected);
                $scope.invitedPeople.splice(index, 1);
                $scope.invitedPeople.selected = undefined;
            };

            $scope.createTrip = function (tripType) {
                var queryParam = {
                    tripName: $scope.tripName,
                    tripType: tripType
                };
                if (tripType == tripTypeEnum.Group) {
                    angular.extend(queryParam, { invitedPeople: $scope.invitedPeople });
                }
                var createTripPromise = tripCreateRepository.createTrip({ userId: $scope.userId }, queryParam).$promise;
                createTripPromise.then(function (result) {
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.init();
        }

    ]);
globalModule.factory('tripCreateRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/trip", {},
            {
                createTrip: {
                    method: 'POST',
                    url: 'api/trip/createTrip'
                }
            });
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