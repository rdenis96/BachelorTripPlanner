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