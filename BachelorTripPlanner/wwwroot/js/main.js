var globalModule = angular.module('globalModule', [
    // Angular modules
    'ngRoute', 'ngAnimate', 'ngCookies', 'ngResource', 'ngSanitize', 'ngTouch', 'ngStorage', 'ui.bootstrap', 'ui.select', 'toastr', 'cgBusy'
]);

globalModule.config([
    '$routeProvider', '$httpProvider', '$locationProvider', 'toastrConfig',
    function ($routeProvider, $httpProvider, $locationProvider, toastrConfig) {
        $httpProvider.interceptors.push('authInterceptor');
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
var httpStatusCodeEnum = {
    OK: 200,
    NotModified: 304,
    BadRequest: 400,
    Unauthorized: 401,
    Forbidden: 403,
    NotFound: 404,
    InternalServerError: 500,
};

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
    Single: 0,
    Group: 1
};

var notificationTypes = {
    TripInvitation: 0,
    FriendRequest: 1,
    TripKicked: 2
};
globalModule.directive('interestsSlider', function ($timeout, $window) {
    return {
        restrict: 'E',
        scope: {
            interests: '='
        },
        link: function (scope, elem, attrs) {

            var transportsIcons = {
                Bus: 'Bus',
                Metro: 'Metro',
                Train: 'Train',
                Boat: 'Boat',
                Tram: 'Tram',
                Taxi: 'Taxi',
                Underground: 'Underground',
                Subway: 'Subway',
                Helicopter: 'Helicopter',
                Plane: 'Plane',
                Rent: 'Rent'
            }

            scope.getTransportIcon = function (transport) {
                transport = transport.trim();
                switch (transport) {
                    case transportsIcons.Bus:
                        return "Images/Common/BusIcon.png";
                    case transportsIcons.Metro:
                        return "Images/Common/MetroIcon.png";
                    case transportsIcons.Train:
                        return "Images/Common/TrainIcon.png";
                    case transportsIcons.Boat:
                        return "Images/Common/BoatIcon.png";
                    case transportsIcons.Tram:
                        return "Images/Common/TramIcon.png";
                    case transportsIcons.Taxi:
                        return "Images/Common/TaxiIcon.png";
                    case transportsIcons.Underground:
                        return "Images/Common/UndergroundIcon.png";
                    case transportsIcons.Subway:
                        return "Images/Common/SubwayIcon.png";
                    case transportsIcons.Helicopter:
                        return "Images/Common/HelicopterIcon.png";
                    case transportsIcons.Plane:
                        return "Images/Common/PlaneIcon.png";
                    case transportsIcons.Rent:
                        return "Images/Common/RentIcon.png";
                }
            }

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

                if (scope.interests.length > 0) {
                    scope.interests[scope.currentIndex].visible = true; // make the current interest visible
                }
            });

            var timer;
            var sliderFunc = function () {
                timer = $timeout(function () {
                    scope.next();
                    timer = $timeout(sliderFunc, 4000);
                }, 4000);
            };

            sliderFunc();

            scope.$on('$destroy', function () {
                $timeout.cancel(timer); // when the scope is getting destroyed, cancel the timer
            });
        },
        templateUrl: 'AppViews/Home/home-suggested-interests-slidetemplate.html'
    };
});
globalModule.factory('authInterceptor', [
    '$rootScope', '$q', '$location', '$window', '$timeout', '$localStorage',
    function ($rootScope, $q, $location, $window, $timeout, $localStorage) {
        return {
            request: function (config) {
                if ($localStorage.AccessToken !== undefined && $localStorage.TPUserId !== undefined) {
                    config.headers.Authorization = "Bearer " + $localStorage.AccessToken;
                }

                return config || $q.when(config);
            },
            response: function (response) {
                if (response) {
                    if (response.config.url.toLowerCase() === "api/landingpage/login") {
                        $localStorage.AccessToken = response.data.token;
                        $localStorage.TPUserId = response.data.userId;
                    }
                }

                return response;
            },
            responseError: function (response) {
                if (response) {
                    //unauthorized || forbidden - session expired
                    if (response.status === httpStatusCodeEnum.Unauthorized || response.status === httpStatusCodeEnum.Forbidden) {
                        $window.location.href = '/welcome';
                    }
                }

                return $q.reject(response);
            }
        }
    }
]);
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
globalModule.factory('notificationsRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/notifications", {},
            {
                getNotifications: {
                    method: 'GET',
                    url: 'api/notifications/getNotifications',
                    isArray: true
                },
                sendNotification: {
                    method: 'POST',
                    url: 'api/notifications/sendNotification'
                },
                respondNotification: {
                    method: 'POST',
                    url: 'api/notifications/respondNotification'
                },
                deleteNotification: {
                    method: 'POST',
                    url: 'api/notifications/deleteNotification'
                }
            });
    }

]);
globalModule.controller("HeaderController",
    ['$scope', '$window', '$localStorage', '$uibModal', 'homeRepository', 'tripRepository', 'notificationsRepository', 'toastr',
        function ($scope, $window, $localStorage, $uibModal, homeRepository, tripRepository, notificationsRepository, toastr) {
            $scope.notificationTypes = notificationTypes;

            $scope.isLogged = ($localStorage.TPUserId !== null && $localStorage.TPUserId !== undefined) && ($localStorage.AccessToken !== null && $localStorage.AccessToken !== undefined);
            $scope.init = function () {
                if ($scope.isLogged === true) {
                    $scope.userId = $localStorage.TPUserId;
                    $scope.getUserGroupTrips();
                    $scope.getNotifications();
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
                $localStorage.TPUserId = undefined;
                $localStorage.AccessToken = undefined;
                $window.location.href = '/welcome';
            };

            $scope.respondNotification = function (notification, respondValue) {
                var queryParam = {
                    IsAccepted: respondValue,
                    Notification: notification
                }

                switch (notification.type) {
                    case notificationTypes.TripInvitation:
                    case notificationTypes.FriendRequest:
                        $scope.respondNotificationPromise = notificationsRepository.respondNotification(queryParam).$promise;
                        $scope.respondNotificationPromise.then(function (result) {
                            $window.location.reload();
                        }).catch(function (result) {
                            toastr.warning(result.data);
                        });
                        break;
                    case notificationTypes.TripKicked:
                        $scope.deleteNotificationPromise = notificationsRepository.deleteNotification(notification).$promise;
                        $scope.deleteNotificationPromise.then(function (result) {
                            $window.location.reload();
                        }).catch(function (result) {
                            toastr.warning(result.data);
                        });
                }
            }

            $scope.getUserGroupTrips = function () {
                $scope.getUserGroupTripsPromise = tripRepository.getUserHeaderTrips({ userId: $scope.userId }).$promise;
                $scope.getUserGroupTripsPromise.then(function (result) {
                    $scope.userGroupTrips = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.getNotifications = function () {
                $scope.getNotificationsPromise = notificationsRepository.getNotifications({ userId: $scope.userId }).$promise;
                $scope.getNotificationsPromise.then(function (result) {
                    $scope.notifications = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }
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
                },
                getUserTrips: {
                    method: 'GET',
                    url: 'api/account/getUserTrips',
                    isArray: true
                },
                getFriends: {
                    method: 'GET',
                    url: 'api/account/getFriends',
                    isArray: true
                },
                removeFriend: {
                    method: 'GET',
                    url: 'api/account/removeFriend'
                },
                createFriend: {
                    method: 'POST',
                    url: 'api/account/createFriend'
                }
            });
    }

]);
globalModule.controller("AccountInterestsController",
    ['$scope', 'data', '$localStorage', 'accountRepository', 'toastr', '$uibModalInstance',
        function ($scope, data, $localStorage, accountRepository, toastr, $uibModalInstance) {
            $scope.userInterest = data.userInterest;
            $scope.tripId = data.tripId;
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

                var queryParam = {
                    userId: $scope.userId,
                    tripId: $scope.tripId
                };

                $scope.getAvailableCountriesPromise = accountRepository.getAvailableCountries(queryParam).$promise;
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
                    tripId: $scope.tripId,
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

                var queryParam = {
                    userId: $scope.userId,
                    tripId: $scope.tripId
                };

                $scope.getAvailableWeatherPromise = accountRepository.getAvailableWeather(queryParam).$promise;
                $scope.getAvailableWeatherPromise.then(function (result) {
                    $scope.weatherList = result;
                    $scope.userWeather = $scope.extractListFromString($scope.userInterest.weather);
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.initTransport = function () {
                $scope.userId = $localStorage.TPUserId;

                var queryParam = {
                    userId: $scope.userId,
                    tripId: $scope.tripId
                };

                $scope.getAvailableTransportPromise = accountRepository.getAvailableTransport(queryParam).$promise;
                $scope.getAvailableTransportPromise.then(function (result) {
                    $scope.transportList = result;
                    $scope.userTransport = $scope.extractListFromString($scope.userInterest.transports);
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.initTouristAttractions = function () {
                $scope.userId = $localStorage.TPUserId;

                var queryParam = {
                    userId: $scope.userId,
                    tripId: $scope.tripId
                };

                $scope.getAvailableTouristAttractionsPromise = accountRepository.getAvailableTouristAttractions(queryParam).$promise;
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
                    cities: $scope.userInterest.cities,
                    tripId: $scope.tripId
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
                    weather: $scope.userWeather,
                    tripId: $scope.tripId
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
                    transport: $scope.userTransport,
                    tripId: $scope.tripId
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
                    touristAttractions: $scope.userTouristAttractions,
                    tripId: $scope.tripId
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
    ['$scope', '$window', '$localStorage', '$uibModal', 'accountRepository', 'tripRepository', 'toastr',
        function ($scope, $window, $localStorage, $uibModal, accountRepository, tripRepository, toastr) {
            $scope.user = {};
            $scope.trips = {};

            $scope.init = function () {
                $scope.userId = $localStorage.TPUserId;
                var getUserTripsPromise = accountRepository.getUserTrips({ userId: $scope.userId }).$promise;
                getUserTripsPromise.then(function (result) {
                    $scope.trips = result;

                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.containsSearchText = function (trip) {
                if ($scope.searchText === null || $scope.searchText === undefined) {
                    return true;
                }
                return trip.name.toLowerCase().indexOf($scope.searchText.toLowerCase()) !== -1;
            };

            $scope.enableEdit = function (trip) {
                if (trip.editMode === undefined || trip.editMode === false) {
                    trip.editMode = true;
                }
                else {
                    trip.editMode = !trip.editMode;
                    //save changes
                }
            }

            $scope.redirectToTrip = function (trip) { 
                if (trip.isDeleted === false) {
                    $window.location.href = '/trip/tripPlanner/' + trip.id;
                }
            };

            $scope.leaveTrip = function (tripId) {
                $scope.leaveTripPromise = tripRepository.leaveTrip({ userId: $scope.userId, tripId: tripId }).$promise;
                $scope.leaveTripPromise.then(function (result) {
                    $scope.init();
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.init();
        }

    ]);
globalModule.controller("FriendsController",
    ['$scope', '$window', '$localStorage', '$uibModal', 'accountRepository', 'toastr',
        function ($scope, $window, $localStorage, $uibModal, accountRepository, toastr) {
            $scope.friends = {};

            $scope.init = function () {
                $scope.userId = $localStorage.TPUserId;
                var getFriendsPromise = accountRepository.getFriends({ userId: $scope.userId }).$promise;
                getFriendsPromise.then(function (result) {
                    $scope.friends = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.containsSearchText = function (friend) {
                if ($scope.searchText === null || $scope.searchText === undefined) {
                    return true;
                }
                return friend.name.toLowerCase().indexOf($scope.searchText.toLowerCase()) !== -1;
            };

            $scope.addFriend = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'AppViews/Account/friends-modal.html',
                    controller: 'FriendsModalController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userId: $scope.userId
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.init();
                });
            }

            $scope.removeFriend = function (id) {
                $scope.removeFriendPromise = accountRepository.removeFriend({ id: id }).$promise;
                $scope.removeFriendPromise.then(function () {
                    $scope.init();
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.init();
        }

    ]);
globalModule.controller("FriendsModalController",
    ['$scope', 'data', '$window', '$http', '$localStorage', '$uibModalInstance', 'accountRepository', 'toastr', '$routeParams', '$uibModal',
        function ($scope, data, $window, $http, $localStorage, $uibModalInstance, accountRepository, toastr, $routeParams, $uibModal) {
            $scope.userId = data.userId;

            $scope.friendEmail = "";

            $scope.submit = function () {
                $scope.createFriendPromise = accountRepository.createFriend({ userId: $scope.userId, friendEmail: $scope.friendEmail }).$promise;
                $scope.createFriendPromise.then(function () {
                    toastr.success('Friend request was sent to' + $scope.friendEmail);
                    $scope.friendEmail = "";
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.close = function () {
                $uibModalInstance.close();
            };
        }

    ]);
globalModule.controller("TripCreateController",
    ['$scope', '$window', '$http', '$location', '$localStorage', 'tripCreateRepository', 'accountRepository', 'toastr',
        function ($scope, $window, $http, $location, $localStorage, tripCreateRepository, accountRepository, toastr) {
            $scope.invitedPersonEmail = "";
            $scope.invitedPeople = [];
            $scope.friends = [];

            $scope.tripMainPageTabsEnum = tripMainPageTabsEnum;
            $scope.tripTypeEnum = tripTypeEnum;
            $scope.selectedTab = tripMainPageTabsEnum.MainTab;

            $scope.init = function () {
                $scope.userId = $localStorage.TPUserId;
            };

            $scope.changeTab = function (tab) {
                $scope.tripName = '';
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
                        $scope.loadFriends();
                        break;
                    }
                }
            };

            $scope.goBackToMainTab = function () {
                $scope.tripName = '';
                $scope.selectedTab = tripMainPageTabsEnum.MainTab;
            };

            $scope.addInvitedPerson = function () {
                if (validateEmail($scope.invitedPersonEmail)) {
                    if ($scope.invitedPeople.indexOf($scope.invitedPersonEmail) !== -1) {
                        toastr.error("The typed person is already invited!");
                    }
                    else {
                        $scope.invitedPeople.push($scope.invitedPersonEmail);
                        $scope.invitedPersonEmail = "";
                        $scope.$apply();
                    }
                }
                else {
                    toastr.error("The email is not valid!");
                }
            };

            $scope.loadFriends = function () {
                $scope.getFriendsPromise = accountRepository.getFriends({ userId: $scope.userId }).$promise;
                $scope.getFriendsPromise.then(function (result) {
                    $scope.friends = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.onSelectedFriend = function (item) {
                var index = $scope.invitedPeople.indexOf(item.friendAccount.email);
                if (index === undefined || index < 0) {
                    $scope.invitedPeople.push(item.friendAccount.email);
                    $scope.$apply();
                }
                else {
                    toastr.warning("The selected friend is already invited!");
                }
            }

            $scope.removeInvitedPerson = function (person) {
                var index = $scope.invitedPeople.indexOf(person);
                $scope.invitedPeople.splice(index, 1);
                $scope.apply();
            };

            function isNullOrWhitespace(input) {
                if (typeof input === 'undefined' || input == null) return true;
                return input.replace(/\s/g, '').length < 1;
            }

            $scope.createTrip = function (tripType) {
                if (isNullOrWhitespace($scope.tripName)) {
                    toastr.error("Trip Name field must not be empty!");
                    return;
                }

                var queryParam = {
                    tripName: $scope.tripName,
                    tripType: tripType == tripTypeEnum.Single ? 0 : 1
                };
                if (tripType == tripTypeEnum.Group) {
                    angular.extend(queryParam, { invitedPeople: $scope.invitedPeople });
                }

                var createTripPromise = tripCreateRepository.createTrip({ userId: $scope.userId }, queryParam).$promise;
                createTripPromise.then(function (result) {
                    if (result != null) {
                        $location.url('/trip/tripPlanner/' + result.id);
                    }
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            function validateEmail(email) {
                const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                return re.test(String(email).toLowerCase());
            }

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
globalModule.controller("TripPlannerController",
    ['$scope', '$window', '$http', '$localStorage', 'tripRepository', 'toastr', '$routeParams', '$uibModal',
        function ($scope, $window, $http, $localStorage, tripRepository, toastr, $routeParams, $uibModal) {
            $scope.invitedPersonEmail = "";
            $scope.invitedPeople = [];
            $scope.suggestedInterests = [];

            $scope.isAdmin = false;
            $scope.isGroupTrip = false;
            $scope.canLoadMore = true;

            $scope.messageText = "";

            $scope.suggestedInterestsLoaded = false;
            $scope.tripId = null;

            $scope.tripTypeEnum = tripTypeEnum;

            $scope.initInterests = function () {
                $scope.userId = $localStorage.TPUserId;
                $scope.getUserInterestPromise = tripRepository.getUserInterestForTrip({ userId: $scope.userId, tripId: $scope.tripId }).$promise;
                $scope.getUserInterestPromise.then(function (result) {
                    $scope.userInterest = result;
                    $scope.initInterestsForTrip(false);
                    $scope.initMessagesForTrip();
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.loadMore = function () {
                $scope.initInterestsForTrip(true);
            }

            $scope.getTrip = function (tripId) {
                $scope.getTripPromise = tripRepository.getTrip({ tripId: tripId }).$promise;
                $scope.getTripPromise.then(function (result) {
                    $scope.isGroupTrip = result.type == tripTypeEnum.Group;
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
                                userInterest: $scope.userInterest,
                                tripId: $scope.tripId
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
                                userInterest: $scope.userInterest,
                                tripId: $scope.tripId
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
                                userInterest: $scope.userInterest,
                                tripId: $scope.tripId
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
                                userInterest: $scope.userInterest,
                                tripId: $scope.tripId
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            };

            $scope.resetUserInterests = function () {
                $scope.resetUserInterestsPromise = tripRepository.resetUserInterests({ userId: $scope.userId, tripId: $scope.tripId }).$promise;
                $scope.resetUserInterestsPromise.then(function (result) {
                    if (result) {
                        $scope.initInterests();
                    }
                    else {
                        toastr.warning("A problem occured while reseting interests! Please try again!");
                    }
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.leaveTrip = function () {
                $scope.leaveTripPromise = tripRepository.leaveTrip({ userId: $scope.userId, tripId: $scope.tripId }).$promise;
                $scope.leaveTripPromise.then(function (result) {
                    $window.location.href = '/';
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.init = function () {
                $scope.userId = $localStorage.TPUserId;
                $scope.tripId = parseInt($routeParams.id);
                $scope.getTrip($scope.tripId);
                $scope.initInterests();
                $scope.setAdmin();
            };

            $scope.initInterestsForTrip = function (isLoadMoreLevelPressed = false) {
                $scope.getInterestsForTripPromise = tripRepository.getInterestsForTrip({ tripId: $scope.tripId, isLoadMoreLevelPressed: isLoadMoreLevelPressed }).$promise;
                $scope.getInterestsForTripPromise.then(function (result) {
                    $scope.suggestedInterests = result;
                    $scope.suggestedInterestsLoaded = true;
                }).catch(function (result) {
                    $scope.canLoadMore = false;
                    toastr.warning(result.data);
                });;
            }

            $scope.initMessagesForTrip = function () {
                $scope.getInterestsForTripPromise = tripRepository.getMessagesForTrip({ tripId: $scope.tripId }).$promise;
                $scope.getInterestsForTripPromise.then(function (result) {
                    $scope.messages = result.map(function (message) {
                        return {
                            id: message.id,
                            senderId: message.senderId,
                            text: message.text,
                            isSender: $scope.userId == message.senderId,
                            date: message.date,
                            senderEmail: message.senderEmail
                        };
                    });
                }).catch(function (result) {
                    toastr.warning(result.data);
                });;
            }

            $scope.refreshMessages = function () {
                $scope.initMessagesForTrip();
            }

            $scope.sendMessage = function () {
                var newMessage = {
                    senderId: $scope.userId,
                    tripId: $scope.tripId,
                    text: $scope.messageText,
                    date: new Date(),
                };

                $scope.createMessagePromise = tripRepository.createMessage(newMessage).$promise;
                $scope.createMessagePromise.then(function (result) {
                    $scope.messageText = "";
                    $scope.initMessagesForTrip();
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.getFirstLetterFromEmail = function (email) {
                var emailTrimed = email.trim();
                return emailTrimed[0].toUpperCase().concat(emailTrimed[1].toUpperCase());
            }

            $scope.setAdmin = function () {
                $scope.getUserAdmin = tripRepository.isUserAdmin({ userId: $scope.userId, tripId: $scope.tripId }).$promise;
                $scope.getUserAdmin.then(function (result) {
                    $scope.isAdmin = result.isAdmin;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.initManageMembersModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'AppViews/Trip/trip-planner-manage-members-modal.html',
                    controller: 'TripPlannerManageMembersModalController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userId: $scope.userId,
                                tripId: $scope.tripId,
                                isAdmin: $scope.isAdmin
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            }
        }

    ]);
globalModule.controller("TripPlannerManageMembersModalController",
    ['$scope', 'data', '$window', '$http', '$localStorage', '$uibModalInstance', 'tripRepository', 'accountRepository', 'toastr', '$routeParams', '$uibModal',
        function ($scope, data, $window, $http, $localStorage, $uibModalInstance, tripRepository, accountRepository, toastr, $routeParams, $uibModal) {
            $scope.userId = data.userId;
            $scope.tripId = data.tripId;
            $scope.isAdmin = data.isAdmin;

            $scope.newMemberEmail = "";
            $scope.canShowNewMemberFields = false;

            $scope.members = [];
            $scope.friends = [];

            $scope.enableAddNewMember = function () {
                if ($scope.canShowNewMemberFields) {
                    $scope.canShowNewMemberFields = false;
                } else {
                    $scope.newMemberEmail = "";
                    $scope.canShowNewMemberFields = true;
                }
            }

            $scope.addNewMember = function () {
                $scope.addNewMemberPromise = tripRepository.addNewTripMember({ adminid: $scope.userId, tripId: $scope.tripId, newMemberEmail: $scope.newMemberEmail }).$promise;
                $scope.addNewMemberPromise.then(function (result) {
                    $scope.members = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.loadFriends = function () {
                $scope.getFriendsPromise = accountRepository.getFriends({ userId: $scope.userId }).$promise;
                $scope.getFriendsPromise.then(function (result) {
                    $scope.friends = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.onSelectedFriend = function (item) {
                $scope.newMemberEmail = item.friendAccount.email;
                $scope.$apply();
            }

            $scope.submit = function () {
                var list = [];
                $scope.members.forEach(function (item) { list.push(item) });

                $scope.updateTripUsersPromise = tripRepository.updateTripUsers(list).$promise;
                $scope.updateTripUsersPromise.then(function (result) {
                    $scope.members = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.removeUser = function (member) {
                $scope.removeUserFromTripPromise = tripRepository.removeUserFromTrip({ adminId: $scope.userId, userId: member.userId, tripId: $scope.tripId }).$promise;
                $scope.removeUserFromTripPromise.then(function (result) {
                    if (result) {
                        var index = $scope.members.indexOf(member);
                        $scope.members.splice(index, 1);
                    }
                    else {
                        toastr.warning("The user couldn't be removed from this trip!");
                    }
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.close = function () {
                $uibModalInstance.close();
            };

            $scope.init = function () {
                $scope.getTripUsersPromise = tripRepository.getTripUsers({ tripId: $scope.tripId }).$promise;
                $scope.getTripUsersPromise.then(function (result) {
                    $scope.members = result;
                    $scope.loadFriends();
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.init();
        }

    ]);
globalModule.factory('tripRepository', [
    '$resource',
    function ($resource) {
        return $resource("api/trip", {},
            {
                getUserHeaderTrips: {
                    method: 'GET',
                    url: 'api/trip/getUserHeaderTrips',
                    isArray: true
                },
                getUserInterestForTrip: {
                    method: 'GET',
                    url: 'api/trip/getUserInterestForTrip'
                },
                getInterestsForTrip: {
                    method: 'GET',
                    url: 'api/trip/getSuggestedInterests',
                    isArray: true
                },
                getMessagesForTrip: {
                    method: 'GET',
                    url: 'api/trip/getMessages',
                    isArray: true
                },
                createMessage: {
                    method: 'POST',
                    url: 'api/trip/createMessage'
                },
                isUserAdmin: {
                    method: 'GET',
                    url: 'api/trip/isUserAdmin'
                },
                getTripUsers: {
                    method: 'GET',
                    url: 'api/trip/getTripUsers',
                    isArray: true
                },
                updateTripUsers: {
                    method: 'POST',
                    url: 'api/trip/updateTripUsers',
                    isArray: true
                },
                addNewTripMember: {
                    method: 'GET',
                    url: 'api/trip/addNewTripMember',
                    isArray: true
                },
                removeUserFromTrip: {
                    method: 'GET',
                    url: 'api/trip/removeUserFromTrip'
                },
                resetUserInterests: {
                    method: 'GET',
                    url: 'api/trip/resetUserInterests'
                },
                leaveTrip: {
                    method: 'GET',
                    url: 'api/trip/leaveTrip'
                },
                getTrip: {
                    method: 'GET',
                    url: 'api/trip/getTrip'
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