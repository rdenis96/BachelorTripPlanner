globalModule.controller("TripPlannerManageMembersModalController",
    ['$scope', 'data', '$window', '$http', '$localStorage', '$uibModalInstance', 'tripRepository', 'toastr', '$routeParams', '$uibModal',
        function ($scope, data, $window, $http, $localStorage, $uibModalInstance, tripRepository, toastr, $routeParams, $uibModal) {
            $scope.userId = data.userId;
            $scope.tripId = data.tripId;
            $scope.isAdmin = data.isAdmin;

            $scope.newMemberEmail = "";
            $scope.canShowNewMemberFields = false;

            $scope.members = [];

            $scope.enableAddNewMember = function () {
                if ($scope.canShowNewMemberFields) {
                    $scope.canShowNewMemberFields = false;
                } else {
                    $scope.newMemberEmail = "";
                    $scope.canShowNewMemberFields = true;
                }
            }

            $scope.addNewMember = function () {
                $scope.addNewMemberPromise = tripRepository.addNewTripMember({ adminid: $scope.userId, tripId: $scope.tripId, newMemberEmail: $scope.newMemberEmail }).$promise;
                $scope.addNewMemberPromise.then(function (result) {
                    $scope.members = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.submit = function () {
                var list = [];
                $scope.members.forEach(function (item) { list.push(item) });

                $scope.updateTripUsersPromise = tripRepository.updateTripUsers(list).$promise;
                $scope.updateTripUsersPromise.then(function (result) {
                    $scope.members = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            };

            $scope.removeUser = function (member) {
                $scope.removeUserFromTripPromise = tripRepository.removeUserFromTrip({ adminId: $scope.userId, userId: member.userId, tripId: $scope.tripId }).$promise;
                $scope.removeUserFromTripPromise.then(function (result) {
                    if (result) {
                        var index = $scope.members.indexOf(member);
                        $scope.members.splice(index, 1);
                    }
                    else {
                        toastr.warning("The user couldn't be removed from this trip!");
                    }
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.close = function () {
                $uibModalInstance.close();
            };

            $scope.init = function () {
                $scope.getTripUsersPromise = tripRepository.getTripUsers({ tripId: $scope.tripId }).$promise;
                $scope.getTripUsersPromise.then(function (result) {
                    $scope.members = result;
                }).catch(function (result) {
                    toastr.warning(result.data);
                });
            }

            $scope.init();
        }

    ]);