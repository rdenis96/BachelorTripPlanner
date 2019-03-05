globalModule.controller("AccountInterestsController",
    ['$scope', '$localStorage', 'accountRepository', 'toastr',
        function ($scope, $localStorage, accountRepository, toastr) {
            $scope.user = {};
            $scope.countriesList = [];

            $scope.initCountryCity = function () {
                $scope.userId = $localStorage.TPUserId;
                var getCountriesPromise = accountRepository.getCountries().$promise;
                getCountriesPromise.then(function (result) {
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
        }

    ]);