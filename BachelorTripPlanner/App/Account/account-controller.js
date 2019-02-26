globalModule.controller("AccountController",
    ['$scope', '$localStorage', 'accountRepository', 'toastr',
        function ($scope, $localStorage, accountRepository, toastr) {
            $scope.user = {};

            $scope.initEditAccount = function () {
                $scope.userId = $localStorage.TPUserId;
                var getUserPromise = accountRepository.getUser({ userId: $scope.userId }).$promise;
                getUserPromise.then(function (result) {
                    $scope.user = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

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