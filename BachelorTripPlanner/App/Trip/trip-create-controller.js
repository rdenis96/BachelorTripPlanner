globalModule.controller("TripCreateController",
    ['$scope', '$window', '$http', '$localStorage', 'tripCreateRepository', 'toastr',
        function ($scope, $window, $http, $localStorage, tripCreateRepository, toastr) {
            $scope.invitedPersonEmail = "";
            $scope.invitedPeople = [];

            $scope.tripMainPageTabsEnum = tripMainPageTabsEnum;
            $scope.tripTypeEnum = tripTypeEnum;
            $scope.selectedTab = tripMainPageTabsEnum.MainTab;

            $scope.init = function () {
                $scope.userId = $localStorage.TPUserId;
            };

            $scope.changeTab = function (tab) {
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
                        break;
                    }
                }
            };

            $scope.goBackToMainTab = function () {
                $scope.selectedTab = tripMainPageTabsEnum.MainTab;
            };

            $scope.addInvitedPerson = function () {
                $scope.invitedPeople.push($scope.invitedPersonEmail);
                $scope.invitedPersonEmail = "";
            };

            $scope.removeInvitedPerson = function () {
                var index = $scope.invitedPeople.indexOf($scope.invitedPeople.selected);
                $scope.invitedPeople.splice(index, 1);
                $scope.invitedPeople.selected = undefined;
            };

            $scope.createTrip = function (tripType) {
                var queryParam = {
                    tripName: $scope.tripName,
                    tripType: tripType
                };
                if (tripType == tripTypeEnum.Group) {
                    angular.extend(queryParam, { invitedPeople: $scope.invitedPeople });
                }
                var createTripPromise = tripCreateRepository.createTrip({ userId: $scope.userId }, queryParam).$promise;
                createTripPromise.then(function (result) {
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.init();
        }

    ]);