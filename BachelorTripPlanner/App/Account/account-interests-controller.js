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