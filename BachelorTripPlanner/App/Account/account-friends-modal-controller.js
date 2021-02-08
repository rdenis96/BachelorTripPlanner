globalModule.controller("FriendsModalController",
    ['$scope', 'data', '$window', '$http', '$localStorage', '$uibModalInstance', 'accountRepository', 'toastr', '$routeParams', '$uibModal',
        function ($scope, data, $window, $http, $localStorage, $uibModalInstance, accountRepository, toastr, $routeParams, $uibModal) {
            $scope.userId = data.userId;

            $scope.friendEmail = "";

            $scope.submit = function () {
                $scope.createFriendPromise = accountRepository.createFriend({ userId: $scope.userId, friendEmail: $scope.friendEmail }).$promise;
                $scope.createFriendPromise.then(function () {
                    toastr.success('Friend request was sent to ' + $scope.friendEmail);
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