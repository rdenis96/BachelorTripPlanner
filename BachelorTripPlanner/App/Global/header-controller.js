globalModule.controller("HeaderController",
    ['$scope', '$window', '$localStorage', '$uibModal', 'homeRepository', 'tripRepository', 'notificationsRepository', 'toastr',
        function ($scope, $window, $localStorage, $uibModal, homeRepository, tripRepository, notificationsRepository, toastr) {
            $scope.notificationTypes = notificationTypes;

            $scope.isLogged = $localStorage.TPUserId !== null && $localStorage.TPUserId !== undefined;
            $scope.init = function () {
                if ($scope.isLogged === true) {
                    $scope.userId = $localStorage.TPUserId;
                    $scope.getUserGroupTrips();
                    $scope.getNotifications();
                }
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
                            $scope.getNotifications();
                        }).catch(function (result) {
                            toastr.warning(result.data);
                        });
                        break;
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