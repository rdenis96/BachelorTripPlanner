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