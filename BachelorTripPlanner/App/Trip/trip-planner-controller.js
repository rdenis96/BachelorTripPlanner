globalModule.controller("TripPlannerController",
    ['$scope', '$window', '$http', '$localStorage', 'tripRepository', 'toastr', '$routeParams', '$uibModal',
        function ($scope, $window, $http, $localStorage, tripRepository, toastr, $routeParams, $uibModal) {
            $scope.invitedPersonEmail = "";
            $scope.invitedPeople = [];
            $scope.suggestedInterests = [];

            $scope.isAdmin = false;

            $scope.messageText = "";

            $scope.suggestedInterestsLoaded = false;
            $scope.tripId = null;

            $scope.tripTypeEnum = tripTypeEnum;

            $scope.initInterests = function () {
                $scope.userId = $localStorage.TPUserId;
                $scope.getUserInterestPromise = tripRepository.getUserInterestForTrip({ userId: $scope.userId, tripId: $scope.tripId }).$promise;
                $scope.getUserInterestPromise.then(function (result) {
                    $scope.userInterest = result;
                    $scope.initInterestsForTrip();
                    $scope.initMessagesForTrip();
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
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userInterest: $scope.userInterest,
                                tripId: $scope.tripId
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            };

            $scope.openWeatherModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'AppViews/Account/interest-weather-modal.html',
                    controller: 'AccountInterestsController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userInterest: $scope.userInterest,
                                tripId: $scope.tripId
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            };

            $scope.openTransportsModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'AppViews/Account/interest-transport-modal.html',
                    controller: 'AccountInterestsController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userInterest: $scope.userInterest,
                                tripId: $scope.tripId
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            };

            $scope.openTouristAttractionsModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'AppViews/Account/interest-tourist-attractions-modal.html',
                    controller: 'AccountInterestsController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userInterest: $scope.userInterest,
                                tripId: $scope.tripId
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            };

            $scope.resetUserInterests = function () {
                $scope.resetUserInterestsPromise = tripRepository.resetUserInterests({ userId: $scope.userId, tripId: $scope.tripId }).$promise;
                $scope.resetUserInterestsPromise.then(function (result) {
                    if (result) {
                        $scope.initInterests();
                    }
                    else {
                        toastr.warning("A problem occured while reseting interests! Please try again!");
                    }
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.init = function () {
                $scope.userId = $localStorage.TPUserId;
                $scope.tripId = $routeParams.id;
                $scope.initInterests();
                $scope.setAdmin();
            };

            $scope.initInterestsForTrip = function () {
                $scope.getInterestsForTripPromise = tripRepository.getInterestsForTrip({ tripId: $scope.tripId }).$promise;
                $scope.getInterestsForTripPromise.then(function (result) {
                    $scope.suggestedInterests = result;
                    $scope.suggestedInterestsLoaded = true;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });;
            }

            $scope.initMessagesForTrip = function () {
                $scope.getInterestsForTripPromise = tripRepository.getMessagesForTrip({ tripId: $scope.tripId }).$promise;
                $scope.getInterestsForTripPromise.then(function (result) {
                    $scope.messages = result.map(function (message) {
                        return {
                            id: message.id,
                            senderId: message.senderId,
                            text: message.text,
                            isSender: $scope.userId == message.senderId,
                            date: message.date,
                            senderEmail: message.senderEmail
                        };
                    });
                }).catch(function (result) {
                    toastr.warning(result.data);
                });;
            }

            $scope.refreshMessages = function () {
                $scope.initMessagesForTrip();
            }

            $scope.sendMessage = function () {
                var newMessage = {
                    senderId: $scope.userId,
                    tripId: $scope.tripId,
                    text: $scope.messageText,
                    date: new Date(),
                };

                $scope.createMessagePromise = tripRepository.createMessage(newMessage).$promise;
                $scope.createMessagePromise.then(function (result) {
                    $scope.messageText = "";
                    $scope.initMessagesForTrip();
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.getFirstLetterFromEmail = function (email) {
                var emailTrimed = email.trim();
                return emailTrimed[0].toUpperCase().concat(emailTrimed[1].toUpperCase());
            }

            $scope.setAdmin = function () {
                $scope.getUserAdmin = tripRepository.isUserAdmin({ userId: $scope.userId, tripId: $scope.tripId }).$promise;
                $scope.getUserAdmin.then(function (result) {
                    $scope.isAdmin = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.initManageMembersModal = function () {
                var modalInstance = $uibModal.open({
                    templateUrl: 'AppViews/Trip/trip-planner-manage-members-modal.html',
                    controller: 'TripPlannerManageMembersModalController',
                    backdrop: 'static',
                    keyboard: false,
                    size: 'lg',
                    resolve: {
                        data: function () {
                            return {
                                userId: $scope.userId,
                                tripId: $scope.tripId,
                                isAdmin: $scope.isAdmin
                            };
                        }
                    }
                }).closed.then(function () {
                    $scope.initInterests();
                });
            }
        }

    ]);