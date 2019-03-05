globalModule.controller("AccountInterestsController",
    ['$scope', '$localStorage', 'accountRepository', 'toastr', '$uibModalInstance',
        function ($scope, $localStorage, accountRepository, toastr, $uibModalInstance) {
            $scope.user = {};
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