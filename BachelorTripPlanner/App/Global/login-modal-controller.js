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