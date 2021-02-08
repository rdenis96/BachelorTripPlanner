globalModule.controller("TripCreateController",
    ['$scope', '$window', '$http', '$location', '$localStorage', 'tripCreateRepository', 'accountRepository', 'toastr',
        function ($scope, $window, $http, $location, $localStorage, tripCreateRepository, accountRepository, toastr) {
            $scope.invitedPersonEmail = "";
            $scope.invitedPeople = [];
            $scope.friends = [];

            $scope.tripMainPageTabsEnum = tripMainPageTabsEnum;
            $scope.tripTypeEnum = tripTypeEnum;
            $scope.selectedTab = tripMainPageTabsEnum.MainTab;

            $scope.init = function () {
                $scope.userId = $localStorage.TPUserId;
            };

            $scope.changeTab = function (tab) {
                $scope.tripName = '';
                switch (tab) {
                    case tripMainPageTabsEnum.MainTab: {
                        $scope.selectedTab = tripMainPageTabsEnum.MainTab;
                        break;
                    }
                    case tripMainPageTabsEnum.SingleTrip: {
                        $scope.selectedTab = tripMainPageTabsEnum.SingleTrip;
                        break;
                    }
                    case tripMainPageTabsEnum.GroupTrip: {
                        $scope.selectedTab = tripMainPageTabsEnum.GroupTrip;
                        $scope.loadFriends();
                        break;
                    }
                }
            };

            $scope.goBackToMainTab = function () {
                $scope.tripName = '';
                $scope.selectedTab = tripMainPageTabsEnum.MainTab;
            };

            $scope.addInvitedPerson = function () {
                if (validateEmail($scope.invitedPersonEmail)) {
                    if ($scope.invitedPeople.indexOf($scope.invitedPersonEmail) !== -1) {
                        toastr.error("The typed person is already invited!");
                    }
                    else {
                        $scope.invitedPeople.push($scope.invitedPersonEmail);
                        $scope.invitedPersonEmail = "";
                        $scope.$apply();
                    }
                }
                else {
                    toastr.error("The email is not valid!");
                }
            };

            $scope.loadFriends = function () {
                $scope.getFriendsPromise = accountRepository.getFriends({ userId: $scope.userId }).$promise;
                $scope.getFriendsPromise.then(function (result) {
                    $scope.friends = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.onSelectedFriend = function (item) {
                var index = $scope.invitedPeople.indexOf(item.friendAccount.email);
                if (index === undefined || index < 0) {
                    $scope.invitedPeople.push(item.friendAccount.email);
                    $scope.$apply();
                }
                else {
                    toastr.warning("The selected friend is already invited!");
                }
            }

            $scope.removeInvitedPerson = function (person) {
                var index = $scope.invitedPeople.indexOf(person);
                $scope.invitedPeople.splice(index, 1);
                $scope.apply();
            };

            function isNullOrWhitespace(input) {
                if (typeof input === 'undefined' || input == null) return true;
                return input.replace(/\s/g, '').length < 1;
            }

            $scope.createTrip = function (tripType) {
                if (isNullOrWhitespace($scope.tripName)) {
                    toastr.error("Trip Name field must not be empty!");
                    return;
                }

                var queryParam = {
                    tripName: $scope.tripName,
                    tripType: tripType
                };
                if (tripType == tripTypeEnum.Group) {
                    angular.extend(queryParam, { invitedPeople: $scope.invitedPeople });
                }

                var createTripPromise = tripCreateRepository.createTrip({ userId: $scope.userId }, queryParam).$promise;
                createTripPromise.then(function (result) {
                    if (result != null) {
                        $location.url('/trip/tripPlanner/' + result.id);
                    }
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            function validateEmail(email) {
                const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
                return re.test(String(email).toLowerCase());
            }

            $scope.init();
        }

    ]);