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